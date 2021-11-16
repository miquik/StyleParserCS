using System.Collections.Generic;

/// <summary>
/// TermRect.java
/// 
/// Created on 22. 3. 2017, 16:00:02 by burgetr
/// </summary>
namespace StyleParserCS.css
{

    /// <summary>
    /// A term that represents a rect(a, b, c, d) value used in clip: properties.
    /// It always holds a list of exactly four lengths.
    /// @author burgetr
    /// </summary>
    public interface TermRect : Term<IList<TermLength>>
    {

    }

}