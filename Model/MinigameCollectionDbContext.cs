using Microsoft.EntityFrameworkCore;
using PersistentModel.Model.Banking;
using PersistentModel.Model.PlayerManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentModel
{
    public class MinigameCollectionDbContext : DbContext
    {
        public DbSet<GilTransaction> GilTransactions { get; set; }

        public DbSet<PlayerCashRecord> PlayerCashRecords { get; set; }

        public DbSet<PlayerIdentifier> playerIdentifiers { get; set; }

        public DbSet<PlayerOOGData> PlayerOOGEntries { get; set; }

        public string DbPath { get; }

        public MinigameCollectionDbContext(string configDir)
        {
            DbPath = $"{configDir}MinigameCollection.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
            base.OnConfiguring(optionsBuilder);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
        }
    }
}
