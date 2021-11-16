using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace StyleParserCS.csskit
{

    using CssEscape = StyleParserCS.unbescape.CssEscape;

    using CSSFactory = StyleParserCS.css.CSSFactory;
    using StyleParserCS.css;
    using TermAngle = StyleParserCS.css.TermAngle;
    using TermFactory = StyleParserCS.css.TermFactory;
    using TermFloatValue = StyleParserCS.css.TermFloatValue;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermIdent = StyleParserCS.css.TermIdent;
    using TermInteger = StyleParserCS.css.TermInteger;
    using TermLength = StyleParserCS.css.TermLength;
    using TermLengthOrPercent = StyleParserCS.css.TermLengthOrPercent;
    using TermList = StyleParserCS.css.TermList;
    using TermNumber = StyleParserCS.css.TermNumber;
    using TermLength_Unit = StyleParserCS.css.TermLength_Unit;
    using TermOperator = StyleParserCS.css.TermOperator;
    using TermString = StyleParserCS.css.TermString;

    /// <summary>
    /// TermFunction, holds function
    /// @author Jan Svercl, VUT Brno, 2008
    /// @author Karel Piwko
    /// @author Radek Burget
    /// </summary>
    public class TermFunctionImpl : TermListImpl, TermFunction
    {

        protected internal static readonly TermOperator DEFAULT_ARG_SEP = CSSFactory.TermFactory.createOperator(',');

        protected internal string functionName;
        protected internal bool valid;


        protected internal TermFunctionImpl()
        {
            valid = true;
        }

        /// <returns> the functionName </returns>
        public virtual string FunctionName
        {
            get
            {
                return functionName;
            }
        }

        /// <param name="functionName"> the functionName to set </param>
        public virtual TermFunction setFunctionName(string functionName)
        {
            if (string.ReferenceEquals(functionName, null))
            {
                throw new System.ArgumentException("Invalid functionName in function (null)");
            }
            this.functionName = functionName;
            return this;
        }

        public virtual bool Valid
        {
            get
            {
                return valid;
            }
            set
            {
                this.valid = value;
            }
        }


        public override TermList setValue(IList<Term> value)
        { //TODO the minus operation is duplicate to getSeparatedValues()?
          //ORIGINAL LINE: this.value = new java.util.ArrayList<>();
            this.value = new List<Term>();

            // Treat '-' as modifying the next argument, instead of as an operator
            bool prevMinus = false;

            //ORIGINAL LINE: for (StyleParserCS.css.Term<?> term : value)
            foreach (Term term in value)
            {
                if (term is TermOperator && ((TermOperator)term).Value == '-')
                {
                    prevMinus = true;
                }
                else if (prevMinus)
                {
                    if (prependMinus(term))
                    {
                        this.value.RemoveAt(this.value.Count - 1); // Remove merged minus
                    }

                    prevMinus = false;
                }

                this.value.Add(term);
            }

            return this;
        }

        protected internal virtual bool prependMinus(Term term)
        {
            bool merged = false;

            if (term is TermFloatValue)
            { // includes TermAngle, TermLength, etc.
                TermFloatValue floatT = (TermFloatValue)term;
                floatT.setValue(-1 * floatT.Value);
                merged = true;
            }
            else if (term is TermIdent)
            {
                TermIdent ident = (TermIdent)term;
                ident.setValue("-" + ident.Value);
                merged = true;
            }
            else if (term is TermFunction)
            {
                TermFunction func = (TermFunction)term;
                func.setFunctionName("-" + func.FunctionName);
                merged = true;
            }

            return merged;
        }

        //ORIGINAL LINE: @Override public java.util.List<java.util.List<StyleParserCS.css.Term<?>>> getSeparatedArgs(StyleParserCS.css.Term<?> separator)
        public virtual IList<IList<Term>> getSeparatedArgs(Term separator)
        {
            //ORIGINAL LINE: java.util.List<java.util.List<StyleParserCS.css.Term<?>>> ret = new java.util.ArrayList<>();
            IList<IList<Term>> ret = new List<IList<Term>>();
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> cur = new java.util.ArrayList<>();
            IList<Term> cur = new List<Term>();
            //ORIGINAL LINE: for (StyleParserCS.css.Term<?> t : this)
            foreach (Term t in this)
            {
                if (t.Equals(separator))
                {
                    ret.Add(cur);
                    //ORIGINAL LINE: cur = new java.util.ArrayList<>();
                    cur = new List<Term>();
                }
                else
                {
                    cur.Add(t);
                }
            }
            if (cur.Count > 0)
            {
                ret.Add(cur);
            }

            return ret;
        }

        //ORIGINAL LINE: @Override public java.util.List<StyleParserCS.css.Term<?>> getSeparatedValues(StyleParserCS.css.Term<?> separator, boolean allowKeywords)
        public virtual IList<Term> getSeparatedValues(Term separator, bool allowKeywords)
        {
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> ret = new java.util.ArrayList<>();
            IList<Term> ret = new List<Term>();
            TermOperator curOp = null; //an optional unary operator before the value
                                       //ORIGINAL LINE: StyleParserCS.css.Term<?> curVal = null;
            Term curVal = null;
            //ORIGINAL LINE: for (StyleParserCS.css.Term<?> t : this)
            foreach (Term t in this)
            {
                if (t.Equals(separator))
                {
                    if (curVal != null)
                    {
                        if (curOp != null)
                        {
                            if (curVal is TermFloatValue)
                            {
                                if (curOp.Value == '-')
                                {
                                    float newVal = -((TermFloatValue)curVal).Value;
                                    curVal = (Term)((TermFloatValue)curVal).shallowClone();
                                    ((TermFloatValue)curVal).setValue(newVal);
                                }
                                else if (curOp.Value != '+')
                                {
                                    return null; //invalid operator
                                }
                            }
                            else
                            {
                                return null; //operator combined with ident
                            }
                        }
                        ret.Add(curVal);
                        curVal = null;
                        curOp = null;
                    }
                    else
                    {
                        return null; //value missing
                    }
                }
                else if (t is TermOperator)
                {
                    if (curOp == null && curVal == null)
                    {
                        curOp = (TermOperator)t;
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (t is TermFloatValue || t is TermString)
                {
                    if (curVal == null)
                    {
                        curVal = t;
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (allowKeywords && t is TermIdent)
                {
                    if (curVal == null)
                    {
                        curVal = t;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            //the last value
            if (curVal != null)
            {
                if (curOp != null)
                {
                    if (curVal is TermFloatValue)
                    {
                        if (curOp.Value == '-')
                        {
                            float newVal = -((TermFloatValue)curVal).Value;
                            curVal = (Term)((TermFloatValue)curVal).shallowClone();
                            ((TermFloatValue)curVal).setValue(newVal);
                        }
                        else if (curOp.Value != '+')
                        {
                            return null; //invalid operator
                        }
                    }
                    else
                    {
                        return null; //operator combined with ident
                    }
                }
                ret.Add(curVal);
            }
            else
            {
                return null; //value missing
            }

            return ret;
        }

        //ORIGINAL LINE: @Override public java.util.List<StyleParserCS.css.Term<?>> getValues(boolean allowKeywords)
        public virtual IList<Term> getValues(bool allowKeywords)
        {
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> ret = new java.util.ArrayList<>();
            IList<Term> ret = new List<Term>();
            TermOperator curOp = null; //an optional unary operator before the value
                                       //ORIGINAL LINE: for (StyleParserCS.css.Term<?> t : this)
            foreach (Term t in this)
            {
                if (t is TermOperator)
                {
                    if (curOp == null)
                    {
                        curOp = (TermOperator)t;
                    }
                    else
                    {
                        return null; //repeating operator
                    }
                }
                else if (t is TermFloatValue)
                {
                    TermFloatValue curVal = (TermFloatValue)t;
                    if (curOp != null)
                    {
                        if (curOp.Value == '-')
                        {
                            float newVal = -curVal.Value;
                            curVal = (TermFloatValue)curVal.shallowClone();
                            curVal.setValue(newVal);
                        }
                        else if (curOp.Value != '+')
                        {
                            return null; //invalid operator
                        }
                    }
                    ret.Add((Term)curVal);
                    curVal = null;
                    curOp = null;
                }
                else if (t is TermIdent)
                { //identifiers if allowed
                    if (allowKeywords && curOp == null)
                    {
                        ret.Add(t);
                    }
                    else
                    {
                        return null; //operator combined with ident
                    }
                }
                else
                {
                    if (curOp == null)
                    {
                        ret.Add(t);
                    }
                    else
                    {
                        return null; //operator combined with non-numeric term
                    }
                }
            }

            if (curOp != null)
            {
                return null; //an operator followed with no value
            }

            return ret;
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            // append operator
            if (operatorv != null)
            {
                sb.Append(operatorv.value());
            }

            sb.Append(CssEscape.escapeCssIdentifier(functionName)).Append(OutputUtil.FUNCTION_OPENING);
            sb = OutputUtil.appendFunctionArgs(sb, value).Append(OutputUtil.FUNCTION_CLOSING);

            return sb.ToString();
        }


        /* (non-Javadoc)
		 * @see java.lang.Object#hashCode()
		 */
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = base.GetHashCode();
            result = prime * result + ((string.ReferenceEquals(functionName, null)) ? 0 : functionName.GetHashCode());
            return result;
        }


        /* (non-Javadoc)
		 * @see java.lang.Object#equals(java.lang.Object)
		 */
        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            //ORIGINAL LINE: if (!super.equals(obj))
            // TOCHECK
            // if (!base.SequenceEqual(obj))
            // {
            //    return false;
            // }
            if (!(obj is TermFunctionImpl))
            {
                return false;
            }
            TermFunctionImpl other = (TermFunctionImpl)obj;
            if (string.ReferenceEquals(functionName, null))
            {
                if (!string.ReferenceEquals(other.functionName, null))
                {
                    return false;
                }
            }
            else if (!functionName.Equals(other.functionName))
            {
                return false;
            }
            return true;
        }

        //========================================================================

        protected internal virtual bool isNumberArg(Term term)
        {
            return term is TermNumber || term is TermInteger;
        }

        protected internal virtual float getNumberArg(Term term)
        {
            if (term is TermNumber)
            {
                return ((TermNumber)term).Value;
            }
            else
            {
                return ((TermInteger)term).Value;
            }
        }

        protected internal virtual TermAngle getAngleArg(Term term)
        {
            if (term is TermAngle)
            {
                return (TermAngle)term;
            }
            else if (isNumberArg(term) && getNumberArg(term) == 0)
            {
                return CSSFactory.TermFactory.createAngle(0.0f);
            }
            else
            {
                return null;
            }
        }

        protected internal virtual TermLength getLengthArg(Term term)
        {
            if (term is TermLength)
            {
                return (TermLength)term;
            }
            else if (isNumberArg(term) && getNumberArg(term) == 0)
            {
                return CSSFactory.TermFactory.createLength(0.0f);
            }
            else
            {
                return null;
            }
        }

        protected internal virtual TermLengthOrPercent getLengthOrPercentArg(Term term)
        {
            if (term is TermLengthOrPercent)
            {
                return (TermLengthOrPercent)term;
            }
            else if (isNumberArg(term) && getNumberArg(term) == 0)
            {
                return CSSFactory.TermFactory.createLength(0.0f);
            }
            else
            {
                return null;
            }
        }

        protected internal virtual TermAngle convertSideOrCorner(IList<Term> aarg)
        {
            if (aarg.Count > 1 && aarg.Count <= 3)
            {
                TermAngle angle = null;
                //ORIGINAL LINE: StyleParserCS.css.Term<?> toTerm = aarg.get(0);
                Term toTerm = (Term)aarg[0];
                //ORIGINAL LINE: StyleParserCS.css.Term<?> dir1 = aarg.get(1);
                Term dir1 = (Term)aarg[1];
                //ORIGINAL LINE: StyleParserCS.css.Term<?> dir2 = (aarg.size() == 3) ? aarg.get(2) : null;
                Term dir2 = (aarg.Count == 3) ? (Term)aarg[2] : null;
                if (toTerm is TermIdent && toTerm.ToString().Equals("to") && dir1 is TermIdent && (dir2 == null || dir2 is TermIdent))
                {

                    //ORIGINAL LINE: final StyleParserCS.css.TermFactory tf = StyleParserCS.css.CSSFactory.getTermFactory();
                    TermFactory tf = CSSFactory.TermFactory;

                    switch (dir1.ToString())
                    {
                        case "top":
                            if (dir2 == null)
                            {
                                angle = tf.createAngle("0", TermLength_Unit.deg, 1);
                            }
                            else if (dir2.ToString().Equals("left"))
                            {
                                angle = tf.createAngle("315", TermLength_Unit.deg, 1);
                            }
                            else if (dir2.ToString().Equals("right"))
                            {
                                angle = tf.createAngle("45", TermLength_Unit.deg, 1);
                            }
                            break;
                        case "right":
                            if (dir2 == null)
                            {
                                angle = tf.createAngle("90", TermLength_Unit.deg, 1);
                            }
                            else if (dir2.ToString().Equals("top"))
                            {
                                angle = tf.createAngle("45", TermLength_Unit.deg, 1);
                            }
                            else if (dir2.ToString().Equals("bottom"))
                            {
                                angle = tf.createAngle("135", TermLength_Unit.deg, 1);
                            }
                            break;
                        case "bottom":
                            if (dir2 == null)
                            {
                                angle = tf.createAngle("180", TermLength_Unit.deg, 1);
                            }
                            else if (dir2.ToString().Equals("left"))
                            {
                                angle = tf.createAngle("225", TermLength_Unit.deg, 1);
                            }
                            else if (dir2.ToString().Equals("right"))
                            {
                                angle = tf.createAngle("135", TermLength_Unit.deg, 1);
                            }
                            break;
                        case "left":
                            if (dir2 == null)
                            {
                                angle = tf.createAngle("270", TermLength_Unit.deg, 1);
                            }
                            else if (dir2.ToString().Equals("top"))
                            {
                                angle = tf.createAngle("315", TermLength_Unit.deg, 1);
                            }
                            else if (dir2.ToString().Equals("bottom"))
                            {
                                angle = tf.createAngle("225", TermLength_Unit.deg, 1);
                            }
                            break;
                    }
                }
                return angle;
            }
            else
            {
                return null;
            }
        }

    }

}