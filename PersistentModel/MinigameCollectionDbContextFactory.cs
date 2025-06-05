using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentModel
{
    public class MinigameCollectionDbContextFactory : IDesignTimeDbContextFactory<MinigameCollectionDbContext>
    {
        public MinigameCollectionDbContext CreateDbContext(string[] args)
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var fullPath = Path.Combine(appDataPath, "XIVLauncher", "pluginConfigs", "MinigameCollection");

            var configDir = fullPath + Path.DirectorySeparatorChar;
            var dbContext = new MinigameCollectionDbContext(configDir);

            dbContext.InitializeIfNeeded();

            return dbContext;
        }
    }
}
