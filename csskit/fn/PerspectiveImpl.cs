using System.Collections.Generic;

namespace StyleParserCS.csskit.fn
{

    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermLength = StyleParserCS.css.TermLength;
    using TermList = StyleParserCS.css.TermList;

    public class PerspectiveImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_Perspective
    {

        private TermLength distance;

        public PerspectiveImpl()
        {
            Valid = false; //arguments are required
        }

        public virtual TermLength Distance
        {
            get
            {
                return distance;
            }
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            if (args != null && args.Count == 1 && (distance = getLengthArg(args[0])) != null)
            {
                Valid = true;
            }
            return this;
        }
    }
}