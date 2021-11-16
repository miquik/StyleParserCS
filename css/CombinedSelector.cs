using System;

namespace StyleParserCS.css
{
    /// <summary>
    /// CombinedSelector of CSS declaration block.
    /// Acts as collection of Selectors with ability to get directly 
    /// the last one. 
    /// 
    /// Computes specificity of selector.
    /// 
    /// @author kapy
    /// @author Jan Svercl, VUT Brno, 2008
    /// </summary>
    public interface CombinedSelector : Rule<Selector>
    {

        /// <summary>
        /// Gets last Selector stored in list,
        /// so its values are easily read </summary>
        /// <returns> Last Selector or null, </returns>
        /// <exception cref="UnsupportedOperationException"> In case that there is no simple selector inside </exception>
        //ORIGINAL LINE: public Selector getLastSelector() throws UnsupportedOperationException;
        Selector LastSelector { get; }

        /// <summary>
        /// Reads the pseudo element of the last simple selector as defined in the CSS specs </summary>
        /// <returns> the pseudo-element or null if none is specified </returns>
        Selector_PseudoElementType PseudoElementType { get; }

        /// <summary>
        /// Computes specificity according to the CSS rules </summary>
        /// <returns> the computed specificity </returns>
        CombinedSelector_Specificity computeSpecificity();

        /// <summary>
        /// Specificity of given selector
        /// @author kapy
        /// 
        /// </summary>


    }

    public interface CombinedSelector_Specificity : IComparable<CombinedSelector_Specificity>
    {

        /// <summary>
        /// Specificity levels
        /// @author kapy
        /// 
        /// </summary>
        // int compareTo(CombinedSelector_Specificity o);

        /// <summary>
        /// Gets specificity of level </summary>
        /// <param name="level"> Specificity level </param>
        /// <returns> Numerical value of specificity </returns>
        int get(CombinedSelector_Specificity_Level level);

        /// <summary>
        /// Adds one to specificity level </summary>
        /// <param name="level"> </param>
        void add(CombinedSelector_Specificity_Level level);
    }

    public enum CombinedSelector_Specificity_Level
    {
        A,
        B,
        C,
        D

        /// <summary>
        /// Compare specificities
        /// </summary>
    }

}