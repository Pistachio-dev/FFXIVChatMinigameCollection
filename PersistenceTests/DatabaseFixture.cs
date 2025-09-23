using Microsoft.EntityFrameworkCore;
using PersistentModel;
using PersistentModel.Model.Banking;
using PersistentModel.Model.PlayerManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceTests
{
    public sealed class DatabaseFixture : IDisposable
    {
        internal readonly DbContextOptions<MinigameCollectionDbContext> options;
        internal readonly MinigameCollectionDbContext context;

        public DatabaseFixture()
        {
            options = new DbContextOptionsBuilder<MinigameCollectionDbContext>().UseInMemoryDatabase(databaseName: "Minigame Collection").Options;
            context = new MinigameCollectionDbContext(options);
            context.Database.Migrate();
        }

        public Task DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            this.context.Database.EnsureDeleted();
            this.context.Dispose();
        }
    }
}
