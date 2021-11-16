using System.Collections.Generic;

/// <summary>
/// TermBracketedIdent.java
/// 
/// Created on 30. 11. 2018, 10:26:43 by burgetr
/// </summary>
namespace StyleParserCS.css
{

    /// <summary>
    /// Represents a list of identifiers in square brackets (e.g. in grid-template)
    /// 
    /// @author burgetr
    /// </summary>
    public interface TermBracketedIdents : Term<IList<TermIdent>>, IList<TermIdent>
    {
    }

}