using System.Collections.Generic;

/*
 */
namespace StyleParserCS.csskit.fn
{
    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermIdent = StyleParserCS.css.TermIdent;
    using TermInteger = StyleParserCS.css.TermInteger;
    using TermList = StyleParserCS.css.TermList;

    /// 
    /// <summary>
    /// @author Petr Mikulík
    /// </summary>
    public class StepsImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_Steps
    {

        private int _numberOfSteps;
        private StyleParserCS.css.TermFunction_Steps_Direction _direciton;

        public StepsImpl()
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
                    if (setNumberOfSteps(args[0]))
                    {
                        _direciton = StyleParserCS.css.TermFunction_Steps_Direction.END;
                        Valid = true;
                    }
                }
                else if (args.Count == 2)
                {
                    if (setNumberOfSteps(args[0]) && setDirection(args[1]))
                    {
                        Valid = true;
                    }
                }
            }
            return this;
        }

        public virtual int NumberOfSteps
        {
            get { return _numberOfSteps; }
        }

        public virtual StyleParserCS.css.TermFunction_Steps_Direction Direction
        {
            get { return _direciton; }
        }

        private bool setNumberOfSteps(IList<Term> argTerms)
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
                        _numberOfSteps = value;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool setDirection(IList<Term> argTerms)
        {
            if (argTerms.Count == 1)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = argTerms.get(0);
                Term t = argTerms[0];
                if (t is TermIdent)
                {
                    string value = ((TermIdent)t).Value;
                    foreach (StyleParserCS.css.TermFunction_Steps_Direction d in StyleParserCS.css.TermFunction_Steps_Direction.List)
                    {
                        if (d.ToString().Equals(value))
                        {
                            _direciton = d;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

    }

}