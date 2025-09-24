using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentModel
{
    internal class MinigameCollectionDbContextFactory : IDesignTimeDbContextFactory<MinigameCollectionDbContext>
    {
        public MinigameCollectionDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MinigameCollectionDbContext>();

            optionsBuilder.UseSqlite(GetConnectionString());

            return new MinigameCollectionDbContext(optionsBuilder.Options);
        }

        internal static string GetConnectionString()
        {
            return $"Data Source=database.db";
            var dbPath = GetPath();
            return $"Data Source={dbPath}";
        }

        private static string GetPath()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var fullPath = Path.Combine(appDataPath, "XIVLauncher", "pluginConfigs", "MinigameCollection");

            var configDir = fullPath + Path.DirectorySeparatorChar;

            return $"{configDir}MinigameCollection.db";
        }
    }
}
