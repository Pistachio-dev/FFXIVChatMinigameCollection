using Microsoft.EntityFrameworkCore.Metadata;
using Model.PlayerManagement;
using PersistentModel.Model;
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
