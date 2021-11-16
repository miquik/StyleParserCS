/// <summary>
/// TermLengthOrPercent.java
/// 
/// Created on 13.11.2008, 15:58:28 by burgetr
/// </summary>
namespace StyleParserCS.css
{
    /// <summary>
    /// Holds a float number which is either a CSS length or percentage.
    /// 
    /// @author burgetr
    /// </summary>
    public interface TermLengthOrPercent : TermFloatValue
    {

        bool Percentage { get; }

    }

}