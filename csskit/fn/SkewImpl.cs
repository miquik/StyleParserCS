using System.Collections.Generic;

namespace StyleParserCS.csskit.fn
{

    using CSSFactory = StyleParserCS.css.CSSFactory;
    using StyleParserCS.css;
    using TermAngle = StyleParserCS.css.TermAngle;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermList = StyleParserCS.css.TermList;

    public class SkewImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_Skew
    {

        private TermAngle skewX;
        private TermAngle skewY;

        public SkewImpl()
        {
            Valid = false; //arguments are required
        }

        public virtual TermAngle SkewX
        {
            get
            {
                return skewX;
            }
        }

        public virtual TermAngle SkewY
        {
            get
            {
                return skewY;
            }
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            if (args != null)
            {
                if (args.Count == 2 && (skewX = getAngleArg(args[0])) != null && (skewY = getAngleArg(args[1])) != null)
                {
                    Valid = true;
                }
                else if (Count == 1 && (skewX = getAngleArg(args[0])) != null)
                {
                    skewY = CSSFactory.TermFactory.createAngle(0.0f);
                    Valid = true;
                }
            }
            return this;
        }
    }
}