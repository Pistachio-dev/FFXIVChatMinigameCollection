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

        public DbSet<PlayerIdentifier> PlayerIdentifiers { get; set; }

        public DbSet<PlayerOOGData> PlayerOOGData { get; set; }

        public string DbPath { get; private set; }

        internal static bool Initialized;

        public MinigameCollectionDbContext()
        {
            DbPath = GetPath();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
            
            base.OnConfiguring(optionsBuilder);
        }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlayerOOGData>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<PlayerCashRecord>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<GilTransaction>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<PlayerIdentifier>()
                .HasKey(i => i.Id);
        }
        private string GetPath()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var fullPath = Path.Combine(appDataPath, "XIVLauncher", "pluginConfigs", "MinigameCollection");

            var configDir = fullPath + Path.DirectorySeparatorChar;

            return $"{configDir}MinigameCollection.db";
        }

        internal void ApplyPendingMigrations()
        {
            if (Initialized) return;

            var pendingMigrations = Database.GetPendingMigrations();

            if (pendingMigrations.Any())
            {
                Database.Migrate();
            }

            Initialized = true;
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
