using System;
using System.Collections.Generic;

namespace StyleParserCS.csskit.fn
{
    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermIdent = StyleParserCS.css.TermIdent;
    using TermLength = StyleParserCS.css.TermLength;
    using TermList = StyleParserCS.css.TermList;
    using TermPercent = StyleParserCS.css.TermPercent;

    /// 
    /// <summary>
    /// @author Petr Mikulík
    /// </summary>
    public class MinMaxImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_MinMax
    {

        private const string MIN_CONTENT = "min-content";
        private const string MAX_CONTENT = "max-content";
        private const string AUTO = "auto";

        private StyleParserCS.css.TermFunction_MinMax_Unit _min;
        private StyleParserCS.css.TermFunction_MinMax_Unit _max;

        public MinMaxImpl()
        {
            Valid = false;
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, true);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, true);
            if (args != null && args.Count == 2)
            {
                if (setArgument(true, args[0]) && setArgument(false, args[1]))
                {
                    Valid = true;
                }
            }
            return this;
        }

        public virtual StyleParserCS.css.TermFunction_MinMax_Unit Min
        {
            get
            {
                return _min;
            }
        }

        public virtual StyleParserCS.css.TermFunction_MinMax_Unit Max
        {
            get
            {
                return _max;
            }
        }

        private bool setArgument(bool isMin, Term argTerm)
        {
            if (argTerm is TermLength)
            {
                if (isMin)
                {
                    _min = StyleParserCS.css.TermFunction_MinMax_Unit.createWithLenght((TermLength)argTerm);
                }
                else
                {
                    _max = StyleParserCS.css.TermFunction_MinMax_Unit.createWithLenght((TermLength)argTerm);
                }
            }
            else if (argTerm is TermPercent)
            {
                if (isMin)
                {
                    _min = StyleParserCS.css.TermFunction_MinMax_Unit.createWithLenght((TermPercent)argTerm);
                }
                else
                {
                    _max = StyleParserCS.css.TermFunction_MinMax_Unit.createWithLenght((TermPercent)argTerm);
                }
            }
            else if (argTerm is TermIdent)
            {
                string value = ((TermIdent)argTerm).Value;
                if (value.Equals(MIN_CONTENT, StringComparison.OrdinalIgnoreCase))
                {
                    if (isMin)
                    {
                        _min = StyleParserCS.css.TermFunction_MinMax_Unit.createWithMinContent();
                    }
                    else
                    {
                        _max = StyleParserCS.css.TermFunction_MinMax_Unit.createWithMinContent();
                    }
                }
                else if (value.Equals(MAX_CONTENT, StringComparison.OrdinalIgnoreCase))
                {
                    if (isMin)
                    {
                        _min = StyleParserCS.css.TermFunction_MinMax_Unit.createWithMaxContent();
                    }
                    else
                    {
                        _max = StyleParserCS.css.TermFunction_MinMax_Unit.createWithMaxContent();
                    }
                }
                else if (value.Equals(AUTO, StringComparison.OrdinalIgnoreCase))
                {
                    if (isMin)
                    {
                        _min = StyleParserCS.css.TermFunction_MinMax_Unit.createWithAuto();
                    }
                    else
                    {
                        _max = StyleParserCS.css.TermFunction_MinMax_Unit.createWithAuto();
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

    }

}