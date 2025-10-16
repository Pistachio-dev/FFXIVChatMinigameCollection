using FluentAssertions.Equivalency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceTests.Comparers
{
    internal static class EquivalencyOptionsExtensions
    {
        internal static EquivalencyOptions<T> ShallowPlayer<T>(this EquivalencyOptions<T> opts)
        {
            return opts.Using(new ShallowPlayerComparer()); 
        }

        internal static EquivalencyOptions<T> LooseDate<T>(this EquivalencyOptions<T> opts, TimeSpan timeSpan = default)
        {
            if (timeSpan == default) { timeSpan = TimeSpan.FromSeconds(1); }
            return opts.Using(new LooseDateComparer(timeSpan));
        }
    
    }
}
