using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Common.Banking.Data
{
    public class PlayerCashRecord
    {
        public Guid PlayerId { get; set; }
        public long GilBalance { get; set; }
        public List<GilTransaction> History { get; set; } = new();
    }
}
