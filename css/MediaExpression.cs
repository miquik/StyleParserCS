/// <summary>
/// MediaExpression.java
/// 
/// Created on 26. 6. 2014, 15:39:25 by burgetr
/// </summary>
namespace StyleParserCS.css
{
    /// <summary>
    /// Holds the media expression: the tested feature and the values
    /// @author burgetr
    /// </summary>
    public interface MediaExpression : Rule<Term>
    {
        /// <summary>
        /// Obtains the name of the tested feature. </summary>
        /// <returns> The feature name </returns>
        string Feature { get; set; }
    }

}