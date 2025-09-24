using PersistentModel.Model.Banking;
using PersistentModel.Model.PlayerManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceTests
{
    internal class PlayerRepositoryTestData
    {
        public static List<PlayerOOGDataEntity> CreateRandomPlayers(int amount)
        {
            List<PlayerOOGDataEntity> entities = new List<PlayerOOGDataEntity>();
            Random rng = new Random();

            for (int i = 0; i < amount; i++)
            {
                var entity = new PlayerOOGDataEntity
                {
                    Id = (uint)rng.Next(int.MaxValue),
                    Name = Guid.NewGuid().ToString(),
                    World = GetRandomString(SampleWorlds, rng),
                    CreatedAtUtc = DateTime.UtcNow - TimeSpan.FromSeconds(rng.Next(int.MaxValue))
                };

                var cashRecord = new PlayerCashRecordEntity()
                {
                    Id = (uint)rng.Next(int.MaxValue),
                    StoredReal = rng.Next(int.MaxValue),
                    StoredFake = rng.Next(int.MaxValue),
                    InUseReal = rng.Next(9999),
                    InUseFake = rng.Next(9999),
                };

                entity.CashRecord = cashRecord;
                entities.Add(entity);
            }

            for (int i = 0; i < amount; i++)
            {
                int transactionCount = rng.Next(10);
                for (int j = 0; j < transactionCount; j++)
                {
                    var transaction = new GilTransactionEntity
                    {
                        HostPlayer = entities.ElementAt(rng.Next(entities.Count)),
                        PatronPlayer = entities.ElementAt(rng.Next(entities.Count)),
                        IsRealGil = rng.Next(2) % 2 == 0,
                        IsHouseCut = rng.Next(2) % 2 == 0,
                        Amount = rng.Next(1000000),
                        WhenUtc = DateTime.UtcNow - TimeSpan.FromSeconds(rng.Next(int.MaxValue))
                    };
                    entities[i].CashRecord.History.Add(transaction);
                }
            }

            return entities;
        }

        public static string GetRandomString(string[] samples, Random rng)
        {
            return samples[rng.Next(samples.Length)];
        }

        public static string[] SampleWorlds = [
            "Omega",
            "Sagittarius",
            "Raiden",
            "Zodiark",
            "Ravana",
            "Sophia",
            "Sephirot",
            "Balmung",
            "Phantom"
        ];
    }
}
