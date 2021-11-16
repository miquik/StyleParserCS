using System.Collections.Generic;

namespace StyleParserCS.csskit.fn
{

    using StyleParserCS.css;
    using TermAngle = StyleParserCS.css.TermAngle;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermList = StyleParserCS.css.TermList;

    public class RotateYImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_RotateY
    {

        private TermAngle angle;

        public RotateYImpl()
        {
            Valid = false; //arguments are required
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
            if (args != null && args.Count == 1 && (angle = getAngleArg(args[0])) != null)
            {
                Valid = true;
            }
            return this;
        }
    }
}