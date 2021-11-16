using System.Collections.Generic;

/// <summary>
/// GenericGradient.java
/// 
/// Created on 17. 5. 2018, 12:52:23 by burgetr
/// </summary>
namespace StyleParserCS.csskit.fn
{

    using StyleParserCS.css;
    using TermColor = StyleParserCS.css.TermColor;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermLengthOrPercent = StyleParserCS.css.TermLengthOrPercent;
    using TermFunction_Gradient_ColorStop = StyleParserCS.css.TermFunction_Gradient_ColorStop;

    /// <summary>
    /// Implementation of common features of all the gradient functions.
    /// 
    /// @author burgetr
    /// </summary>
    public class GenericGradient : TermFunctionImpl
    {
        private IList<TermFunction_Gradient_ColorStop> colorStops;

        public GenericGradient()
        {
            Valid = false;
        }

        public virtual IList<TermFunction_Gradient_ColorStop> ColorStops
        {
            get
            {
                return colorStops;
            }
        }

        protected internal virtual void loadColorStops(IList<IList<Term>> args, int firstStop)
        {
            colorStops = decodeColorStops(args, firstStop);
        }

        /// <summary>
        /// Loads the color stops from the gunction arguments. </summary>
        /// <param name="args"> the comma-separated function arguments </param>
        /// <param name="firstStop"> the first argument to start with </param>
        /// <returns> the list of color stops or {@code null} when the arguments are invalid or missing </returns>
        protected internal virtual IList<TermFunction_Gradient_ColorStop> decodeColorStops(IList<IList<Term>> args, int firstStop)
        {
            bool valid = true;
            IList<TermFunction_Gradient_ColorStop> colorStops = null;
            if (args.Count > firstStop)
            {
                colorStops = new List<TermFunction_Gradient_ColorStop>();
                for (int i = firstStop; valid && i < args.Count; i++)
                {
                    //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> sarg = args.get(i);
                    IList<Term> sarg = (IList<Term>)args[i];
                    if (sarg.Count == 1 || sarg.Count == 2)
                    {
                        //ORIGINAL LINE: StyleParserCS.css.Term<?> tclr = sarg.get(0);
                        Term tclr = sarg[0];
                        //ORIGINAL LINE: StyleParserCS.css.Term<?> tlen = (sarg.size() == 2) ? sarg.get(1) : null;
                        Term tlen = (sarg.Count == 2) ? sarg[1] : null;
                        if (tclr is TermColor && (tlen == null || tlen is TermLengthOrPercent))
                        {
                            TermFunction_Gradient_ColorStop newStop = new ColorStopImpl((TermColor)tclr, (TermLengthOrPercent)tlen);
                            colorStops.Add(newStop);
                        }
                        else
                        {
                            valid = false;
                        }
                    }
                    else
                    {
                        valid = false;
                    }
                }
            }
            if (valid && colorStops != null && colorStops.Count > 0)
            {
                return colorStops;
            }
            else
            {
                return null;
            }
        }

    }

}