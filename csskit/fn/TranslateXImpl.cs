using System.Collections.Generic;

namespace StyleParserCS.csskit.fn
{

    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermLengthOrPercent = StyleParserCS.css.TermLengthOrPercent;
    using TermList = StyleParserCS.css.TermList;

    public class TranslateXImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_TranslateX
    {

        private TermLengthOrPercent translate;

        public TranslateXImpl()
        {
            Valid = false; //arguments are required
        }

        public virtual TermLengthOrPercent Translate
        {
            get
            {
                return translate;
            }
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            if (args != null && args.Count == 1 && (translate = getLengthOrPercentArg(args[0])) != null)
            {
                Valid = true;
            }
            return this;
        }
    }
}