using StyleParserCS.csskit;
/// <summary>
/// TermCalc.java
/// 
/// Created on 17. 2. 2017, 15:52:35 by burgetr
/// </summary>
namespace StyleParserCS.css
{
    /// <summary>
    /// A value of the calc() expression. It may correspond to length, frequency, angle, time, number, or integer.
    /// @author burgetr
    /// </summary>
    public interface TermCalc : TermFloatValue
    {

        /// <summary>
        /// Returns the calc() arguments (in postfix notation). </summary>
        /// <returns> The list of arguments passed to the calc() function in postfix order. </returns>
        CalcArgs Args { get; }

    }

}