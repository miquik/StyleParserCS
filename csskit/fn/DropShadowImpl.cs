using System.Collections.Generic;

namespace StyleParserCS.csskit.fn
{

    using StyleParserCS.css;
    using TermColor = StyleParserCS.css.TermColor;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermLength = StyleParserCS.css.TermLength;
    using TermList = StyleParserCS.css.TermList;

    public class DropShadowImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_DropShadow
    {

        private TermLength offsetX;
        private TermLength offsetY;
        private TermLength blurRadius;
        private TermColor color;

        public DropShadowImpl()
        {
            Valid = false;
        }

        public virtual TermLength OffsetX
        {
            get
            {
                return offsetX;
            }
        }

        public virtual TermLength OffsetY
        {
            get
            {
                return offsetY;
            }
        }

        public virtual TermLength BlurRadius
        {
            get
            {
                return blurRadius;
            }
        }

        public virtual TermColor Color
        {
            get
            {
                return color;
            }
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getValues(false);
            IList<Term> args = getValues(false);
            if (args.Count >= 2)
            {
                //find the color if used (the first or last value)
                if (args[0] is TermColor)
                {
                    color = (TermColor)args[0];
                    args.RemoveAt(0);
                }
                else if (args[args.Count - 1] is TermColor)
                {
                    color = (TermColor)args[args.Count - 1];
                    args.RemoveAt(args.Count - 1);
                }
                //interpret the remaining lengths
                if (args.Count >= 2)
                {
                    if ((offsetX = getLengthArg(args[0])) != null && (offsetY = getLengthArg(args[1])) != null)
                    {
                        Valid = true;
                    }
                    if (args.Count >= 3)
                    {
                        if ((blurRadius = getLengthArg(args[2])) != null)
                        {
                            Valid = true;
                        }
                        else
                        {
                            Valid = false;
                        }
                    }
                }
            }
            return this;
        }

    }

}