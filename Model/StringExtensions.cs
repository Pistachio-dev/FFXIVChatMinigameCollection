using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public static class StringExtensions
    {
        public static bool TryGetSplitName(this string fullName, out string name, out string world)
        {
            name = string.Empty; world = string.Empty;
            if (fullName == null)
            {
                return false;
            }

            var split = fullName.Split('@');
            if (split.Length != 2)
            {
                return false;
            }

            name = split[0];
            world = split[1];

            return true;
        }
    }
}
