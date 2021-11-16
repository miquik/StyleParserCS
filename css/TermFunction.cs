using Ardalis.SmartEnum;
using System.Collections.Generic;

namespace StyleParserCS.css
{

    /// <summary>
    /// Holds name of CSS function and terms stored inside
    /// @author Jan Svercl, VUT Brno, 2008
    /// @author Karel Piwko, 2008
    /// </summary>
    public interface TermFunction : TermList
    {

        /// <summary>
        /// Gets the name of the function. </summary>
        /// <returns> The function name. </returns>
        string FunctionName { get; }

        TermFunction setFunctionName(string functionName);

        /// <summary>
        /// Checks whether the arguments passed using {@code setValue()} are valid for this function. </summary>
        /// <returns> {@code true} when the function has valid arguments </returns>
        bool Valid { get; }

        /// <summary>
        /// Splits the list of arguments to several lists based on the given separator term. </summary>
        /// <param name="separator"> The term used as a separator (typically TermOperator(','). </param>
        /// <returns> A list of term lists corresponding to the individual separated arguments. </returns>
        //ORIGINAL LINE: public java.util.List<java.util.List<Term<?>>> getSeparatedArgs(Term<?> separator);
        IList<IList<Term>> getSeparatedArgs(Term separator);

        /// <summary>
        /// Splits the list of arguments using the given separator term and tries to convert the
        /// arguments to typed values (lengths, times, numbers, etc...) </summary>
        /// <param name="separator"> The term used as a separator (typically TermOperator(','). </param>
        /// <returns> A list of typed numeric values or idents (if keywords allowed) or {@code null} in case that the arguments cannot be converted to values.  </returns>
        //ORIGINAL LINE: public java.util.List<Term<?>> getSeparatedValues(Term<?> separator, boolean allowKeywords);
        IList<Term> getSeparatedValues(Term separator, bool allowKeywords);

        /// <summary>
        /// Tries to convert the terms to a list of typed values (lengths, times, numbers, etc...). </summary>
        /// <returns> A list of typed numeric values or idents (if keywords allowed) or {@code null} in case that the arguments cannot be converted to values.  </returns>
        //ORIGINAL LINE: public java.util.List<Term<?>> getValues(boolean allowKeywords);
        IList<Term> getValues(bool allowKeywords);


        //========================================================================

        //========================================================================

        //========================================================================

        //========================================================================

        //========================================================================

        //========================================================================

    }

    public interface TermFunction_TransformFunction : TermFunction
    {

    }

    public interface TermFunction_Matrix : TermFunction_TransformFunction
    {
        float[] Values { get; }
    }

    public interface TermFunction_Matrix3d : TermFunction_TransformFunction
    {
        float[] Values { get; }
    }

    public interface TermFunction_Perspective : TermFunction_TransformFunction
    {
        TermLength Distance { get; }
    }

    public interface TermFunction_Rotate : TermFunction_TransformFunction
    {
        TermAngle Angle { get; }
    }

    public interface TermFunction_Rotate3d : TermFunction_TransformFunction
    {
        float X { get; }
        float Y { get; }
        float Z { get; }
        TermAngle Angle { get; }
    }

    public interface TermFunction_RotateX : TermFunction_TransformFunction
    {
        TermAngle Angle { get; }
    }

    public interface TermFunction_RotateY : TermFunction_TransformFunction
    {
        TermAngle Angle { get; }
    }

    public interface TermFunction_RotateZ : TermFunction_TransformFunction
    {
        TermAngle Angle { get; }
    }

    public interface TermFunction_Scale : TermFunction_TransformFunction
    {
        float ScaleX { get; }
        float ScaleY { get; }
    }

    public interface TermFunction_Scale3d : TermFunction_TransformFunction
    {
        float ScaleX { get; }
        float ScaleY { get; }
        float ScaleZ { get; }
    }

    public interface TermFunction_ScaleX : TermFunction_TransformFunction
    {
        float Scale { get; }
    }

    public interface TermFunction_ScaleY : TermFunction_TransformFunction
    {
        float Scale { get; }
    }

    public interface TermFunction_ScaleZ : TermFunction_TransformFunction
    {
        float Scale { get; }
    }

    public interface TermFunction_Skew : TermFunction_TransformFunction
    {
        TermAngle SkewX { get; }
        TermAngle SkewY { get; }
    }

    public interface TermFunction_SkewX : TermFunction_TransformFunction
    {
        TermAngle Skew { get; }
    }

    public interface TermFunction_SkewY : TermFunction_TransformFunction
    {
        TermAngle Skew { get; }
    }

    public interface TermFunction_Translate : TermFunction_TransformFunction
    {
        TermLengthOrPercent TranslateX { get; }
        TermLengthOrPercent TranslateY { get; }
    }

    public interface TermFunction_Translate3d : TermFunction_TransformFunction
    {
        TermLengthOrPercent TranslateX { get; }
        TermLengthOrPercent TranslateY { get; }
        TermLengthOrPercent TranslateZ { get; }
    }

    public interface TermFunction_TranslateX : TermFunction_TransformFunction
    {
        TermLengthOrPercent Translate { get; }
    }

    public interface TermFunction_TranslateY : TermFunction_TransformFunction
    {
        TermLengthOrPercent Translate { get; }
    }

    public interface TermFunction_TranslateZ : TermFunction_TransformFunction
    {
        TermLengthOrPercent Translate { get; }
    }

    public interface TermFunction_Gradient : TermFunction
    {

        /// <summary>
        /// Is this a repeating gradient? </summary>
        bool Repeating { get; }
    }

    public interface TermFunction_Gradient_ColorStop
    {
        TermColor Color { get; }
        TermLengthOrPercent Length { get; }
    }

    public interface TermFunction_LinearGradient : TermFunction_Gradient
    {
        TermAngle Angle { get; }
        IList<TermFunction_Gradient_ColorStop> ColorStops { get; }
    }

    public interface TermFunction_RadialGradient : TermFunction_Gradient
    {
        /// <summary>
        /// Obtains the shape (ELLIPSE or CIRCLE) </summary>
        TermIdent Shape { get; }
        /// <summary>
        /// Obtains the circle/ellipse sizes (one value for circle, two for ellipse) </summary>
        TermLengthOrPercent[] Size { get; }
        /// <summary>
        /// Obtains the circle/ellipse size specified by an identifier (e.g. closest-side) </summary>
        TermIdent SizeIdent { get; }
        /// <summary>
        /// Obtains the 'at' position (always two values) </summary>
        TermLengthOrPercent[] Position { get; }
        /// <summary>
        /// Obtains the color stops </summary>
        IList<TermFunction_Gradient_ColorStop> ColorStops { get; }
    }

    public interface TermFunction_FilterFunction : TermFunction
    {
    }

    public interface TermFunction_Blur : TermFunction_FilterFunction
    {
        TermLength Radius { get; }
    }

    public interface TermFunction_Brightness : TermFunction_FilterFunction
    {
        float Amount { get; }
    }

    public interface TermFunction_Contrast : TermFunction_FilterFunction
    {
        float Amount { get; }
    }

    public interface TermFunction_DropShadow : TermFunction_FilterFunction
    {
        TermLength OffsetX { get; }
        TermLength OffsetY { get; }
        TermLength BlurRadius { get; }
        TermColor Color { get; }
    }

    public interface TermFunction_Grayscale : TermFunction_FilterFunction
    {
        float Amount { get; }
    }

    public interface TermFunction_HueRotate : TermFunction_FilterFunction
    {
        TermAngle Angle { get; }
    }

    public interface TermFunction_Invert : TermFunction_FilterFunction
    {
        float Amount { get; }
    }

    public interface TermFunction_Opacity : TermFunction_FilterFunction
    {
        float Amount { get; }
    }

    public interface TermFunction_Saturate : TermFunction_FilterFunction
    {
        float Amount { get; }
    }

    public interface TermFunction_Sepia : TermFunction_FilterFunction
    {
        float Amount { get; }
    }

    public interface TermFunction_CounterFunction : TermFunction
    {
    }

    public interface TermFunction_Counter : TermFunction_CounterFunction
    {
        string Name { get; }
        CSSProperty_ListStyleType Style { get; }
    }

    public interface TermFunction_Counters : TermFunction_CounterFunction
    {
        string Name { get; }
        CSSProperty_ListStyleType Style { get; }
        string Separator { get; }
    }

    public interface TermFunction_Attr : TermFunction
    {
        string Name { get; }
    }

    public interface TermFunction_GridFunction : TermFunction
    {
    }

    public interface TermFunction_FitContent : TermFunction_GridFunction
    {

        TermLengthOrPercent Maximum { get; }
    }

    public interface TermFunction_MinMax : TermFunction_GridFunction
    {

        TermFunction_MinMax_Unit Min { get; }

        TermFunction_MinMax_Unit Max { get; }
    }

    public class TermFunction_MinMax_Unit
    {

        internal TermLengthOrPercent _lenght;
        internal bool _isMinContent;
        internal bool _isMaxContent;
        internal bool _isAuto;

        public static TermFunction_MinMax_Unit createWithLenght(TermLengthOrPercent lenght)
        {
            return new TermFunction_MinMax_Unit(lenght, false, false, false);
        }

        public static TermFunction_MinMax_Unit createWithMinContent()
        {
            return new TermFunction_MinMax_Unit(null, true, false, false);
        }

        public static TermFunction_MinMax_Unit createWithMaxContent()
        {
            return new TermFunction_MinMax_Unit(null, false, true, false);
        }

        public static TermFunction_MinMax_Unit createWithAuto()
        {
            return new TermFunction_MinMax_Unit(null, false, false, true);
        }

        internal TermFunction_MinMax_Unit(TermLengthOrPercent lenght, bool isMinContent, bool isMaxContent, bool isAuto)
        {
            _lenght = lenght;
            _isMinContent = isMinContent;
            _isMaxContent = isMaxContent;
            _isAuto = isAuto;
        }

        public virtual TermLengthOrPercent Lenght
        {
            get
            {
                return _lenght;
            }
            set
            {
                _lenght = value;
                _isMinContent = false;
                _isMaxContent = false;
                _isAuto = false;
            }
        }


        public virtual bool IsMinContent
        {
            get
            {
                return _isMinContent;
            }
            set
            {
                _lenght = null;
                _isMinContent = value;
                _isMaxContent = false;
                _isAuto = false;
            }
        }


        public virtual bool IsMaxContent
        {
            get
            {
                return _isMaxContent;
            }
            set
            {
                _lenght = null;
                _isMinContent = false;
                _isMaxContent = value;
                _isAuto = false;
            }
        }


        public virtual bool IsAuto
        {
            get
            {
                return _isAuto;
            }
            set
            {
                _lenght = null;
                _isMinContent = false;
                _isMaxContent = false;
                _isAuto = value;
            }
        }

    }

    public interface TermFunction_Repeat : TermFunction_GridFunction
    {

        TermFunction_Repeat_Unit NumberOfRepetitions { get; }

        //ORIGINAL LINE: public java.util.List<Term<?>> getRepeatedTerms();
        IList<Term> RepeatedTerms { get; }
    }

    public class TermFunction_Repeat_Unit
    {

        internal int _numberOfRepetition;
        internal bool _isAutoFit;
        internal bool _isAutoFill;

        public static TermFunction_Repeat_Unit createWithNRepetitions(int n)
        {
            return new TermFunction_Repeat_Unit(n, false, false);
        }

        public static TermFunction_Repeat_Unit createWithAutoFit()
        {
            return new TermFunction_Repeat_Unit(-1, true, false);
        }

        public static TermFunction_Repeat_Unit createWithAutoFill()
        {
            return new TermFunction_Repeat_Unit(-1, false, true);
        }

        internal TermFunction_Repeat_Unit(int numberOfRepetitions, bool isAutoFit, bool isAutoFill)
        {
            _numberOfRepetition = numberOfRepetitions;
            _isAutoFit = isAutoFit;
            _isAutoFill = isAutoFill;
        }

        public virtual TermFunction_Repeat_Unit setNumberOfRepetition(int n)
        {
            if (n <= 0)
            {
                throw new System.ArgumentException("Number of repetitions must be positive.");
            }
            _isAutoFit = false;
            _isAutoFill = false;
            _numberOfRepetition = n;
            return this;
        }

        public virtual TermFunction_Repeat_Unit setAutoFit()
        {
            _isAutoFit = true;
            _isAutoFill = false;
            _numberOfRepetition = -1;
            return this;
        }

        public virtual TermFunction_Repeat_Unit setAutoFill()
        {
            _isAutoFit = false;
            _isAutoFill = true;
            _numberOfRepetition = -1;
            return this;
        }

        public virtual int NumberOfRepetitions
        {
            get
            {
                return _numberOfRepetition;
            }
        }

        public virtual bool AutoFit
        {
            get
            {
                return _isAutoFit;
            }
        }

        public virtual bool AutoFill
        {
            get
            {
                return _isAutoFill;
            }
        }
    }

    public interface TermFunction_TimingFunction : TermFunction
    {
    }

    public interface TermFunction_Steps : TermFunction_TimingFunction
    {

        int NumberOfSteps { get; }
        TermFunction_Steps_Direction Direction { get; }
    }

    public sealed class TermFunction_Steps_Direction : SmartEnum<TermFunction_Steps_Direction, string>
    {
        public static readonly TermFunction_Steps_Direction JUMP_START = new TermFunction_Steps_Direction("JUMP_START", "jump-start");
        public static readonly TermFunction_Steps_Direction JUMP_END = new TermFunction_Steps_Direction("JUMP_END", "jump-end");
        public static readonly TermFunction_Steps_Direction JUMP_BOTH = new TermFunction_Steps_Direction("JUMP_BOTH", "jump-both");
        public static readonly TermFunction_Steps_Direction JUMP_NONE = new TermFunction_Steps_Direction("JUMP_NONE", "jump-none");
        public static readonly TermFunction_Steps_Direction START = new TermFunction_Steps_Direction("START", "start");
        public static readonly TermFunction_Steps_Direction END = new TermFunction_Steps_Direction("END", "end");
        internal TermFunction_Steps_Direction(string name, string text) : base(name, text)
        {
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public interface TermFunction_Frames : TermFunction_TimingFunction
    {
        int Frames { get; }
    }

    public interface TermFunction_CubicBezier : TermFunction_TimingFunction
    {
        float X1 { get; }
        float Y1 { get; }
        float X2 { get; }
        float Y2 { get; }
    }

}