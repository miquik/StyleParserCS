using System.Collections.Generic;

/*
 */
namespace StyleParserCS.csskit.fn
{
    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermList = StyleParserCS.css.TermList;
    using TermNumber = StyleParserCS.css.TermNumber;

    /// 
    /// <summary>
    /// @author Petr Mikulík
    /// </summary>
    public class CubicBezierImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_CubicBezier
    {

        private readonly float[] _values = new float[4];

        public CubicBezierImpl()
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
                if (args.Count == 4)
                {
                    if (setValues(args))
                    {
                        Valid = true;
                    }
                }
            }
            return this;
        }

        public virtual float X1
        {
            get
            {
                return _values[0];
            }
        }

        public virtual float Y1
        {
            get
            {
                return _values[1];
            }
        }

        public virtual float X2
        {
            get
            {
                return _values[2];
            }
        }

        public virtual float Y2
        {
            get
            {
                return _values[3];
            }
        }

        private bool setValues(IList<IList<Term>> args)
        {
            for (int i = 0; i < args.Count; i++)
            {                
                if (!setValueAt(i, args[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private bool setValueAt(int index, IList<Term> argTerms)
        {
            if (argTerms.Count == 1)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = argTerms.get(0);
                Term t = (Term)argTerms[0];
                if (t is TermNumber)
                {
                    float value = ((TermNumber)t).Value;
                    if (index == 1 || index == 3 || value >= 0 && value <= 1)
                    {
                        _values[index] = value;
                        return true;
                    }
                }
            }
            return false;
        }

    }

}