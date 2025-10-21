using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Model.Banking;
using Model.PlayerManagement;
using PersistenceTests.Comparers;
using PersistentModel.Model;
using PersistentModel.Model.Banking;
using PersistentModel.Model.PlayerManagement;
using PersistentModel.Repository;
using System.Linq;
using System.Xml.Linq;

namespace PersistenceTests
{
    public class PlayerRepositoryTests : DatabaseUsingTest
    {
        public PlayerRepositoryTests():base()
        {
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void PopulateTestDb_WithRandomData_DataIsInserted()
        {
            // Arrange
            var players = PlayerRepositoryTestData.CreateRandomPlayers(20);
            context.AddRange(players);
            context.SaveChanges();
            int expectedPlayerCount = players.Count;
            int expectedCashRecordCount = players.Count;
            int expectedTransactionCount = players.SelectMany(p => p.CashRecord.History).Count();

            // Act
            context.AddRange(players);

            // Assert
            Assert.Equal(expectedPlayerCount, context.PlayerOOGData.Count());
            Assert.Equal(expectedCashRecordCount, context.PlayerCashRecords.Count());
            Assert.Equal(expectedTransactionCount, context.GilTransactions.Count());
        }

        #region Create Player
        [Fact]
        public void CreatePlayer_NonExisting_IsCreated()
        {
            // Arrange
            string name = "John Frusciante";
            string world = "California";
            var player = new PlayerOOGData(name, world);
            var repo = new PlayerRepository(context);

            // Act
            var result = repo.CreatePlayer(player);

            // Assert
            Assert.True(result);
            AssertPlayerCreated(1, name, world);
        }

        [Fact]
        public void CreatePlayer_Existing_NotCreated()
        {
            // Arrange
            string name = "John Frusciante";
            string world = "California";
            var player = new PlayerOOGData(name, world);
            var repo = new PlayerRepository(context);
            context.PlayerOOGData.Add(EntityMapper.Mapper.Map<PlayerOOGDataEntity>(new PlayerOOGData(name, world)));
            context.SaveChanges();

            // Act
            var result = repo.CreatePlayer(player);

            // Assert
            Assert.False(result);
            var players = context.PlayerOOGData.ToList();
            Assert.Single(players);
            players[0].Name = name;
            players[0].World = world;
        }

        [Fact]
        public void CreatePlayer_OnlyNameMatches_Created()
        {
            // Arrange
            string name = "John Frusciante";
            string world = "California";
            var player = new PlayerOOGData(name, world);
            var repo = new PlayerRepository(context);
            repo.CreatePlayer(new PlayerOOGData(name, "OtherWorld"));
            context.SaveChanges();

            // Act
            var result = repo.CreatePlayer(player);

            // Assert
            Assert.True(result);
            AssertPlayerCreated(2, name, world);
        }

        [Fact]
        public void CreatePlayer_OnlyWorldMatches_Created()
        {
            // Arrange
            string name = "John Frusciante";
            string world = "California";
            var player = new PlayerOOGData(name, world);
            var repo = new PlayerRepository(context);
            repo.CreatePlayer(new PlayerOOGData("other name", world));
            context.SaveChanges();

            // Act
            var result = repo.CreatePlayer(player);

            // Assert
            Assert.True(result);
            AssertPlayerCreated(2, name, world);
        }
        #endregion
        #region Get Player
        [Fact]
        public void GetPlayer_Existing_ReturnedWithCashRecordButNoHistory()
        {
            // Arrange
            var players = PlayerRepositoryTestData.CreateRandomPlayers(20);
            context.AddRange(players);
            context.SaveChanges();
            var repo = new PlayerRepository(context);


            var playerExpected = players[10];
            playerExpected.CashRecord.History.Clear();
            playerExpected.PreviousIdentities.Clear();

            //Act
            var player = repo.GetPlayerWithCashRecord($"{playerExpected.Name}@{playerExpected.World}");

            // Assert
            player.Should().BeEquivalentTo(EntityMapper.Mapper.Map<PlayerOOGData>(playerExpected));
        }

        [InlineData("Correct name", "wrong world", true)]
        [InlineData("Wrong name", "Correct world", false)]
        [InlineData("", "", false)]
        [Theory]
        public void GetPlayer_NotExisting_ReturnedNull(string name, string world, bool nameMatches)
        {
            // Arrange
            var players = PlayerRepositoryTestData.CreateRandomPlayers(20);
            context.AddRange(players);
            context.SaveChanges();
            var repo = new PlayerRepository(context);
            var playerExpected = players[10];
            if (nameMatches) { playerExpected.Name = name; }
            else { playerExpected.World = world; }

            //Act
            var player = repo.GetPlayerWithCashRecord($"wrongName@{playerExpected.World}");

            // Assert
            Assert.Null(player);
        }

        #endregion
        #region Update Cash Record

        [Fact]
        public void UpdateCashRecord_PlayerExists_CorrectlyUpdated()
        {
            // Arrange
            var randomPlayers = PlayerRepositoryTestData.CreateRandomPlayers(20);
            context.AddRange(randomPlayers);
            context.SaveChanges();
            var players = context.PlayerOOGData.Include(p => p.CashRecord).ThenInclude(c => c.History).ToList();
            var repo = new PlayerRepository(context);
            var patronEntityExpected = players[10];
            var hostEntityExpected = players[12];
            var hostExpected = EntityMapper.Mapper.Map<PlayerOOGData>(hostEntityExpected);
            var patronExpected = EntityMapper.Mapper.Map<PlayerOOGData>(patronEntityExpected);
            int storedRealAmount = 123;
            int storedFakeAmount = 321;
            int inUseRealAmount = 998;
            int inUseFakeAmount = 542;
            patronExpected.CashRecord.StoredReal = storedRealAmount;
            patronExpected.CashRecord.StoredFake = storedFakeAmount;
            patronExpected.CashRecord.InUseReal = inUseRealAmount;
            patronExpected.CashRecord.InUseFake = inUseFakeAmount;
            patronEntityExpected.PreviousIdentities.Clear();

            DateTime replacementDate = new DateTime(2001, 1, 1);
            patronExpected.CreatedAtUtc = replacementDate;

            GilTransaction gilTransaction = new GilTransaction(hostExpected, patronExpected, true, 888);
            patronEntityExpected.CashRecord.History.Add(EntityMapper.Mapper.Map<GilTransactionEntity>(gilTransaction));

            //Act
            bool result = repo.UpdateCashRecord(patronExpected, gilTransaction);

            // Assert
            var updatedPatron = context.PlayerOOGData.Include(c => c.CashRecord).ThenInclude(c => c.History)
                .First(p => p.Name == patronEntityExpected.Name && p.World == patronEntityExpected.World);
            updatedPatron.CreatedAtUtc.Should().NotBe(replacementDate);
            var updatedCashRecord = EntityMapper.Mapper.Map<PlayerCashRecord>(updatedPatron.CashRecord);

            updatedCashRecord.Should()
                .BeEquivalentTo(patronExpected.CashRecord, opt => opt.ShallowPlayer().LooseDate());            
        }


        [Fact]
        public void UpdateCashRecord_PlayerDoesNotExist_UpdateCancelled()
        {
            // Arrange
            var randomPlayers = PlayerRepositoryTestData.CreateRandomPlayers(20);
            context.AddRange(randomPlayers);
            context.SaveChanges();
            var players = context.PlayerOOGData.Include(p => p.CashRecord).ThenInclude(c => c.History).ToList();
            var repo = new PlayerRepository(context);
            var patronEntityExpected = players[10];
            var hostEntityExpected = players[12];
            var hostExpected = EntityMapper.Mapper.Map<PlayerOOGData>(hostEntityExpected);
            var patronExpected = EntityMapper.Mapper.Map<PlayerOOGData>(patronEntityExpected);
            int storedRealAmount = 123;
            int storedFakeAmount = 321;
            int inUseRealAmount = 998;
            int inUseFakeAmount = 542;
            patronExpected.CashRecord.StoredReal = storedRealAmount;
            patronExpected.CashRecord.StoredFake = storedFakeAmount;
            patronExpected.CashRecord.InUseReal = inUseRealAmount;
            patronExpected.CashRecord.InUseFake = inUseFakeAmount;
            patronEntityExpected.PreviousIdentities.Clear();

            string replacementName = "this should break the search";
            string previousName = patronEntityExpected.Name;
            patronExpected.Name = replacementName;

            GilTransaction gilTransaction = new GilTransaction(hostExpected, patronExpected, true, 888);
            patronEntityExpected.CashRecord.History.Add(EntityMapper.Mapper.Map<GilTransactionEntity>(gilTransaction));
            var previousTransactionCount = context.GilTransactions.Count();

            //Act
            bool result = repo.UpdateCashRecord(patronExpected, gilTransaction);

            // Assert
            result.Should().BeFalse();
            var updatedPatron = context.PlayerOOGData.Include(c => c.CashRecord).ThenInclude(c => c.History)
                .Count(p => p.Name == patronEntityExpected.Name && p.World == patronEntityExpected.World).Should().Be(1);

            context.GilTransactions.Count().Should().Be(previousTransactionCount);            
        }


        #endregion

        #region UpdateIdentity
        [Fact]
        public void UpdateAlias_PlayerExists_NameAndWorldUpdatedAndPreviousStored()
        {
            // Arrange
            var randomPlayers = PlayerRepositoryTestData.CreateRandomPlayers(20);
            context.AddRange(randomPlayers);
            context.SaveChanges();
            var repo = new PlayerRepository(context);
            var chosenPlayerEntity = randomPlayers[5];
            var previousIdentityCount = chosenPlayerEntity.PreviousIdentities.Count;
            var newIdentity = new PlayerIdentifier("new name", "new world");

            //Act
            bool result = repo.UpdateAlias(chosenPlayerEntity.Name, chosenPlayerEntity.World, newIdentity);

            // Assert
            result.Should().BeTrue();
            var player = context.PlayerOOGData.Include(p => p.PreviousIdentities)
                .First(p => p.Id == chosenPlayerEntity.Id);

            player.Name.Should().Be(newIdentity.Name);
            player.World.Should().Be(newIdentity.World);
            var existingIdentityRecords = context.PlayerIdentifiers.Where(id => id.PlayerOOGData.Id == player.Id).ToList();
            existingIdentityRecords.Count.Should().Be(previousIdentityCount + 1);
            player.PreviousIdentities.Count.Should().Be(existingIdentityRecords.Count);
            var newlyCreatedIdentifier = EntityMapper.Mapper.Map<PlayerIdentifier>(player.PreviousIdentities.Last());
            newlyCreatedIdentifier.Should()
                .BeEquivalentTo(new PlayerIdentifier
                { Name = chosenPlayerEntity.Name,
                  World = chosenPlayerEntity.World,
                  DateMetUtc = DateTime.UtcNow
                }, opt => opt.LooseDate());
        }
        #endregion

        private void AssertPlayerCreated(int totalPlayersExpected, string name, string world)
        {
            var players = context.PlayerOOGData.ToList();
            Assert.Equal(totalPlayersExpected, players.Count);
            var player = players.Single(p => p.Name == name && p.World == world);
            player.Name = name;
            player.World = world;
            Assert.Empty(player.PreviousIdentities);
            Assert.NotNull(player.CashRecord);
            Assert.Equal(0, player.CashRecord.InUseReal);
            Assert.Equal(0, player.CashRecord.InUseFake);
            Assert.Equal(0, player.CashRecord.StoredReal);
            Assert.Equal(0, player.CashRecord.StoredFake);

            var cashRecords = context.PlayerCashRecords.ToList();
            Assert.Equal(totalPlayersExpected, cashRecords.Count);
            Assert.Single(cashRecords, record => record.PlayerOOGData.Name == name 
                && record.PlayerOOGData.World == world
                && record.PlayerOOGData.Id == player.Id);
        }
    }
}
