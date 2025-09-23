using Model.PlayerManagement;
using PersistentModel.Repository;
using System.Linq;

namespace PersistenceTests
{
    public class PlayerRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture data;

        public PlayerRepositoryTests(DatabaseFixture data)
        {
            this.data = data;
        }

        [Fact]
        public void CreatePlayer_NonExisting_IsCreated()
        {
            // Arrange
            string name = "John Frusciante";
            string world = "California";
            var player = new PlayerOOGData(name, world);
            var repo = new PlayerRepository(data.context);

            // Act
            repo.CreatePlayer(player);

            // Assert
            var players = data.context.PlayerOOGData.ToList();
            Assert.Single(players);
            players[0].Name = name;
            players[0].World = world;
            Assert.Empty(players[0].PreviousIdentities);
            Assert.NotNull(players[0].CashRecord);
            Assert.Equal(0, players[0].CashRecord.InUseReal);
            Assert.Equal(0, players[0].CashRecord.InUseFake);
            Assert.Equal(0, players[0].CashRecord.StoredReal);
            Assert.Equal(0, players[0].CashRecord.StoredFake);

            var cashRecord = data.context.PlayerCashRecords.Single();
            Assert.Equal(players[0], cashRecord.PlayerOOGData);
            Assert.Equal(players[0].Id, cashRecord.PlayerOOGDataID);

        }
    }
}
