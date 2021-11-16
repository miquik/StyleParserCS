using System;
using System.Collections.Generic;

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
    public class RepeatImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_Repeat
    {

        private const string AUTO_FIT = "auto-fit";
        private const string AUTO_FILL = "auto-fill";

        private StyleParserCS.css.TermFunction_Repeat_Unit _numberOfRepetitions;
        //ORIGINAL LINE: private java.util.List<StyleParserCS.css.Term<?>> _repeatedTerms;
        private IList<Term> _repeatedTerms;

        public RepeatImpl()
        {
            Valid = false;
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<java.util.List<StyleParserCS.css.Term<?>>> args = getSeparatedArgs((Term)DEFAULT_ARG_SEP);
            IList<IList<Term>> args = getSeparatedArgs((Term)DEFAULT_ARG_SEP);
            if (args != null && args.Count == 2)
            {
                if (setNumberOfRepetitions(args[0]) && setRepeatedTerms(args[1]))
                {
                    Valid = true;
                }
            }
            return this;
        }

        public virtual StyleParserCS.css.TermFunction_Repeat_Unit NumberOfRepetitions
        {
            get { return _numberOfRepetitions; }
        }

        //ORIGINAL LINE: @Override public java.util.List<StyleParserCS.css.Term<?>> getRepeatedTerms()
        public virtual IList<Term> RepeatedTerms
        {
            get
            {
                return _repeatedTerms;
            }
        }

        private bool setNumberOfRepetitions(IList<Term> argTerms)
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
                        _numberOfRepetitions = StyleParserCS.css.TermFunction_Repeat_Unit.createWithNRepetitions(value);
                        return true;
                    }
                }
                else if (t is TermIdent)
                {
                    string value = ((TermIdent)t).Value;
                    if (value.Equals(AUTO_FIT, StringComparison.OrdinalIgnoreCase))
                    {
                        _numberOfRepetitions = StyleParserCS.css.TermFunction_Repeat_Unit.createWithAutoFit();
                        return true;
                    }
                    else if (value.Equals(AUTO_FILL, StringComparison.OrdinalIgnoreCase))
                    {
                        _numberOfRepetitions = StyleParserCS.css.TermFunction_Repeat_Unit.createWithAutoFill();
                        return true;
                    }
                }
            }
            return false;
        }

        private bool setRepeatedTerms(IList<Term> argTerms)
        {
            _repeatedTerms = argTerms;
            return true;
        }

    }

}