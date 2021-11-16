using AngleSharp.Dom;
using StyleParserCS.css;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// ElementMatcherSafeCS.java
/// 
/// Created on 25. 11. 2015, 15:17:40 by burgetr
/// </summary>
namespace StyleParserCS.csskit
{
    /// <summary>
    /// A case-sensitive matcher that corresponds to the XHTML mode matching. 
    /// 
    /// This is a safe implementation of the element matcher. It should be compatible with any
    /// DOM implementation. On the other hand, its performance is slightly worse because of some
    /// additional tests required due to the differences among the DOM implementations. 
    /// 
    /// @author burgetr
    /// </summary>
    public class ElementMatcherSafeCS : ElementMatcher
    {
        public const string CLASS_DELIM = " ";
        public const string CLASS_ATTR = "class";
        public const string ID_ATTR = "id";

        public virtual string getAttribute(IElement e, string name)
        {
            //ORIGINAL LINE: final String ret = e.getAttribute(name);
            string ret = e.GetAttribute(name);
            return (string.ReferenceEquals(ret, null)) ? "" : ret;
        }

        public virtual ICollection<string> elementClasses(IElement e)
        {
            if (e.HasAttribute(CLASS_ATTR))
            {
                string classNames = getAttribute(e, CLASS_ATTR);

                ICollection<string> list = new List<string>();
                foreach (string cname in classNames.Split(CLASS_DELIM[0]))
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

        public virtual bool matchesClass(IElement e, string className)
        {
            if (e.HasAttribute(CLASS_ATTR))
            {
                //ORIGINAL LINE: final String classNames = e.getAttribute(CLASS_ATTR);
                string classNames = e.GetAttribute(CLASS_ATTR);
                //ORIGINAL LINE: final int len = className.length();
                int len = className.Length;
                int lastIndex = 0;

                while ((lastIndex = classNames.IndexOf(className, lastIndex, StringComparison.Ordinal)) != -1)
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


        public virtual string elementID(IElement e)
        {
            return getAttribute(e, ID_ATTR);
        }

        public virtual bool matchesID(IElement e, string id)
        {
            return id.Equals(elementID(e));
        }

        public virtual string elementName(IElement e)
        {
            return e.NodeName;
        }

        public virtual bool matchesName(IElement e, string name)
        {
            return name.Equals(elementName(e));
        }

        public virtual bool matchesAttribute(IElement e, string name, string value, StyleParserCS.css.Selector_Operator o)
        {
            //ORIGINAL LINE: final org.w3c.dom.INode attributeNode = e.getAttributeNode(name);
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