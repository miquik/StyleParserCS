using AngleSharp.Dom;
using System;

/// <summary>
/// MatchCondition.java
/// 
/// Created on 1.7.2013, 11:03:32 by burgetr
/// </summary>
namespace StyleParserCS.css
{
    /// <summary>
    /// An additional condition for matching the selectors.
    /// 
    /// @author burgetr
    /// </summary>
    public interface MatchCondition : ICloneable
    {

        /// <summary>
        /// Checks whether the condition is satisfied for the given element and selector part. </summary>
        /// <param name="e"> The element to be tested. </param>
        /// <param name="selpart"> The selector part. </param>
        /// <returns> <code>true</code> when the condition is satisfied </returns>
        bool isSatisfied(IElement e, Selector_SelectorPart selpart);

    }

}