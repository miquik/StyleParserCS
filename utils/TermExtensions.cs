using StyleParserCS.css;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StyleParserCS.utils
{
    public static class TermExtensions
    {
        public static string GetValueAsString(this Term term)
        {
            if (term is Term<char>)
            {
                return ((Term<char>)term).Value.ToString();
            }
            return String.Empty;
        }
    }
}
