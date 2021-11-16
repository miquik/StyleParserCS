using Ardalis.SmartEnum;
using System.Collections.Generic;

namespace StyleParserCS.css
{
    using Color = StyleParserCS.csskit.Color;

    /// <summary>
    /// Holds color value directly usable in Java.
    /// @author Jan Svercl, VUT Brno, 2008
    /// @author Karel Piwko, 2008
    /// </summary>
    public interface TermColor : Term<Color>
    {

        /// <summary>
        /// When the color was created using a special keyword such as 'transparent'
        /// or 'currentColor', this method may be used to test this condition. </summary>
        /// <returns> The corresponding <seealso cref="Keyword"/> value </returns>
        TermColor_Keyword Keyword { get; }

        /// <summary>
        /// Checks whether the given color is entirely transparent. </summary>
        /// <returns> {@code true} when the color is entirely transparent </returns>
        bool Transparent { get; }

    }

    public sealed class TermColor_Keyword : SmartEnum<TermColor_Keyword, string>
    {
        public static readonly TermColor_Keyword none = new TermColor_Keyword("none", "");
        public static readonly TermColor_Keyword TRANSPARENT = new TermColor_Keyword("TRANSPARENT", "transparent");
        public static readonly TermColor_Keyword CURRENT_COLOR = new TermColor_Keyword("CURRENT_COLOR", "currentColor");
        internal TermColor_Keyword(string name, string text) : base(name, text)
        {
        }

        public override string ToString()
        {
            return Value;
        }
    }

}