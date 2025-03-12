using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class CommonExtension
    {
        public static bool FindWord(this string s, string searchWord)
        {

            bool result = false;
            string[] words = s.ToUpper().Split(" ");

            foreach (string w in words)
            {
                if (w == searchWord.ToUpper())
                {
                    return true;
                }
            }

            return result;
        }
    }
}
