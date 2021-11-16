using AngleSharp.Dom;
using StyleParserCS.css;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// ElementMatcherSimpleCI.java
/// 
/// Created on 25. 11. 2015, 15:29:26 by burgetr
/// </summary>
namespace StyleParserCS.csskit
{
    using ElementMatcher = StyleParserCS.css.ElementMatcher;
    using Selector = StyleParserCS.css.Selector;

    /// <summary>
    /// A case-insensitive matcher that corresponds to the HTML quirks mode matching. 
    /// 
    /// This is a simplified implementation of the element matcher. This implementation requires
    /// that the {@code IElement.getAttribute()} method provided by the DOM implementation returns
    /// an empty string (not {@code null}) when the attribute is not defined. 
    /// 
    /// @author burgetr
    /// </summary>
    public class ElementMatcherSimpleCI : ElementMatcher
    {
        public const string CLASS_DELIM = " ";
        public const string CLASS_ATTR = "class";
        public const string ID_ATTR = "id";

        //ORIGINAL LINE: public String getAttribute(final org.w3c.dom.IElement e, final String name)
        public virtual string getAttribute(IElement e, string name)
        {
            return e.GetAttribute(name);
        }

        //ORIGINAL LINE: public java.util.Collection<String> elementClasses(final org.w3c.dom.IElement e)
        public virtual ICollection<string> elementClasses(IElement e)
        {
            //ORIGINAL LINE: final String classNames = e.getAttribute(CLASS_ATTR);
            string classNames = e.GetAttribute(CLASS_ATTR);
            if (classNames.Length > 0)
            {
                ICollection<string> list = new List<string>();
                foreach (string cname in classNames.ToLower().Split(CLASS_DELIM[0]))
                {
                    string cnames = cname.Trim();
                    if (cnames.Length > 0)
                    {
                        list.Add(cnames);
                    }
                }
                return list;
            }
            else
            {
                return new List<string>();
            }
        }

        //ORIGINAL LINE: public boolean matchesClass(final org.w3c.dom.IElement e, final String className)
        public virtual bool matchesClass(IElement e, string className)
        {
            //ORIGINAL LINE: final String classNames = e.getAttribute(CLASS_ATTR).toLowerCase();
            string classNames = e.GetAttribute(CLASS_ATTR).ToLower();
            if (classNames.Length > 0)
            {
                //ORIGINAL LINE: final String search = className.toLowerCase();
                string search = className.ToLower();
                //ORIGINAL LINE: final int len = className.length();
                int len = className.Length;
                int lastIndex = 0;

                while ((lastIndex = classNames.IndexOf(search, lastIndex, StringComparison.Ordinal)) != -1)
                {
                    if ((lastIndex == 0 || char.IsWhiteSpace(classNames[lastIndex - 1])) && (lastIndex + len == classNames.Length || char.IsWhiteSpace(classNames[lastIndex + len])))
                    {
                        return true;
                    }
                    lastIndex += len;
                }
                return false;
            }
            else
            {
                return false;
            }
        }


        //ORIGINAL LINE: public String elementID(final org.w3c.dom.IElement e)
        public virtual string elementID(IElement e)
        {
            return e.GetAttribute(ID_ATTR);
        }

        //ORIGINAL LINE: public boolean matchesID(final org.w3c.dom.IElement e, final String id)
        public virtual bool matchesID(IElement e, string id)
        {
            return id.Equals(e.GetAttribute(ID_ATTR), StringComparison.OrdinalIgnoreCase);
        }

        //ORIGINAL LINE: public String elementName(final org.w3c.dom.IElement e)
        public virtual string elementName(IElement e)
        {
            return e.NodeName;
        }

        //ORIGINAL LINE: public boolean matchesName(final org.w3c.dom.IElement e, final String name)
        public virtual bool matchesName(IElement e, string name)
        {
            return name.Equals(e.NodeName, StringComparison.OrdinalIgnoreCase);
        }

        //ORIGINAL LINE: public boolean matchesAttribute(final org.w3c.dom.IElement e, final String name, final String value, final StyleParserCS.css.Selector_Operator o)
        public virtual bool matchesAttribute(IElement e, string name, string value, StyleParserCS.css.Selector_Operator o)
        {
            //ORIGINAL LINE: final org.w3c.dom.Node attributeNode = e.getAttributeNode(name);
            INode attributeNode = e.Attributes[name];
            if (attributeNode != null && o != null)
            {
                string attributeValue = attributeNode.NodeValue;

                switch (o.Name)
                {
                    case nameof(Selector_Operator.EQUALS):
                        return attributeValue.Equals(value);
                    case nameof(Selector_Operator.INCLUDES):
                        if (value.Length == 0 || containsWhitespace(value))
                        {
                            return false;
                        }
                        else
                        {
                            attributeValue = " " + attributeValue + " ";
                            return Regex.IsMatch(attributeValue, ".* " + value + " .*");
                        }
                    case nameof(Selector_Operator.DASHMATCH):
                        return Regex.IsMatch(attributeValue, "^" + value + "(-.*|$)");
                    case nameof(Selector_Operator.CONTAINS):
                        return value.Length > 0 && Regex.IsMatch(attributeValue, ".*" + value + ".*");
                    case nameof(Selector_Operator.STARTSWITH):
                        return value.Length > 0 && Regex.IsMatch(attributeValue, "^" + value + ".*");
                    case nameof(Selector_Operator.ENDSWITH):
                        return value.Length > 0 && Regex.IsMatch(attributeValue, ".*" + value + "$");
                    default:
                        return true;
                }
            }
            else
            {
                return false;
            }
        }

        private static bool containsWhitespace(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsWhiteSpace(s[i]))
                {
                    return true;
                }
            }
            return false;
        }

    }

}