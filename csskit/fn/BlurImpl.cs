using System.Collections.Generic;

namespace StyleParserCS.csskit.fn
{

    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermLength = StyleParserCS.css.TermLength;
    using TermList = StyleParserCS.css.TermList;

    public class BlurImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_Blur
    {

        private TermLength radius;

        public BlurImpl()
        {
            Valid = false;
        }

        public virtual TermLength Radius
        {
            get
            {
                return radius;
            }
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            if (args != null && args.Count == 1 && (radius = getLengthArg(args[0])) != null)
            {
                Valid = true;
            }
            return this;
        }

    }

}