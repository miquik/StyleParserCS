﻿using AngleSharp.Dom;
using System;

/// <summary>
/// ElementMatcherSafeStd.java
/// 
/// Created on 26. 11. 2015, 15:20:08 by burgetr
/// </summary>
namespace StyleParserCS.csskit
{
    /// <summary>
    /// A matcher that corresponds to the HTML standard mode matching. The matching is case-sensitive
    /// except of element names which are case-insensitive. 
    /// 
    /// This is a safe implementation of the element matcher. It should be compatible with any
    /// DOM implementation. On the other hand, its performance is slightly worse because of some
    /// additional tests required due to the differences among the DOM implementations. 
    /// 
    /// @author burgetr
    /// </summary>
    public class ElementMatcherSafeStd : ElementMatcherSafeCS
    {

        public override bool matchesName(IElement e, string name)
        {
            return name.Equals(elementName(e), StringComparison.OrdinalIgnoreCase);
        }

    }

}