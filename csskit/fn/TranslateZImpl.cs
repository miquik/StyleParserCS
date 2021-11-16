using System.Collections.Generic;

namespace StyleParserCS.csskit.fn
{
    using StyleParserCS.css;

    public class TranslateZImpl : TermFunctionImpl, TermFunction_TranslateZ
    {

        private TermLengthOrPercent translate;

        public TranslateZImpl()
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