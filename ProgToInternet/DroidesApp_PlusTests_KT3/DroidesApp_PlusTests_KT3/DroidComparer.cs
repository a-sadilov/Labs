using System;
using System.Collections.Generic;
using System.Text;

namespace DroidesApp_PlusTests_KT3
{
    class DroidComparer : IComparer<Droid>
    {
        public int Compare(Droid d1, Droid d2)
        {
            return d1.CompareTo(d2);

        }
    }
}
