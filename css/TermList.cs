using System.Collections.Generic;

namespace StyleParserCS.css
{

    /// <summary>
    /// Holds list of terms and allows access to them as to collection
    /// @author kapy
    /// 
    /// </summary>
    public interface TermList : Term<IList<Term>>, IList<Term>
    {

    }

}