using System.Collections.Generic;

namespace StyleParserCS.csskit.fn
{

    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermList = StyleParserCS.css.TermList;

    public class ScaleImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_Scale
    {

        private float scaleX;
        private float scaleY;

        public ScaleImpl()
        {
            Valid = false; //arguments are required
        }

        public virtual float ScaleX
        {
            get
            {
                return scaleX;
            }
        }

        public virtual float ScaleY
        {
            get
            {
                return scaleY;
            }
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            if (args != null)
            {
                if (args.Count == 2 && isNumberArg(args[0]) && isNumberArg(args[1]))
                {
                    scaleX = getNumberArg(args[0]);
                    scaleY = getNumberArg(args[1]);
                    Valid = true;
                }
                else if (Count == 1 && isNumberArg(args[0]))
                {
                    scaleX = scaleY = getNumberArg(args[0]);
                    Valid = true;
                }
            }
            return this;
        }
    }
}