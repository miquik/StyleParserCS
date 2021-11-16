using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StyleParserCS.css
{
    public interface TermNumeric: Term
    {
        /// <summary>
        /// Returns unit of type or <code>null</code> if not defined
        /// for numeric types that does not allow units </summary>
        /// <returns> Unit or <code>null</code> </returns>
        TermLength_Unit Unit { get; }
        /// <summary>
        /// Sets unit </summary>
        /// <param name="unit"> Unit value </param>
        /// <returns> Modified object to allow chaining </returns>
        TermNumeric setUnit(TermLength_Unit unit);

        /// <summary>
        /// Sets the value to zero. </summary>
        /// <returns> Modified object to allow chaining </returns>
        TermNumeric setZero();
    }



    /// <summary>
    /// Holds value of numeric type. This could be integer or float
    /// according to &lt;T&gt;.
    /// @author kapy
    /// @author burgetr
    /// </summary>
    /// @param <T> Type of value stored in term </param>
    public interface TermNumeric<T> : Term<T>, TermNumeric // TODO: where T : Numeric
    {
        /// <summary>
        /// Sets unit </summary>
        /// <param name="unit"> Unit value </param>
        /// <returns> Modified object to allow chaining </returns>
        new TermNumeric<T> setUnit(TermLength_Unit unit);

        /// <summary>
        /// Sets the value to zero. </summary>
        /// <returns> Modified object to allow chaining </returns>
        new TermNumeric<T> setZero();
    }

    public sealed class TermLength_Unit : SmartEnum<TermLength_Unit, string>
    {
        public static readonly TermLength_Unit none = new TermLength_Unit("none", "", TermType.none);
        public static readonly TermLength_Unit em = new TermLength_Unit("em", "em", TermType.length);
        public static readonly TermLength_Unit ex = new TermLength_Unit("ex", "ex", TermType.length);
        public static readonly TermLength_Unit ch = new TermLength_Unit("ch", "ch", TermType.length);
        public static readonly TermLength_Unit rem = new TermLength_Unit("rem", "rem", TermType.length);
        public static readonly TermLength_Unit vw = new TermLength_Unit("vw", "vw", TermType.length);
        public static readonly TermLength_Unit vh = new TermLength_Unit("vh", "vh", TermType.length);
        public static readonly TermLength_Unit vmin = new TermLength_Unit("vmin", "vmin", TermType.length);
        public static readonly TermLength_Unit vmax = new TermLength_Unit("vmax", "vmax", TermType.length);
        public static readonly TermLength_Unit cm = new TermLength_Unit("cm", "cm", TermType.length);
        public static readonly TermLength_Unit mm = new TermLength_Unit("mm", "mm", TermType.length);
        public static readonly TermLength_Unit q = new TermLength_Unit("q", "q", TermType.length);
        public static readonly TermLength_Unit inch = new TermLength_Unit("inch", "inch", TermType.length);
        public static readonly TermLength_Unit pt = new TermLength_Unit("pt", "pt", TermType.length);
        public static readonly TermLength_Unit pc = new TermLength_Unit("pc", "pc", TermType.length);
        public static readonly TermLength_Unit px = new TermLength_Unit("px", "px", TermType.length);
        public static readonly TermLength_Unit fr = new TermLength_Unit("fr", "fr", TermType.length);
        public static readonly TermLength_Unit deg = new TermLength_Unit("deg", "deg", TermType.angle);
        public static readonly TermLength_Unit rad = new TermLength_Unit("rad", "rad", TermType.angle);
        public static readonly TermLength_Unit grad = new TermLength_Unit("grad", "grad", TermType.angle);
        public static readonly TermLength_Unit turn = new TermLength_Unit("turn", "turn", TermType.angle);
        public static readonly TermLength_Unit ms = new TermLength_Unit("ms", "ms", TermType.time);
        public static readonly TermLength_Unit s = new TermLength_Unit("s", "s", TermType.time);
        public static readonly TermLength_Unit hz = new TermLength_Unit("hz", "hz", TermType.frequency);
        public static readonly TermLength_Unit khz = new TermLength_Unit("khz", "khz", TermType.frequency);
        public static readonly TermLength_Unit dpi = new TermLength_Unit("dpi", "dpi", TermType.resolution);
        public static readonly TermLength_Unit dpcm = new TermLength_Unit("dpcm", "dpcm", TermType.resolution);
        public static readonly TermLength_Unit dppx = new TermLength_Unit("dppx", "dppx", TermType.resolution);

        public enum TermType
        {
            angle,
            length,
            time,
            frequency,
            resolution,
            none
        }
        internal static readonly IDictionary<string, TermLength_Unit> map;
        static TermLength_Unit()
        {
            map = List.ToDictionary(k => k.Value, v => v);
        }

        internal string value_Conflict;
        internal TermType type;

        internal TermLength_Unit(string name, string value, TermType type) : base(name, value)
        {
            this.type = type;
        }

        public string value()
        {
            return Value;
        }

        public TermType Type
        {
            get
            {
                return type;
            }
        }

        public static TermLength_Unit findByValue(string value)
        {
            return map.ContainsKey(value) ? map[value] : null;
        }

        public bool Angle
        {
            get
            {
                return Type == TermType.angle;
            }
        }

        public bool Length
        {
            get
            {
                return Type == TermType.length;
            }
        }

        public bool Time
        {
            get
            {
                return Type == TermType.time;
            }
        }

        public bool Frequency
        {
            get
            {
                return Type == TermType.frequency;
            }
        }

        public bool Resolution
        {
            get
            {
                return Type == TermType.resolution;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }

}