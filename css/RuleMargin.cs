using Ardalis.SmartEnum;
using System.Collections.Generic;

namespace StyleParserCS.css
{
    /// <summary>
    /// Contains CSS rules associated with a specific area in the page margin. 
    /// 
    /// @author Bert Frees, 2012
    /// </summary>
    public interface RuleMargin : RuleBlock<Declaration>, PrettyOutput
    {

        /// <summary>
        /// Returns margin area </summary>
        /// <returns> Margin area </returns>
        RuleMargin_MarginArea MarginArea { get; }

    }

    public sealed class RuleMargin_MarginArea : SmartEnum<RuleMargin_MarginArea, string>
    {

        public static readonly RuleMargin_MarginArea TOPLEFTCORNER = new RuleMargin_MarginArea("TOPLEFTCORNER", "top-left-corner");
        public static readonly RuleMargin_MarginArea TOPLEFT = new RuleMargin_MarginArea("TOPLEFT", "top-left");
        public static readonly RuleMargin_MarginArea TOPCENTER = new RuleMargin_MarginArea("TOPCENTER", "top-center");
        public static readonly RuleMargin_MarginArea TOPRIGHT = new RuleMargin_MarginArea("TOPRIGHT", "top-right");
        public static readonly RuleMargin_MarginArea TOPRIGHTCORNER = new RuleMargin_MarginArea("TOPRIGHTCORNER", "top-right-corner");
        public static readonly RuleMargin_MarginArea BOTTOMLEFTCORNER = new RuleMargin_MarginArea("BOTTOMLEFTCORNER", "bottom-left-corner");
        public static readonly RuleMargin_MarginArea BOTTOMLEFT = new RuleMargin_MarginArea("BOTTOMLEFT", "bottom-left");
        public static readonly RuleMargin_MarginArea BOTTOMCENTER = new RuleMargin_MarginArea("BOTTOMCENTER", "bottom-center");
        public static readonly RuleMargin_MarginArea BOTTOMRIGHT = new RuleMargin_MarginArea("BOTTOMRIGHT", "bottom-right");
        public static readonly RuleMargin_MarginArea BOTTOMRIGHTCORNER = new RuleMargin_MarginArea("BOTTOMRIGHTCORNER", "bottom-right-corner");
        public static readonly RuleMargin_MarginArea LEFTTOP = new RuleMargin_MarginArea("LEFTTOP", "left-top");
        public static readonly RuleMargin_MarginArea LEFTMIDDLE = new RuleMargin_MarginArea("LEFTMIDDLE", "left-middle");
        public static readonly RuleMargin_MarginArea LEFTBOTTOM = new RuleMargin_MarginArea("LEFTBOTTOM", "left-bottom");
        public static readonly RuleMargin_MarginArea RIGHTTOP = new RuleMargin_MarginArea("RIGHTTOP", "right-top");
        public static readonly RuleMargin_MarginArea RIGHTMIDDLE = new RuleMargin_MarginArea("RIGHTMIDDLE", "right-middle");
        public static readonly RuleMargin_MarginArea RIGHTBOTTOM = new RuleMargin_MarginArea("RIGHTBOTTOM", "right-bottom");

        internal RuleMargin_MarginArea(string name, string value) : base(name, value)
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }

}