using AngleSharp.Dom;
using StyleParserCS.utils;
using System;
using System.Collections.Generic;

/// <summary>
/// DeclarationMap.java
/// 
/// Created on 22.1.2010, 16:23:07 by burgetr
/// </summary>
namespace StyleParserCS.domassign
{
    using Declaration = StyleParserCS.css.Declaration;
    using Selector_PseudoElementType = StyleParserCS.css.Selector_PseudoElementType;

    /// <summary>
    /// This is a map that assigns a sorted list of declarations to each element 
    /// and an optional pseudo-element. 
    /// 
    /// @author burgetr
    /// </summary>
    public class DeclarationMap : MultiMap<IElement, Selector_PseudoElementType, IList<Declaration>>
    {

        /// <summary>
        /// Adds a declaration for a specified list. If the list does not exist yet, it is created. </summary>
        /// <param name="el"> the element that the declaration belongs to </param>
        /// <param name="pseudo"> an optional pseudo-element or null </param>
        /// <param name="decl"> the new declaration </param>
        public virtual void addDeclaration(IElement el, Selector_PseudoElementType pseudo, Declaration decl)
        {
            IList<Declaration> list = getOrCreate(el, pseudo);
            list.Add(decl);
        }

        /// <summary>
        /// Sorts the given list according to the rule specificity. </summary>
        /// <param name="el"> the element to which the list is assigned </param>
        /// <param name="pseudo"> an optional pseudo-element or null </param>
        public virtual void sortDeclarations(IElement el, Selector_PseudoElementType pseudo)
        {
            IList<Declaration> list = get(el, pseudo);
            if (list != null)
            {
                list.Sort();
            }
        }

        protected internal override IList<Declaration> createDataInstance()
        {
            return new List<Declaration>();
        }


    }


}