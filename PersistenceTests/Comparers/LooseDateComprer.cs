using System.Diagnostics.CodeAnalysis;

namespace PersistenceTests.Comparers
{
    internal class LooseDateComparer : IEqualityComparer<DateTime>
    {
        private readonly TimeSpan timeSpan;

        public LooseDateComparer(TimeSpan timeSpan)
        {
            this.timeSpan = timeSpan;
        }

        public bool Equals(DateTime x, DateTime y)
        {
            return x - y <= timeSpan;
        }

        public int GetHashCode([DisallowNull] DateTime obj)
        {
            return base.GetHashCode();
        }
    }
}
