using System.Collections.Generic;

namespace StyleParserCS.csskit.fn
{

    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermList = StyleParserCS.css.TermList;

    public class MatrixImpl : TermFunctionImpl, StyleParserCS.css.TermFunction_Matrix
    {

        private float[] values;

        public MatrixImpl()
        {
            Valid = false; //arguments are required
        }

        public virtual float[] Values
        {
            get
            {
                return values;
            }
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            IList<Term> args = getSeparatedValues((Term)DEFAULT_ARG_SEP, false);
            if (args != null && args.Count == 6)
            {
                values = new float[6];
                Valid = true;
                for (int i = 0; i < 6; i++)
                {
                    if (isNumberArg(args[i]))
                    {
                        values[i] = getNumberArg(args[i]);
                    }
                    else
                    {
                        Valid = false;
                    }
                }
            }
            return this;
        }
    }
}