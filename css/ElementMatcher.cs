using AngleSharp.Dom;
using System.Collections.Generic;

/// <summary>
/// ElementMatcher.java
/// 
/// Created on 25. 11. 2015, 13:32:58 by burgetr
/// </summary>
namespace StyleParserCS.css
{
    /// <summary>
    /// IElement matcher provides an abstraction of the DOM access and the element matching
    /// based on their names, classes and IDs. The implementation may be adjusted according
    /// to the used DOM implementation. Different implementations may be case sensitive
    /// or insensitive depending on the DOM source (e.g. HTML vs. XML).
    /// 
    /// @author burgetr
    /// </summary>
    public interface ElementMatcher
    {
        /// <summary>
        /// Obtains the value of an element attribute. </summary>
        /// <param name="e"> The DOM element. </param>
        /// <param name="name"> Attribute name. </param>
        /// <returns> The value of the given attribute or an empty string when the attribute
        /// is not present. </returns>
        string getAttribute(IElement e, string name);

        /// <summary>
        /// Obtains a collection of class names assigned to the given element
        /// using its {@code class} attribute. </summary>
        /// <param name="e"> The DOM element </param>
        /// <returns> the list of class names (possibly empty). </returns>
        ICollection<string> elementClasses(IElement e);

        /// <summary>
        /// Checks whether the given element has the given class assigned. </summary>
        /// <param name="e"> The DOM element </param>
        /// <param name="className"> the class name to check </param>
        /// <returns> {@code true} when any of the element classes matches the given class. </returns>
        bool matchesClass(IElement e, string className);

        /// <summary>
        /// Obtains the ID of the given element when specified. </summary>
        /// <param name="e"> The DOM element </param>
        /// <returns> The element ID or an empty string when not specified. </returns>
        string elementID(IElement e);

        /// <summary>
        /// Checks whether the given element has the given ID assigned. </summary>
        /// <param name="e"> The DOM element </param>
        /// <param name="id"> the class name to check </param>
        /// <returns> {@code true} when the element has the given ID. </returns>
        bool matchesID(IElement e, string id);

        /// <summary>
        /// Obtains the name of the given element. </summary>
        /// <param name="e"> The DOM element </param>
        /// <returns> The element name. </returns>
        string elementName(IElement e);

        /// <summary>
        /// Checks whether the given element has the given name. </summary>
        /// <param name="e"> The DOM element </param>
        /// <param name="name"> the element name to check </param>
        /// <returns> {@code true} when the element has the given name. </returns>
        bool matchesName(IElement e, string name);

        /// <summary>
        /// Checks whether the given element attribute has the given value. </summary>
        /// <param name="e"> The DOM element </param>
        /// <param name="name"> the attribute name </param>
        /// <param name="value"> the attribute value </param>
        /// <returns> {@code true} when the given attribute of the element has the given value. </returns>
        bool matchesAttribute(IElement e, string name, string value, Selector_Operator o);

    }

}