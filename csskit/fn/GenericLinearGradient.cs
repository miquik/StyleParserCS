using System.Collections.Generic;

/// <summary>
/// GenericLinearGradient.java
/// 
/// Created on 17. 5. 2018, 13:10:28 by burgetr
/// </summary>
namespace StyleParserCS.csskit.fn
{

    using StyleParserCS.css;
    using TermAngle = StyleParserCS.css.TermAngle;
    using TermList = StyleParserCS.css.TermList;

    /// <summary>
    /// Base class for both the linear and repating-linear gradient implementations.
    /// 
    /// @author burgetr
    /// </summary>
    public class GenericLinearGradient : GenericGradient
    {

        private TermAngle angle;

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
            //ORIGINAL LINE: java.util.List<java.util.List<StyleParserCS.css.Term<?>>> args = getSeparatedArgs((Term)DEFAULT_ARG_SEP);
            IList<IList<Term>> args = getSeparatedArgs((Term)DEFAULT_ARG_SEP);
            if (args.Count > 1)
            {
                int firstStop = 0;
                //check for an angle
                //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> aarg = args.get(0);
                IList<Term> aarg = args[0];
                if (aarg.Count == 1 && (angle = getAngleArg(aarg[0])) != null)
                {
                    firstStop = 1;
                }
                else if ((angle = convertSideOrCorner(aarg)) != null)
                {
                    firstStop = 1;
                }
                //check for stops
                loadColorStops(args, firstStop);
                if (ColorStops != null)
                {
                    Valid = true;
                }
            }
            return this;
        }

    }

}