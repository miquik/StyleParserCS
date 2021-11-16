using System.Collections.Generic;

/*
 */

namespace StyleParserCS.csskit.fn
{
    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermInteger = StyleParserCS.css.TermInteger;
    using TermList = StyleParserCS.css.TermList;

    /// 
    /// <summary>
    /// @author Petr Mikulík
    /// </summary>
    public class FramesImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_Frames
    {

        private int _frames;

        public FramesImpl()
        {
            Valid = false;
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<java.util.List<StyleParserCS.css.Term<?>>> args = getSeparatedArgs((Term)DEFAULT_ARG_SEP);
            IList<IList<Term>> args = getSeparatedArgs((Term)DEFAULT_ARG_SEP);
            if (args != null)
            {
                if (args.Count == 1)
                {
                    if (setFrames(args[0]))
                    {
                        Valid = true;
                    }
                }
            }
            return this;
        }

        public virtual int Frames
        {
            get { return _frames; }
        }

        private bool setFrames(IList<Term> argTerms)
        {
            if (argTerms.Count == 1)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = argTerms.get(0);
                Term t = argTerms[0];
                if (t is TermInteger)
                {
                    int value = ((TermInteger)t).IntValue;
                    if (value > 0)
                    {
                        _frames = value;
                        return true;
                    }
                }
            }
            return false;
        }

    }

}