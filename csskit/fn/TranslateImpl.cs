using System.Collections.Generic;

namespace StyleParserCS.csskit.fn
{

    using CSSFactory = StyleParserCS.css.CSSFactory;
    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermLengthOrPercent = StyleParserCS.css.TermLengthOrPercent;
    using TermList = StyleParserCS.css.TermList;

    public class TranslateImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_Translate
    {

        private TermLengthOrPercent translateX;
        private TermLengthOrPercent translateY;

        public TranslateImpl()
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

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            if (args != null)
            {
                if (args.Count == 2 && (translateX = getLengthOrPercentArg(args[0])) != null && (translateY = getLengthOrPercentArg(args[1])) != null)
                {
                    Valid = true;
                }
                else if (Count == 1 && (translateX = getLengthOrPercentArg(args[0])) != null)
                {
                    translateY = CSSFactory.TermFactory.createLength(0.0f);
                    Valid = true;
                }
            }
            return this;
        }
    }
}