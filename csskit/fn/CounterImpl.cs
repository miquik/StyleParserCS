using System.Collections.Generic;

/// 
namespace StyleParserCS.csskit.fn
{

    using CSSProperty_ListStyleType = StyleParserCS.css.CSSProperty_ListStyleType;
    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermIdent = StyleParserCS.css.TermIdent;
    using TermList = StyleParserCS.css.TermList;

    /// <summary>
    /// @author burgetr
    /// 
    /// </summary>
    public class CounterImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_Counter
    {

        public static IDictionary<string, CSSProperty_ListStyleType> allowedStyles;
        static CounterImpl()
        {
            allowedStyles = new Dictionary<string, CSSProperty_ListStyleType>(CSSProperty_ListStyleType.List.Count - 4);
            foreach (CSSProperty_ListStyleType item in CSSProperty_ListStyleType.List)
            {
                if (item != CSSProperty_ListStyleType.INHERIT && item != CSSProperty_ListStyleType.INITIAL && item != CSSProperty_ListStyleType.UNSET)
                {
                    allowedStyles[item.ToString()] = item;
                }
            }
        }

        private string name;
        private CSSProperty_ListStyleType style;

        public CounterImpl()
        {
            Valid = false;
        }

        public virtual string Name
        {
            get
            {
                return name;
            }
        }

        public virtual CSSProperty_ListStyleType Style
        {
            get
            {
                return style;
            }
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, true);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, true);
            if (args != null && (args.Count == 1 || args.Count == 2))
            {
                //check for name
                if (args[0] is TermIdent)
                {
                    name = ((TermIdent)args[0]).Value;
                    Valid = true;
                }
                //an optional style
                if (args.Count == 2)
                {
                    if (args[1] is TermIdent)
                    {
                        //ORIGINAL LINE: final String styleString = ((StyleParserCS.css.TermIdent) args.get(1)).getValue();
                        string styleString = ((TermIdent)args[1]).Value;
                        style = allowedStyles[styleString.ToLower()];
                        if (style == null)
                        {
                            Valid = false; //unknown style
                        }
                    }
                    else
                    {
                        Valid = false;
                    }
                }
            }
            return this;
        }

    }

}