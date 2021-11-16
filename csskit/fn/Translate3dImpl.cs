using System.Collections.Generic;

namespace StyleParserCS.csskit.fn
{

    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermLengthOrPercent = StyleParserCS.css.TermLengthOrPercent;
    using TermList = StyleParserCS.css.TermList;

    public class Translate3dImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_Translate3d
    {

        private TermLengthOrPercent translateX;
        private TermLengthOrPercent translateY;
        private TermLengthOrPercent translateZ;

        public Translate3dImpl()
        {
            Valid = false; //arguments are required
        }

        public virtual TermLengthOrPercent TranslateX
        {
            get
            {
                return translateX;
            }
        }

        public virtual TermLengthOrPercent TranslateY
        {
            get
            {
                return translateY;
            }
        }

        public virtual TermLengthOrPercent TranslateZ
        {
            get
            {
                return translateZ;
            }
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            if (args != null && args.Count == 3 && (translateX = getLengthOrPercentArg(args[0])) != null && (translateY = getLengthOrPercentArg(args[1])) != null && (translateZ = getLengthOrPercentArg(args[2])) != null)
            {
                Valid = true;
            }
            return this;
        }
    }
}