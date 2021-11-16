using System.Collections.Generic;

namespace StyleParserCS.csskit.fn
{
    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermLengthOrPercent = StyleParserCS.css.TermLengthOrPercent;
    using TermList = StyleParserCS.css.TermList;

    /// 
    /// <summary>
    /// @author Petr Mikulík
    /// </summary>
    public class FitContentImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_FitContent
    {

        private TermLengthOrPercent _max;

        public FitContentImpl()
        {
            Valid = false;
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, true);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, true);
            if (args != null && args.Count == 1)
            {
                _max = getLengthOrPercentArg(args[0]);
                if (_max != null)
                {
                    Valid = true;
                }
            }
            return this;
        }

        public virtual TermLengthOrPercent Maximum
        {
            get
            {
                return _max;
            }
        }

    }

}