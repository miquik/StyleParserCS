using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// MediaSpec.java
/// 
/// Created on 25. 6. 2014, 13:28:43 by burgetr
/// </summary>
namespace StyleParserCS.css
{


    /// <summary>
    /// This class represents the features of the output media used for displaying the document. It specifies the name
    /// of the output media type (e.g. "screen", "print", etc.) and further features defined by the CSS3 specification.
    /// The default values of the features are the following (corresponding to a standard desktop station):
    /// 
    /// <ul>
    /// <li><code>width: 1100px</code></li>
    /// <li><code>height: 850px</code></li>
    /// <li><code>device-width: 1920px</code></li>
    /// <li><code>device-height: 1200px</code></li>
    /// <li><code>color: 8</code></li>
    /// <li><code>color-index: 0</code></li>
    /// <li><code>monochrome: 0</code></li>
    /// <li><code>resolution: 96dpi</code></li>
    /// <li><code>scan: progressive</code></li>
    /// <li><code>grid: 0</code></li>
    /// </ul>
    /// 
    /// @author burgetr
    /// </summary>
    public class MediaSpec
    {

        /// <summary>
        /// Known media query features based on the specification </summary>
        /// <seealso cref= <a href="http://www.w3.org/TR/css3-mediaqueries/">http://www.w3.org/TR/css3-mediaqueries/</a> </seealso>
        public sealed class Feature : SmartEnum<Feature>
        {
            public static readonly Feature WIDTH = new Feature("WIDTH", 0, true);
            public static readonly Feature HEIGHT = new Feature("HEIGHT", 1, true);
            public static readonly Feature DEVICE_WIDTH = new Feature("DEVICE_WIDTH", 2, true);
            public static readonly Feature DEVICE_HEIGHT = new Feature("DEVICE_HEIGHT", 3, true);
            public static readonly Feature ORIENTATION = new Feature("ORIENTATION", 4, false);
            public static readonly Feature ASPECT_RATIO = new Feature("ASPECT_RATIO", 5, true);
            public static readonly Feature DEVICE_ASPECT_RATIO = new Feature("DEVICE_ASPECT_RATIO", 6, true);
            public static readonly Feature COLOR = new Feature("COLOR", 7, true);
            public static readonly Feature COLOR_INDEX = new Feature("COLOR_INDEX", 8, true);
            public static readonly Feature MONOCHROME = new Feature("MONOCHROME", 9, true);
            public static readonly Feature RESOLUTION = new Feature("RESOLUTION", 10, true);
            public static readonly Feature SCAN = new Feature("SCAN", 11, false);
            public static readonly Feature GRID = new Feature("GRID", 12, false);

            internal bool prefixed;

            internal Feature(string name, int value, bool prefixed) : base(name, value)
            {
                this.prefixed = prefixed;
            }

            /// <summary>
            /// Is the min/max prefix allowed for this feature? </summary>
            /// <returns> {@code true} if the feature may be prefixed </returns>
            public bool Prefixed
            {
                get
                {
                    return prefixed;
                }
            }

            public override string ToString()
            {
                return Name;
            }
        }

        /// <summary>
        /// the values of 'em' used for computing the pixel lengths </summary>
        public const float em = 16.0f;
        /// <summary>
        /// the values of 'em' used for computing the pixel lengths </summary>
        public const float ex = 10.0f;
        /// <summary>
        /// CSS3 uses a fixed value of 96DPI for computing the lengths </summary>
        public const float dpi = 96.0f;

        protected internal static IDictionary<string, Feature> featureMap;

        static MediaSpec()
        {
            featureMap = new Dictionary<string, Feature>(13);
            featureMap["width"] = Feature.WIDTH;
            featureMap["height"] = Feature.HEIGHT;
            featureMap["device-width"] = Feature.DEVICE_WIDTH;
            featureMap["device-height"] = Feature.DEVICE_HEIGHT;
            featureMap["orientation"] = Feature.ORIENTATION;
            featureMap["aspect-ratio"] = Feature.ASPECT_RATIO;
            featureMap["device-aspect-ratio"] = Feature.DEVICE_ASPECT_RATIO;
            featureMap["color"] = Feature.COLOR;
            featureMap["color-index"] = Feature.COLOR_INDEX;
            featureMap["monochrome"] = Feature.MONOCHROME;
            featureMap["resolution"] = Feature.RESOLUTION;
            featureMap["scan"] = Feature.SCAN;
            featureMap["grid"] = Feature.GRID;
        }


        /// <summary>
        /// Media type name (e.g. "screen") </summary>
        protected internal string type;

        /// <summary>
        /// Output area width in pixels </summary>
        protected internal float width;
        /// <summary>
        /// Output area height in pixels </summary>
        protected internal float height;
        /// <summary>
        /// Output device width in pixels </summary>
        protected internal float deviceWidth;
        /// <summary>
        /// Oputput device height in pixels </summary>
        protected internal float deviceHeight;
        /// <summary>
        /// Color depth (bits per color) or 0 for no colors </summary>
        protected internal int color;
        /// <summary>
        /// Number of entries in the color lookup table or 0 when no indexed colors are used </summary>
        protected internal int colorIndex;
        /// <summary>
        /// Bits per pixel of a monochrome frame buffer or 0 when the device is not monochrome </summary>
        protected internal int monochrome;
        /// <summary>
        /// Resolution in DPI </summary>
        protected internal float resolution;
        /// <summary>
        /// Indicates interlaced scanning for "tv" media types </summary>
        protected internal bool scanInterlace;
        /// <summary>
        /// 1 for a grid device, 0 for bitmap device </summary>
        protected internal int grid;

        /// <summary>
        /// Creates a new media specification with the given media type and default values of the features. </summary>
        /// <param name="type"> The media type (e.g. "screen") </param>
        public MediaSpec(string type)
        {
            loadDefaults();
            this.type = type.Trim().ToLower(); // TOCHECK Locale.ENGLISH);
        }

        /// <summary>
        /// Obtains the media type of this specification. </summary>
        /// <returns> The media type name (e.g. "screen") </returns>
        public virtual string Type
        {
            get
            {
                return type;
            }
        }

        /// <summary>
        /// Obtains the width of the display area. </summary>
        /// <returns> the width in pixels. </returns>
        public virtual float Width
        {
            get
            {
                return width;
            }
            set
            {
                this.width = value;
            }
        }


        /// <summary>
        /// Obtains the height of the display area. </summary>
        /// <returns> the height in pixels. </returns>
        public virtual float Height
        {
            get
            {
                return height;
            }
            set
            {
                this.height = value;
            }
        }


        /// <summary>
        /// Sets the width and height height of the display area. </summary>
        /// <param name="width"> The width in pixels. </param>
        /// <param name="height"> The height in pixels. </param>
        public virtual void setDimensions(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Obtains the width of the rendering surface of the output device. </summary>
        /// <returns> the width in pixels. </returns>
        public virtual float DeviceWidth
        {
            get
            {
                return deviceWidth;
            }
            set
            {
                this.deviceWidth = value;
            }
        }


        /// <summary>
        /// Obtains the height of the rendering surface of the output device. </summary>
        /// <returns> the height in pixels. </returns>
        public virtual float DeviceHeight
        {
            get
            {
                return deviceHeight;
            }
            set
            {
                this.deviceHeight = value;
            }
        }


        /// <summary>
        /// Sets the width of the rendering surface of the output device. </summary>
        /// <param name="deviceWidth"> the width in pixels. </param>
        /// <param name="deviceHeight"> the height in pixels. </param>
        public virtual void setDeviceDimensions(float deviceWidth, float deviceHeight)
        {
            this.deviceWidth = deviceWidth;
            this.deviceHeight = deviceHeight;
        }

        /// <summary>
        /// Obtains the number of bits per color component of the output device. </summary>
        /// <returns> The number of bits or 0 if the device is not a color device. </returns>
        public virtual int Color
        {
            get
            {
                return color;
            }
            set
            {
                this.color = value;
            }
        }


        /// <summary>
        /// Obtains the number of entries in the color lookup table of the output device. </summary>
        /// <returns> The number of lookup table entries or 0 if the device does not use a color lookup table. </returns>
        public virtual int ColorIndex
        {
            get
            {
                return colorIndex;
            }
            set
            {
                this.colorIndex = value;
            }
        }


        /// <summary>
        /// Obtains the number of bits per pixel in a monochrome frame buffer. </summary>
        /// <returns> The number of bits per pixel or 0 if the device is not a monochrome device. </returns>
        public virtual int Monochrome
        {
            get
            {
                return monochrome;
            }
            set
            {
                this.monochrome = value;
            }
        }


        /// <summary>
        /// Sets the resolution of the output device, i.e. the density of the pixels. </summary>
        /// <returns> The resolution in DPI. </returns>
        public virtual float Resolution
        {
            get
            {
                return resolution;
            }
            set
            {
                this.resolution = value;
            }
        }


        /// <summary>
        /// Checks if the device is using the interlaced or progressive scanning (for "tv" media only). </summary>
        /// <returns> {@code true} if the device is using the interlaced scanning, {@code false} for progressive scanning. </returns>
        public virtual bool ScanInterlace
        {
            get
            {
                return scanInterlace;
            }
            set
            {
                this.scanInterlace = value;
            }
        }


        /// <summary>
        /// Checks whether the output device is grid or bitmap. </summary>
        /// <returns> If the output device is grid-based (e.g., a "tty" terminal, or a phone display with only one fixed font), the value will be 1. Otherwise, the value will be 0. </returns>
        public virtual int Grid
        {
            get
            {
                return grid;
            }
            set
            {
                this.grid = value;
            }
        }


        /// <summary>
        /// Obtains the aspect ratio of the display area defined as width / height. </summary>
        /// <returns> The aspect radio. </returns>
        public virtual float AspectRatio
        {
            get
            {
                return width / height;
            }
        }

        /// <summary>
        /// Obtains the aspect ratio of the device defined as deviceWidth / deviceHeight. </summary>
        /// <returns> The aspect radio. </returns>
        public virtual float DeviceAspectRation
        {
            get
            {
                return deviceWidth / deviceHeight;
            }
        }

        /// <summary>
        /// Checks whether the display area orientation is portrait or landscape. </summary>
        /// <returns> {@code true} when the value of the {@code height} media feature is greater than or equal to the value
        ///  of the {@code width} media feature. </returns>
        public virtual bool Portrait
        {
            get
            {
                return height >= width; //http://www.w3.org/TR/css3-mediaqueries/#orientation
            }
        }

        //===============================================================================================

        /// <summary>
        /// Checks if this media specification matches a given media query. </summary>
        /// <param name="q"> The media query </param>
        /// <returns> {@code true} when this media specification matches the given media query. </returns>
        public virtual bool matches(MediaQuery q)
        {
            //match the media type
            if (!string.ReferenceEquals(q.Type, null))
            {
                if (q.Type.Equals("all"))
                {
                    if (q.Negative)
                    {
                        return false; //"NOT all" doesn't match to anything
                    }
                }
                else if (q.Type.Equals(this.Type) == q.Negative) //other than all
                {
                    return false;
                }
            }
            //match the eventual expressions
            foreach (MediaExpression e in q)
            {
                if (!this.matches(e))
                {
                    return false;
                }
            }
            //everything matched
            return true;
        }

        /// <summary>
        /// Checks if this media specification matches a given media query expression. </summary>
        /// <param name="e"> The media query expression </param>
        /// <returns> {@code true} when this media specification matches the given expression. </returns>
        public virtual bool matches(MediaExpression e)
        {
            string fs = e.Feature;
            bool isMin = false;
            bool isMax = false;
            if (fs.StartsWith("min-", StringComparison.Ordinal))
            {
                isMin = true;
                fs = fs.Substring(4);
            }
            else if (fs.StartsWith("max-", StringComparison.Ordinal))
            {
                isMax = true;
                fs = fs.Substring(4);
            }

            Feature feature = getFeatureByName(fs);
            if (feature != null && (!(isMin || isMax) || feature.Prefixed)) //the name (including prefixes) is allowed
            {
                switch (feature.Name)
                {
                    case nameof(Feature.WIDTH):
                        return valueMatches(getExpressionLengthPx(e), width, isMin, isMax);
                    case nameof(Feature.HEIGHT):
                        return valueMatches(getExpressionLengthPx(e), height, isMin, isMax);
                    case nameof(Feature.DEVICE_WIDTH):
                        return valueMatches(getExpressionLengthPx(e), deviceWidth, isMin, isMax);
                    case nameof(Feature.DEVICE_HEIGHT):
                        return valueMatches(getExpressionLengthPx(e), deviceHeight, isMin, isMax);
                    case nameof(Feature.ORIENTATION):
                        string oid = getExpressionIdentifier(e);
                        if (string.ReferenceEquals(oid, null))
                        {
                            return false;
                        }
                        else if (oid.Equals("portrait"))
                        {
                            return Portrait;
                        }
                        else if (oid.Equals("landscape"))
                        {
                            return !Portrait;
                        }
                        else
                        {
                            return false;
                        }
                    case nameof(Feature.ASPECT_RATIO):
                        return valueMatches(getExpressionRatio(e), AspectRatio, isMin, isMax);
                    case nameof(Feature.DEVICE_ASPECT_RATIO):
                        return valueMatches(getExpressionRatio(e), DeviceAspectRation, isMin, isMax);
                    case nameof(Feature.COLOR):
                        return valueMatches(getExpressionInteger(e), color, isMin, isMax);
                    case nameof(Feature.COLOR_INDEX):
                        return valueMatches(getExpressionInteger(e), colorIndex, isMin, isMax);
                    case nameof(Feature.MONOCHROME):
                        return valueMatches(getExpressionInteger(e), monochrome, isMin, isMax);
                    case nameof(Feature.RESOLUTION):
                        return valueMatches(getExpressionResolution(e), resolution, isMin, isMax);
                    case nameof(Feature.SCAN):
                        string sid = getExpressionIdentifier(e);
                        if (string.ReferenceEquals(sid, null))
                        {
                            return false;
                        }
                        else if (sid.Equals("progressive"))
                        {
                            return !scanInterlace;
                        }
                        else if (sid.Equals("interlace"))
                        {
                            return scanInterlace;
                        }
                        else
                        {
                            return false;
                        }
                    case nameof(Feature.GRID):
                        int? gval = getExpressionInteger(e);
                        if (gval == null)
                        {
                            return false;
                        }
                        else if (gval == 0 || gval == 1) //0 and 1 are the only allowed values
                        {
                            return gval.Value == grid;
                        }
                        else
                        {
                            return false;
                        }
                    default:
                        return false;
                }
            }
            else
            {
                return false; //results in "not all" for the whole query
            }
        }

        /// <summary>
        /// Checks whether this media specification matches to at least one of the given media queries. </summary>
        /// <param name="queries"> The list of media queries to be matched. </param>
        /// <returns> {@code true} when at least one query matches, {@code false} when no query matches.  </returns>
        public virtual bool matchesOneOf(IList<MediaQuery> queries)
        {
            foreach (MediaQuery q in queries)
            {
                if (matches(q))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks whether the media specification matches an empty or missing media query, i.e. whether the
        /// media or import rules with no media queries should be accepted with this media specification. </summary>
        /// <returns> {@code true} when this media specification matches an empty media query </returns>
        public virtual bool matchesEmpty()
        {
            return true;
        }

        /// <summary>
        /// Obtains the feature based on its name. </summary>
        /// <param name="name"> The name of the feature </param>
        /// <returns> The feature corresponding to the name or {@code null} if the name is unknown </returns>
        protected internal virtual Feature getFeatureByName(string name)
        {
            return featureMap[name];
        }

        /// <summary>
        /// Checks whether a value coresponds to the given criteria. </summary>
        /// <param name="current"> the actual media value </param>
        /// <param name="required"> the required value or {@code null} for invalid requirement </param>
        /// <param name="min"> {@code true} when the required value is the minimal one </param>
        /// <param name="max"> {@code true} when the required value is the maximal one </param>
        /// <returns> {@code true} when the value matches the criteria. </returns>
        protected internal virtual bool valueMatches(float? required, float current, bool min, bool max)
        {
            if (required != null)
            {
                if (min)
                {
                    return (current >= required.Value);
                }
                else if (max)
                {
                    return (current <= required.Value);
                }
                else
                {
                    return current == required.Value;
                }
            }
            else
            {
                return false; //invalid values don't match
            }
        }

        /// <summary>
        /// Checks whether a value coresponds to the given criteria. </summary>
        /// <param name="required"> the required value </param>
        /// <param name="current"> the tested value or {@code null} for invalid value </param>
        /// <param name="min"> {@code true} when the required value is the minimal one </param>
        /// <param name="max"> {@code true} when the required value is the maximal one </param>
        /// <returns> {@code true} when the value matches the criteria. </returns>
        protected internal virtual bool valueMatches(int? required, int current, bool min, bool max)
        {
            if (required != null)
            {
                if (min)
                {
                    return (current >= required.Value);
                }
                else if (max)
                {
                    return (current <= required.Value);
                }
                else
                {
                    return current == required.Value;
                }
            }
            else
            {
                return false; //invalid values don't match
            }
        }

        /// <summary>
        /// Obtains the length specified by the given media query expression. </summary>
        /// <param name="e"> The media query expression specifying a length. </param>
        /// <returns> The length converted to pixels or {@code null} when the value cannot be converted to length.  </returns>
        protected internal virtual float? getExpressionLengthPx(MediaExpression e)
        {
            if (e.Count == 1) //the length requires exactly one value
            {
                //ORIGINAL LINE: Term<?> term = e.get(0);
                Term term = e[0];
                if (term is TermLength)
                {
                    return pxLength((TermLength)term);
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

        /// <summary>
        /// Obtains the resolution specified by the given media query expression. </summary>
        /// <param name="e"> The media query expression specifying a resolution. </param>
        /// <returns> The length converted to pixels or {@code null} when the value cannot be converted to resolution.  </returns>
        protected internal virtual float? getExpressionResolution(MediaExpression e)
        {
            if (e.Count == 1) //the length requires exactly one value
            {
                //ORIGINAL LINE: Term<?> term = e.get(0);
                Term term = e[0];
                if (term is TermResolution)
                {
                    return dpiResolution((TermResolution)term);
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

        /// <summary>
        /// Obtains the ratio specified by the given media query expression. </summary>
        /// <param name="e"> The media query expression specifying a ratio. </param>
        /// <returns> The length converted to pixels or {@code null} when the value cannot be converted to ratio.  </returns>
        protected internal virtual float? getExpressionRatio(MediaExpression e)
        {
            if (e.Count == 2) //the ratio is two integer values
            {
                //ORIGINAL LINE: Term<?> term1 = e.get(0);
                Term term1 = e[0];
                //ORIGINAL LINE: Term<?> term2 = e.get(1);
                Term term2 = e[1];
                if (term1 is TermInteger && term2 is TermInteger && (((TermInteger)term2).Operator == Term_Operator.SLASH))
                {
                    return ((TermInteger)term1).Value / ((TermInteger)term2).Value;
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

        /// <summary>
        /// Obtains the integer specified by the given media query expression. </summary>
        /// <param name="e"> The media query expression specifying an integer. </param>
        /// <returns> The length converted to pixels or {@code null} when the value cannot be converted to integer.  </returns>
        protected internal virtual int? getExpressionInteger(MediaExpression e)
        {
            if (e.Count == 1) //the length requires exactly one value
            {
                //ORIGINAL LINE: Term<?> term = e.get(0);
                Term term = e[0];
                if (term is TermInteger)
                {
                    return ((TermInteger)term).IntValue;
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

        /// <summary>
        /// Obtains the identifier specified by the given media query expression. </summary>
        /// <param name="e"> The media query expression specifying an identifier. </param>
        /// <returns> The identifier name or {@code null} when the value cannot be converted to an identifier.  </returns>
        protected internal virtual string getExpressionIdentifier(MediaExpression e)
        {
            if (e.Count == 1) //the length requires exactly one value
            {
                //ORIGINAL LINE: Term<?> term = e.get(0);
                Term term = e[0];
                if (term is TermIdent)
                {
                    return ((TermIdent)term).Value.Trim().ToLower(); // TOCHECK Locale.ENGLISH);
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

        /// <summary>
        /// Converts a length from a CSS length to 'px'. </summary>
        /// <param name="spec"> the CSS length specification </param>
        /// <returns> the length in 'px' or {@code null} when the unit is invalid  </returns>
        protected internal virtual float? pxLength(TermLength spec)
        {
            float nval = spec.Value;
            TermLength_Unit unit = spec.Unit;            

            switch (unit.value())
            {
                case nameof(TermLength_Unit.pt):
                    return (nval * dpi) / 72.0f;
                case nameof(TermLength_Unit.inch):
                    return nval * dpi;
                case nameof(TermLength_Unit.cm):
                    return (nval * dpi) / 2.54f;
                case nameof(TermLength_Unit.mm):
                    return (nval * dpi) / 25.4f;
                case nameof(TermLength_Unit.q):
                    return (nval * dpi) / (2.54f * 40f);
                case nameof(TermLength_Unit.pc):
                    return (nval * 12 * dpi) / 72.0f;
                case nameof(TermLength_Unit.px):
                    return nval;
                case nameof(TermLength_Unit.em):
                    return em * nval;
                case nameof(TermLength_Unit.ex):
                    return ex * nval;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Converts a resolution from a CSS length to 'dpi'. </summary>
        /// <param name="spec"> the CSS resolution specification </param>
        /// <returns> the resolution in 'dpi' or {@code null} when the unit is invalid  </returns>
        protected internal virtual float? dpiResolution(TermResolution spec)
        {
            float nval = spec.Value;
            TermLength_Unit unit = spec.Unit;

            switch (unit.value())
            {
                case nameof(TermLength_Unit.dpi):
                    return nval;
                case nameof(TermLength_Unit.dpcm):
                    return nval * 2.54f;
                case nameof(TermLength_Unit.dppx):
                    return nval * Resolution;
                default:
                    return null;
            }
        }

        //===============================================================================================

        /// <summary>
        /// Loads some reasonable defaults that correspond to a normal desktop configuration.
        /// </summary>
        protected internal virtual void loadDefaults()
        {
            width = 1100;
            height = 850;
            deviceWidth = 1920;
            deviceHeight = 1200;
            color = 8;
            colorIndex = 0;
            monochrome = 0;
            resolution = 96;
            scanInterlace = false;
            grid = 0;
        }

        //===============================================================================================

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append(type).Append('[');
            ret.Append("width:").Append(width).Append("; ");
            ret.Append("height:").Append(height).Append("; ");
            ret.Append("device-width:").Append(deviceWidth).Append("; ");
            ret.Append("device-height:").Append(deviceHeight).Append("; ");
            ret.Append("color:").Append(color).Append("; ");
            ret.Append("color-index:").Append(colorIndex).Append("; ");
            ret.Append("monochrome:").Append(monochrome).Append("; ");
            ret.Append("resolution:").Append(resolution).Append("; ");
            ret.Append("scan:").Append(scanInterlace ? "interlace" : "progressive").Append("; ");
            ret.Append("grid:").Append(grid).Append(";");
            ret.Append(']');
            return ret.ToString();
        }

    }

}