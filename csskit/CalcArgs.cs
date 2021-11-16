using System;
using System.Collections.Generic;

/// 
namespace StyleParserCS.csskit
{
    using StyleParserCS.css;
    using TermFloatValue = StyleParserCS.css.TermFloatValue;
    using TermInteger = StyleParserCS.css.TermInteger;
    using TermNumber = StyleParserCS.css.TermNumber;
    using TermOperator = StyleParserCS.css.TermOperator;
    using TermPercent = StyleParserCS.css.TermPercent;


    /// <summary>
    /// This class tracks the calc() arguments (postorder) and their resulting type.
    /// @author burgetr
    /// </summary>
    // public class CalcArgs : List<Term>
    public class CalcArgs : List<Term>
    {

        private const long serialVersionUID = 1L;
        // private static readonly Logger log = LoggerFactory.getLogger(typeof(CalcArgs));

        public static readonly StringEvaluator stringEvaluator;
        static CalcArgs()
        {
            stringEvaluator = new StringEvaluator();
        }

        private TermLength_Unit.TermType utype = TermLength_Unit.TermType.none; //expected value type
        private bool isint = true; //all the values are integers?
        private bool valid = true;

        public CalcArgs(IList<Term> terms) : base(terms.Count)
        {
            scanArguments(terms);
        }

        public virtual TermLength_Unit.TermType Type
        {
            get
            {
                return utype;
            }
        }

        public virtual bool Int
        {
            get
            {
                return isint;
            }
        }

        public virtual bool Valid
        {
            get
            {
                return valid;
            }
        }

        protected internal virtual void scanArguments(IList<Term> args)
        {
            //tansform expression to a postfix notation
            LinkedList<TermOperator> stack = new LinkedList<TermOperator>();
            bool unary = true;
            //ORIGINAL LINE: for (StyleParserCS.css.Term<?> t : args)
            foreach (Term t in args)
            {
                if (t is TermFloatValue)
                {
                    this.Add(t);
                    considerType((TermFloatValue)t);
                    unary = false;
                    if (!valid)
                    {
                        break; //type mismatch
                    }
                }
                else if (t is TermOperator)
                {
                    TermOperator op = (TermOperator)t;
                    if (unary && op.Value == '-')
                    {
                        op = (TermOperator)op.shallowClone();
                        op.setValue('~');
                    }
                    //ORIGINAL LINE: final int p = getPriority(op);
                    int p = getPriority(op);
                    if (p != -1)
                    {
                        TermOperator top = stack.First.Value;
                        if (top == null || top.Value == '(' || p > getPriority(top))
                        {
                            stack.AddFirst(op);
                        }
                        else
                        {
                            do
                            {
                                this.Add(top);
                                stack.RemoveFirst();
                                top = stack.First.Value;
                            } while (top != null && top.Value != '(' && p <= getPriority(top));
                            stack.AddFirst(op);
                        }
                        unary = true;
                    }
                    else if (op.Value == '(')
                    {
                        stack.AddFirst(op);
                        unary = true;
                    }
                    else if (op.Value == ')')
                    {
                        if (stack.Count > 0)
                        {
                            TermOperator top = stack.First.Value;
                            stack.RemoveFirst();
                            while (top != null && top.Value != '(' && stack.Count > 0)
                            {
                                this.Add(top);
                                top = stack.First.Value;
                                stack.RemoveFirst();
                            }
                        }

                        unary = false;
                    }
                    else
                    {
                        valid = false;
                        break;
                    }
                }
                else
                {
                    valid = false;
                    break;
                }
            }
            while (stack.Count > 0)
            {
                this.Add(stack.First.Value);
                stack.RemoveFirst();
            }
        }

        private int getPriority(TermOperator op)
        {
            char c = op.Value;
            switch (c)
            {
                case '+':
                case '-':
                    return 0;
                case '*':
                case '/':
                    return 1;
                case '~':
                    return 2; //unary -
                default:
                    return -1;
            }
        }

        //isLength, isFrequency, isAngle, isTime, isNumber, isInteger

        private void considerType(TermFloatValue term)
        {

            StyleParserCS.css.TermLength_Unit unit = term.Unit;
            if (utype == StyleParserCS.css.TermLength_Unit.TermType.none)
            { //only a number
                if (unit != null && unit.Type != StyleParserCS.css.TermLength_Unit.TermType.none)
                {
                    utype = unit.Type;
                }
                else if (term is TermPercent)
                {
                    utype = StyleParserCS.css.TermLength_Unit.TermType.length; //percentages have no unit
                }
                else if (term is TermNumber)
                {
                    isint = false; //not an integer
                }
            }
            else
            { //some type already assigned, check for mismatches
                if (unit != null && unit.Type != StyleParserCS.css.TermLength_Unit.TermType.none)
                {
                    if (unit.Type != utype)
                    {
                        valid = false;
                    }
                }
            }
        }

        //=========================================================================

        //ORIGINAL LINE: public <T> T evaluate(Evaluator<T> eval) throws IllegalArgumentException
        public virtual T evaluate<T>(Evaluator<T> eval)
        {
            try
            {
                LinkedList<T> stack = new LinkedList<T>();
                //ORIGINAL LINE: for (StyleParserCS.css.Term<?> t : this)
                foreach (Term t in this)
                {
                    if (t is TermOperator)
                    {
                        T val;
                        if (((TermOperator)t).Value == '~')
                        {
                            T val1 = stack.First.Value;
                            stack.RemoveFirst();
                            val = eval.evaluateOperator(val1, (TermOperator)t);
                        }
                        else
                        {
                            T val2 = stack.First.Value;
                            stack.RemoveFirst();
                            T val1 = stack.First.Value;
                            stack.RemoveFirst();
                            val = eval.evaluateOperator(val1, val2, (TermOperator)t);
                        }
                        stack.AddFirst(val);
                    }
                    else if (t is TermFloatValue)
                    {
                        T val = eval.evaluateArgument((TermFloatValue)t);
                        stack.AddFirst(val);
                    }
                }
                return stack.First.Value;
            }
            catch (Exception e)
            {
                throw new System.ArgumentException("Couldn't evaluate calc() expression", e);
            }
        }

        //=========================================================================

        /// <summary>
        /// A generic evaluator that is able to obtain a resulting value of the given type
        /// from the expressions.
        /// 
        /// @author burgetr </summary>
        /// @param <T> the type of the resulting value </param>
        public interface Evaluator<T>
        {

            T evaluateArgument(TermFloatValue val);

            T evaluateOperator(T val1, T val2, TermOperator op);

            T evaluateOperator(T val, TermOperator op);

        }

        /// <summary>
        /// A pre-defined evaluator that produces a string representation of the expression.
        /// 
        /// @author burgetr
        /// </summary>
        public class StringEvaluator : Evaluator<string>
        {

            public virtual string evaluateArgument(TermFloatValue val)
            {
                return val.ToString();
            }

            public virtual string evaluateOperator(string val1, string val2, TermOperator op)
            {
                return "(" + val1 + " " + op.ToString() + " " + val2.ToString() + ")";
            }

            public virtual string evaluateOperator(string val, TermOperator op)
            {
                if (op.Value == '~')
                {
                    return "-" + val;
                }
                else
                {
                    return op.Value + val;
                }
            }

        }

        /// <summary>
        /// An abstract pre-defined evaluator that produces a double value from the expression.
        /// Implementations must provide the {@code resolveValue()} method that evaluates an atomic value.
        /// 
        /// @author burgetr
        /// </summary>
        public abstract class DoubleEvaluator : Evaluator<double>
        {
            public abstract double evaluateOperator(double val, TermOperator op);
            public abstract double evaluateOperator(double val1, double val2, TermOperator op);

            public virtual double evaluateArgument(TermFloatValue val)
            {
                if (val is TermNumber || val is TermInteger)
                {
                    return Convert.ToDouble(val.Value);
                }
                else
                {
                    return resolveValue(val);
                }
            }

            public virtual double? evaluateOperator(double? val1, double? val2, TermOperator op)
            {
                switch (op.Value)
                {
                    case '+':
                        return val1.Value + val2.Value;
                    case '-':
                        return val1.Value - val2.Value;
                    case '*':
                        return val1.Value * val2.Value;
                    case '/':
                        return val1.Value / val2.Value;
                    default:
                        // log.error("Unknown operator {} in expression", op);
                        return 0.0;
                }
            }

            public virtual double? evaluateOperator(double? val, TermOperator op)
            {
                if (op.Value == '~')
                {
                    return -val.Value;
                }
                else
                {
                    // log.error("Unknown unary operator {} in expression", op);
                    return val;
                }
            }

            /// <summary>
            /// Evaluates an atomic value. </summary>
            /// <param name="val"> the input value specification </param>
            /// <returns> the evaluated value </returns>
            public abstract double resolveValue(TermFloatValue val);

        }

        /// <summary>
        /// An abstract pre-defined evaluator that produces a float value from the expression.
        /// Implementations must provide the {@code resolveValue()} method that evaluates an atomic value.
        /// 
        /// @author burgetr
        /// </summary>
        public abstract class FloatEvaluator : Evaluator<float>
        {
            public abstract float evaluateOperator(float val, TermOperator op);
            public abstract float evaluateOperator(float val1, float val2, TermOperator op);

            public virtual float evaluateArgument(TermFloatValue val)
            {
                if (val is TermNumber || val is TermInteger)
                {
                    return Convert.ToSingle(val.Value);
                }
                else
                {
                    return resolveValue(val);
                }
            }

            public virtual float? evaluateOperator(float? val1, float? val2, TermOperator op)
            {
                switch (op.Value)
                {
                    case '+':
                        return val1.Value + val2.Value;
                    case '-':
                        return val1.Value - val2.Value;
                    case '*':
                        return val1.Value * val2.Value;
                    case '/':
                        return val1.Value / val2.Value;
                    default:
                        // log.error("Unknown operator {} in expression", op);
                        return 0.0f;
                }
            }

            public virtual float? evaluateOperator(float? val, TermOperator op)
            {
                if (op.Value == '~')
                {
                    return -val.Value;
                }
                else
                {
                    // log.error("Unknown unary operator {} in expression", op);
                    return val;
                }
            }

            /// <summary>
            /// Evaluates an atomic value. </summary>
            /// <param name="val"> the input value specification </param>
            /// <returns> the evaluated value </returns>
            public abstract float resolveValue(TermFloatValue val);

        }

    }

}