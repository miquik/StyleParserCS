using System.Collections.Generic;

namespace StyleParserCS.csskit.fn
{

    using StyleParserCS.css;
    using TermAngle = StyleParserCS.css.TermAngle;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermList = StyleParserCS.css.TermList;

    public class Rotate3dImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_Rotate3d
    {

        private float x;
        private float y;
        private float z;
        private TermAngle angle;

        public Rotate3dImpl()
        {
            Valid = false; //arguments are required
        }

        public virtual float X
        {
            get
            {
                return x;
            }
        }

        public virtual float Y
        {
            get
            {
                return y;
            }
        }


        public virtual float Z
        {
            get
            {
                return z;
            }
        }

        public virtual TermAngle Angle
        {
            get
            {
                return angle;
            }
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            if (args != null && args.Count == 4 && isNumberArg(args[0]) && isNumberArg(args[1]) && isNumberArg(args[2]) && (angle = getAngleArg(args[3])) != null)
            {
                x = getNumberArg(args[0]);
                y = getNumberArg(args[1]);
                z = getNumberArg(args[2]);
                Valid = true;
            }
            return this;
        }
    }
}