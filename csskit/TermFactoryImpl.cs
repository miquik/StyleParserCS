using System;
using System.Collections.Generic;

namespace StyleParserCS.csskit
{

    using CSSFactory = StyleParserCS.css.CSSFactory;
    using CSSProperty = StyleParserCS.css.CSSProperty;
    using StyleParserCS.css;
    using TermAngle = StyleParserCS.css.TermAngle;
    using TermBracketedIdents = StyleParserCS.css.TermBracketedIdents;
    using TermCalc = StyleParserCS.css.TermCalc;
    using TermColor = StyleParserCS.css.TermColor;
    using TermExpression = StyleParserCS.css.TermExpression;
    using TermFactory = StyleParserCS.css.TermFactory;
    using TermFrequency = StyleParserCS.css.TermFrequency;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermIdent = StyleParserCS.css.TermIdent;
    using TermInteger = StyleParserCS.css.TermInteger;
    using TermLength = StyleParserCS.css.TermLength;
    using TermList = StyleParserCS.css.TermList;
    using TermNumber = StyleParserCS.css.TermNumber;
    using TermPercent = StyleParserCS.css.TermPercent;
    using TermPropertyValue = StyleParserCS.css.TermPropertyValue;
    using TermRect = StyleParserCS.css.TermRect;
    using TermResolution = StyleParserCS.css.TermResolution;
    using TermString = StyleParserCS.css.TermString;
    using TermTime = StyleParserCS.css.TermTime;
    using TermURI = StyleParserCS.css.TermURI;
    using TermUnicodeRange = StyleParserCS.css.TermUnicodeRange;
    using TermLength_Unit = StyleParserCS.css.TermLength_Unit;
    using AttrImpl = StyleParserCS.csskit.fn.AttrImpl;
    using BlurImpl = StyleParserCS.csskit.fn.BlurImpl;
    using BrightnessImpl = StyleParserCS.csskit.fn.BrightnessImpl;
    using ContrastImpl = StyleParserCS.csskit.fn.ContrastImpl;
    using CounterImpl = StyleParserCS.csskit.fn.CounterImpl;
    using CountersImpl = StyleParserCS.csskit.fn.CountersImpl;
    using DropShadowImpl = StyleParserCS.csskit.fn.DropShadowImpl;
    using GrayscaleImpl = StyleParserCS.csskit.fn.GrayscaleImpl;
    using HueRotateImpl = StyleParserCS.csskit.fn.HueRotateImpl;
    using InvertImpl = StyleParserCS.csskit.fn.InvertImpl;
    using LinearGradientImpl = StyleParserCS.csskit.fn.LinearGradientImpl;
    using Matrix3dImpl = StyleParserCS.csskit.fn.Matrix3dImpl;
    using MatrixImpl = StyleParserCS.csskit.fn.MatrixImpl;
    using OpacityImpl = StyleParserCS.csskit.fn.OpacityImpl;
    using PerspectiveImpl = StyleParserCS.csskit.fn.PerspectiveImpl;
    using RadialGradientImpl = StyleParserCS.csskit.fn.RadialGradientImpl;
    using RepeatingLinearGradientImpl = StyleParserCS.csskit.fn.RepeatingLinearGradientImpl;
    using RepeatingRadialGradientImpl = StyleParserCS.csskit.fn.RepeatingRadialGradientImpl;
    using Rotate3dImpl = StyleParserCS.csskit.fn.Rotate3dImpl;
    using RotateImpl = StyleParserCS.csskit.fn.RotateImpl;
    using RotateXImpl = StyleParserCS.csskit.fn.RotateXImpl;
    using RotateYImpl = StyleParserCS.csskit.fn.RotateYImpl;
    using RotateZImpl = StyleParserCS.csskit.fn.RotateZImpl;
    using SaturateImpl = StyleParserCS.csskit.fn.SaturateImpl;
    using Scale3dImpl = StyleParserCS.csskit.fn.Scale3dImpl;
    using ScaleImpl = StyleParserCS.csskit.fn.ScaleImpl;
    using ScaleXImpl = StyleParserCS.csskit.fn.ScaleXImpl;
    using ScaleYImpl = StyleParserCS.csskit.fn.ScaleYImpl;
    using ScaleZImpl = StyleParserCS.csskit.fn.ScaleZImpl;
    using SepiaImpl = StyleParserCS.csskit.fn.SepiaImpl;
    using SkewImpl = StyleParserCS.csskit.fn.SkewImpl;
    using SkewXImpl = StyleParserCS.csskit.fn.SkewXImpl;
    using SkewYImpl = StyleParserCS.csskit.fn.SkewYImpl;
    using Translate3dImpl = StyleParserCS.csskit.fn.Translate3dImpl;
    using TranslateImpl = StyleParserCS.csskit.fn.TranslateImpl;
    using TranslateXImpl = StyleParserCS.csskit.fn.TranslateXImpl;
    using TranslateYImpl = StyleParserCS.csskit.fn.TranslateYImpl;
    using TranslateZImpl = StyleParserCS.csskit.fn.TranslateZImpl;
    using TermOperator = StyleParserCS.css.TermOperator;
    using CubicBezierImpl = StyleParserCS.csskit.fn.CubicBezierImpl;
    using FitContentImpl = StyleParserCS.csskit.fn.FitContentImpl;
    using FramesImpl = StyleParserCS.csskit.fn.FramesImpl;
    using MinMaxImpl = StyleParserCS.csskit.fn.MinMaxImpl;
    using RepeatImpl = StyleParserCS.csskit.fn.RepeatImpl;
    using StepsImpl = StyleParserCS.csskit.fn.StepsImpl;
    using System.Globalization;

    public class TermFactoryImpl : TermFactory
    {

        private static readonly TermFactory instance;

        static TermFactoryImpl()
        {
            instance = new TermFactoryImpl();
        }

        public static TermFactory Instance
        {
            get
            {
                return instance;
            }
        }

        public TermFactoryImpl()
        {
        }

        public virtual TermAngle createAngle(float value)
        {
            return (TermAngle)(new TermAngleImpl()).setValue(value);
        }

        public virtual TermAngle createAngle(string value, TermLength_Unit unit, int unary)
        {
            return (TermAngle)(new TermAngleImpl()).setUnit(unit).setValue(convertFloat(value, unit.value(), unary));
        }

        public virtual TermCalc createCalc(IList<Term> args)
        {
            CalcArgs cargs = new CalcArgs(args);
            if (cargs.Valid)
            {
                switch (cargs.Type)
                {
                    case TermLength_Unit.TermType.length:
                        return new TermCalcLengthImpl(cargs);
                    case TermLength_Unit.TermType.angle:
                        return new TermCalcAngleImpl(cargs);
                    case TermLength_Unit.TermType.frequency:
                        return new TermCalcFrequencyImpl(cargs);
                    case TermLength_Unit.TermType.resolution:
                        return new TermCalcResolutionImpl(cargs);
                    case TermLength_Unit.TermType.time:
                        return new TermCalcTimeImpl(cargs);
                    case TermLength_Unit.TermType.none:
                        if (cargs.Int)
                        {
                            return new TermCalcIntegerImpl(cargs);
                        }
                        else
                        {
                            return new TermCalcNumberImpl(cargs);
                        }
                }
            }
            return null;
        }

        public virtual TermColor createColor(TermIdent ident)
        {
            return TermColorImpl.getColorByIdent(ident);
        }

        public virtual TermColor createColor(string hash)
        {
            return TermColorImpl.getColorByHash(hash);
        }

        public virtual TermColor createColor(int r, int g, int b)
        {
            return new TermColorImpl(r, g, b);
        }

        public virtual TermColor createColor(int r, int g, int b, int a)
        {
            return new TermColorImpl(r, g, b, a);
        }

        public virtual TermColor createColor(TermFunction function)
        {
            return TermColorImpl.getColorByFunction(function);
        }

        public virtual TermFrequency createFrequency(float value)
        {
            return (TermFrequency)(new TermFrequencyImpl()).setValue(value);
        }

        public virtual TermFrequency createFrequency(string value, TermLength_Unit unit, int unary)
        {
            return (TermFrequency)(new TermFrequencyImpl()).setUnit(unit).setValue(convertFloat(value, unit.value(), unary));
        }

        public virtual TermExpression createExpression(string expr)
        {
            return (new TermExpressionImpl()).setValue(expr);
        }

        public virtual TermFunction createFunction(string name)
        {
            return createFunctionByName(name, null);
        }

        public virtual TermFunction createFunction(string name, IList<Term> args)
        {
            return createFunctionByName(name, args);
        }

        public virtual TermIdent createIdent(string value)
        {
            return (TermIdent)(new TermIdentImpl()).setValue(value);
        }

        public virtual TermIdent createIdent(string value, bool dash)
        {
            if (!dash)
            {
                return (TermIdent)(new TermIdentImpl()).setValue(value);
            }
            else
            {
                return (TermIdent)(new TermIdentImpl()).setValue("-" + value);
            }
        }

        public virtual TermBracketedIdents createBracketedIdents()
        {
            return new TermBracketedIdentsImpl();
        }

        public virtual TermBracketedIdents createBracketedIdents(int initialSize)
        {
            return new TermBracketedIdentsImpl(initialSize);
        }

        public virtual TermInteger createInteger(int value)
        {
            return (TermInteger)(new TermIntegerImpl()).setValue(value);
        }

        public virtual TermInteger createInteger(string value, int unary)
        {
            int? retInt = convertInteger(value, null, unary);
            if (retInt == null)
            {
                return null;
            }
            return (TermInteger)(new TermIntegerImpl()).setValue(retInt.Value);
        }

        public virtual TermLength createLength(float value)
        {
            return (TermLength)(new TermLengthImpl()).setValue(value);
        }

        public virtual TermLength createLength(float value, TermLength_Unit unit)
        {
            return (TermLength)(new TermLengthImpl()).setUnit(unit).setValue(value);
        }

        public virtual TermLength createLength(string value, TermLength_Unit unit, int unary)
        {
            return (TermLength)(new TermLengthImpl()).setUnit(unit).setValue(convertFloat(value, unit.value(), unary));
        }

        public virtual TermList createList()
        {
            return new TermListImpl();
        }

        public virtual TermList createList(int initialSize)
        {
            return new TermListImpl(initialSize);
        }

        public virtual TermNumber createNumber(float value)
        {
            return (TermNumber)(new TermNumberImpl()).setValue(value);
        }

        public virtual TermNumber createNumber(string value, int unary)
        {
            return (TermNumber)(new TermNumberImpl()).setValue(convertFloat(value, null, unary));
        }

        //ORIGINAL LINE: public StyleParserCS.css.TermNumeric<?> createNumeric(String value, int unary)
        public virtual TermNumeric createNumeric(string value, int unary)
        {
            TermNumeric retNumeric = (TermNumeric)createInteger(value, unary);
            if (retNumeric != null)
            {
                return retNumeric;
            }
            return (TermNumeric)createNumber(value, unary); ;
            /*
            try
            {
                return (TermNumeric)createInteger(value, unary);
            }
            catch (System.ArgumentException)
            {
                return (TermNumeric)createNumber(value, unary);
            }
            */
        }

        public virtual TermResolution createResolution(float value)
        {
            return (TermResolution)(new TermResolutionImpl()).setValue(value);
        }

        public virtual TermResolution createResolution(string value, TermLength_Unit unit, int unary)
        {
            return (TermResolution)(new TermResolutionImpl()).setUnit(unit).setValue(convertFloat(value, unit.value(), unary));
        }

        public virtual TermNumeric<float> createDimension(string value, int unary)
        {
            //find the end of the numeric value
            int valend = value.Length - 1;
            while (valend >= 0 && !(value[valend] >= '0' && value[valend] <= '9'))
            {
                valend--;
            }
            //split the number and the unit
            if (valend >= 0 && valend < value.Length - 1)
            {
                //ORIGINAL LINE: final String upart = value.substring(valend + 1);
                string upart = value.Substring(valend + 1);
                //ORIGINAL LINE: final StyleParserCS.css.TermLength_Unit unit = StyleParserCS.css.TermLength_Unit.findByValue(upart.toLowerCase());
                TermLength_Unit unit = TermLength_Unit.findByValue(upart.ToLower());
                if (unit != null)
                {
                    //ORIGINAL LINE: final String vpart = value.substring(0, valend + 1);
                    string vpart = value.Substring(0, valend + 1);
                    float f;
                    if (float.TryParse(vpart, NumberStyles.Float, CultureInfo.InvariantCulture, out f) == false)
                    {
                        return null;
                    }
                    /*
                    try
                    {
                        f = float.Parse(vpart, CultureInfo.InvariantCulture) * unary;
                    }
                    catch (System.FormatException)
                    {
                        return null; //not a float number
                    }
                    */
                    switch (unit.Type)
                    {
                        case TermLength_Unit.TermType.angle:
                            return (TermNumeric<float>)(new TermAngleImpl()).setUnit(unit).setValue(f);
                        case TermLength_Unit.TermType.frequency:
                            return (TermNumeric<float>)(new TermFrequencyImpl()).setUnit(unit).setValue(f);
                        case TermLength_Unit.TermType.length:
                            return (TermNumeric<float>)(new TermLengthImpl()).setUnit(unit).setValue(f);
                        case TermLength_Unit.TermType.resolution:
                            return (TermNumeric<float>)(new TermResolutionImpl()).setUnit(unit).setValue(f);
                        case TermLength_Unit.TermType.time:
                            return (TermNumeric<float>)(new TermTimeImpl()).setUnit(unit).setValue(f);
                        default:
                            return null;
                    }
                }
                else
                {
                    return null; //unknown unit
                }
            }
            else
            {
                return null; //value or unit missing
            }
        }

        //ORIGINAL LINE: @SuppressWarnings("unchecked") public <K, V> StyleParserCS.css.TermPair<K, V> createPair(K key, V value)
        public virtual TermPair<K, V> createPair<K, V>(K key, V value)
        {
            return (TermPair<K, V>)(new TermPairImpl<K, V>()).setKey(key).setValue(value);
        }

        public virtual TermPercent createPercent(float value)
        {
            return (TermPercent)(new TermPercentImpl()).setValue(value);
        }

        public virtual TermPercent createPercent(string value, int unary)
        {
            return (TermPercent)(new TermPercentImpl()).setValue(convertFloat(value, OutputUtil.PERCENT_SIGN, unary));
        }

        public virtual TermPropertyValue createPropertyValue(CSSProperty property, Term value)
        {
            return (TermPropertyValue)(new TermPropertyValueImpl()).setKey(property).setValue(value);
        }

        public virtual TermRect createRect(TermFunction function)
        {
            if (function.FunctionName.Equals("rect", StringComparison.OrdinalIgnoreCase))
            {
                //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = function.getValues(true);
                IList<Term> args = function.getValues(true); //try the rect(0 0 0 0) syntax
                if (args == null || args.Count != 4)
                {
                    args = function.getSeparatedValues((Term)CSSFactory.TermFactory.createOperator(','), true); //try the rect(0, 0, 0, 0) syntax
                }
                if (args != null && args.Count == 4)
                { //check the argument count and types
                    for (int i = 0; i < 4; i++)
                    {
                        //ORIGINAL LINE: StyleParserCS.css.Term<?> val = args.get(i);
                        Term val = args[i];
                        if (val is TermIdent)
                        {
                            if (((TermIdent)val).Value.ToLower() == "auto") //replace 'auto' with null
                            {
                                args[i] = null;
                            }
                        }
                        else if (!(val is TermLength))
                        {
                            return null;
                        }
                    }
                    return createRect((TermLength)args[0], (TermLength)args[1], (TermLength)args[2], (TermLength)args[3]);
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

        public virtual TermRect createRect(TermLength a, TermLength b, TermLength c, TermLength d)
        {
            return new TermRectImpl(a, b, c, d);
        }

        public virtual TermString createString(string value)
        {
            return (new TermStringImpl()).setValue(value);
        }

        public virtual Term<V> createTerm<V>(V value)
        {
            return (new TermImpl<V>()).setValue(value);
        }

        public virtual TermTime createTime(float value)
        {
            return (TermTime)(new TermTimeImpl()).setUnit(TermLength_Unit.s).setValue(value);
        }

        public virtual TermTime createTime(float value, TermLength_Unit unit)
        {
            return (TermTime)(new TermTimeImpl()).setUnit(unit).setValue(value);
        }

        public virtual TermTime createTime(string value, TermLength_Unit unit, int unary)
        {
            return (TermTime)(new TermTimeImpl()).setUnit(unit).setValue(convertFloat(value, unit.value(), unary));
        }

        public virtual TermUnicodeRange createUnicodeRange(string value)
        {
            return (new TermUnicodeRangeImpl()).setValue(value);
        }

        public virtual TermURI createURI(string value)
        {
            return (new TermURIImpl()).setValue(value);
        }

        public virtual TermURI createURI(string value, Uri basev)
        {
            return (new TermURIImpl()).setValue(value).setBase(basev);
        }

        public virtual TermOperator createOperator(char value)
        {
            return (TermOperator)(new TermOperatorImpl()).setValue(value);
        }

        /// <summary>
        ///**********************************************************************
        /// HELPERS *
        /// ***********************************************************************
        /// </summary>

        //ORIGINAL LINE: protected System.Nullable<float> convertFloat(String value, String unit, int unary) throws IllegalArgumentException
        protected internal virtual float convertFloat(string value, string unit, int unary)
        {

            try
            {
                if (!string.ReferenceEquals(unit, null))
                {
                    // trim & lowercase
                    value = value.Trim().ToLower();
                    // trim units from value
                    if (value.EndsWith(unit, StringComparison.Ordinal))
                    {
                        value = value.Substring(0, value.Length - unit.Length);
                    }
                }
                return float.Parse(value, CultureInfo.InvariantCulture) * unary;
            }
            catch (System.FormatException e)
            {
                throw new System.ArgumentException("Invalid number format " + value, e);
            }
            catch (System.NullReferenceException)
            {
                throw new System.ArgumentException("Invalid null format");
            }
        }

        // TOCHECK!!!!
        //ORIGINAL LINE: protected System.Nullable<int> convertInteger(String value, String unit, int unary) throws IllegalArgumentException
        protected internal virtual int? convertInteger(string value, string unit, int unary)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            if (!string.ReferenceEquals(unit, null))
            {
                // trim & lowercase
                value = value.Trim().ToLower();
                // trim units from value
                if (value.EndsWith(unit, StringComparison.Ordinal))
                {
                    value = value.Substring(0, value.Length - unit.Length);
                }
            }

            if (Int64.TryParse(value, out long lval))
            {
                lval *= unary;
                if (lval > int.MaxValue)
                {
                    return int.MaxValue;
                }
                else if (lval < int.MinValue)
                {
                    return int.MinValue;
                }
                else
                {
                    return (int)lval;
                }
            }
            return null;
            // long lval = Convert.ToInt64(value) * unary;
            /*
            try
            {
                if (!string.ReferenceEquals(unit, null))
                {
                    // trim & lowercase
                    value = value.Trim().ToLower();
                    // trim units from value
                    if (value.EndsWith(unit, StringComparison.Ordinal))
                    {
                        value = value.Substring(0, value.Length - unit.Length);
                    }
                }

                long lval = Convert.ToInt64(value) * unary;
                if (lval > int.MaxValue)
                {
                    return int.MaxValue;
                }
                else if (lval < int.MinValue)
                {
                    return int.MinValue;
                }
                else
                {
                    return (int)lval;
                }
            }
            catch (System.FormatException e)
            {
                throw new System.ArgumentException("Invalid number format " + value, e);
            }
            catch (System.NullReferenceException)
            {
                throw new System.ArgumentException("Invalid null format");
            }
            */
        }

        protected internal virtual TermFunction createFunctionByName(string name, IList<Term> args)
        {
            TermFunction fn;
            switch (name)
            {
                case "matrix":
                    fn = new MatrixImpl();
                    break;
                case "matrix3d":
                    fn = new Matrix3dImpl();
                    break;
                case "perspective":
                    fn = new PerspectiveImpl();
                    break;
                case "rotate":
                    fn = new RotateImpl();
                    break;
                case "rotate3d":
                    fn = new Rotate3dImpl();
                    break;
                case "rotatex":
                    fn = new RotateXImpl();
                    break;
                case "rotatey":
                    fn = new RotateYImpl();
                    break;
                case "rotatez":
                    fn = new RotateZImpl();
                    break;
                case "scale":
                    fn = new ScaleImpl();
                    break;
                case "scale3d":
                    fn = new Scale3dImpl();
                    break;
                case "scalex":
                    fn = new ScaleXImpl();
                    break;
                case "scaley":
                    fn = new ScaleYImpl();
                    break;
                case "scalez":
                    fn = new ScaleZImpl();
                    break;
                case "skew":
                    fn = new SkewImpl();
                    break;
                case "skewx":
                    fn = new SkewXImpl();
                    break;
                case "skewy":
                    fn = new SkewYImpl();
                    break;
                case "translate":
                    fn = new TranslateImpl();
                    break;
                case "translate3d":
                    fn = new Translate3dImpl();
                    break;
                case "translatex":
                    fn = new TranslateXImpl();
                    break;
                case "translatey":
                    fn = new TranslateYImpl();
                    break;
                case "translatez":
                    fn = new TranslateZImpl();
                    break;
                case "linear-gradient":
                    fn = new LinearGradientImpl();
                    break;
                case "repeating-linear-gradient":
                    fn = new RepeatingLinearGradientImpl();
                    break;
                case "radial-gradient":
                    fn = new RadialGradientImpl();
                    break;
                case "repeating-radial-gradient":
                    fn = new RepeatingRadialGradientImpl();
                    break;
                case "blur":
                    fn = new BlurImpl();
                    break;
                case "brightness":
                    fn = new BrightnessImpl();
                    break;
                case "contrast":
                    fn = new ContrastImpl();
                    break;
                case "drop-shadow":
                    fn = new DropShadowImpl();
                    break;
                case "grayscale":
                    fn = new GrayscaleImpl();
                    break;
                case "hue-rotate":
                    fn = new HueRotateImpl();
                    break;
                case "invert":
                    fn = new InvertImpl();
                    break;
                case "opacity":
                    fn = new OpacityImpl();
                    break;
                case "saturate":
                    fn = new SaturateImpl();
                    break;
                case "sepia":
                    fn = new SepiaImpl();
                    break;
                case "counter":
                    fn = new CounterImpl();
                    break;
                case "counters":
                    fn = new CountersImpl();
                    break;
                case "attr":
                    fn = new AttrImpl();
                    break;
                case "fit-content":
                    fn = new FitContentImpl();
                    break;
                case "minmax":
                    fn = new MinMaxImpl();
                    break;
                case "repeat":
                    fn = new RepeatImpl();
                    break;
                case "cubic-bezier":
                    fn = new CubicBezierImpl();
                    break;
                case "steps":
                    fn = new StepsImpl();
                    break;
                case "frames":
                    fn = new FramesImpl();
                    break;
                default:
                    fn = new TermFunctionImpl();
                    break;
            }
            if (fn != null)
            {
                fn.setFunctionName(name);
                fn.setValue(args);
                if (!fn.Valid)
                {
                    fn = null; //invalid arguments
                }
            }
            return fn;
        }

    }

}