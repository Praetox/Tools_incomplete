using System;
using System.Collections.Generic;
using System.Text;

namespace cssmod
{
    class cb
    {
        public static string[] split(string a, string b)
        {
            return a.Split(new string[] { b },
                StringSplitOptions.None);
        }
    }
}
