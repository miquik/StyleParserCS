/// <summary>
/// MediaQuery.java
/// 
/// Created on 26. 6. 2014, 15:34:42 by burgetr
/// </summary>
namespace StyleParserCS.css
{
    /// <summary>
    /// Holds the media query: the media type and a list of media expressions
    /// 
    /// @author burgetr
    /// </summary>
    public interface MediaQuery : Rule<MediaExpression>
    {

        /// <summary>
        /// Sets the negative flag (i.e. the media query starts with NOT) </summary>
        /// <param name="negative"> </param>
        bool Negative { set; get; }


        /// <summary>
        /// Sets the media type. </summary>
        /// <param name="type"> </param>
        string Type { set; get; }
    }

}