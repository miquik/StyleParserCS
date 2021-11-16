using AngleSharp.Dom;
using System;

/// <summary>
/// ElementMatcherSimpleStd.java
/// 
/// Created on 26. 11. 2015, 15:30:15 by burgetr
/// </summary>
namespace StyleParserCS.csskit
{
    /// <summary>
    /// A matcher that corresponds to the HTML standard mode matching. The matching is case-sensitive
    /// except of element names which are case-insensitive. 
    /// 
    /// This is a simplified implementation of the element matcher. This implementation requires
    /// that the {@code Element.getAttribute()} method provided by the DOM implementation returns
    /// an empty string (not {@code null}) when the attribute is not defined. 
    /// 
    /// @author burgetr
    /// </summary>
    public class ElementMatcherSimpleStd : ElementMatcherSimpleCS
    {

        public override bool matchesName(IElement e, string name)
        {
            return name.Equals(elementName(e), StringComparison.OrdinalIgnoreCase);
        }

    }

}