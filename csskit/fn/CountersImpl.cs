using System.Collections.Generic;

/// 
namespace StyleParserCS.csskit.fn
{

    using CSSProperty_ListStyleType = StyleParserCS.css.CSSProperty_ListStyleType;
    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermIdent = StyleParserCS.css.TermIdent;
    using TermList = StyleParserCS.css.TermList;
    using TermString = StyleParserCS.css.TermString;

    /// <summary>
    /// @author burgetr
    /// 
    /// </summary>
    public class CountersImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_Counters
    {

        private string name;
        private CSSProperty_ListStyleType style;
        private string separator;

        public CountersImpl()
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

        public virtual string Separator
        {
            get
            {
                return separator;
            }
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, true);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, true);
            if (args != null && (args.Count == 2 || args.Count == 3))
            {
                //check for name and separator
                if (args[0] is TermIdent && args[1] is TermString)
                {
                    name = ((TermIdent)args[0]).Value;
                    separator = ((TermString)args[1]).Value;
                    Valid = true;
                }
                //an optional style
                if (args.Count == 3)
                {
                    if (args[2] is TermIdent)
                    {
                        //ORIGINAL LINE: final String styleString = ((StyleParserCS.css.TermIdent) args.get(2)).getValue();
                        string styleString = ((TermIdent)args[2]).Value;
                        style = CounterImpl.allowedStyles[styleString.ToLower()];
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