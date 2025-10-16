using Model.PlayerManagement;
using PersistentModel.Model.PlayerManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceTests.Comparers
{
    internal class ShallowPlayerComparer : IEqualityComparer<PlayerOOGData>
    {
        public bool Equals(PlayerOOGData? x, PlayerOOGData? y)
        {
            return x != null && y != null && x.Name == y.Name && x.World == y.World;
        }

        public int GetHashCode([DisallowNull] PlayerOOGData obj)
        {
            return base.GetHashCode();
        }
    }
}
