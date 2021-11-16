using System.Collections.Generic;

/// 
namespace StyleParserCS.csskit.fn
{

    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermIdent = StyleParserCS.css.TermIdent;
    using TermList = StyleParserCS.css.TermList;

    /// <summary>
    /// @author burgetr
    /// 
    /// </summary>
    public class AttrImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_Attr
    {

        private string name;

        public AttrImpl()
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

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, true);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, true);
            if (args != null && args.Count == 1)
            {
                if (args[0] is TermIdent)
                {
                    name = ((TermIdent)args[0]).Value;
                    Valid = true;
                }
            }
            return this;
        }

    }

}