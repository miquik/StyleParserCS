using System.Collections.Generic;

/// <summary>
/// GenericFilterFunction.java
/// 
/// Created on 18. 5. 2018, 12:37:05 by burgetr
/// </summary>
namespace StyleParserCS.csskit.fn
{

    using StyleParserCS.css;
    using TermList = StyleParserCS.css.TermList;
    using TermPercent = StyleParserCS.css.TermPercent;

    /// 
    /// <summary>
    /// @author burgetr
    /// </summary>
    public class GenericFilterFunction : TermFunctionImpl
    {

        private float amount;

        public GenericFilterFunction()
        {
            Valid = false;
        }

        public virtual float Amount
        {
            get
            {
                return amount;
            }
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            if (args != null && args.Count == 1)
            {
                //ORIGINAL LINE: final StyleParserCS.css.Term<?> arg = args.get(0);
                Term arg = args[0];
                if (isNumberArg(arg))
                {
                    amount = getNumberArg(args[0]);
                    Valid = true;
                }
                else if (arg is TermPercent)
                {
                    amount = ((TermPercent)arg).Value / 100.0f;
                    Valid = true;
                }
            }
            return this;
        }

    }

}