using AngleSharp.Dom;
using Ardalis.SmartEnum;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StyleParserCS.css
{
    /// <summary>
    /// Acts as collection of parsed parts of Selector (Parts)
    /// with extended functionality.
    /// 
    /// Items are defined within this interface.
    /// 
    /// 
    /// @author kapy
    /// @author Jan Svercl, VUT Brno, 2008
    /// </summary>
    public interface Selector : Rule<Selector_SelectorPart>
    {

        /// <summary>
        /// Combinator for simple selectors 
        /// @author kapy
        /// 
        /// </summary>

        /// <summary>
        /// Operator for SelectorPart attributes 
        /// @author kapy
        /// 
        /// </summary>

        /// <summary>
        /// A pseudo-class for @page rules
        /// </summary>

        /// <summary>
        /// A pseudo-class
        /// </summary>

        /// <summary>
        /// A pseudo-element
        /// </summary>

        /// <summary>
        /// Returns combinator of this and other simple selector </summary>
        /// <returns> Combinator </returns>
        Selector_Combinator Combinator { get; }

        /// <summary>
        /// Sets combinator </summary>
        /// <param name="combinator"> Combinator between this and other selector </param>
        /// <returns> Modified instance </returns>
        Selector setCombinator(Selector_Combinator combinator);

        /// <summary>
        /// Name of CSS class which is affected by this selector </summary>
        /// <returns> Name of CSS class </returns>
        string ClassName { get; }

        /// <summary>
        /// ID of CSS item which is affected by this selector </summary>
        /// <returns> ID of CSS item </returns>
        string IDName { get; }

        /// <summary>
        /// Name of HTML element which is affected by this selector </summary>
        /// <returns> Name of HTML element </returns>
        string ElementName { get; }

        /// <summary>
        /// Reads the pseudo-element of the selector </summary>
        /// <returns> the used pseudo-element or <code>null</code> if no pseudo-element is specified </returns>
        Selector_PseudoElementType PseudoElementType { get; }

        /// <summary>
        /// Checks where the specified pseudo-class is in this selector </summary>
        /// <param name="pct"> </param>
        /// <returns> <code>true</code> if the selector has the specified pseudo-class </returns>
        //ORIGINAL LINE: public boolean hasPseudoClass(final Selector_PseudoClassType pct);
        bool hasPseudoClass(Selector_PseudoClassType pct);

        /// <summary>
        /// Modifies specificity according to CSS standard </summary>
        /// <param name="spec"> Specificity to be modified </param>
        void computeSpecificity(CombinedSelector_Specificity spec);

        /// <summary>
        /// Matches simple selector against DOM element using the default element matcher
        /// and the default match condition registered in CSSFactory. </summary>
        /// <param name="e"> IElement </param>
        /// <returns> <code>true</code> in case of match </returns>
        bool matches(IElement e);

        /// <summary>
        /// Matches simple selector against DOM element with an additional condition </summary>
        /// <param name="e"> IElement </param>
        /// <param name="matcher"> IElement matcher to be used </param>
        /// <param name="cond"> An additional condition to be applied </param>
        /// <returns> <code>true</code> in case of match </returns>
        bool matches(IElement e, ElementMatcher matcher, MatchCondition cond);

        /// <summary>
        /// Interface for handling items
        /// @author kapy
        /// 
        /// </summary>

        /// <summary>
        /// IElement name
        /// @author kapy
        /// 
        /// </summary>

        /// <summary>
        /// IElement attribute
        /// @author kapy
        /// 
        /// </summary>

        /// <summary>
        /// IElement class
        /// @author kapy
        /// 
        /// </summary>

        /// <summary>
        /// IElement id
        /// @author kapy
        /// 
        /// </summary>
    }

    public sealed class Selector_Combinator : SmartEnum<Selector_Combinator, string>
    {
        public static readonly Selector_Combinator DESCENDANT = new Selector_Combinator("DESCENDANT", " ");
        public static readonly Selector_Combinator ADJACENT = new Selector_Combinator("ADJACENT", "+");
        public static readonly Selector_Combinator PRECEDING = new Selector_Combinator("PRECEDING", "~");
        public static readonly Selector_Combinator CHILD = new Selector_Combinator("CHILD", ">");
        
        internal Selector_Combinator(string name, string value) : base(name, value)
        {
        }

        public string value()
        {
            return Value;
        }
        public override string ToString()
        {
            return Name;
        }
    }

    public sealed class Selector_Operator : SmartEnum<Selector_Operator, string>
    {
        public static readonly Selector_Operator EQUALS = new Selector_Operator("EQUALS", "=");
        public static readonly Selector_Operator INCLUDES = new Selector_Operator("INCLUDES", "~=");
        public static readonly Selector_Operator DASHMATCH = new Selector_Operator("DASHMATCH", "|=");
        public static readonly Selector_Operator CONTAINS = new Selector_Operator("CONTAINS", "*=");
        public static readonly Selector_Operator STARTSWITH = new Selector_Operator("STARTSWITH", "^=");
        public static readonly Selector_Operator ENDSWITH = new Selector_Operator("ENDSWITH", "$=");
        public static readonly Selector_Operator NO_OPERATOR = new Selector_Operator("NO_OPERATOR", "");

        internal Selector_Operator(string name, string value) : base(name, value)
        {
        }

        public string value()
        {
            return Value;
        }
        public override string ToString()
        {
            return Name;
        }
    }

    public sealed class Selector_PseudoPageType : SmartEnum<Selector_PseudoPageType, string>
    {
        public static readonly Selector_PseudoPageType BLANK = new Selector_PseudoPageType("BLANK", "blank");
        public static readonly Selector_PseudoPageType FIRST = new Selector_PseudoPageType("FIRST", "first");
        public static readonly Selector_PseudoPageType LEFT = new Selector_PseudoPageType("LEFT", "left");
        public static readonly Selector_PseudoPageType RIGHT = new Selector_PseudoPageType("RIGHT", "right");
        public static readonly Selector_PseudoPageType vendor = new Selector_PseudoPageType("vendor", ""); // Vendor-prefixed

        internal static readonly IDictionary<string, Selector_PseudoPageType> lookup = new ConcurrentDictionary<string, Selector_PseudoPageType>();

        internal Selector_PseudoPageType(string name, string namev) : base(name, namev)
        {
        }
        
        // TOCHECK: dont work        
        public static Selector_PseudoPageType ForNameLookup(string name)
        {
            if (string.ReferenceEquals(name, null))
            {
                return null;
            }
            if (name.StartsWith("-", StringComparison.Ordinal) || name.StartsWith("_", StringComparison.Ordinal))
            {
                return vendor;
            }
            if (lookup.Count == 0)
            {
                foreach (Selector_PseudoPageType type in List)
                {
                    if (!string.ReferenceEquals(type.Name, null))
                    {
                        lookup.Add(type.Name, type);
                    }
                }
            }
            return lookup[name.ToLower()];
        }
        

        public override string ToString()
        {
            return Name;
        }
    }

    public sealed class Selector_PseudoClassType : SmartEnum<Selector_PseudoClassType, string>
    {
        public static readonly Selector_PseudoClassType ACTIVE = new Selector_PseudoClassType("ACTIVE", "active");
        public static readonly Selector_PseudoClassType ANY = new Selector_PseudoClassType("ANY", "any");
        public static readonly Selector_PseudoClassType ANY_LINK = new Selector_PseudoClassType("ANY_LINK", "any-link");
        public static readonly Selector_PseudoClassType CHECKED = new Selector_PseudoClassType("CHECKED", "checked");
        public static readonly Selector_PseudoClassType DEFAULT = new Selector_PseudoClassType("DEFAULT", "default");
        public static readonly Selector_PseudoClassType DEFINED = new Selector_PseudoClassType("DEFINED", "defined");
        public static readonly Selector_PseudoClassType DIR = new Selector_PseudoClassType("DIR", "dir");
        public static readonly Selector_PseudoClassType DISABLED = new Selector_PseudoClassType("DISABLED", "disabled");
        public static readonly Selector_PseudoClassType EMPTY = new Selector_PseudoClassType("EMPTY", "empty");
        public static readonly Selector_PseudoClassType ENABLED = new Selector_PseudoClassType("ENABLED", "enabled");
        public static readonly Selector_PseudoClassType FIRST_CHILD = new Selector_PseudoClassType("FIRST_CHILD", "first-child");
        public static readonly Selector_PseudoClassType FIRST_OF_TYPE = new Selector_PseudoClassType("FIRST_OF_TYPE", "first-of-type");
        public static readonly Selector_PseudoClassType FULLSCREEN = new Selector_PseudoClassType("FULLSCREEN", "fullscreen");
        public static readonly Selector_PseudoClassType FOCUS = new Selector_PseudoClassType("FOCUS", "focus");
        public static readonly Selector_PseudoClassType FOCUS_WITHIN = new Selector_PseudoClassType("FOCUS_WITHIN", "focus-within");
        public static readonly Selector_PseudoClassType HAS = new Selector_PseudoClassType("HAS", "has");
        public static readonly Selector_PseudoClassType HOVER = new Selector_PseudoClassType("HOVER", "hover");
        public static readonly Selector_PseudoClassType INDETERMINATE = new Selector_PseudoClassType("INDETERMINATE", "indeterminate");
        public static readonly Selector_PseudoClassType IN_RANGE = new Selector_PseudoClassType("IN_RANGE", "in-range");
        public static readonly Selector_PseudoClassType INVALID = new Selector_PseudoClassType("INVALID", "invalid");
        public static readonly Selector_PseudoClassType LANG = new Selector_PseudoClassType("LANG", "lang");
        public static readonly Selector_PseudoClassType LAST_CHILD = new Selector_PseudoClassType("LAST_CHILD", "last-child");
        public static readonly Selector_PseudoClassType LAST_OF_TYPE = new Selector_PseudoClassType("LAST_OF_TYPE", "last-of-type");
        public static readonly Selector_PseudoClassType LINK = new Selector_PseudoClassType("LINK", "link");
        public static readonly Selector_PseudoClassType NOT = new Selector_PseudoClassType("NOT", "not");
        public static readonly Selector_PseudoClassType NTH_CHILD = new Selector_PseudoClassType("NTH_CHILD", "nth-child");
        public static readonly Selector_PseudoClassType NTH_LAST_CHILD = new Selector_PseudoClassType("NTH_LAST_CHILD", "nth-last-child");
        public static readonly Selector_PseudoClassType NTH_LAST_OF_TYPE = new Selector_PseudoClassType("NTH_LAST_OF_TYPE", "nth-last-of-type");
        public static readonly Selector_PseudoClassType NTH_OF_TYPE = new Selector_PseudoClassType("NTH_OF_TYPE", "nth-of-type");
        public static readonly Selector_PseudoClassType ONLY_CHILD = new Selector_PseudoClassType("ONLY_CHILD", "only-child");
        public static readonly Selector_PseudoClassType ONLY_OF_TYPE = new Selector_PseudoClassType("ONLY_OF_TYPE", "only-of-type");
        public static readonly Selector_PseudoClassType OPTIONAL = new Selector_PseudoClassType("OPTIONAL", "optional");
        public static readonly Selector_PseudoClassType OUT_OF_RANGE = new Selector_PseudoClassType("OUT_OF_RANGE", "out-of-range");
        public static readonly Selector_PseudoClassType PLACEHOLDER_SHOWN = new Selector_PseudoClassType("PLACEHOLDER_SHOWN", "placeholder-shown");
        public static readonly Selector_PseudoClassType READ_ONLY = new Selector_PseudoClassType("READ_ONLY", "read-only");
        public static readonly Selector_PseudoClassType READ_WRITE = new Selector_PseudoClassType("READ_WRITE", "read-write");
        public static readonly Selector_PseudoClassType REQUIRED = new Selector_PseudoClassType("REQUIRED", "required");
        public static readonly Selector_PseudoClassType ROOT = new Selector_PseudoClassType("ROOT", "root");
        public static readonly Selector_PseudoClassType SCOPE = new Selector_PseudoClassType("SCOPE", "scope");
        public static readonly Selector_PseudoClassType TARGET = new Selector_PseudoClassType("TARGET", "target");
        public static readonly Selector_PseudoClassType VALID = new Selector_PseudoClassType("VALID", "valid");
        public static readonly Selector_PseudoClassType VISITED = new Selector_PseudoClassType("VISITED", "visited");
        public static readonly Selector_PseudoClassType vendor = new Selector_PseudoClassType("vendor", ""); // Vendor-prefixed

        static readonly IDictionary<string, Selector_PseudoClassType> lookup = 
                new ConcurrentDictionary<string, Selector_PseudoClassType>();

        /*
        static readonly Lazy<Dictionary<string, TEnum>> _fromName =
            new Lazy<Dictionary<string, TEnum>>(() => _enumOptions.Value.ToDictionary(item => item.Name));
        */
        /*
        static readonly Lazy<IDictionary<string, Selector_PseudoClassType>> lookup = 
            new Lazy<IDictionary<string, Selector_PseudoClassType>>(() =>
                new ConcurrentDictionary<string, Selector_PseudoClassType>(List
                    .Where(x => x.Name != null)
                    .ToDictionary(x => x.Name, x => x)));
        */

        internal Selector_PseudoClassType(string name,  string namev) : base(name, namev)
        {
        }

        // TOCHECK: dont work        
        public static Selector_PseudoClassType ForNameLookup(string name)
        {            
            if (string.ReferenceEquals(name, null))
            {
                return null;
            }
            if (name.StartsWith("-", StringComparison.Ordinal) || name.StartsWith("_", StringComparison.Ordinal))
            {
                return vendor;
            }
            if (lookup.Count == 0)
            {
                foreach (Selector_PseudoClassType type in List)
                {
                    if (!string.ReferenceEquals(type.Name, null))
                    {
                        lookup.Add(type.Name, type);
                    }
                }
            }
            // TOCHECK: dictionary tryget???!?!?
            return lookup[name.ToUpper()];
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public sealed class Selector_PseudoElementType : SmartEnum<Selector_PseudoElementType, string>
    {
        public static readonly Selector_PseudoElementType FIRST_LINE = new Selector_PseudoElementType("FIRST_LINE", "first-line");
        public static readonly Selector_PseudoElementType FIRST_LETTER = new Selector_PseudoElementType("FIRST_LETTER", "first-letter");
        public static readonly Selector_PseudoElementType BEFORE = new Selector_PseudoElementType("BEFORE", "before");
        public static readonly Selector_PseudoElementType AFTER = new Selector_PseudoElementType("AFTER", "after");
        public static readonly Selector_PseudoElementType BACKDROP = new Selector_PseudoElementType("BACKDROP", "backdrop");
        public static readonly Selector_PseudoElementType CUE = new Selector_PseudoElementType("CUE", "cue");
        public static readonly Selector_PseudoElementType GRAMMAR_ERROR = new Selector_PseudoElementType("GRAMMAR_ERROR", "grammar-error");
        public static readonly Selector_PseudoElementType PLACEHOLDER = new Selector_PseudoElementType("PLACEHOLDER", "placeholder");
        public static readonly Selector_PseudoElementType SELECTION = new Selector_PseudoElementType("SELECTION", "selection");
        public static readonly Selector_PseudoElementType SPELLING_ERROR = new Selector_PseudoElementType("SPELLING_ERROR", "spelling-error");
        public static readonly Selector_PseudoElementType vendor = new Selector_PseudoElementType("vendor", ""); // Vendor-prefixed
        internal static readonly IDictionary<string, Selector_PseudoElementType> lookup = new ConcurrentDictionary<string, Selector_PseudoElementType>();

        internal Selector_PseudoElementType(string name, string namev) : base(name, namev)
        {
        }

        // TOCHECK: dont work        
        public static Selector_PseudoElementType ForNameLookup(string name)
        {
            if (string.ReferenceEquals(name, null))
            {
                return null;
            }
            if (name.StartsWith("-", StringComparison.Ordinal) || name.StartsWith("_", StringComparison.Ordinal))
            {
                return vendor;
            }
            if (lookup.Count == 0)
            {
                foreach (Selector_PseudoElementType type in List)
                {
                    if (!string.ReferenceEquals(type.Name, null))
                    {
                        lookup.Add(type.Name, type);
                    }
                }
            }
            // TOCHECK: https://stackoverflow.com/questions/1273139/c-sharp-java-hashmap-equivalent
            return lookup[name.ToUpper()];
        }
        

        public override string ToString()
        {
            return Name;
        }
    }

    public interface Selector_SelectorPart
    {
        bool matches(IElement e, ElementMatcher matcher, MatchCondition cond);
        void computeSpecificity(CombinedSelector_Specificity spec);
    }

    public interface Selector_ElementName : Selector_SelectorPart
    {
        string Name { get; }
        Selector_ElementName setName(string name);
    }

    public static class Selector_ElementName_Fields
    {
        public const string WILDCARD = "*";
    }

    public interface Selector_ElementAttribute : Selector_SelectorPart
    {

        string Attribute { get; }
        Selector_ElementAttribute setAttribute(string attribute);

        string Value { get; }
        Selector_ElementAttribute setValue(string value);

        Selector_Operator Operator { get; set; }
    }

    public interface Selector_ElementClass : Selector_SelectorPart
    {
        string ClassName { get; }
        Selector_ElementClass setClassName(string name);
    }

    public interface Selector_ElementID : Selector_SelectorPart
    {
        string ID { get; }
        Selector_ElementID setID(string id);
    }

    public interface Selector_ElementDOM : Selector_SelectorPart
    {
        IElement IElement { get; }
        Selector_ElementDOM setElement(IElement e);
    }

    public interface Selector_PseudoPage : Selector_SelectorPart
    {
        string Name { get; }
        Selector_PseudoPageType Type { get; }
    }

    public interface Selector_PseudoClass : Selector_SelectorPart
    {
        string Name { get; }
        string FunctionValue { get; }
        Selector_PseudoClassType Type { get; }
        Selector NestedSelector { get; }
    }

    public interface Selector_PseudoElement : Selector_SelectorPart
    {
        string Name { get; }
        string FunctionValue { get; }
        Selector_PseudoElementType Type { get; }
        Selector NestedSelector { get; }
    }

}