using System.Collections.Generic;

namespace StyleParserCS.csskit.fn
{

    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermList = StyleParserCS.css.TermList;

    public class Scale3dImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_Scale3d
    {

        private float scaleX;
        private float scaleY;
        private float scaleZ;

        public Scale3dImpl()
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

        public virtual float ScaleZ
        {
            get
            {
                return scaleZ;
            }
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            if (args != null && args.Count == 3 && isNumberArg(args[0]) && isNumberArg(args[1]) && isNumberArg(args[2]))
            {
                scaleX = getNumberArg(args[0]);
                scaleY = getNumberArg(args[1]);
                scaleZ = getNumberArg(args[2]);
                Valid = true;
            }
            return this;
        }
    }
}