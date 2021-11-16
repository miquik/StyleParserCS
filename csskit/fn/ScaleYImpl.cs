using System.Collections.Generic;

namespace StyleParserCS.csskit.fn
{

    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermList = StyleParserCS.css.TermList;

    public class ScaleYImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_ScaleY
    {

        private float scale;

        public ScaleYImpl()
        {
            Valid = false; //arguments are required
        }

        public virtual float Scale
        {
            get
            {
                return scale;
            }
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            if (args != null && args.Count == 1 && isNumberArg(args[0]))
            {
                scale = getNumberArg(args[0]);
                Valid = true;
            }
            return this;
        }
    }
}