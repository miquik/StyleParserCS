using System;

namespace StyleParserCS.css
{

    /// <summary>
    /// Holds CSS URI value
    /// 
    /// @author kapy
    /// @author Jan Svercl, VUT Brno, 2008
    /// 
    /// </summary>
    public interface TermURI : Term<string>
    {

        TermURI setBase(Uri basev);
        Uri Base { get; }

    }

}