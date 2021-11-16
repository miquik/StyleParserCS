using Ardalis.SmartEnum;
using StyleParserCS.css;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace StyleParserCS.css
{

    /// <summary>
    /// Interface for definition of CSS properties. This interface simplifies storing
    /// of values in maps, and provides basic inheritance support.
    /// 
    /// All implementations of this interface should provide static value with
    /// signature:
    /// 
    /// <pre><b>public static</b> CSSProperty valueOf(String value);</pre>
    /// 
    /// to retrieve instance of property by string value. Since enum classes
    /// provides this value automatically, it is encouraged to use them.
    /// 
    /// For make use of enums easier, this contract should be followed:
    /// 
    /// All values directly represented in CSS style sheet such as:
    /// <code>float: <b>left</b>;</code> or <code>background-repeat: 
    /// <b>repeat-x</b>;</code> should to converted to upper case and 
    /// all not alphanumeric characters should be converted into underscores
    /// (<code>_</code>), for example <code>REPEAT_X</code>
    /// 
    /// All other values, with essentially requires additional data, should 
    /// broke enum standard and use lower case letters only. This way it is
    /// guaranteed that this value won't never be considered as a keyword. 
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public interface CSSProperty
    {

        /// <summary>
        /// CSS "inherit" keyword for retrieving instance by Translator object
        /// </summary>

        /// <summary>
        /// The CSS property value identifier used for denoting that the values should be
        /// represented by a nested list of TermValuePair terms. This is used for the
        /// <seealso cref="ValueType.LIST"/> type properties.
        /// </summary>


        /// <summary>
        /// Property value type. </summary>

        /// <summary>
        /// Allows declarations of properties to inherit or to be inherited
        /// </summary>
        /// <returns> <code>true</code> in case that this property could be inherited
        ///         from parent, <code>false</code> elsewhere </returns>
        bool inherited();

        /// <summary>
        /// Allows to check whether property equals <code>inherit</code> value
        /// </summary>
        /// <returns> <code>true</code>if value is <code>INHERIT</code>,
        ///         <code>false</code> otherwise </returns>
        bool equalsInherit();

        /// <summary>
        /// Allows to check whether property equals <code>initial</code> value
        /// </summary>
        /// <returns> <code>true</code>if value is <code>INITIAL</code>,
        ///         <code>false</code> otherwise </returns>
        bool equalsInitial();

        /// <summary>
        /// Allows to check whether property equals <code>unset</code> value
        /// </summary>
        /// <returns> <code>true</code>if value is <code>UNSET</code>,
        ///         <code>false</code> otherwise </returns>
        bool equalsUnset();

        /// <summary>
        /// Gets the value type. </summary>
        /// <returns> The value type. </returns>
        CSSProperty_ValueType ValueType { get; }

        /// <summary>
        /// Textual representation of CSS property
        /// </summary>
        /// <returns> String </returns>
        string ToString();

        /// <summary>
        ///*************************************************************
        /// TRANSLATOR *
        /// ***************************************************************
        /// </summary>

        /// <summary>
        /// Retrieves value of property of given class and text value
        /// 
        /// @author kapy
        /// 
        /// </summary>

        /// <summary>
        ///**********************************************************************
        /// CSS PROPERTIES *
        /// ***********************************************************************
        /// </summary>



        /// <summary>
        /// A generic property used for all the properties not supported by another implementation.
        /// </summary>

    }

    public static class CSSProperty_Fields
    {
        public const string INHERIT_KEYWORD = "INHERIT";
        public const string INITIAL_KEYWORD = "INITIAL";
        public const string UNSET_KEYWORD = "UNSET";
        public const string NESTED_LIST = "nested_list";
        public static readonly string FONT_SERIF = "serif";
        public static readonly string FONT_SANS_SERIF = "sans serif";
        public static readonly string FONT_MONOSPACED = "monospace";
        public const string FONT_CURSIVE = "Zapf-Chancery";
        public const string FONT_FANTASY = "Western";
    }

    public enum CSSProperty_ValueType
    {
        /// <summary>
        /// The property has a single value or a space-separated list of values
        /// with different semantics. 
        /// </summary>
        SIMPLE,
        /// <summary>
        /// The values may be repeated multiple times (usually a space-separated list such
        /// as background-* properties. 
        /// </summary>
        LIST
    }

    public class CSSProperty_Translator
    {

        /// <summary>
        /// Methods cache
        /// </summary>
        internal static IDictionary<Type, System.Reflection.MethodInfo> translators = new Dictionary<Type, System.Reflection.MethodInfo>();

        /// <summary>
        /// Retrieves CSSProperty by its name and class
        /// </summary>
        /// <param name="type">
        ///            Class of CSSProperty </param>
        /// <param name="value">
        ///            Text value </param>
        /// <returns> CSSProperty if found, <code>null</code> elsewhere </returns>
        //ORIGINAL LINE: @SuppressWarnings("unchecked") public static final <T extends CSSProperty> T valueOf(Class<T> type, String value)
        public static CSSProperty valueOf(Type type, string value)
        {
            try
            {
                switch (type.Name)
                {
                    case "CSSProperty_Azimuth": return CSSProperty_Azimuth.FromName(value);
                    case "CSSProperty_Color": return CSSProperty_Color.FromName(value);
                    case "CSSProperty_Cue": return CSSProperty_Cue.FromName(value);
                    case "CSSProperty_Background": return CSSProperty_Background.FromName(value);
                    case "CSSProperty_BackgroundAttachment": return CSSProperty_BackgroundAttachment.FromName(value);
                    case "CSSProperty_BackgroundColor": return CSSProperty_BackgroundColor.FromName(value);
                    case "CSSProperty_BackgroundImage": return CSSProperty_BackgroundImage.FromName(value);
                    case "CSSProperty_BackgroundPosition": return CSSProperty_BackgroundPosition.FromName(value);
                    case "CSSProperty_BackgroundRepeat": return CSSProperty_BackgroundRepeat.FromName(value);
                    case "CSSProperty_BackgroundSize": return CSSProperty_BackgroundSize.FromName(value);
                    case "CSSProperty_BackgroundOrigin": return CSSProperty_BackgroundOrigin.FromName(value);
                    case "CSSProperty_Border": return CSSProperty_Border.FromName(value);
                    case "CSSProperty_BorderCollapse": return CSSProperty_BorderCollapse.FromName(value);
                    case "CSSProperty_BorderColor": return CSSProperty_BorderColor.FromName(value);
                    case "CSSProperty_BorderRadius": return CSSProperty_BorderRadius.FromName(value);
                    case "CSSProperty_BorderSpacing": return CSSProperty_BorderSpacing.FromName(value);
                    case "CSSProperty_BorderStyle": return CSSProperty_BorderStyle.FromName(value);
                    case "CSSProperty_BorderWidth": return CSSProperty_BorderWidth.FromName(value);
                    case "CSSProperty_BoxShadow": return CSSProperty_BoxShadow.FromName(value);
                    case "CSSProperty_BoxSizing": return CSSProperty_BoxSizing.FromName(value);
                    case "CSSProperty_Elevation": return CSSProperty_Elevation.FromName(value);
                    case "CSSProperty_Font": return CSSProperty_Font.FromName(value);
                    case "CSSProperty_FontFamily": return CSSProperty_FontFamily.FromName(value);
                    case "CSSProperty_FontSize": return CSSProperty_FontSize.FromName(value);
                    case "CSSProperty_FontStyle": return CSSProperty_FontStyle.FromName(value);
                    case "CSSProperty_FontVariant": return CSSProperty_FontVariant.FromName(value);
                    case "CSSProperty_FontWeight": return CSSProperty_FontWeight.FromName(value);
                    case "CSSProperty_LineHeight": return CSSProperty_LineHeight.FromName(value);
                    case "CSSProperty_TabSize": return CSSProperty_TabSize.FromName(value);
                    case "CSSProperty_CaptionSide": return CSSProperty_CaptionSide.FromName(value);
                    case "CSSProperty_Content": return CSSProperty_Content.FromName(value);
                    case "CSSProperty_CounterIncrement": return CSSProperty_CounterIncrement.FromName(value);
                    case "CSSProperty_CounterReset": return CSSProperty_CounterReset.FromName(value);
                    case "CSSProperty_Clear": return CSSProperty_Clear.FromName(value);
                    case "CSSProperty_Clip": return CSSProperty_Clip.FromName(value);
                    case "CSSProperty_Cursor": return CSSProperty_Cursor.FromName(value);
                    case "CSSProperty_Direction": return CSSProperty_Direction.FromName(value);
                    case "CSSProperty_Display": return CSSProperty_Display.FromName(value);
                    case "CSSProperty_Width": return CSSProperty_Width.FromName(value);
                    case "CSSProperty_MinWidth": return CSSProperty_MinWidth.FromName(value);
                    case "CSSProperty_MaxWidth": return CSSProperty_MaxWidth.FromName(value);
                    case "CSSProperty_Height": return CSSProperty_Height.FromName(value);
                    case "CSSProperty_MinHeight": return CSSProperty_MinHeight.FromName(value);
                    case "CSSProperty_MaxHeight": return CSSProperty_MaxHeight.FromName(value);
                    case "CSSProperty_EmptyCells": return CSSProperty_EmptyCells.FromName(value);
                    case "CSSProperty_Floating": return CSSProperty_Floating.FromName(value);
                    case "CSSProperty_ListStyle": return CSSProperty_ListStyle.FromName(value);
                    case "CSSProperty_ListStyleImage": return CSSProperty_ListStyleImage.FromName(value);
                    case "CSSProperty_ListStylePosition": return CSSProperty_ListStylePosition.FromName(value);
                    case "CSSProperty_ListStyleType": return CSSProperty_ListStyleType.FromName(value);
                    case "CSSProperty_Margin": return CSSProperty_Margin.FromName(value);
                    case "CSSProperty_Opacity": return CSSProperty_Opacity.FromName(value);
                    case "CSSProperty_Orphans": return CSSProperty_Orphans.FromName(value);
                    case "CSSProperty_Outline": return CSSProperty_Outline.FromName(value);
                    case "CSSProperty_OutlineWidth": return CSSProperty_OutlineWidth.FromName(value);
                    case "CSSProperty_OutlineStyle": return CSSProperty_OutlineStyle.FromName(value);
                    case "CSSProperty_OutlineColor": return CSSProperty_OutlineColor.FromName(value);
                    case "CSSProperty_Overflow": return CSSProperty_Overflow.FromName(value);
                    case "CSSProperty_Padding": return CSSProperty_Padding.FromName(value);
                    case "CSSProperty_PageBreak": return CSSProperty_PageBreak.FromName(value);
                    case "CSSProperty_PageBreakInside": return CSSProperty_PageBreakInside.FromName(value);
                    case "CSSProperty_Pause": return CSSProperty_Pause.FromName(value);
                    case "CSSProperty_PitchRange": return CSSProperty_PitchRange.FromName(value);
                    case "CSSProperty_Pitch": return CSSProperty_Pitch.FromName(value);
                    case "CSSProperty_PlayDuring": return CSSProperty_PlayDuring.FromName(value);
                    case "CSSProperty_Position": return CSSProperty_Position.FromName(value);
                    case "CSSProperty_Richness": return CSSProperty_Richness.FromName(value);
                    case "CSSProperty_SpeakHeader": return CSSProperty_SpeakHeader.FromName(value);
                    case "CSSProperty_SpeakNumeral": return CSSProperty_SpeakNumeral.FromName(value);
                    case "CSSProperty_SpeakPunctuation": return CSSProperty_SpeakPunctuation.FromName(value);
                    case "CSSProperty_Speak": return CSSProperty_Speak.FromName(value);
                    case "CSSProperty_SpeechRate": return CSSProperty_SpeechRate.FromName(value);
                    case "CSSProperty_Stress": return CSSProperty_Stress.FromName(value);
                    case "CSSProperty_Top": return CSSProperty_Top.FromName(value);
                    case "CSSProperty_Right": return CSSProperty_Right.FromName(value);
                    case "CSSProperty_Bottom": return CSSProperty_Bottom.FromName(value);
                    case "CSSProperty_Left": return CSSProperty_Left.FromName(value);
                    case "CSSProperty_Quotes": return CSSProperty_Quotes.FromName(value);
                    case "CSSProperty_TableLayout": return CSSProperty_TableLayout.FromName(value);
                    case "CSSProperty_TextAlign": return CSSProperty_TextAlign.FromName(value);
                    case "CSSProperty_TextDecoration": return CSSProperty_TextDecoration.FromName(value);
                    case "CSSProperty_TextIndent": return CSSProperty_TextIndent.FromName(value);
                    case "CSSProperty_TextTransform": return CSSProperty_TextTransform.FromName(value);
                    case "CSSProperty_Transform": return CSSProperty_Transform.FromName(value);
                    case "CSSProperty_TransformOrigin": return CSSProperty_TransformOrigin.FromName(value);
                    case "CSSProperty_UnicodeBidi": return CSSProperty_UnicodeBidi.FromName(value);
                    case "CSSProperty_UnicodeRange": return CSSProperty_UnicodeRange.FromName(value);
                    case "CSSProperty_VerticalAlign": return CSSProperty_VerticalAlign.FromName(value);
                    case "CSSProperty_Visibility": return CSSProperty_Visibility.FromName(value);
                    case "CSSProperty_VoiceFamily": return CSSProperty_VoiceFamily.FromName(value);
                    case "CSSProperty_Volume": return CSSProperty_Volume.FromName(value);
                    case "CSSProperty_WhiteSpace": return CSSProperty_WhiteSpace.FromName(value);
                    case "CSSProperty_Widows": return CSSProperty_Widows.FromName(value);
                    case "CSSProperty_WordSpacing": return CSSProperty_WordSpacing.FromName(value);
                    case "CSSProperty_LetterSpacing": return CSSProperty_LetterSpacing.FromName(value);
                    case "CSSProperty_ZIndex": return CSSProperty_ZIndex.FromName(value);
                    case "CSSProperty_AlignContent": return CSSProperty_AlignContent.FromName(value);
                    case "CSSProperty_AlignItems": return CSSProperty_AlignItems.FromName(value);
                    case "CSSProperty_AlignSelf": return CSSProperty_AlignSelf.FromName(value);
                    case "CSSProperty_Flex": return CSSProperty_Flex.FromName(value);
                    case "CSSProperty_FlexFlow": return CSSProperty_FlexFlow.FromName(value);
                    case "CSSProperty_FlexBasis": return CSSProperty_FlexBasis.FromName(value);
                    case "CSSProperty_FlexDirection": return CSSProperty_FlexDirection.FromName(value);
                    case "CSSProperty_FlexGrow": return CSSProperty_FlexGrow.FromName(value);
                    case "CSSProperty_FlexShrink": return CSSProperty_FlexShrink.FromName(value);
                    case "CSSProperty_FlexWrap": return CSSProperty_FlexWrap.FromName(value);
                    case "CSSProperty_JustifyContent": return CSSProperty_JustifyContent.FromName(value);
                    case "CSSProperty_Order": return CSSProperty_Order.FromName(value);
                    case "CSSProperty_Filter": return CSSProperty_Filter.FromName(value);
                    case "CSSProperty_BackdropFilter": return CSSProperty_BackdropFilter.FromName(value);
                    case "CSSProperty_Grid": return CSSProperty_Grid.FromName(value);
                    case "CSSProperty_GridStartEnd": return CSSProperty_GridStartEnd.FromName(value);
                    case "CSSProperty_GridGap": return CSSProperty_GridGap.FromName(value);
                    case "CSSProperty_GridTemplateAreas": return CSSProperty_GridTemplateAreas.FromName(value);
                    case "CSSProperty_GridTemplateRowsColumns": return CSSProperty_GridTemplateRowsColumns.FromName(value);
                    case "CSSProperty_GridAutoFlow": return CSSProperty_GridAutoFlow.FromName(value);
                    case "CSSProperty_GridAutoRowsColumns": return CSSProperty_GridAutoRowsColumns.FromName(value);
                    case "CSSProperty_Animation": return CSSProperty_Animation.FromName(value);
                    case "CSSProperty_AnimationDelay": return CSSProperty_AnimationDelay.FromName(value);
                    case "CSSProperty_AnimationDirection": return CSSProperty_AnimationDirection.FromName(value);
                    case "CSSProperty_AnimationDuration": return CSSProperty_AnimationDuration.FromName(value);
                    case "CSSProperty_AnimationFillMode": return CSSProperty_AnimationFillMode.FromName(value);
                    case "CSSProperty_AnimationIterationCount": return CSSProperty_AnimationIterationCount.FromName(value);
                    case "CSSProperty_AnimationName": return CSSProperty_AnimationName.FromName(value);
                    case "CSSProperty_AnimationPlayState": return CSSProperty_AnimationPlayState.FromName(value);
                    case "CSSProperty_AnimationTimingFunction": return CSSProperty_AnimationTimingFunction.FromName(value);
                    case "CSSProperty_Transition": return CSSProperty_Transition.FromName(value);
                    case "CSSProperty_TransitionDelay": return CSSProperty_TransitionDelay.FromName(value);
                    case "CSSProperty_TransitionDuration": return CSSProperty_TransitionDuration.FromName(value);
                    case "CSSProperty_TransitionProperty": return CSSProperty_TransitionProperty.FromName(value);
                    case "CSSProperty_TransitionTimingFunction": return CSSProperty_TransitionTimingFunction.FromName(value);
                    case "CSSProperty_GenericCSSPropertyProxy": return CSSProperty_GenericCSSPropertyProxy.valueOf(value);
                    default:
                        return default;
                }
                /*
                if (translators.TryGetValue(type, out System.Reflection.MethodInfo m) == false)
                {
                    m = type.GetMethod("FromName", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    if (m != null)
                    {
                        // try valueof
                        return (CSSProperty)m.Invoke(null, new object[] { value, false });
                    }
                    m = type.GetMethod("valueOf", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    if (m != null)
                    {
                        return (CSSProperty)m.Invoke(null, new object[] { value });
                    }
                }
                return default;
                */
            }
            catch (Exception ex)
            {
                return default;
                /*
				throw new IllegalArgumentException("Unable to get: " + value
						+ " for: " + type.getName(), e);
				*/
            }
        }

        /// <summary>
        /// Creates "inherit" instance </summary>
        /// <param name="type"> Type of CSS property </param>
        /// <returns> Should always return CSS instance. If <code>null</code> is returned, something
        /// is flawed. </returns>
        public static CSSProperty createInherit(Type type)
        {
            return valueOf(type, CSSProperty_Fields.INHERIT_KEYWORD);
        }

        /// <summary>
        /// Creates the "nested_list" instance for a given property. </summary>
        /// @param <T> </param>
        /// <param name="type"> The property type </param>
        /// <returns> The corresponding property instance or {@code null} when the corresponding
        /// value is not defined for the given property. </returns>
        public static CSSProperty createNestedListValue(Type type)
        {
            return valueOf(type, CSSProperty_Fields.NESTED_LIST);
        }
    }

    public sealed class CSSProperty_Azimuth : SmartEnum<CSSProperty_Azimuth, string>, CSSProperty
    {
        public static readonly CSSProperty_Azimuth angle = new CSSProperty_Azimuth("angle", "");
        public static readonly CSSProperty_Azimuth LEFT_SIDE = new CSSProperty_Azimuth("LEFT_SIDE", "left-side");
        public static readonly CSSProperty_Azimuth FAR_LEFT = new CSSProperty_Azimuth("FAR_LEFT", "far-left");
        public static readonly CSSProperty_Azimuth LEFT = new CSSProperty_Azimuth("LEFT", "left");
        public static readonly CSSProperty_Azimuth CENTER_LEFT = new CSSProperty_Azimuth("CENTER_LEFT", "center-left");
        public static readonly CSSProperty_Azimuth CENTER = new CSSProperty_Azimuth("CENTER", "center");
        public static readonly CSSProperty_Azimuth CENTER_RIGHT = new CSSProperty_Azimuth("CENTER_RIGHT", "center-right");
        public static readonly CSSProperty_Azimuth RIGHT = new CSSProperty_Azimuth("RIGHT", "right");
        public static readonly CSSProperty_Azimuth FAR_RIGHT = new CSSProperty_Azimuth("FAR_RIGHT", "far-right");
        public static readonly CSSProperty_Azimuth RIGHT_SIDE = new CSSProperty_Azimuth("RIGHT_SIDE", "right-side");
        public static readonly CSSProperty_Azimuth BEHIND = new CSSProperty_Azimuth("BEHIND", "behind");
        public static readonly CSSProperty_Azimuth LEFTWARDS = new CSSProperty_Azimuth("LEFTWARDS", "leftwards");
        public static readonly CSSProperty_Azimuth RIGHTWARDS = new CSSProperty_Azimuth("RIGHTWARDS", "rightwards");
        public static readonly CSSProperty_Azimuth INHERIT = new CSSProperty_Azimuth("INHERIT", "inherit");
        public static readonly CSSProperty_Azimuth INITIAL = new CSSProperty_Azimuth("INITIAL", "initial");
        public static readonly CSSProperty_Azimuth UNSET = new CSSProperty_Azimuth("UNSET", "unset");

        internal CSSProperty_Azimuth(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return true;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
            // return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Azimuth, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }



    public sealed class CSSProperty_Color : SmartEnum<CSSProperty_Color, string>, CSSProperty
    {
        public static readonly CSSProperty_Color color = new CSSProperty_Color("color", "");
        public static readonly CSSProperty_Color INHERIT = new CSSProperty_Color("INHERIT", "inherit");
        public static readonly CSSProperty_Color INITIAL = new CSSProperty_Color("INITIAL", "initial");
        public static readonly CSSProperty_Color UNSET = new CSSProperty_Color("UNSET", "unset");


        internal CSSProperty_Color(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return true;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Color, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_Cue : SmartEnum<CSSProperty_Cue, string>, CSSProperty
    {
        public static readonly CSSProperty_Cue component_values = new CSSProperty_Cue("component_values", "");
        public static readonly CSSProperty_Cue uri = new CSSProperty_Cue("uri", "");
        public static readonly CSSProperty_Cue NONE = new CSSProperty_Cue("NONE", "none");
        public static readonly CSSProperty_Cue INHERIT = new CSSProperty_Cue("INHERIT", "inherit");
        public static readonly CSSProperty_Cue INITIAL = new CSSProperty_Cue("INITIAL", "initial");
        public static readonly CSSProperty_Cue UNSET = new CSSProperty_Cue("UNSET", "unset");


        internal CSSProperty_Cue(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Cue, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_Background : SmartEnum<CSSProperty_Background, string>, CSSProperty
    {
        public static readonly CSSProperty_Background nested_list = new CSSProperty_Background("nested_list", "");
        public static readonly CSSProperty_Background component_values = new CSSProperty_Background("component_values", "");
        public static readonly CSSProperty_Background INHERIT = new CSSProperty_Background("INHERIT", "inherit");
        public static readonly CSSProperty_Background INITIAL = new CSSProperty_Background("INITIAL", "initial");
        public static readonly CSSProperty_Background UNSET = new CSSProperty_Background("UNSET", "unset");


        internal CSSProperty_Background(string name, string value) : base(name, value)
        {

        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.LIST;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Background, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_BackgroundAttachment : SmartEnum<CSSProperty_BackgroundAttachment, string>, CSSProperty
    {
        public static readonly CSSProperty_BackgroundAttachment nested_list = new CSSProperty_BackgroundAttachment("nested_list", "");
        public static readonly CSSProperty_BackgroundAttachment SCROLL = new CSSProperty_BackgroundAttachment("SCROLL", "scroll");
        public static readonly CSSProperty_BackgroundAttachment FIXED = new CSSProperty_BackgroundAttachment("FIXED", "fixed");
        public static readonly CSSProperty_BackgroundAttachment INHERIT = new CSSProperty_BackgroundAttachment("INHERIT", "inherit");
        public static readonly CSSProperty_BackgroundAttachment INITIAL = new CSSProperty_BackgroundAttachment("INITIAL", "initial");
        public static readonly CSSProperty_BackgroundAttachment UNSET = new CSSProperty_BackgroundAttachment("UNSET", "unset");


        internal CSSProperty_BackgroundAttachment(string name, string value) : base(name, value)
        {

        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.LIST;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_BackgroundAttachment, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_BackgroundColor : SmartEnum<CSSProperty_BackgroundColor, string>, CSSProperty
    {
        public static readonly CSSProperty_BackgroundColor color = new CSSProperty_BackgroundColor("color", "");
        public static readonly CSSProperty_BackgroundColor INHERIT = new CSSProperty_BackgroundColor("INHERIT", "inherit");
        public static readonly CSSProperty_BackgroundColor INITIAL = new CSSProperty_BackgroundColor("INITIAL", "initial");
        public static readonly CSSProperty_BackgroundColor UNSET = new CSSProperty_BackgroundColor("UNSET", "unset");

        internal CSSProperty_BackgroundColor(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_BackgroundColor, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_BackgroundImage : SmartEnum<CSSProperty_BackgroundImage, string>, CSSProperty
    {
        public static readonly CSSProperty_BackgroundImage nested_list = new CSSProperty_BackgroundImage("nested_list", "");
        public static readonly CSSProperty_BackgroundImage uri = new CSSProperty_BackgroundImage("uri", "");
        public static readonly CSSProperty_BackgroundImage gradient = new CSSProperty_BackgroundImage("gradient", "");
        public static readonly CSSProperty_BackgroundImage NONE = new CSSProperty_BackgroundImage("NONE", "none");
        public static readonly CSSProperty_BackgroundImage INHERIT = new CSSProperty_BackgroundImage("INHERIT", "inherit");
        public static readonly CSSProperty_BackgroundImage INITIAL = new CSSProperty_BackgroundImage("INITIAL", "initial");
        public static readonly CSSProperty_BackgroundImage UNSET = new CSSProperty_BackgroundImage("UNSET", "unset");

        internal CSSProperty_BackgroundImage(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.LIST;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_BackgroundImage, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_BackgroundPosition : SmartEnum<CSSProperty_BackgroundPosition, string>, CSSProperty
    {
        public static readonly CSSProperty_BackgroundPosition nested_list = new CSSProperty_BackgroundPosition("nested_list", "");
        public static readonly CSSProperty_BackgroundPosition list_values = new CSSProperty_BackgroundPosition("list_values", "");
        public static readonly CSSProperty_BackgroundPosition LEFT = new CSSProperty_BackgroundPosition("LEFT", "left");
        public static readonly CSSProperty_BackgroundPosition CENTER = new CSSProperty_BackgroundPosition("CENTER", "center");
        public static readonly CSSProperty_BackgroundPosition RIGHT = new CSSProperty_BackgroundPosition("RIGHT", "right");
        public static readonly CSSProperty_BackgroundPosition TOP = new CSSProperty_BackgroundPosition("TOP", "top");
        public static readonly CSSProperty_BackgroundPosition BOTTOM = new CSSProperty_BackgroundPosition("BOTTOM", "bottom");
        public static readonly CSSProperty_BackgroundPosition INHERIT = new CSSProperty_BackgroundPosition("INHERIT", "inherit");
        public static readonly CSSProperty_BackgroundPosition INITIAL = new CSSProperty_BackgroundPosition("INITIAL", "initial");
        public static readonly CSSProperty_BackgroundPosition UNSET = new CSSProperty_BackgroundPosition("UNSET", "unset");

        internal CSSProperty_BackgroundPosition(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.LIST;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_BackgroundPosition, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_BackgroundRepeat : SmartEnum<CSSProperty_BackgroundRepeat, string>, CSSProperty
    {
        public static readonly CSSProperty_BackgroundRepeat nested_list = new CSSProperty_BackgroundRepeat("nested_list", "");
        public static readonly CSSProperty_BackgroundRepeat REPEAT = new CSSProperty_BackgroundRepeat("REPEAT", "repeat");
        public static readonly CSSProperty_BackgroundRepeat REPEAT_X = new CSSProperty_BackgroundRepeat("REPEAT_X", "repeat-x");
        public static readonly CSSProperty_BackgroundRepeat REPEAT_Y = new CSSProperty_BackgroundRepeat("REPEAT_Y", "repeat-y");
        public static readonly CSSProperty_BackgroundRepeat NO_REPEAT = new CSSProperty_BackgroundRepeat("NO_REPEAT", "no-repeat");
        public static readonly CSSProperty_BackgroundRepeat INHERIT = new CSSProperty_BackgroundRepeat("INHERIT", "inherit");
        public static readonly CSSProperty_BackgroundRepeat INITIAL = new CSSProperty_BackgroundRepeat("INITIAL", "initial");
        public static readonly CSSProperty_BackgroundRepeat UNSET = new CSSProperty_BackgroundRepeat("UNSET", "unset");

        internal CSSProperty_BackgroundRepeat(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.LIST;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_BackgroundRepeat, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_BackgroundSize : SmartEnum<CSSProperty_BackgroundSize, string>, CSSProperty
    {
        public static readonly CSSProperty_BackgroundSize nested_list = new CSSProperty_BackgroundSize("nested_list", "");
        public static readonly CSSProperty_BackgroundSize list_values = new CSSProperty_BackgroundSize("list_values", "");
        public static readonly CSSProperty_BackgroundSize CONTAIN = new CSSProperty_BackgroundSize("CONTAIN", "contain");
        public static readonly CSSProperty_BackgroundSize COVER = new CSSProperty_BackgroundSize("COVER", "cover");
        public static readonly CSSProperty_BackgroundSize INHERIT = new CSSProperty_BackgroundSize("INHERIT", "inherit");
        public static readonly CSSProperty_BackgroundSize INITIAL = new CSSProperty_BackgroundSize("INITIAL", "initial");
        public static readonly CSSProperty_BackgroundSize UNSET = new CSSProperty_BackgroundSize("UNSET", "unset");

        internal CSSProperty_BackgroundSize(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.LIST;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_BackgroundSize, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_BackgroundOrigin : SmartEnum<CSSProperty_BackgroundOrigin, string>, CSSProperty
    {
        public static readonly CSSProperty_BackgroundOrigin nested_list = new CSSProperty_BackgroundOrigin("nested_list", "");
        public static readonly CSSProperty_BackgroundOrigin BORDER_BOX = new CSSProperty_BackgroundOrigin("BORDER_BOX", "border-box");
        public static readonly CSSProperty_BackgroundOrigin PADDING_BOX = new CSSProperty_BackgroundOrigin("PADDING_BOX", "padding-box");
        public static readonly CSSProperty_BackgroundOrigin CONTENT_BOX = new CSSProperty_BackgroundOrigin("CONTENT_BOX", "content-box");
        public static readonly CSSProperty_BackgroundOrigin INHERIT = new CSSProperty_BackgroundOrigin("INHERIT", "inherit");
        public static readonly CSSProperty_BackgroundOrigin INITIAL = new CSSProperty_BackgroundOrigin("INITIAL", "initial");
        public static readonly CSSProperty_BackgroundOrigin UNSET = new CSSProperty_BackgroundOrigin("UNSET", "unset");

        internal CSSProperty_BackgroundOrigin(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.LIST;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_BackgroundOrigin, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_Border : SmartEnum<CSSProperty_Border, string>, CSSProperty
    {
        public static readonly CSSProperty_Border component_values = new CSSProperty_Border("component_values", "");
        public static readonly CSSProperty_Border INHERIT = new CSSProperty_Border("INHERIT", "inherit");
        public static readonly CSSProperty_Border INITIAL = new CSSProperty_Border("INITIAL", "initial");
        public static readonly CSSProperty_Border UNSET = new CSSProperty_Border("UNSET", "unset");

        internal CSSProperty_Border(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Border, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_BorderCollapse : SmartEnum<CSSProperty_BorderCollapse, string>, CSSProperty
    {
        public static readonly CSSProperty_BorderCollapse COLLAPSE = new CSSProperty_BorderCollapse("COLLAPSE", "collapse");
        public static readonly CSSProperty_BorderCollapse SEPARATE = new CSSProperty_BorderCollapse("SEPARATE", "separate");
        public static readonly CSSProperty_BorderCollapse INHERIT = new CSSProperty_BorderCollapse("INHERIT", "inherit");
        public static readonly CSSProperty_BorderCollapse INITIAL = new CSSProperty_BorderCollapse("INITIAL", "initial");
        public static readonly CSSProperty_BorderCollapse UNSET = new CSSProperty_BorderCollapse("UNSET", "unset");

        internal CSSProperty_BorderCollapse(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_BorderCollapse, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_BorderColor : SmartEnum<CSSProperty_BorderColor, string>, CSSProperty
    {
        public static readonly CSSProperty_BorderColor color = new CSSProperty_BorderColor("color", "");
        public static readonly CSSProperty_BorderColor component_values = new CSSProperty_BorderColor("component_values", "");
        public static readonly CSSProperty_BorderColor INHERIT = new CSSProperty_BorderColor("INHERIT", "inherit");
        public static readonly CSSProperty_BorderColor INITIAL = new CSSProperty_BorderColor("INITIAL", "initial");
        public static readonly CSSProperty_BorderColor UNSET = new CSSProperty_BorderColor("UNSET", "unset");

        internal CSSProperty_BorderColor(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_BorderColor, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_BorderRadius : SmartEnum<CSSProperty_BorderRadius, string>, CSSProperty
    {
        public static readonly CSSProperty_BorderRadius component_values = new CSSProperty_BorderRadius("component_values", "");
        public static readonly CSSProperty_BorderRadius list_values = new CSSProperty_BorderRadius("list_values", "");
        public static readonly CSSProperty_BorderRadius INHERIT = new CSSProperty_BorderRadius("INHERIT", "inherit");
        public static readonly CSSProperty_BorderRadius INITIAL = new CSSProperty_BorderRadius("INITIAL", "initial");
        public static readonly CSSProperty_BorderRadius UNSET = new CSSProperty_BorderRadius("UNSET", "unset");

        internal CSSProperty_BorderRadius(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_BorderRadius, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_BorderSpacing : SmartEnum<CSSProperty_BorderSpacing, string>, CSSProperty
    {
        public static readonly CSSProperty_BorderSpacing list_values = new CSSProperty_BorderSpacing("list_values", "");
        public static readonly CSSProperty_BorderSpacing INHERIT = new CSSProperty_BorderSpacing("INHERIT", "inherit");
        public static readonly CSSProperty_BorderSpacing INITIAL = new CSSProperty_BorderSpacing("INITIAL", "initial");
        public static readonly CSSProperty_BorderSpacing UNSET = new CSSProperty_BorderSpacing("UNSET", "unset");

        internal CSSProperty_BorderSpacing(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return true;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_BorderSpacing, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_BorderStyle : SmartEnum<CSSProperty_BorderStyle, string>, CSSProperty
    {
        public static readonly CSSProperty_BorderStyle component_values = new CSSProperty_BorderStyle("component_values", "");
        public static readonly CSSProperty_BorderStyle NONE = new CSSProperty_BorderStyle("NONE", "none");
        public static readonly CSSProperty_BorderStyle HIDDEN = new CSSProperty_BorderStyle("HIDDEN", "hidden");
        public static readonly CSSProperty_BorderStyle DOTTED = new CSSProperty_BorderStyle("DOTTED", "dotted");
        public static readonly CSSProperty_BorderStyle DASHED = new CSSProperty_BorderStyle("DASHED", "dashed");
        public static readonly CSSProperty_BorderStyle SOLID = new CSSProperty_BorderStyle("SOLID", "solid");
        public static readonly CSSProperty_BorderStyle DOUBLE = new CSSProperty_BorderStyle("DOUBLE", "double");
        public static readonly CSSProperty_BorderStyle GROOVE = new CSSProperty_BorderStyle("GROOVE", "groove");
        public static readonly CSSProperty_BorderStyle RIDGE = new CSSProperty_BorderStyle("RIDGE", "ridge");
        public static readonly CSSProperty_BorderStyle INSET = new CSSProperty_BorderStyle("INSET", "inset");
        public static readonly CSSProperty_BorderStyle OUTSET = new CSSProperty_BorderStyle("OUTSET", "outset");
        public static readonly CSSProperty_BorderStyle INHERIT = new CSSProperty_BorderStyle("INHERIT", "inherit");
        public static readonly CSSProperty_BorderStyle INITIAL = new CSSProperty_BorderStyle("INITIAL", "initial");
        public static readonly CSSProperty_BorderStyle UNSET = new CSSProperty_BorderStyle("UNSET", "unset");

        internal CSSProperty_BorderStyle(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_BorderStyle, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_BorderWidth : SmartEnum<CSSProperty_BorderWidth, string>, CSSProperty
    {
        public static readonly CSSProperty_BorderWidth component_values = new CSSProperty_BorderWidth("component_values", "");
        public static readonly CSSProperty_BorderWidth length = new CSSProperty_BorderWidth("length", "");
        public static readonly CSSProperty_BorderWidth THIN = new CSSProperty_BorderWidth("THIN", "thin");
        public static readonly CSSProperty_BorderWidth MEDIUM = new CSSProperty_BorderWidth("MEDIUM", "medium");
        public static readonly CSSProperty_BorderWidth THICK = new CSSProperty_BorderWidth("THICK", "thick");
        public static readonly CSSProperty_BorderWidth INHERIT = new CSSProperty_BorderWidth("INHERIT", "inherit");
        public static readonly CSSProperty_BorderWidth INITIAL = new CSSProperty_BorderWidth("INITIAL", "initial");
        public static readonly CSSProperty_BorderWidth UNSET = new CSSProperty_BorderWidth("UNSET", "unset");

        internal CSSProperty_BorderWidth(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_BorderWidth, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_BoxShadow : SmartEnum<CSSProperty_BoxShadow, string>, CSSProperty
    {
        public static readonly CSSProperty_BoxShadow component_values = new CSSProperty_BoxShadow("component_values", "");
        public static readonly CSSProperty_BoxShadow NONE = new CSSProperty_BoxShadow("NONE", "none");
        public static readonly CSSProperty_BoxShadow INHERIT = new CSSProperty_BoxShadow("INHERIT", "inherit");
        public static readonly CSSProperty_BoxShadow INITIAL = new CSSProperty_BoxShadow("INITIAL", "initial");
        public static readonly CSSProperty_BoxShadow UNSET = new CSSProperty_BoxShadow("UNSET", "unset");

        internal CSSProperty_BoxShadow(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return true;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_BoxShadow, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_BoxSizing : SmartEnum<CSSProperty_BoxSizing, string>, CSSProperty
    {
        public static readonly CSSProperty_BoxSizing CONTENT_BOX = new CSSProperty_BoxSizing("CONTENT_BOX", "content-box");
        public static readonly CSSProperty_BoxSizing BORDER_BOX = new CSSProperty_BoxSizing("BORDER_BOX", "border-box");
        public static readonly CSSProperty_BoxSizing INHERIT = new CSSProperty_BoxSizing("INHERIT", "inherit");
        public static readonly CSSProperty_BoxSizing INITIAL = new CSSProperty_BoxSizing("INITIAL", "initial");
        public static readonly CSSProperty_BoxSizing UNSET = new CSSProperty_BoxSizing("UNSET", "unset");

        internal CSSProperty_BoxSizing(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_BoxSizing, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_Elevation : SmartEnum<CSSProperty_Elevation, string>, CSSProperty
    {
        public static readonly CSSProperty_Elevation angle = new CSSProperty_Elevation("angle", "");
        public static readonly CSSProperty_Elevation BELOW = new CSSProperty_Elevation("BELOW", "below");
        public static readonly CSSProperty_Elevation LEVEL = new CSSProperty_Elevation("LEVEL", "level");
        public static readonly CSSProperty_Elevation ABOVE = new CSSProperty_Elevation("ABOVE", "above");
        public static readonly CSSProperty_Elevation HIGHER = new CSSProperty_Elevation("HIGHER", "higher");
        public static readonly CSSProperty_Elevation LOWER = new CSSProperty_Elevation("LOWER", "lower");
        public static readonly CSSProperty_Elevation INHERIT = new CSSProperty_Elevation("INHERIT", "inherit");
        public static readonly CSSProperty_Elevation INITIAL = new CSSProperty_Elevation("INITIAL", "initial");
        public static readonly CSSProperty_Elevation UNSET = new CSSProperty_Elevation("UNSET", "unset");

        internal CSSProperty_Elevation(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return true;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Elevation, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_Font : SmartEnum<CSSProperty_Font, string>, CSSProperty
    {
        public static readonly CSSProperty_Font component_values = new CSSProperty_Font("component_values", "");
        public static readonly CSSProperty_Font CAPTION = new CSSProperty_Font("CAPTION", "caption");
        public static readonly CSSProperty_Font ICON = new CSSProperty_Font("ICON", "icon");
        public static readonly CSSProperty_Font MENU = new CSSProperty_Font("MENU", "menu");
        public static readonly CSSProperty_Font MESSAGE_BOX = new CSSProperty_Font("MESSAGE_BOX", "message-box");
        public static readonly CSSProperty_Font SMALL_CAPTION = new CSSProperty_Font("SMALL_CAPTION", "small-caption");
        public static readonly CSSProperty_Font STATUS_BAR = new CSSProperty_Font("STATUS_BAR", "status-bar");
        public static readonly CSSProperty_Font INHERIT = new CSSProperty_Font("INHERIT", "inherit");
        public static readonly CSSProperty_Font INITIAL = new CSSProperty_Font("INITIAL", "initial");
        public static readonly CSSProperty_Font UNSET = new CSSProperty_Font("UNSET", "unset");

        internal CSSProperty_Font(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return true;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Font, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_FontFamily : SmartEnum<CSSProperty_FontFamily, string>, CSSProperty
    {
        public static readonly CSSProperty_FontFamily list_values = new CSSProperty_FontFamily("list_values", "", "");
        public static readonly CSSProperty_FontFamily SERIF = new CSSProperty_FontFamily("SERIF", "serif", CSSProperty_Fields.FONT_SERIF);
        public static readonly CSSProperty_FontFamily SANS_SERIF = new CSSProperty_FontFamily("SANS_SERIF", "sans-serif", CSSProperty_Fields.FONT_SANS_SERIF);
        public static readonly CSSProperty_FontFamily CURSIVE = new CSSProperty_FontFamily("CURSIVE", "cursive", CSSProperty_Fields.FONT_CURSIVE);
        public static readonly CSSProperty_FontFamily FANTASY = new CSSProperty_FontFamily("FANTASY", "fantasy", CSSProperty_Fields.FONT_FANTASY);
        public static readonly CSSProperty_FontFamily MONOSPACE = new CSSProperty_FontFamily("MONOSPACE", "monospace", CSSProperty_Fields.FONT_MONOSPACED);
        public static readonly CSSProperty_FontFamily INHERIT = new CSSProperty_FontFamily("INHERIT", "inherit", "");
        public static readonly CSSProperty_FontFamily INITIAL = new CSSProperty_FontFamily("INITIAL", "initial", "");
        public static readonly CSSProperty_FontFamily UNSET = new CSSProperty_FontFamily("UNSET", "unset", "");

        internal string awtval;

        internal CSSProperty_FontFamily(string name, string value, string awtval) : base(name, value)
        {
            this.awtval = awtval;
        }

        public bool inherited()
        {
            return true;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_FontFamily, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }


        public string AWTValue
        {
            get
            {
                return awtval;
            }
        }
    }

    public sealed class CSSProperty_FontSize : SmartEnum<CSSProperty_FontSize, string>, CSSProperty
    {
        public static readonly CSSProperty_FontSize percentage = new CSSProperty_FontSize("percentage", "");
        public static readonly CSSProperty_FontSize length = new CSSProperty_FontSize("length", "");
        public static readonly CSSProperty_FontSize XX_SMALL = new CSSProperty_FontSize("XX_SMALL", "xx-small");
        public static readonly CSSProperty_FontSize X_SMALL = new CSSProperty_FontSize("X_SMALL", "x-small");
        public static readonly CSSProperty_FontSize SMALL = new CSSProperty_FontSize("SMALL", "small");
        public static readonly CSSProperty_FontSize MEDIUM = new CSSProperty_FontSize("MEDIUM", "medium");
        public static readonly CSSProperty_FontSize LARGE = new CSSProperty_FontSize("LARGE", "large");
        public static readonly CSSProperty_FontSize X_LARGE = new CSSProperty_FontSize("X_LARGE", "x-large");
        public static readonly CSSProperty_FontSize XX_LARGE = new CSSProperty_FontSize("XX_LARGE", "xx-large");
        public static readonly CSSProperty_FontSize LARGER = new CSSProperty_FontSize("LARGER", "larger");
        public static readonly CSSProperty_FontSize SMALLER = new CSSProperty_FontSize("SMALLER", "smaller");
        public static readonly CSSProperty_FontSize INHERIT = new CSSProperty_FontSize("INHERIT", "inherit");
        public static readonly CSSProperty_FontSize INITIAL = new CSSProperty_FontSize("INITIAL", "initial");
        public static readonly CSSProperty_FontSize UNSET = new CSSProperty_FontSize("UNSET", "unset");

        internal CSSProperty_FontSize(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_FontSize, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_FontStyle : SmartEnum<CSSProperty_FontStyle, string>, CSSProperty
    {
        public static readonly CSSProperty_FontStyle NORMAL = new CSSProperty_FontStyle("NORMAL", "normal");
        public static readonly CSSProperty_FontStyle ITALIC = new CSSProperty_FontStyle("ITALIC", "italic");
        public static readonly CSSProperty_FontStyle OBLIQUE = new CSSProperty_FontStyle("OBLIQUE", "oblique");
        public static readonly CSSProperty_FontStyle INHERIT = new CSSProperty_FontStyle("INHERIT", "inherit");
        public static readonly CSSProperty_FontStyle INITIAL = new CSSProperty_FontStyle("INITIAL", "initial");
        public static readonly CSSProperty_FontStyle UNSET = new CSSProperty_FontStyle("UNSET", "unset");


        internal CSSProperty_FontStyle(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return true;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_FontStyle, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_FontVariant : SmartEnum<CSSProperty_FontVariant, string>, CSSProperty
    {
        public static readonly CSSProperty_FontVariant SMALL_CAPS = new CSSProperty_FontVariant("SMALL_CAPS", "small-caps");
        public static readonly CSSProperty_FontVariant NORMAL = new CSSProperty_FontVariant("NORMAL", "normal");
        public static readonly CSSProperty_FontVariant INHERIT = new CSSProperty_FontVariant("INHERIT", "inherit");
        public static readonly CSSProperty_FontVariant INITIAL = new CSSProperty_FontVariant("INITIAL", "initial");
        public static readonly CSSProperty_FontVariant UNSET = new CSSProperty_FontVariant("UNSET", "unset");

        internal CSSProperty_FontVariant(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return true;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_FontVariant, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_FontWeight : SmartEnum<CSSProperty_FontWeight, string>, CSSProperty
    {
        public static readonly CSSProperty_FontWeight numeric_100 = new CSSProperty_FontWeight("numeric_100", "100");
        public static readonly CSSProperty_FontWeight numeric_200 = new CSSProperty_FontWeight("numeric_200", "200");
        public static readonly CSSProperty_FontWeight numeric_300 = new CSSProperty_FontWeight("numeric_300", "300");
        public static readonly CSSProperty_FontWeight numeric_400 = new CSSProperty_FontWeight("numeric_400", "400");
        public static readonly CSSProperty_FontWeight numeric_500 = new CSSProperty_FontWeight("numeric_500", "500");
        public static readonly CSSProperty_FontWeight numeric_600 = new CSSProperty_FontWeight("numeric_600", "600");
        public static readonly CSSProperty_FontWeight numeric_700 = new CSSProperty_FontWeight("numeric_700", "700");
        public static readonly CSSProperty_FontWeight numeric_800 = new CSSProperty_FontWeight("numeric_800", "800");
        public static readonly CSSProperty_FontWeight numeric_900 = new CSSProperty_FontWeight("numeric_900", "900");
        public static readonly CSSProperty_FontWeight NORMAL = new CSSProperty_FontWeight("NORMAL", "normal");
        public static readonly CSSProperty_FontWeight BOLD = new CSSProperty_FontWeight("BOLD", "bold");
        public static readonly CSSProperty_FontWeight BOLDER = new CSSProperty_FontWeight("BOLDER", "bolder");
        public static readonly CSSProperty_FontWeight LIGHTER = new CSSProperty_FontWeight("LIGHTER", "lighter");
        public static readonly CSSProperty_FontWeight INHERIT = new CSSProperty_FontWeight("INHERIT", "inherit");
        public static readonly CSSProperty_FontWeight INITIAL = new CSSProperty_FontWeight("INITIAL", "initial");
        public static readonly CSSProperty_FontWeight UNSET = new CSSProperty_FontWeight("UNSET", "unset");

        internal CSSProperty_FontWeight(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return true;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_FontWeight, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_LineHeight : SmartEnum<CSSProperty_LineHeight, string>, CSSProperty
    {
        public static readonly CSSProperty_LineHeight number = new CSSProperty_LineHeight("number", "");
        public static readonly CSSProperty_LineHeight length = new CSSProperty_LineHeight("length", "");
        public static readonly CSSProperty_LineHeight percentage = new CSSProperty_LineHeight("percentage", "");
        public static readonly CSSProperty_LineHeight NORMAL = new CSSProperty_LineHeight("NORMAL", "normal");
        public static readonly CSSProperty_LineHeight INHERIT = new CSSProperty_LineHeight("INHERIT", "inherit");
        public static readonly CSSProperty_LineHeight INITIAL = new CSSProperty_LineHeight("INITIAL", "initial");
        public static readonly CSSProperty_LineHeight UNSET = new CSSProperty_LineHeight("UNSET", "unset");

        internal CSSProperty_LineHeight(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return true;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(SmartEnum<CSSProperty_LineHeight, string> other)
        {
            bool areEqualBase = base.Equals(other);
            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }
    }

    public sealed class CSSProperty_TabSize : SmartEnum<CSSProperty_TabSize, string>, CSSProperty
    {
        public static readonly CSSProperty_TabSize integer = new CSSProperty_TabSize("integer", "");
        public static readonly CSSProperty_TabSize length = new CSSProperty_TabSize("length", "");
        public static readonly CSSProperty_TabSize INHERIT = new CSSProperty_TabSize("INHERIT", "inherit");
        public static readonly CSSProperty_TabSize INITIAL = new CSSProperty_TabSize("INITIAL", "initial");
        public static readonly CSSProperty_TabSize UNSET = new CSSProperty_TabSize("UNSET", "unset");

        internal CSSProperty_TabSize(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return true;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_TabSize, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_CaptionSide : SmartEnum<CSSProperty_CaptionSide, string>, CSSProperty
    {
        public static readonly CSSProperty_CaptionSide TOP = new CSSProperty_CaptionSide("TOP", "top");
        public static readonly CSSProperty_CaptionSide BOTTOM = new CSSProperty_CaptionSide("BOTTOM", "bottom");
        public static readonly CSSProperty_CaptionSide INHERIT = new CSSProperty_CaptionSide("INHERIT", "inherit");
        public static readonly CSSProperty_CaptionSide INITIAL = new CSSProperty_CaptionSide("INITIAL", "initial");
        public static readonly CSSProperty_CaptionSide UNSET = new CSSProperty_CaptionSide("UNSET", "unset");

        internal CSSProperty_CaptionSide(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return true;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }


        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_CaptionSide, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_Content : SmartEnum<CSSProperty_Content, string>, CSSProperty
    {
        public static readonly CSSProperty_Content list_values = new CSSProperty_Content("list_values", "");
        public static readonly CSSProperty_Content NORMAL = new CSSProperty_Content("NORMAL", "normal");
        public static readonly CSSProperty_Content NONE = new CSSProperty_Content("NONE", "none");
        public static readonly CSSProperty_Content INHERIT = new CSSProperty_Content("INHERIT", "inherit");
        public static readonly CSSProperty_Content INITIAL = new CSSProperty_Content("INITIAL", "initial");
        public static readonly CSSProperty_Content UNSET = new CSSProperty_Content("UNSET", "unset");

        internal CSSProperty_Content(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Content, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_CounterIncrement : SmartEnum<CSSProperty_CounterIncrement, string>, CSSProperty
    {
        public static readonly CSSProperty_CounterIncrement list_values = new CSSProperty_CounterIncrement("list_values", "");
        public static readonly CSSProperty_CounterIncrement NONE = new CSSProperty_CounterIncrement("NONE", "none");
        public static readonly CSSProperty_CounterIncrement INHERIT = new CSSProperty_CounterIncrement("INHERIT", "inherit");
        public static readonly CSSProperty_CounterIncrement INITIAL = new CSSProperty_CounterIncrement("INITIAL", "initial");
        public static readonly CSSProperty_CounterIncrement UNSET = new CSSProperty_CounterIncrement("UNSET", "unset");

        internal CSSProperty_CounterIncrement(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_CounterIncrement, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_CounterReset : SmartEnum<CSSProperty_CounterReset, string>, CSSProperty
    {
        public static readonly CSSProperty_CounterReset list_values = new CSSProperty_CounterReset("list_values", "");
        public static readonly CSSProperty_CounterReset NONE = new CSSProperty_CounterReset("NONE", "none");
        public static readonly CSSProperty_CounterReset INHERIT = new CSSProperty_CounterReset("INHERIT", "inherit");
        public static readonly CSSProperty_CounterReset INITIAL = new CSSProperty_CounterReset("INITIAL", "initial");
        public static readonly CSSProperty_CounterReset UNSET = new CSSProperty_CounterReset("UNSET", "unset");

        internal CSSProperty_CounterReset(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_CounterReset, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_Clear : SmartEnum<CSSProperty_Clear, string>, CSSProperty
    {
        public static readonly CSSProperty_Clear NONE = new CSSProperty_Clear("NONE", "none");
        public static readonly CSSProperty_Clear LEFT = new CSSProperty_Clear("LEFT", "left");
        public static readonly CSSProperty_Clear RIGHT = new CSSProperty_Clear("RIGHT", "right");
        public static readonly CSSProperty_Clear BOTH = new CSSProperty_Clear("BOTH", "both");
        public static readonly CSSProperty_Clear INHERIT = new CSSProperty_Clear("INHERIT", "inherit");
        public static readonly CSSProperty_Clear INITIAL = new CSSProperty_Clear("INITIAL", "initial");
        public static readonly CSSProperty_Clear UNSET = new CSSProperty_Clear("UNSET", "unset");

        internal CSSProperty_Clear(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Clear, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_Clip : SmartEnum<CSSProperty_Clip, string>, CSSProperty
    {
        public static readonly CSSProperty_Clip shape = new CSSProperty_Clip("shape", "");
        public static readonly CSSProperty_Clip AUTO = new CSSProperty_Clip("AUTO", "auto");
        public static readonly CSSProperty_Clip INHERIT = new CSSProperty_Clip("INHERIT", "inherit");
        public static readonly CSSProperty_Clip INITIAL = new CSSProperty_Clip("INITIAL", "initial");
        public static readonly CSSProperty_Clip UNSET = new CSSProperty_Clip("UNSET", "unset");

        internal CSSProperty_Clip(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Clip, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_Cursor : SmartEnum<CSSProperty_Cursor, string>, CSSProperty
    {
        public static readonly CSSProperty_Cursor AUTO = new CSSProperty_Cursor("AUTO", "auto");
        public static readonly CSSProperty_Cursor CROSSHAIR = new CSSProperty_Cursor("CROSSHAIR", "crosshair");
        public static readonly CSSProperty_Cursor DEFAULT = new CSSProperty_Cursor("DEFAULT", "default");
        public static readonly CSSProperty_Cursor POINTER = new CSSProperty_Cursor("POINTER", "pointer");
        public static readonly CSSProperty_Cursor MOVE = new CSSProperty_Cursor("MOVE", "move");
        public static readonly CSSProperty_Cursor E_RESIZE = new CSSProperty_Cursor("E_RESIZE", "e-resize");
        public static readonly CSSProperty_Cursor NE_RESIZE = new CSSProperty_Cursor("NE_RESIZE", "ne-resize");
        public static readonly CSSProperty_Cursor NW_RESIZE = new CSSProperty_Cursor("NW_RESIZE", "nw-resize");
        public static readonly CSSProperty_Cursor N_RESIZE = new CSSProperty_Cursor("N_RESIZE", "n-resize");
        public static readonly CSSProperty_Cursor SE_RESIZE = new CSSProperty_Cursor("SE_RESIZE", "se-resize");
        public static readonly CSSProperty_Cursor SW_RESIZE = new CSSProperty_Cursor("SW_RESIZE", "sw-resize");
        public static readonly CSSProperty_Cursor S_RESIZE = new CSSProperty_Cursor("S_RESIZE", "s-resize");
        public static readonly CSSProperty_Cursor W_RESIZE = new CSSProperty_Cursor("W_RESIZE", "w-resize");
        public static readonly CSSProperty_Cursor TEXT = new CSSProperty_Cursor("TEXT", "text");
        public static readonly CSSProperty_Cursor WAIT = new CSSProperty_Cursor("WAIT", "wait");
        public static readonly CSSProperty_Cursor PROGRESS = new CSSProperty_Cursor("PROGRESS", "progress");
        public static readonly CSSProperty_Cursor HELP = new CSSProperty_Cursor("HELP", "help");
        public static readonly CSSProperty_Cursor INHERIT = new CSSProperty_Cursor("INHERIT", "inherit");
        public static readonly CSSProperty_Cursor INITIAL = new CSSProperty_Cursor("INITIAL", "initial");
        public static readonly CSSProperty_Cursor UNSET = new CSSProperty_Cursor("UNSET", "unset");

        internal CSSProperty_Cursor(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return true;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Cursor, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_Direction : SmartEnum<CSSProperty_Direction, string>, CSSProperty
    {
        public static readonly CSSProperty_Direction LTR = new CSSProperty_Direction("LTR", "ltr");
        public static readonly CSSProperty_Direction RTL = new CSSProperty_Direction("RTL", "rtl");
        public static readonly CSSProperty_Direction INHERIT = new CSSProperty_Direction("INHERIT", "inherit");
        public static readonly CSSProperty_Direction INITIAL = new CSSProperty_Direction("INITIAL", "initial");
        public static readonly CSSProperty_Direction UNSET = new CSSProperty_Direction("UNSET", "unset");

        internal CSSProperty_Direction(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return true;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Direction, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_Display : SmartEnum<CSSProperty_Display, string>, CSSProperty
    {
        public static readonly CSSProperty_Display INLINE = new CSSProperty_Display("INLINE", "inline");
        public static readonly CSSProperty_Display BLOCK = new CSSProperty_Display("BLOCK", "block");
        public static readonly CSSProperty_Display LIST_ITEM = new CSSProperty_Display("LIST_ITEM", "list-item");
        public static readonly CSSProperty_Display RUN_IN = new CSSProperty_Display("RUN_IN", "run-in");
        public static readonly CSSProperty_Display INLINE_BLOCK = new CSSProperty_Display("INLINE_BLOCK", "inline-block");
        public static readonly CSSProperty_Display TABLE = new CSSProperty_Display("TABLE", "table");
        public static readonly CSSProperty_Display INLINE_TABLE = new CSSProperty_Display("INLINE_TABLE", "inline-table");
        public static readonly CSSProperty_Display TABLE_ROW_GROUP = new CSSProperty_Display("TABLE_ROW_GROUP", "table-row-group");
        public static readonly CSSProperty_Display TABLE_HEADER_GROUP = new CSSProperty_Display("TABLE_HEADER_GROUP", "table-header-group");
        public static readonly CSSProperty_Display TABLE_FOOTER_GROUP = new CSSProperty_Display("TABLE_FOOTER_GROUP", "table-footer-group");
        public static readonly CSSProperty_Display TABLE_ROW = new CSSProperty_Display("TABLE_ROW", "table-row");
        public static readonly CSSProperty_Display TABLE_COLUMN_GROUP = new CSSProperty_Display("TABLE_COLUMN_GROUP", "table-column-group");
        public static readonly CSSProperty_Display TABLE_COLUMN = new CSSProperty_Display("TABLE_COLUMN", "table-column");
        public static readonly CSSProperty_Display TABLE_CELL = new CSSProperty_Display("TABLE_CELL", "table-cell");
        public static readonly CSSProperty_Display TABLE_CAPTION = new CSSProperty_Display("TABLE_CAPTION", "table-caption");
        public static readonly CSSProperty_Display FLEX = new CSSProperty_Display("FLEX", "flex");
        public static readonly CSSProperty_Display INLINE_FLEX = new CSSProperty_Display("INLINE_FLEX", "inline-flex");
        public static readonly CSSProperty_Display GRID = new CSSProperty_Display("GRID", "grid");
        public static readonly CSSProperty_Display INLINE_GRID = new CSSProperty_Display("INLINE_GRID", "inline-grid");
        public static readonly CSSProperty_Display NONE = new CSSProperty_Display("NONE", "none");
        public static readonly CSSProperty_Display INHERIT = new CSSProperty_Display("INHERIT", "inherit");
        public static readonly CSSProperty_Display INITIAL = new CSSProperty_Display("INITIAL", "initial");
        public static readonly CSSProperty_Display UNSET = new CSSProperty_Display("UNSET", "unset");


        internal CSSProperty_Display(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Display, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_Width : SmartEnum<CSSProperty_Width, string>, CSSProperty
    {
        public static readonly CSSProperty_Width length = new CSSProperty_Width("length", "");
        public static readonly CSSProperty_Width percentage = new CSSProperty_Width("percentage", "");
        public static readonly CSSProperty_Width AUTO = new CSSProperty_Width("AUTO", "auto");
        public static readonly CSSProperty_Width INHERIT = new CSSProperty_Width("INHERIT", "inherit");
        public static readonly CSSProperty_Width INITIAL = new CSSProperty_Width("INITIAL", "initial");
        public static readonly CSSProperty_Width UNSET = new CSSProperty_Width("UNSET", "unset");

        internal CSSProperty_Width(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Width, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_MinWidth : SmartEnum<CSSProperty_MinWidth, string>, CSSProperty
    {
        public static readonly CSSProperty_MinWidth length = new CSSProperty_MinWidth("length", "");
        public static readonly CSSProperty_MinWidth percentage = new CSSProperty_MinWidth("percentage", "");
        public static readonly CSSProperty_MinWidth INHERIT = new CSSProperty_MinWidth("INHERIT", "inherit");
        public static readonly CSSProperty_MinWidth INITIAL = new CSSProperty_MinWidth("INITIAL", "initial");
        public static readonly CSSProperty_MinWidth UNSET = new CSSProperty_MinWidth("UNSET", "unset");

        internal CSSProperty_MinWidth(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_MinWidth, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_MaxWidth : SmartEnum<CSSProperty_MaxWidth, string>, CSSProperty
    {
        public static readonly CSSProperty_MaxWidth length = new CSSProperty_MaxWidth("length", "");
        public static readonly CSSProperty_MaxWidth percentage = new CSSProperty_MaxWidth("percentage", "");
        public static readonly CSSProperty_MaxWidth NONE = new CSSProperty_MaxWidth("NONE", "none");
        public static readonly CSSProperty_MaxWidth INHERIT = new CSSProperty_MaxWidth("INHERIT", "inherit");
        public static readonly CSSProperty_MaxWidth INITIAL = new CSSProperty_MaxWidth("INITIAL", "initial");
        public static readonly CSSProperty_MaxWidth UNSET = new CSSProperty_MaxWidth("UNSET", "unset");

        internal CSSProperty_MaxWidth(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_MaxWidth, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_Height : SmartEnum<CSSProperty_Height, string>, CSSProperty
    {
        public static readonly CSSProperty_Height length = new CSSProperty_Height("length", "");
        public static readonly CSSProperty_Height percentage = new CSSProperty_Height("percentage", "");
        public static readonly CSSProperty_Height AUTO = new CSSProperty_Height("AUTO", "auto");
        public static readonly CSSProperty_Height INHERIT = new CSSProperty_Height("INHERIT", "inherit");
        public static readonly CSSProperty_Height INITIAL = new CSSProperty_Height("INITIAL", "initial");
        public static readonly CSSProperty_Height UNSET = new CSSProperty_Height("UNSET", "unset");

        internal CSSProperty_Height(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Height, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_MinHeight : SmartEnum<CSSProperty_MinHeight, string>, CSSProperty
    {
        public static readonly CSSProperty_MinHeight length = new CSSProperty_MinHeight("length", "");
        public static readonly CSSProperty_MinHeight percentage = new CSSProperty_MinHeight("percentage", "");
        public static readonly CSSProperty_MinHeight INHERIT = new CSSProperty_MinHeight("INHERIT", "inherit");
        public static readonly CSSProperty_MinHeight INITIAL = new CSSProperty_MinHeight("INITIAL", "initial");
        public static readonly CSSProperty_MinHeight UNSET = new CSSProperty_MinHeight("UNSET", "unset");

        internal CSSProperty_MinHeight(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_MinHeight, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_MaxHeight : SmartEnum<CSSProperty_MaxHeight, string>, CSSProperty
    {
        public static readonly CSSProperty_MaxHeight length = new CSSProperty_MaxHeight("length", "");
        public static readonly CSSProperty_MaxHeight percentage = new CSSProperty_MaxHeight("percentage", "");
        public static readonly CSSProperty_MaxHeight NONE = new CSSProperty_MaxHeight("NONE", "none");
        public static readonly CSSProperty_MaxHeight INHERIT = new CSSProperty_MaxHeight("INHERIT", "inherit");
        public static readonly CSSProperty_MaxHeight INITIAL = new CSSProperty_MaxHeight("INITIAL", "initial");
        public static readonly CSSProperty_MaxHeight UNSET = new CSSProperty_MaxHeight("UNSET", "unset");

        internal CSSProperty_MaxHeight(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_MaxHeight, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_EmptyCells : SmartEnum<CSSProperty_EmptyCells, string>, CSSProperty
    {
        public static readonly CSSProperty_EmptyCells SHOW = new CSSProperty_EmptyCells("SHOW", "show");
        public static readonly CSSProperty_EmptyCells HIDE = new CSSProperty_EmptyCells("HIDE", "hide");
        public static readonly CSSProperty_EmptyCells INHERIT = new CSSProperty_EmptyCells("INHERIT", "inherit");
        public static readonly CSSProperty_EmptyCells INITIAL = new CSSProperty_EmptyCells("INITIAL", "initial");
        public static readonly CSSProperty_EmptyCells UNSET = new CSSProperty_EmptyCells("UNSET", "unset");

        internal CSSProperty_EmptyCells(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return true;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_EmptyCells, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_Floating : SmartEnum<CSSProperty_Floating, string>, CSSProperty
    {
        public static readonly CSSProperty_Floating NONE = new CSSProperty_Floating("NONE", "none");
        public static readonly CSSProperty_Floating LEFT = new CSSProperty_Floating("LEFT", "left");
        public static readonly CSSProperty_Floating RIGHT = new CSSProperty_Floating("RIGHT", "right");
        public static readonly CSSProperty_Floating INHERIT = new CSSProperty_Floating("INHERIT", "inherit");
        public static readonly CSSProperty_Floating INITIAL = new CSSProperty_Floating("INITIAL", "initial");
        public static readonly CSSProperty_Floating UNSET = new CSSProperty_Floating("UNSET", "unset");

        private CSSProperty_Floating(string name, string value) : base(name, value)
        {
        }

        public bool inherited()
        {
            return false;
        }

        public bool equalsInherit()
        {
            return this == INHERIT;
        }

        public bool equalsInitial()
        {
            return this == INITIAL;
        }

        public bool equalsUnset()
        {
            return this == UNSET;
        }

        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }

        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Floating, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }


    public sealed class CSSProperty_ListStyle : SmartEnum<CSSProperty_ListStyle, string>, CSSProperty
    {
        public static readonly CSSProperty_ListStyle component_values = new CSSProperty_ListStyle("component_values", "");
        public static readonly CSSProperty_ListStyle INHERIT = new CSSProperty_ListStyle("INHERIT", "inherit");
        public static readonly CSSProperty_ListStyle INITIAL = new CSSProperty_ListStyle("INITIAL", "initial");
        public static readonly CSSProperty_ListStyle UNSET = new CSSProperty_ListStyle("UNSET", "unset");

        internal CSSProperty_ListStyle(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_ListStyle, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_ListStyleImage : SmartEnum<CSSProperty_ListStyleImage, string>, CSSProperty
    {
        public static readonly CSSProperty_ListStyleImage uri = new CSSProperty_ListStyleImage("uri", "");
        public static readonly CSSProperty_ListStyleImage NONE = new CSSProperty_ListStyleImage("NONE", "none");
        public static readonly CSSProperty_ListStyleImage INHERIT = new CSSProperty_ListStyleImage("INHERIT", "inherit");
        public static readonly CSSProperty_ListStyleImage INITIAL = new CSSProperty_ListStyleImage("INITIAL", "initial");
        public static readonly CSSProperty_ListStyleImage UNSET = new CSSProperty_ListStyleImage("UNSET", "unset");

        internal CSSProperty_ListStyleImage(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_ListStyleImage, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_ListStylePosition : SmartEnum<CSSProperty_ListStylePosition, string>, CSSProperty
    {
        public static readonly CSSProperty_ListStylePosition INSIDE = new CSSProperty_ListStylePosition("INSIDE", "inside");
        public static readonly CSSProperty_ListStylePosition OUTSIDE = new CSSProperty_ListStylePosition("OUTSIDE", "outside");
        public static readonly CSSProperty_ListStylePosition INHERIT = new CSSProperty_ListStylePosition("INHERIT", "inherit");
        public static readonly CSSProperty_ListStylePosition INITIAL = new CSSProperty_ListStylePosition("INITIAL", "initial");
        public static readonly CSSProperty_ListStylePosition UNSET = new CSSProperty_ListStylePosition("UNSET", "unset");

        internal CSSProperty_ListStylePosition(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_ListStylePosition, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_ListStyleType : SmartEnum<CSSProperty_ListStyleType, string>, CSSProperty
    {
        public static readonly CSSProperty_ListStyleType DISC = new CSSProperty_ListStyleType("DISC", "disc");
        public static readonly CSSProperty_ListStyleType CIRCLE = new CSSProperty_ListStyleType("CIRCLE", "circle");
        public static readonly CSSProperty_ListStyleType SQUARE = new CSSProperty_ListStyleType("SQUARE", "square");
        public static readonly CSSProperty_ListStyleType DECIMAL = new CSSProperty_ListStyleType("DECIMAL", "decimal");
        public static readonly CSSProperty_ListStyleType DECIMAL_LEADING_ZERO = new CSSProperty_ListStyleType("DECIMAL_LEADING_ZERO", "decimal-leading-zero");
        public static readonly CSSProperty_ListStyleType LOWER_ROMAN = new CSSProperty_ListStyleType("LOWER_ROMAN", "lower-roman");
        public static readonly CSSProperty_ListStyleType UPPER_ROMAN = new CSSProperty_ListStyleType("UPPER_ROMAN", "upper-roman");
        public static readonly CSSProperty_ListStyleType LOWER_GREEK = new CSSProperty_ListStyleType("LOWER_GREEK", "lower-greek");
        public static readonly CSSProperty_ListStyleType LOWER_LATIN = new CSSProperty_ListStyleType("LOWER_LATIN", "lower-latin");
        public static readonly CSSProperty_ListStyleType UPPER_LATN = new CSSProperty_ListStyleType("UPPER_LATN", "upper-latin");
        public static readonly CSSProperty_ListStyleType ARMENIAN = new CSSProperty_ListStyleType("ARMENIAN", "armenian");
        public static readonly CSSProperty_ListStyleType GEORGIAN = new CSSProperty_ListStyleType("GEORGIAN", "georgian");
        public static readonly CSSProperty_ListStyleType LOWER_ALPHA = new CSSProperty_ListStyleType("LOWER_ALPHA", "lower-alpha");
        public static readonly CSSProperty_ListStyleType UPPER_ALPHA = new CSSProperty_ListStyleType("UPPER_ALPHA", "upper-alpha");
        public static readonly CSSProperty_ListStyleType NONE = new CSSProperty_ListStyleType("NONE", "none");
        public static readonly CSSProperty_ListStyleType INHERIT = new CSSProperty_ListStyleType("INHERIT", "inherit");
        public static readonly CSSProperty_ListStyleType INITIAL = new CSSProperty_ListStyleType("INITIAL", "initial");
        public static readonly CSSProperty_ListStyleType UNSET = new CSSProperty_ListStyleType("UNSET", "unset");

        internal CSSProperty_ListStyleType(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_ListStyleType, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Margin : SmartEnum<CSSProperty_Margin, string>, CSSProperty
    {
        public static readonly CSSProperty_Margin length = new CSSProperty_Margin("length", "");
        public static readonly CSSProperty_Margin percentage = new CSSProperty_Margin("percentage", "");
        public static readonly CSSProperty_Margin component_values = new CSSProperty_Margin("component_values", "");
        public static readonly CSSProperty_Margin AUTO = new CSSProperty_Margin("AUTO", "auto");
        public static readonly CSSProperty_Margin INHERIT = new CSSProperty_Margin("INHERIT", "inherit");
        public static readonly CSSProperty_Margin INITIAL = new CSSProperty_Margin("INITIAL", "initial");
        public static readonly CSSProperty_Margin UNSET = new CSSProperty_Margin("UNSET", "unset");

        internal CSSProperty_Margin(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Margin, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Opacity : SmartEnum<CSSProperty_Opacity, string>, CSSProperty
    {
        public static readonly CSSProperty_Opacity number = new CSSProperty_Opacity("number", "");
        public static readonly CSSProperty_Opacity INHERIT = new CSSProperty_Opacity("INHERIT", "inherit");
        public static readonly CSSProperty_Opacity INITIAL = new CSSProperty_Opacity("INITIAL", "initial");
        public static readonly CSSProperty_Opacity UNSET = new CSSProperty_Opacity("UNSET", "unset");

        internal CSSProperty_Opacity(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Opacity, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Orphans : SmartEnum<CSSProperty_Orphans, string>, CSSProperty
    {
        public static readonly CSSProperty_Orphans integer = new CSSProperty_Orphans("integer", "");
        public static readonly CSSProperty_Orphans INHERIT = new CSSProperty_Orphans("INHERIT", "inherit");
        public static readonly CSSProperty_Orphans INITIAL = new CSSProperty_Orphans("INITIAL", "initial");
        public static readonly CSSProperty_Orphans UNSET = new CSSProperty_Orphans("UNSET", "unset");

        internal CSSProperty_Orphans(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Orphans, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Outline : SmartEnum<CSSProperty_Outline, string>, CSSProperty
    {
        public static readonly CSSProperty_Outline component_values = new CSSProperty_Outline("component_values", "");
        public static readonly CSSProperty_Outline INHERIT = new CSSProperty_Outline("INHERIT", "inherit");
        public static readonly CSSProperty_Outline INITIAL = new CSSProperty_Outline("INITIAL", "initial");
        public static readonly CSSProperty_Outline UNSET = new CSSProperty_Outline("UNSET", "unset");

        internal CSSProperty_Outline(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Outline, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_OutlineWidth : SmartEnum<CSSProperty_OutlineWidth, string>, CSSProperty
    {
        public static readonly CSSProperty_OutlineWidth length = new CSSProperty_OutlineWidth("length", "");
        public static readonly CSSProperty_OutlineWidth THIN = new CSSProperty_OutlineWidth("THIN", "thin");
        public static readonly CSSProperty_OutlineWidth MEDIUM = new CSSProperty_OutlineWidth("MEDIUM", "medium");
        public static readonly CSSProperty_OutlineWidth THICK = new CSSProperty_OutlineWidth("THICK", "thick");
        public static readonly CSSProperty_OutlineWidth INHERIT = new CSSProperty_OutlineWidth("INHERIT", "inherit");
        public static readonly CSSProperty_OutlineWidth INITIAL = new CSSProperty_OutlineWidth("INITIAL", "initial");
        public static readonly CSSProperty_OutlineWidth UNSET = new CSSProperty_OutlineWidth("UNSET", "unset");

        internal CSSProperty_OutlineWidth(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_OutlineWidth, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_OutlineStyle : SmartEnum<CSSProperty_OutlineStyle, string>, CSSProperty
    {
        public static readonly CSSProperty_OutlineStyle NONE = new CSSProperty_OutlineStyle("NONE", "none");
        public static readonly CSSProperty_OutlineStyle DOTTED = new CSSProperty_OutlineStyle("DOTTED", "dotted");
        public static readonly CSSProperty_OutlineStyle DASHED = new CSSProperty_OutlineStyle("DASHED", "dashed");
        public static readonly CSSProperty_OutlineStyle SOLID = new CSSProperty_OutlineStyle("SOLID", "solid");
        public static readonly CSSProperty_OutlineStyle DOUBLE = new CSSProperty_OutlineStyle("DOUBLE", "double");
        public static readonly CSSProperty_OutlineStyle GROOVE = new CSSProperty_OutlineStyle("GROOVE", "groove");
        public static readonly CSSProperty_OutlineStyle RIDGE = new CSSProperty_OutlineStyle("RIDGE", "ridge");
        public static readonly CSSProperty_OutlineStyle INSET = new CSSProperty_OutlineStyle("INSET", "inset");
        public static readonly CSSProperty_OutlineStyle OUTSET = new CSSProperty_OutlineStyle("OUTSET", "outset");
        public static readonly CSSProperty_OutlineStyle HIDDEN = new CSSProperty_OutlineStyle("HIDDEN", "hidden");
        public static readonly CSSProperty_OutlineStyle INHERIT = new CSSProperty_OutlineStyle("INHERIT", "inherit");
        public static readonly CSSProperty_OutlineStyle INITIAL = new CSSProperty_OutlineStyle("INITIAL", "initial");
        public static readonly CSSProperty_OutlineStyle UNSET = new CSSProperty_OutlineStyle("UNSET", "unset");

        internal CSSProperty_OutlineStyle(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_OutlineStyle, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_OutlineColor : SmartEnum<CSSProperty_OutlineColor, string>, CSSProperty
    {
        public static readonly CSSProperty_OutlineColor color = new CSSProperty_OutlineColor("color", "");
        public static readonly CSSProperty_OutlineColor INVERT = new CSSProperty_OutlineColor("INVERT", "invert");
        public static readonly CSSProperty_OutlineColor INHERIT = new CSSProperty_OutlineColor("INHERIT", "inherit");
        public static readonly CSSProperty_OutlineColor INITIAL = new CSSProperty_OutlineColor("INITIAL", "initial");
        public static readonly CSSProperty_OutlineColor UNSET = new CSSProperty_OutlineColor("UNSET", "unset");

        internal CSSProperty_OutlineColor(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_OutlineColor, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Overflow : SmartEnum<CSSProperty_Overflow, string>, CSSProperty
    {
        public static readonly CSSProperty_Overflow component_values = new CSSProperty_Overflow("component_values", "");
        public static readonly CSSProperty_Overflow VISIBLE = new CSSProperty_Overflow("VISIBLE", "visible");
        public static readonly CSSProperty_Overflow HIDDEN = new CSSProperty_Overflow("HIDDEN", "hidden");
        public static readonly CSSProperty_Overflow CLIP = new CSSProperty_Overflow("CLIP", "clip");
        public static readonly CSSProperty_Overflow SCROLL = new CSSProperty_Overflow("SCROLL", "scroll");
        public static readonly CSSProperty_Overflow AUTO = new CSSProperty_Overflow("AUTO", "auto");
        public static readonly CSSProperty_Overflow INHERIT = new CSSProperty_Overflow("INHERIT", "inherit");
        public static readonly CSSProperty_Overflow INITIAL = new CSSProperty_Overflow("INITIAL", "initial");
        public static readonly CSSProperty_Overflow UNSET = new CSSProperty_Overflow("UNSET", "unset");

        internal CSSProperty_Overflow(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Overflow, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Padding : SmartEnum<CSSProperty_Padding, string>, CSSProperty
    {
        public static readonly CSSProperty_Padding length = new CSSProperty_Padding("length", "");
        public static readonly CSSProperty_Padding percentage = new CSSProperty_Padding("percentage", "");
        public static readonly CSSProperty_Padding component_values = new CSSProperty_Padding("component_values", "");
        public static readonly CSSProperty_Padding AUTO = new CSSProperty_Padding("AUTO", "auto");
        public static readonly CSSProperty_Padding INHERIT = new CSSProperty_Padding("INHERIT", "inherit");
        public static readonly CSSProperty_Padding INITIAL = new CSSProperty_Padding("INITIAL", "initial");
        public static readonly CSSProperty_Padding UNSET = new CSSProperty_Padding("UNSET", "unset");

        internal CSSProperty_Padding(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Padding, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_PageBreak : SmartEnum<CSSProperty_PageBreak, string>, CSSProperty
    {
        public static readonly CSSProperty_PageBreak AUTO = new CSSProperty_PageBreak("AUTO", "auto");
        public static readonly CSSProperty_PageBreak ALWAYS = new CSSProperty_PageBreak("ALWAYS", "always");
        public static readonly CSSProperty_PageBreak AVOID = new CSSProperty_PageBreak("AVOID", "avoid");
        public static readonly CSSProperty_PageBreak LEFT = new CSSProperty_PageBreak("LEFT", "left");
        public static readonly CSSProperty_PageBreak RIGHT = new CSSProperty_PageBreak("RIGHT", "right");
        public static readonly CSSProperty_PageBreak INHERIT = new CSSProperty_PageBreak("INHERIT", "inherit");
        public static readonly CSSProperty_PageBreak INITIAL = new CSSProperty_PageBreak("INITIAL", "initial");
        public static readonly CSSProperty_PageBreak UNSET = new CSSProperty_PageBreak("UNSET", "unset");

        internal CSSProperty_PageBreak(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_PageBreak, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_PageBreakInside : SmartEnum<CSSProperty_PageBreakInside, string>, CSSProperty
    {
        public static readonly CSSProperty_PageBreakInside AUTO = new CSSProperty_PageBreakInside("AUTO", "auto");
        public static readonly CSSProperty_PageBreakInside AVOID = new CSSProperty_PageBreakInside("AVOID", "avoid");
        public static readonly CSSProperty_PageBreakInside INHERIT = new CSSProperty_PageBreakInside("INHERIT", "inherit");
        public static readonly CSSProperty_PageBreakInside INITIAL = new CSSProperty_PageBreakInside("INITIAL", "initial");
        public static readonly CSSProperty_PageBreakInside UNSET = new CSSProperty_PageBreakInside("UNSET", "unset");

        internal CSSProperty_PageBreakInside(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_PageBreakInside, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Pause : SmartEnum<CSSProperty_Pause, string>, CSSProperty
    {
        public static readonly CSSProperty_Pause component_values = new CSSProperty_Pause("component_values", "");
        public static readonly CSSProperty_Pause time = new CSSProperty_Pause("time", "");
        public static readonly CSSProperty_Pause percentage = new CSSProperty_Pause("percentage", "");
        public static readonly CSSProperty_Pause INHERIT = new CSSProperty_Pause("INHERIT", "inherit");
        public static readonly CSSProperty_Pause INITIAL = new CSSProperty_Pause("INITIAL", "initial");
        public static readonly CSSProperty_Pause UNSET = new CSSProperty_Pause("UNSET", "unset");

        internal CSSProperty_Pause(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Pause, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_PitchRange : SmartEnum<CSSProperty_PitchRange, string>, CSSProperty
    {
        public static readonly CSSProperty_PitchRange number = new CSSProperty_PitchRange("number", "");
        public static readonly CSSProperty_PitchRange INHERIT = new CSSProperty_PitchRange("INHERIT", "inherit");
        public static readonly CSSProperty_PitchRange INITIAL = new CSSProperty_PitchRange("INITIAL", "initial");
        public static readonly CSSProperty_PitchRange UNSET = new CSSProperty_PitchRange("UNSET", "unset");

        internal CSSProperty_PitchRange(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_PitchRange, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Pitch : SmartEnum<CSSProperty_Pitch, string>, CSSProperty
    {
        public static readonly CSSProperty_Pitch frequency = new CSSProperty_Pitch("frequency", "");
        public static readonly CSSProperty_Pitch X_LOW = new CSSProperty_Pitch("X_LOW", "x-low");
        public static readonly CSSProperty_Pitch LOW = new CSSProperty_Pitch("LOW", "low");
        public static readonly CSSProperty_Pitch MEDIUM = new CSSProperty_Pitch("MEDIUM", "medium");
        public static readonly CSSProperty_Pitch HIGH = new CSSProperty_Pitch("HIGH", "high");
        public static readonly CSSProperty_Pitch X_HIGH = new CSSProperty_Pitch("X_HIGH", "x-high");
        public static readonly CSSProperty_Pitch INHERIT = new CSSProperty_Pitch("INHERIT", "inherit");
        public static readonly CSSProperty_Pitch INITIAL = new CSSProperty_Pitch("INITIAL", "initial");
        public static readonly CSSProperty_Pitch UNSET = new CSSProperty_Pitch("UNSET", "unset");

        internal CSSProperty_Pitch(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Pitch, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_PlayDuring : SmartEnum<CSSProperty_PlayDuring, string>, CSSProperty
    {
        public static readonly CSSProperty_PlayDuring uri = new CSSProperty_PlayDuring("uri", "");
        public static readonly CSSProperty_PlayDuring uri_mix = new CSSProperty_PlayDuring("uri_mix", "");
        public static readonly CSSProperty_PlayDuring uri_repeat = new CSSProperty_PlayDuring("uri_repeat", "");
        public static readonly CSSProperty_PlayDuring AUTO = new CSSProperty_PlayDuring("AUTO", "auto");
        public static readonly CSSProperty_PlayDuring NONE = new CSSProperty_PlayDuring("NONE", "none");
        public static readonly CSSProperty_PlayDuring INHERIT = new CSSProperty_PlayDuring("INHERIT", "inherit");
        public static readonly CSSProperty_PlayDuring INITIAL = new CSSProperty_PlayDuring("INITIAL", "initial");
        public static readonly CSSProperty_PlayDuring UNSET = new CSSProperty_PlayDuring("UNSET", "unset");

        internal CSSProperty_PlayDuring(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_PlayDuring, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Position : SmartEnum<CSSProperty_Position, string>, CSSProperty
    {
        public static readonly CSSProperty_Position STATIC = new CSSProperty_Position("STATIC", "static");
        public static readonly CSSProperty_Position RELATIVE = new CSSProperty_Position("RELATIVE", "relative");
        public static readonly CSSProperty_Position ABSOLUTE = new CSSProperty_Position("ABSOLUTE", "absolute");
        public static readonly CSSProperty_Position FIXED = new CSSProperty_Position("FIXED", "fixed");
        public static readonly CSSProperty_Position INHERIT = new CSSProperty_Position("INHERIT", "inherit");
        public static readonly CSSProperty_Position INITIAL = new CSSProperty_Position("INITIAL", "initial");
        public static readonly CSSProperty_Position UNSET = new CSSProperty_Position("UNSET", "unset");

        internal CSSProperty_Position(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Position, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Richness : SmartEnum<CSSProperty_Richness, string>, CSSProperty
    {
        public static readonly CSSProperty_Richness number = new CSSProperty_Richness("number", "number");
        public static readonly CSSProperty_Richness INHERIT = new CSSProperty_Richness("INHERIT", "inherit");
        public static readonly CSSProperty_Richness INITIAL = new CSSProperty_Richness("INITIAL", "initial");
        public static readonly CSSProperty_Richness UNSET = new CSSProperty_Richness("UNSET", "unset");

        internal CSSProperty_Richness(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Richness, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_SpeakHeader : SmartEnum<CSSProperty_SpeakHeader, string>, CSSProperty
    {
        public static readonly CSSProperty_SpeakHeader ONCE = new CSSProperty_SpeakHeader("ONCE", "once");
        public static readonly CSSProperty_SpeakHeader ALWAYS = new CSSProperty_SpeakHeader("ALWAYS", "always");
        public static readonly CSSProperty_SpeakHeader INHERIT = new CSSProperty_SpeakHeader("INHERIT", "inherit");
        public static readonly CSSProperty_SpeakHeader INITIAL = new CSSProperty_SpeakHeader("INITIAL", "initial");
        public static readonly CSSProperty_SpeakHeader UNSET = new CSSProperty_SpeakHeader("UNSET", "unset");

        internal CSSProperty_SpeakHeader(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_SpeakHeader, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_SpeakNumeral : SmartEnum<CSSProperty_SpeakNumeral, string>, CSSProperty
    {
        public static readonly CSSProperty_SpeakNumeral DIGITS = new CSSProperty_SpeakNumeral("DIGITS", "digits");
        public static readonly CSSProperty_SpeakNumeral CONTINUOUS = new CSSProperty_SpeakNumeral("CONTINUOUS", "continuous");
        public static readonly CSSProperty_SpeakNumeral INHERIT = new CSSProperty_SpeakNumeral("INHERIT", "inherit");
        public static readonly CSSProperty_SpeakNumeral INITIAL = new CSSProperty_SpeakNumeral("INITIAL", "initial");
        public static readonly CSSProperty_SpeakNumeral UNSET = new CSSProperty_SpeakNumeral("UNSET", "unset");

        internal CSSProperty_SpeakNumeral(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_SpeakNumeral, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_SpeakPunctuation : SmartEnum<CSSProperty_SpeakPunctuation, string>, CSSProperty
    {
        public static readonly CSSProperty_SpeakPunctuation CODE = new CSSProperty_SpeakPunctuation("CODE", "code");
        public static readonly CSSProperty_SpeakPunctuation NONE = new CSSProperty_SpeakPunctuation("NONE", "none");
        public static readonly CSSProperty_SpeakPunctuation INHERIT = new CSSProperty_SpeakPunctuation("INHERIT", "inherit");
        public static readonly CSSProperty_SpeakPunctuation INITIAL = new CSSProperty_SpeakPunctuation("INITIAL", "initial");
        public static readonly CSSProperty_SpeakPunctuation UNSET = new CSSProperty_SpeakPunctuation("UNSET", "unset");

        internal CSSProperty_SpeakPunctuation(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_SpeakPunctuation, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Speak : SmartEnum<CSSProperty_Speak, string>, CSSProperty
    {
        public static readonly CSSProperty_Speak NORMAL = new CSSProperty_Speak("NORMAL", "normal");
        public static readonly CSSProperty_Speak NONE = new CSSProperty_Speak("NONE", "none");
        public static readonly CSSProperty_Speak SPELL_OUT = new CSSProperty_Speak("SPELL_OUT", "spell-out");
        public static readonly CSSProperty_Speak INHERIT = new CSSProperty_Speak("INHERIT", "inherit");
        public static readonly CSSProperty_Speak INITIAL = new CSSProperty_Speak("INITIAL", "initial");
        public static readonly CSSProperty_Speak UNSET = new CSSProperty_Speak("UNSET", "unset");

        internal CSSProperty_Speak(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Speak, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_SpeechRate : SmartEnum<CSSProperty_SpeechRate, string>, CSSProperty
    {
        public static readonly CSSProperty_SpeechRate number = new CSSProperty_SpeechRate("number", "");
        public static readonly CSSProperty_SpeechRate X_SLOW = new CSSProperty_SpeechRate("X_SLOW", "x-slow");
        public static readonly CSSProperty_SpeechRate SLOW = new CSSProperty_SpeechRate("SLOW", "slow");
        public static readonly CSSProperty_SpeechRate MEDIUM = new CSSProperty_SpeechRate("MEDIUM", "medium");
        public static readonly CSSProperty_SpeechRate FAST = new CSSProperty_SpeechRate("FAST", "fast");
        public static readonly CSSProperty_SpeechRate X_FAST = new CSSProperty_SpeechRate("X_FAST", "x-fast");
        public static readonly CSSProperty_SpeechRate FASTER = new CSSProperty_SpeechRate("FASTER", "faster");
        public static readonly CSSProperty_SpeechRate SLOWER = new CSSProperty_SpeechRate("SLOWER", "slower");
        public static readonly CSSProperty_SpeechRate INHERIT = new CSSProperty_SpeechRate("INHERIT", "inherit");
        public static readonly CSSProperty_SpeechRate INITIAL = new CSSProperty_SpeechRate("INITIAL", "initial");
        public static readonly CSSProperty_SpeechRate UNSET = new CSSProperty_SpeechRate("UNSET", "unset");

        internal CSSProperty_SpeechRate(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_SpeechRate, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Stress : SmartEnum<CSSProperty_Stress, string>, CSSProperty
    {
        public static readonly CSSProperty_Stress number = new CSSProperty_Stress("number", "");
        public static readonly CSSProperty_Stress INHERIT = new CSSProperty_Stress("INHERIT", "inherit");
        public static readonly CSSProperty_Stress INITIAL = new CSSProperty_Stress("INITIAL", "initial");
        public static readonly CSSProperty_Stress UNSET = new CSSProperty_Stress("UNSET", "unset");

        internal CSSProperty_Stress(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Stress, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Top : SmartEnum<CSSProperty_Top, string>, CSSProperty
    {
        public static readonly CSSProperty_Top length = new CSSProperty_Top("length", "");
        public static readonly CSSProperty_Top percentage = new CSSProperty_Top("percentage", "");
        public static readonly CSSProperty_Top AUTO = new CSSProperty_Top("AUTO", "auto");
        public static readonly CSSProperty_Top INHERIT = new CSSProperty_Top("INHERIT", "inherit");
        public static readonly CSSProperty_Top INITIAL = new CSSProperty_Top("INITIAL", "initial");
        public static readonly CSSProperty_Top UNSET = new CSSProperty_Top("UNSET", "unset");

        internal CSSProperty_Top(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Top, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Right : SmartEnum<CSSProperty_Right, string>, CSSProperty
    {
        public static readonly CSSProperty_Right length = new CSSProperty_Right("length", "");
        public static readonly CSSProperty_Right percentage = new CSSProperty_Right("percentage", "");
        public static readonly CSSProperty_Right AUTO = new CSSProperty_Right("AUTO", "auto");
        public static readonly CSSProperty_Right INHERIT = new CSSProperty_Right("INHERIT", "inherit");
        public static readonly CSSProperty_Right INITIAL = new CSSProperty_Right("INITIAL", "initial");
        public static readonly CSSProperty_Right UNSET = new CSSProperty_Right("UNSET", "unset");

        internal CSSProperty_Right(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Right, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Bottom : SmartEnum<CSSProperty_Bottom, string>, CSSProperty
    {
        public static readonly CSSProperty_Bottom length = new CSSProperty_Bottom("length", "");
        public static readonly CSSProperty_Bottom percentage = new CSSProperty_Bottom("percentage", "");
        public static readonly CSSProperty_Bottom AUTO = new CSSProperty_Bottom("AUTO", "auto");
        public static readonly CSSProperty_Bottom INHERIT = new CSSProperty_Bottom("INHERIT", "inherit");
        public static readonly CSSProperty_Bottom INITIAL = new CSSProperty_Bottom("INITIAL", "initial");
        public static readonly CSSProperty_Bottom UNSET = new CSSProperty_Bottom("UNSET", "unset");

        internal CSSProperty_Bottom(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Bottom, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Left : SmartEnum<CSSProperty_Left, string>, CSSProperty
    {
        public static readonly CSSProperty_Left length = new CSSProperty_Left("length", "");
        public static readonly CSSProperty_Left percentage = new CSSProperty_Left("percentage", "");
        public static readonly CSSProperty_Left AUTO = new CSSProperty_Left("AUTO", "auto");
        public static readonly CSSProperty_Left INHERIT = new CSSProperty_Left("INHERIT", "inherit");
        public static readonly CSSProperty_Left INITIAL = new CSSProperty_Left("INITIAL", "initial");
        public static readonly CSSProperty_Left UNSET = new CSSProperty_Left("UNSET", "unset");

        internal CSSProperty_Left(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Left, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Quotes : SmartEnum<CSSProperty_Quotes, string>, CSSProperty
    {
        public static readonly CSSProperty_Quotes list_values = new CSSProperty_Quotes("list_values", "");
        public static readonly CSSProperty_Quotes NONE = new CSSProperty_Quotes("NONE", "none");
        public static readonly CSSProperty_Quotes INHERIT = new CSSProperty_Quotes("INHERIT", "inherit");
        public static readonly CSSProperty_Quotes INITIAL = new CSSProperty_Quotes("INITIAL", "initial");
        public static readonly CSSProperty_Quotes UNSET = new CSSProperty_Quotes("UNSET", "unset");

        internal CSSProperty_Quotes(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Quotes, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_TableLayout : SmartEnum<CSSProperty_TableLayout, string>, CSSProperty
    {
        public static readonly CSSProperty_TableLayout AUTO = new CSSProperty_TableLayout("AUTO", "auto");
        public static readonly CSSProperty_TableLayout FIXED = new CSSProperty_TableLayout("FIXED", "fixed");
        public static readonly CSSProperty_TableLayout INHERIT = new CSSProperty_TableLayout("INHERIT", "inherit");
        public static readonly CSSProperty_TableLayout INITIAL = new CSSProperty_TableLayout("INITIAL", "initial");
        public static readonly CSSProperty_TableLayout UNSET = new CSSProperty_TableLayout("UNSET", "unset");

        internal CSSProperty_TableLayout(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_TableLayout, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_TextAlign : SmartEnum<CSSProperty_TextAlign, string>, CSSProperty
    {
        public static readonly CSSProperty_TextAlign BY_DIRECTION = new CSSProperty_TextAlign("BY_DIRECTION", "by-direction");
        public static readonly CSSProperty_TextAlign LEFT = new CSSProperty_TextAlign("LEFT", "left");
        public static readonly CSSProperty_TextAlign RIGHT = new CSSProperty_TextAlign("RIGHT", "right");
        public static readonly CSSProperty_TextAlign CENTER = new CSSProperty_TextAlign("CENTER", "center");
        public static readonly CSSProperty_TextAlign JUSTIFY = new CSSProperty_TextAlign("JUSTIFY", "justify");
        public static readonly CSSProperty_TextAlign INHERIT = new CSSProperty_TextAlign("INHERIT", "inherit");
        public static readonly CSSProperty_TextAlign INITIAL = new CSSProperty_TextAlign("INITIAL", "initial");
        public static readonly CSSProperty_TextAlign UNSET = new CSSProperty_TextAlign("UNSET", "unset");

        internal CSSProperty_TextAlign(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_TextAlign, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_TextDecoration : SmartEnum<CSSProperty_TextDecoration, string>, CSSProperty
    {
        public static readonly CSSProperty_TextDecoration list_values = new CSSProperty_TextDecoration("list_values", "");
        public static readonly CSSProperty_TextDecoration UNDERLINE = new CSSProperty_TextDecoration("UNDERLINE", "underline");
        public static readonly CSSProperty_TextDecoration OVERLINE = new CSSProperty_TextDecoration("OVERLINE", "overline");
        public static readonly CSSProperty_TextDecoration BLINK = new CSSProperty_TextDecoration("BLINK", "blink");
        public static readonly CSSProperty_TextDecoration LINE_THROUGH = new CSSProperty_TextDecoration("LINE_THROUGH", "line-through");
        public static readonly CSSProperty_TextDecoration NONE = new CSSProperty_TextDecoration("NONE", "none");
        public static readonly CSSProperty_TextDecoration INHERIT = new CSSProperty_TextDecoration("INHERIT", "inherit");
        public static readonly CSSProperty_TextDecoration INITIAL = new CSSProperty_TextDecoration("INITIAL", "initial");
        public static readonly CSSProperty_TextDecoration UNSET = new CSSProperty_TextDecoration("UNSET", "unset");

        internal CSSProperty_TextDecoration(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_TextDecoration, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_TextIndent : SmartEnum<CSSProperty_TextIndent, string>, CSSProperty
    {
        public static readonly CSSProperty_TextIndent length = new CSSProperty_TextIndent("length", "");
        public static readonly CSSProperty_TextIndent percentage = new CSSProperty_TextIndent("percentage", "");
        public static readonly CSSProperty_TextIndent INHERIT = new CSSProperty_TextIndent("INHERIT", "inherit");
        public static readonly CSSProperty_TextIndent INITIAL = new CSSProperty_TextIndent("INITIAL", "initial");
        public static readonly CSSProperty_TextIndent UNSET = new CSSProperty_TextIndent("UNSET", "unset");

        internal CSSProperty_TextIndent(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_TextIndent, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_TextTransform : SmartEnum<CSSProperty_TextTransform, string>, CSSProperty
    {
        public static readonly CSSProperty_TextTransform CAPITALIZE = new CSSProperty_TextTransform("CAPITALIZE", "capitalize");
        public static readonly CSSProperty_TextTransform UPPERCASE = new CSSProperty_TextTransform("UPPERCASE", "uppercase");
        public static readonly CSSProperty_TextTransform LOWERCASE = new CSSProperty_TextTransform("LOWERCASE", "lowercase");
        public static readonly CSSProperty_TextTransform NONE = new CSSProperty_TextTransform("NONE", "none");
        public static readonly CSSProperty_TextTransform INHERIT = new CSSProperty_TextTransform("INHERIT", "inherit");
        public static readonly CSSProperty_TextTransform INITIAL = new CSSProperty_TextTransform("INITIAL", "initial");
        public static readonly CSSProperty_TextTransform UNSET = new CSSProperty_TextTransform("UNSET", "unset");

        internal CSSProperty_TextTransform(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_TextTransform, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Transform : SmartEnum<CSSProperty_Transform, string>, CSSProperty
    {
        public static readonly CSSProperty_Transform list_values = new CSSProperty_Transform("list_values", "");
        public static readonly CSSProperty_Transform NONE = new CSSProperty_Transform("NONE", "none");
        public static readonly CSSProperty_Transform INHERIT = new CSSProperty_Transform("INHERIT", "inherit");
        public static readonly CSSProperty_Transform INITIAL = new CSSProperty_Transform("INITIAL", "initial");
        public static readonly CSSProperty_Transform UNSET = new CSSProperty_Transform("UNSET", "unset");

        internal CSSProperty_Transform(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Transform, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_TransformOrigin : SmartEnum<CSSProperty_TransformOrigin, string>, CSSProperty
    {
        public static readonly CSSProperty_TransformOrigin list_values = new CSSProperty_TransformOrigin("list_values", "");
        public static readonly CSSProperty_TransformOrigin INHERIT = new CSSProperty_TransformOrigin("INHERIT", "inherit");
        public static readonly CSSProperty_TransformOrigin INITIAL = new CSSProperty_TransformOrigin("INITIAL", "initial");
        public static readonly CSSProperty_TransformOrigin UNSET = new CSSProperty_TransformOrigin("UNSET", "unset");

        internal CSSProperty_TransformOrigin(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_TransformOrigin, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_UnicodeBidi : SmartEnum<CSSProperty_UnicodeBidi, string>, CSSProperty
    {
        public static readonly CSSProperty_UnicodeBidi NORMAL = new CSSProperty_UnicodeBidi("NORMAL", "normal");
        public static readonly CSSProperty_UnicodeBidi EMDEB = new CSSProperty_UnicodeBidi("EMDEB", "embed");
        public static readonly CSSProperty_UnicodeBidi BIDI_OVERRIDE = new CSSProperty_UnicodeBidi("BIDI_OVERRIDE", "bidi-override");
        public static readonly CSSProperty_UnicodeBidi INHERIT = new CSSProperty_UnicodeBidi("INHERIT", "inherit");
        public static readonly CSSProperty_UnicodeBidi INITIAL = new CSSProperty_UnicodeBidi("INITIAL", "initial");
        public static readonly CSSProperty_UnicodeBidi UNSET = new CSSProperty_UnicodeBidi("UNSET", "unset");

        internal CSSProperty_UnicodeBidi(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_UnicodeBidi, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_UnicodeRange : SmartEnum<CSSProperty_UnicodeRange, string>, CSSProperty
    {
        public static readonly CSSProperty_UnicodeRange list_values = new CSSProperty_UnicodeRange("list_values", "");

        private CSSProperty_UnicodeRange(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return false;
        }
        public bool equalsInitial()
        {
            return false;
        }
        public bool equalsUnset()
        {
            return false;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return "";
        }
        public override bool Equals(SmartEnum<CSSProperty_UnicodeRange, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_VerticalAlign : SmartEnum<CSSProperty_VerticalAlign, string>, CSSProperty
    {
        public static readonly CSSProperty_VerticalAlign length = new CSSProperty_VerticalAlign("length", "");
        public static readonly CSSProperty_VerticalAlign percentage = new CSSProperty_VerticalAlign("percentage", "");
        public static readonly CSSProperty_VerticalAlign BASELINE = new CSSProperty_VerticalAlign("BASELINE", "baseline");
        public static readonly CSSProperty_VerticalAlign SUB = new CSSProperty_VerticalAlign("SUB", "sub");
        public static readonly CSSProperty_VerticalAlign SUPER = new CSSProperty_VerticalAlign("SUPER", "super");
        public static readonly CSSProperty_VerticalAlign TOP = new CSSProperty_VerticalAlign("TOP", "top");
        public static readonly CSSProperty_VerticalAlign TEXT_TOP = new CSSProperty_VerticalAlign("TEXT_TOP", "text-top");
        public static readonly CSSProperty_VerticalAlign MIDDLE = new CSSProperty_VerticalAlign("MIDDLE", "middle");
        public static readonly CSSProperty_VerticalAlign BOTTOM = new CSSProperty_VerticalAlign("BOTTOM", "bottom");
        public static readonly CSSProperty_VerticalAlign TEXT_BOTTOM = new CSSProperty_VerticalAlign("TEXT_BOTTOM", "text-bottom");
        public static readonly CSSProperty_VerticalAlign INHERIT = new CSSProperty_VerticalAlign("INHERIT", "inherit");
        public static readonly CSSProperty_VerticalAlign INITIAL = new CSSProperty_VerticalAlign("INITIAL", "initial");
        public static readonly CSSProperty_VerticalAlign UNSET = new CSSProperty_VerticalAlign("UNSET", "unset");

        internal CSSProperty_VerticalAlign(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_VerticalAlign, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Visibility : SmartEnum<CSSProperty_Visibility, string>, CSSProperty
    {
        public static readonly CSSProperty_Visibility VISIBLE = new CSSProperty_Visibility("VISIBLE", "visible");
        public static readonly CSSProperty_Visibility HIDDEN = new CSSProperty_Visibility("HIDDEN", "hidden");
        public static readonly CSSProperty_Visibility COLLAPSE = new CSSProperty_Visibility("COLLAPSE", "collapse");
        public static readonly CSSProperty_Visibility INHERIT = new CSSProperty_Visibility("INHERIT", "inherit");
        public static readonly CSSProperty_Visibility INITIAL = new CSSProperty_Visibility("INITIAL", "initial");
        public static readonly CSSProperty_Visibility UNSET = new CSSProperty_Visibility("UNSET", "unset");

        internal CSSProperty_Visibility(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Visibility, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_VoiceFamily : SmartEnum<CSSProperty_VoiceFamily, string>, CSSProperty
    {
        public static readonly CSSProperty_VoiceFamily list_values = new CSSProperty_VoiceFamily("list_values", "");
        public static readonly CSSProperty_VoiceFamily MALE = new CSSProperty_VoiceFamily("MALE", "male");
        public static readonly CSSProperty_VoiceFamily FEMALE = new CSSProperty_VoiceFamily("FEMALE", "female");
        public static readonly CSSProperty_VoiceFamily CHILD = new CSSProperty_VoiceFamily("CHILD", "child");
        public static readonly CSSProperty_VoiceFamily INHERIT = new CSSProperty_VoiceFamily("INHERIT", "inherit");
        public static readonly CSSProperty_VoiceFamily INITIAL = new CSSProperty_VoiceFamily("INITIAL", "initial");
        public static readonly CSSProperty_VoiceFamily UNSET = new CSSProperty_VoiceFamily("UNSET", "unset");

        internal CSSProperty_VoiceFamily(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_VoiceFamily, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Volume : SmartEnum<CSSProperty_Volume, string>, CSSProperty
    {
        public static readonly CSSProperty_Volume number = new CSSProperty_Volume("number", "");
        public static readonly CSSProperty_Volume percentage = new CSSProperty_Volume("percentage", "");
        public static readonly CSSProperty_Volume SILENT = new CSSProperty_Volume("SILENT", "silent");
        public static readonly CSSProperty_Volume X_SOFT = new CSSProperty_Volume("X_SOFT", "x-soft");
        public static readonly CSSProperty_Volume SOFT = new CSSProperty_Volume("SOFT", "soft");
        public static readonly CSSProperty_Volume MEDIUM = new CSSProperty_Volume("MEDIUM", "medium");
        public static readonly CSSProperty_Volume LOUD = new CSSProperty_Volume("LOUD", "loud");
        public static readonly CSSProperty_Volume X_LOUD = new CSSProperty_Volume("X_LOUD", "x-loud");
        public static readonly CSSProperty_Volume INHERIT = new CSSProperty_Volume("INHERIT", "inherit");
        public static readonly CSSProperty_Volume INITIAL = new CSSProperty_Volume("INITIAL", "initial");
        public static readonly CSSProperty_Volume UNSET = new CSSProperty_Volume("UNSET", "unset");

        internal CSSProperty_Volume(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Volume, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_WhiteSpace : SmartEnum<CSSProperty_WhiteSpace, string>, CSSProperty
    {
        public static readonly CSSProperty_WhiteSpace NORMAL = new CSSProperty_WhiteSpace("NORMAL", "normal");
        public static readonly CSSProperty_WhiteSpace PRE = new CSSProperty_WhiteSpace("PRE", "pre");
        public static readonly CSSProperty_WhiteSpace NOWRAP = new CSSProperty_WhiteSpace("NOWRAP", "nowrap");
        public static readonly CSSProperty_WhiteSpace PRE_WRAP = new CSSProperty_WhiteSpace("PRE_WRAP", "pre-wrap");
        public static readonly CSSProperty_WhiteSpace PRE_LINE = new CSSProperty_WhiteSpace("PRE_LINE", "pre-line");
        public static readonly CSSProperty_WhiteSpace INHERIT = new CSSProperty_WhiteSpace("INHERIT", "inherit");
        public static readonly CSSProperty_WhiteSpace INITIAL = new CSSProperty_WhiteSpace("INITIAL", "initial");
        public static readonly CSSProperty_WhiteSpace UNSET = new CSSProperty_WhiteSpace("UNSET", "unset");

        internal CSSProperty_WhiteSpace(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_WhiteSpace, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Widows : SmartEnum<CSSProperty_Widows, string>, CSSProperty
    {
        public static readonly CSSProperty_Widows integer = new CSSProperty_Widows("integer", "");
        public static readonly CSSProperty_Widows INHERIT = new CSSProperty_Widows("INHERIT", "inherit");
        public static readonly CSSProperty_Widows INITIAL = new CSSProperty_Widows("INITIAL", "initial");
        public static readonly CSSProperty_Widows UNSET = new CSSProperty_Widows("UNSET", "unset");

        internal CSSProperty_Widows(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Widows, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_WordSpacing : SmartEnum<CSSProperty_WordSpacing, string>, CSSProperty
    {
        public static readonly CSSProperty_WordSpacing length = new CSSProperty_WordSpacing("length", "");
        public static readonly CSSProperty_WordSpacing NORMAL = new CSSProperty_WordSpacing("NORMAL", "normal");
        public static readonly CSSProperty_WordSpacing INHERIT = new CSSProperty_WordSpacing("INHERIT", "inherit");
        public static readonly CSSProperty_WordSpacing INITIAL = new CSSProperty_WordSpacing("INITIAL", "initial");
        public static readonly CSSProperty_WordSpacing UNSET = new CSSProperty_WordSpacing("UNSET", "unset");

        internal CSSProperty_WordSpacing(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_WordSpacing, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_LetterSpacing : SmartEnum<CSSProperty_LetterSpacing, string>, CSSProperty
    {
        public static readonly CSSProperty_LetterSpacing length = new CSSProperty_LetterSpacing("length", "");
        public static readonly CSSProperty_LetterSpacing NORMAL = new CSSProperty_LetterSpacing("NORMAL", "normal");
        public static readonly CSSProperty_LetterSpacing INHERIT = new CSSProperty_LetterSpacing("INHERIT", "inherit");
        public static readonly CSSProperty_LetterSpacing INITIAL = new CSSProperty_LetterSpacing("INITIAL", "initial");
        public static readonly CSSProperty_LetterSpacing UNSET = new CSSProperty_LetterSpacing("UNSET", "unset");

        internal CSSProperty_LetterSpacing(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return true;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_LetterSpacing, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_ZIndex : SmartEnum<CSSProperty_ZIndex, string>, CSSProperty
    {
        public static readonly CSSProperty_ZIndex integer = new CSSProperty_ZIndex("integer", "");
        public static readonly CSSProperty_ZIndex AUTO = new CSSProperty_ZIndex("AUTO", "auto");
        public static readonly CSSProperty_ZIndex INHERIT = new CSSProperty_ZIndex("INHERIT", "inherit");
        public static readonly CSSProperty_ZIndex INITIAL = new CSSProperty_ZIndex("INITIAL", "initial");
        public static readonly CSSProperty_ZIndex UNSET = new CSSProperty_ZIndex("UNSET", "unset");

        internal CSSProperty_ZIndex(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_ZIndex, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_AlignContent : SmartEnum<CSSProperty_AlignContent, string>, CSSProperty
    {
        public static readonly CSSProperty_AlignContent FLEX_START = new CSSProperty_AlignContent("FLEX_START", "flex-start");
        public static readonly CSSProperty_AlignContent FLEX_END = new CSSProperty_AlignContent("FLEX_END", "flex-end");
        public static readonly CSSProperty_AlignContent CENTER = new CSSProperty_AlignContent("CENTER", "center");
        public static readonly CSSProperty_AlignContent SPACE_BETWEEN = new CSSProperty_AlignContent("SPACE_BETWEEN", "space-between");
        public static readonly CSSProperty_AlignContent SPACE_AROUND = new CSSProperty_AlignContent("SPACE_AROUND", "space-around");
        public static readonly CSSProperty_AlignContent STRETCH = new CSSProperty_AlignContent("STRETCH", "stretch");
        public static readonly CSSProperty_AlignContent INHERIT = new CSSProperty_AlignContent("INHERIT", "inherit");
        public static readonly CSSProperty_AlignContent INITIAL = new CSSProperty_AlignContent("INITIAL", "initial");
        public static readonly CSSProperty_AlignContent UNSET = new CSSProperty_AlignContent("UNSET", "unset");

        internal CSSProperty_AlignContent(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_AlignContent, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_AlignItems : SmartEnum<CSSProperty_AlignItems, string>, CSSProperty
    {
        public static readonly CSSProperty_AlignItems FLEX_START = new CSSProperty_AlignItems("FLEX_START", "flex-start");
        public static readonly CSSProperty_AlignItems FLEX_END = new CSSProperty_AlignItems("FLEX_END", "flex-end");
        public static readonly CSSProperty_AlignItems CENTER = new CSSProperty_AlignItems("CENTER", "center");
        public static readonly CSSProperty_AlignItems BASELINE = new CSSProperty_AlignItems("BASELINE", "baseline");
        public static readonly CSSProperty_AlignItems STRETCH = new CSSProperty_AlignItems("STRETCH", "stretch");
        public static readonly CSSProperty_AlignItems INHERIT = new CSSProperty_AlignItems("INHERIT", "inherit");
        public static readonly CSSProperty_AlignItems INITIAL = new CSSProperty_AlignItems("INITIAL", "initial");
        public static readonly CSSProperty_AlignItems UNSET = new CSSProperty_AlignItems("UNSET", "unset");

        internal CSSProperty_AlignItems(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_AlignItems, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_AlignSelf : SmartEnum<CSSProperty_AlignSelf, string>, CSSProperty
    {
        public static readonly CSSProperty_AlignSelf AUTO = new CSSProperty_AlignSelf("AUTO", "auto");
        public static readonly CSSProperty_AlignSelf FLEX_START = new CSSProperty_AlignSelf("FLEX_START", "flex-start");
        public static readonly CSSProperty_AlignSelf FLEX_END = new CSSProperty_AlignSelf("FLEX_END", "flex-end");
        public static readonly CSSProperty_AlignSelf CENTER = new CSSProperty_AlignSelf("CENTER", "center");
        public static readonly CSSProperty_AlignSelf BASELINE = new CSSProperty_AlignSelf("BASELINE", "baseline");
        public static readonly CSSProperty_AlignSelf STRETCH = new CSSProperty_AlignSelf("STRETCH", "stretch");
        public static readonly CSSProperty_AlignSelf INHERIT = new CSSProperty_AlignSelf("INHERIT", "inherit");
        public static readonly CSSProperty_AlignSelf INITIAL = new CSSProperty_AlignSelf("INITIAL", "initial");
        public static readonly CSSProperty_AlignSelf UNSET = new CSSProperty_AlignSelf("UNSET", "unset");

        internal CSSProperty_AlignSelf(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return this == INHERIT;
        }
        public bool equalsInherit()
        {
            return false;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_AlignSelf, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Flex : SmartEnum<CSSProperty_Flex, string>, CSSProperty
    {
        public static readonly CSSProperty_Flex component_values = new CSSProperty_Flex("component_values", "");
        public static readonly CSSProperty_Flex INHERIT = new CSSProperty_Flex("INHERIT", "inherit");
        public static readonly CSSProperty_Flex INITIAL = new CSSProperty_Flex("INITIAL", "initial");
        public static readonly CSSProperty_Flex UNSET = new CSSProperty_Flex("UNSET", "unset");

        internal CSSProperty_Flex(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Flex, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_FlexFlow : SmartEnum<CSSProperty_FlexFlow, string>, CSSProperty
    {
        public static readonly CSSProperty_FlexFlow component_values = new CSSProperty_FlexFlow("component_values", "");
        public static readonly CSSProperty_FlexFlow INHERIT = new CSSProperty_FlexFlow("INHERIT", "inherit");
        public static readonly CSSProperty_FlexFlow INITIAL = new CSSProperty_FlexFlow("INITIAL", "initial");
        public static readonly CSSProperty_FlexFlow UNSET = new CSSProperty_FlexFlow("UNSET", "unset");

        internal CSSProperty_FlexFlow(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_FlexFlow, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_FlexBasis : SmartEnum<CSSProperty_FlexBasis, string>, CSSProperty
    {
        public static readonly CSSProperty_FlexBasis CONTENT = new CSSProperty_FlexBasis("CONTENT", "content");
        public static readonly CSSProperty_FlexBasis length = new CSSProperty_FlexBasis("length", "");
        public static readonly CSSProperty_FlexBasis percentage = new CSSProperty_FlexBasis("percentage", "");
        public static readonly CSSProperty_FlexBasis AUTO = new CSSProperty_FlexBasis("AUTO", "auto");
        public static readonly CSSProperty_FlexBasis INHERIT = new CSSProperty_FlexBasis("INHERIT", "inherit");
        public static readonly CSSProperty_FlexBasis INITIAL = new CSSProperty_FlexBasis("INITIAL", "initial");
        public static readonly CSSProperty_FlexBasis UNSET = new CSSProperty_FlexBasis("UNSET", "unset");

        internal CSSProperty_FlexBasis(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_FlexBasis, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_FlexDirection : SmartEnum<CSSProperty_FlexDirection, string>, CSSProperty
    {
        public static readonly CSSProperty_FlexDirection ROW = new CSSProperty_FlexDirection("ROW", "row");
        public static readonly CSSProperty_FlexDirection ROW_REVERSE = new CSSProperty_FlexDirection("ROW_REVERSE", "row-reverse");
        public static readonly CSSProperty_FlexDirection COLUMN = new CSSProperty_FlexDirection("COLUMN", "column");
        public static readonly CSSProperty_FlexDirection COLUMN_REVERSE = new CSSProperty_FlexDirection("COLUMN_REVERSE", "column-reverse");
        public static readonly CSSProperty_FlexDirection INHERIT = new CSSProperty_FlexDirection("INHERIT", "inherit");
        public static readonly CSSProperty_FlexDirection INITIAL = new CSSProperty_FlexDirection("INITIAL", "initial");
        public static readonly CSSProperty_FlexDirection UNSET = new CSSProperty_FlexDirection("UNSET", "unset");

        internal CSSProperty_FlexDirection(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_FlexDirection, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_FlexGrow : SmartEnum<CSSProperty_FlexGrow, string>, CSSProperty
    {
        public static readonly CSSProperty_FlexGrow number = new CSSProperty_FlexGrow("number", "");
        public static readonly CSSProperty_FlexGrow INHERIT = new CSSProperty_FlexGrow("INHERIT", "inherit");
        public static readonly CSSProperty_FlexGrow INITIAL = new CSSProperty_FlexGrow("INITIAL", "initial");
        public static readonly CSSProperty_FlexGrow UNSET = new CSSProperty_FlexGrow("UNSET", "unset");

        internal CSSProperty_FlexGrow(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_FlexGrow, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_FlexShrink : SmartEnum<CSSProperty_FlexShrink, string>, CSSProperty
    {
        public static readonly CSSProperty_FlexShrink number = new CSSProperty_FlexShrink("number", "");
        public static readonly CSSProperty_FlexShrink INHERIT = new CSSProperty_FlexShrink("INHERIT", "inherit");
        public static readonly CSSProperty_FlexShrink INITIAL = new CSSProperty_FlexShrink("INITIAL", "initial");
        public static readonly CSSProperty_FlexShrink UNSET = new CSSProperty_FlexShrink("UNSET", "unset");

        internal CSSProperty_FlexShrink(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_FlexShrink, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_FlexWrap : SmartEnum<CSSProperty_FlexWrap, string>, CSSProperty
    {
        public static readonly CSSProperty_FlexWrap NOWRAP = new CSSProperty_FlexWrap("NOWRAP", "nowrap");
        public static readonly CSSProperty_FlexWrap WRAP = new CSSProperty_FlexWrap("WRAP", "wrap");
        public static readonly CSSProperty_FlexWrap WRAP_REVERSE = new CSSProperty_FlexWrap("WRAP_REVERSE", "wrap-reverse");
        public static readonly CSSProperty_FlexWrap INHERIT = new CSSProperty_FlexWrap("INHERIT", "inherit");
        public static readonly CSSProperty_FlexWrap INITIAL = new CSSProperty_FlexWrap("INITIAL", "initial");
        public static readonly CSSProperty_FlexWrap UNSET = new CSSProperty_FlexWrap("UNSET", "unset");

        internal CSSProperty_FlexWrap(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_FlexWrap, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_JustifyContent : SmartEnum<CSSProperty_JustifyContent, string>, CSSProperty
    {
        public static readonly CSSProperty_JustifyContent FLEX_START = new CSSProperty_JustifyContent("FLEX_START", "flex-start");
        public static readonly CSSProperty_JustifyContent FLEX_END = new CSSProperty_JustifyContent("FLEX_END", "flex-end");
        public static readonly CSSProperty_JustifyContent CENTER = new CSSProperty_JustifyContent("CENTER", "center");
        public static readonly CSSProperty_JustifyContent SPACE_BETWEEN = new CSSProperty_JustifyContent("SPACE_BETWEEN", "space-between");
        public static readonly CSSProperty_JustifyContent SPACE_AROUND = new CSSProperty_JustifyContent("SPACE_AROUND", "space-around");
        public static readonly CSSProperty_JustifyContent INHERIT = new CSSProperty_JustifyContent("INHERIT", "inherit");
        public static readonly CSSProperty_JustifyContent INITIAL = new CSSProperty_JustifyContent("INITIAL", "initial");
        public static readonly CSSProperty_JustifyContent UNSET = new CSSProperty_JustifyContent("UNSET", "unset");

        internal CSSProperty_JustifyContent(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_JustifyContent, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Order : SmartEnum<CSSProperty_Order, string>, CSSProperty
    {
        public static readonly CSSProperty_Order integer = new CSSProperty_Order("integer", "");
        public static readonly CSSProperty_Order INHERIT = new CSSProperty_Order("INHERIT", "inherit");
        public static readonly CSSProperty_Order INITIAL = new CSSProperty_Order("INITIAL", "initial");
        public static readonly CSSProperty_Order UNSET = new CSSProperty_Order("UNSET", "unset");

        internal CSSProperty_Order(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Order, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Filter : SmartEnum<CSSProperty_Filter, string>, CSSProperty
    {
        public static readonly CSSProperty_Filter list_values = new CSSProperty_Filter("list_values", "");
        public static readonly CSSProperty_Filter NONE = new CSSProperty_Filter("NONE", "none");
        public static readonly CSSProperty_Filter INHERIT = new CSSProperty_Filter("INHERIT", "inherit");
        public static readonly CSSProperty_Filter INITIAL = new CSSProperty_Filter("INITIAL", "initial");
        public static readonly CSSProperty_Filter UNSET = new CSSProperty_Filter("UNSET", "unset");

        internal CSSProperty_Filter(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Filter, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_BackdropFilter : SmartEnum<CSSProperty_BackdropFilter, string>, CSSProperty
    {
        public static readonly CSSProperty_BackdropFilter list_values = new CSSProperty_BackdropFilter("list_values", "");
        public static readonly CSSProperty_BackdropFilter NONE = new CSSProperty_BackdropFilter("NONE", "none");
        public static readonly CSSProperty_BackdropFilter INHERIT = new CSSProperty_BackdropFilter("INHERIT", "inherit");
        public static readonly CSSProperty_BackdropFilter INITIAL = new CSSProperty_BackdropFilter("INITIAL", "initial");
        public static readonly CSSProperty_BackdropFilter UNSET = new CSSProperty_BackdropFilter("UNSET", "unset");
        internal CSSProperty_BackdropFilter(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_BackdropFilter, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Grid : SmartEnum<CSSProperty_Grid, string>, CSSProperty
    {
        public static readonly CSSProperty_Grid component_values = new CSSProperty_Grid("component_values", "");
        public static readonly CSSProperty_Grid AUTO_FLOW = new CSSProperty_Grid("AUTO_FLOW", "auto-flow");
        public static readonly CSSProperty_Grid NONE = new CSSProperty_Grid("NONE", "none");
        public static readonly CSSProperty_Grid INHERIT = new CSSProperty_Grid("INHERIT", "inherit");
        public static readonly CSSProperty_Grid INITIAL = new CSSProperty_Grid("INITIAL", "initial");
        public static readonly CSSProperty_Grid UNSET = new CSSProperty_Grid("UNSET", "unset");

        internal CSSProperty_Grid(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Grid, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public sealed class CSSProperty_GridStartEnd : SmartEnum<CSSProperty_GridStartEnd, string>, CSSProperty
    {
        public static readonly CSSProperty_GridStartEnd component_values = new CSSProperty_GridStartEnd("component_values", "");
        public static readonly CSSProperty_GridStartEnd number = new CSSProperty_GridStartEnd("number", "");
        public static readonly CSSProperty_GridStartEnd identificator = new CSSProperty_GridStartEnd("identificator", "");
        public static readonly CSSProperty_GridStartEnd AUTO = new CSSProperty_GridStartEnd("AUTO", "auto");
        public static readonly CSSProperty_GridStartEnd SPAN = new CSSProperty_GridStartEnd("SPAN", "span");
        public static readonly CSSProperty_GridStartEnd INHERIT = new CSSProperty_GridStartEnd("INHERIT", "inherit");
        public static readonly CSSProperty_GridStartEnd INITIAL = new CSSProperty_GridStartEnd("INITIAL", "initial");
        public static readonly CSSProperty_GridStartEnd UNSET = new CSSProperty_GridStartEnd("UNSET", "unset");

        internal CSSProperty_GridStartEnd(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_GridStartEnd, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_GridGap : SmartEnum<CSSProperty_GridGap, string>, CSSProperty
    {
        public static readonly CSSProperty_GridGap component_values = new CSSProperty_GridGap("component_values", "");
        public static readonly CSSProperty_GridGap length = new CSSProperty_GridGap("length", "");
        public static readonly CSSProperty_GridGap NORMAL = new CSSProperty_GridGap("NORMAL", "normal");
        public static readonly CSSProperty_GridGap INHERIT = new CSSProperty_GridGap("INHERIT", "inherit");
        public static readonly CSSProperty_GridGap INITIAL = new CSSProperty_GridGap("INITIAL", "initial");
        public static readonly CSSProperty_GridGap UNSET = new CSSProperty_GridGap("UNSET", "unset");

        internal CSSProperty_GridGap(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_GridGap, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_GridTemplateAreas : SmartEnum<CSSProperty_GridTemplateAreas, string>, CSSProperty
    {
        public static readonly CSSProperty_GridTemplateAreas list_values = new CSSProperty_GridTemplateAreas("list_values", "");
        public static readonly CSSProperty_GridTemplateAreas NONE = new CSSProperty_GridTemplateAreas("NONE", "none");
        public static readonly CSSProperty_GridTemplateAreas INHERIT = new CSSProperty_GridTemplateAreas("INHERIT", "inherit");
        public static readonly CSSProperty_GridTemplateAreas INITIAL = new CSSProperty_GridTemplateAreas("INITIAL", "initial");
        public static readonly CSSProperty_GridTemplateAreas UNSET = new CSSProperty_GridTemplateAreas("UNSET", "unset");

        internal CSSProperty_GridTemplateAreas(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_GridTemplateAreas, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_GridTemplateRowsColumns : SmartEnum<CSSProperty_GridTemplateRowsColumns, string>, CSSProperty
    {
        public static readonly CSSProperty_GridTemplateRowsColumns list_values = new CSSProperty_GridTemplateRowsColumns("list_values", "");
        public static readonly CSSProperty_GridTemplateRowsColumns AUTO = new CSSProperty_GridTemplateRowsColumns("AUTO", "auto");
        public static readonly CSSProperty_GridTemplateRowsColumns MAX_CONTENT = new CSSProperty_GridTemplateRowsColumns("MAX_CONTENT", "max-content");
        public static readonly CSSProperty_GridTemplateRowsColumns MIN_CONTENT = new CSSProperty_GridTemplateRowsColumns("MIN_CONTENT", "min-content");
        public static readonly CSSProperty_GridTemplateRowsColumns NONE = new CSSProperty_GridTemplateRowsColumns("NONE", "none");
        public static readonly CSSProperty_GridTemplateRowsColumns INHERIT = new CSSProperty_GridTemplateRowsColumns("INHERIT", "inherit");
        public static readonly CSSProperty_GridTemplateRowsColumns INITIAL = new CSSProperty_GridTemplateRowsColumns("INITIAL", "initial");
        public static readonly CSSProperty_GridTemplateRowsColumns UNSET = new CSSProperty_GridTemplateRowsColumns("UNSET", "unset");

        internal CSSProperty_GridTemplateRowsColumns(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_GridTemplateRowsColumns, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_GridAutoFlow : SmartEnum<CSSProperty_GridAutoFlow, string>, CSSProperty
    {
        public static readonly CSSProperty_GridAutoFlow component_values = new CSSProperty_GridAutoFlow("component_values", "");
        public static readonly CSSProperty_GridAutoFlow ROW = new CSSProperty_GridAutoFlow("ROW", "row");
        public static readonly CSSProperty_GridAutoFlow COLUMN = new CSSProperty_GridAutoFlow("COLUMN", "column");
        public static readonly CSSProperty_GridAutoFlow DENSE = new CSSProperty_GridAutoFlow("DENSE", "dense");
        public static readonly CSSProperty_GridAutoFlow INHERIT = new CSSProperty_GridAutoFlow("INHERIT", "inherit");
        public static readonly CSSProperty_GridAutoFlow INITIAL = new CSSProperty_GridAutoFlow("INITIAL", "initial");
        public static readonly CSSProperty_GridAutoFlow UNSET = new CSSProperty_GridAutoFlow("UNSET", "unset");

        internal CSSProperty_GridAutoFlow(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_GridAutoFlow, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_GridAutoRowsColumns : SmartEnum<CSSProperty_GridAutoRowsColumns, string>, CSSProperty
    {
        public static readonly CSSProperty_GridAutoRowsColumns length = new CSSProperty_GridAutoRowsColumns("length", "");
        public static readonly CSSProperty_GridAutoRowsColumns list_values = new CSSProperty_GridAutoRowsColumns("list_values", "");
        public static readonly CSSProperty_GridAutoRowsColumns AUTO = new CSSProperty_GridAutoRowsColumns("AUTO", "auto");
        public static readonly CSSProperty_GridAutoRowsColumns MIN_CONTENT = new CSSProperty_GridAutoRowsColumns("MIN_CONTENT", "min-content");
        public static readonly CSSProperty_GridAutoRowsColumns MAX_CONTENT = new CSSProperty_GridAutoRowsColumns("MAX_CONTENT", "max-content");
        public static readonly CSSProperty_GridAutoRowsColumns INHERIT = new CSSProperty_GridAutoRowsColumns("INHERIT", "inherit");
        public static readonly CSSProperty_GridAutoRowsColumns INITIAL = new CSSProperty_GridAutoRowsColumns("INITIAL", "initial");
        public static readonly CSSProperty_GridAutoRowsColumns UNSET = new CSSProperty_GridAutoRowsColumns("UNSET", "unset");

        internal CSSProperty_GridAutoRowsColumns(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_GridAutoRowsColumns, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Animation : SmartEnum<CSSProperty_Animation, string>, CSSProperty
    {
        public static readonly CSSProperty_Animation component_values = new CSSProperty_Animation("component_values", "");
        public static readonly CSSProperty_Animation INHERIT = new CSSProperty_Animation("INHERIT", "inherit");
        public static readonly CSSProperty_Animation INITIAL = new CSSProperty_Animation("INITIAL", "initial");
        public static readonly CSSProperty_Animation UNSET = new CSSProperty_Animation("UNSET", "unset");

        internal CSSProperty_Animation(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Animation, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_AnimationDelay : SmartEnum<CSSProperty_AnimationDelay, string>, CSSProperty
    {
        public static readonly CSSProperty_AnimationDelay time = new CSSProperty_AnimationDelay("time", "");
        public static readonly CSSProperty_AnimationDelay list_values = new CSSProperty_AnimationDelay("list_values", "");
        public static readonly CSSProperty_AnimationDelay INHERIT = new CSSProperty_AnimationDelay("INHERIT", "inherit");
        public static readonly CSSProperty_AnimationDelay INITIAL = new CSSProperty_AnimationDelay("INITIAL", "initial");
        public static readonly CSSProperty_AnimationDelay UNSET = new CSSProperty_AnimationDelay("UNSET", "unset");
        internal CSSProperty_AnimationDelay(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_AnimationDelay, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_AnimationDirection : SmartEnum<CSSProperty_AnimationDirection, string>, CSSProperty
    {
        public static readonly CSSProperty_AnimationDirection list_values = new CSSProperty_AnimationDirection("list_values", "");
        public static readonly CSSProperty_AnimationDirection NORMAL = new CSSProperty_AnimationDirection("NORMAL", "normal");
        public static readonly CSSProperty_AnimationDirection REVERSE = new CSSProperty_AnimationDirection("REVERSE", "reverse");
        public static readonly CSSProperty_AnimationDirection ALTERNATE = new CSSProperty_AnimationDirection("ALTERNATE", "alternate");
        public static readonly CSSProperty_AnimationDirection ALTERNATE_REVERSE = new CSSProperty_AnimationDirection("ALTERNATE_REVERSE", "alternate-reverse");
        public static readonly CSSProperty_AnimationDirection INHERIT = new CSSProperty_AnimationDirection("INHERIT", "inherit");
        public static readonly CSSProperty_AnimationDirection INITIAL = new CSSProperty_AnimationDirection("INITIAL", "initial");
        public static readonly CSSProperty_AnimationDirection UNSET = new CSSProperty_AnimationDirection("UNSET", "unset");

        internal CSSProperty_AnimationDirection(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_AnimationDirection, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_AnimationDuration : SmartEnum<CSSProperty_AnimationDuration, string>, CSSProperty
    {
        public static readonly CSSProperty_AnimationDuration time = new CSSProperty_AnimationDuration("time", "");
        public static readonly CSSProperty_AnimationDuration list_values = new CSSProperty_AnimationDuration("list_values", "");
        public static readonly CSSProperty_AnimationDuration INHERIT = new CSSProperty_AnimationDuration("INHERIT", "inherit");
        public static readonly CSSProperty_AnimationDuration INITIAL = new CSSProperty_AnimationDuration("INITIAL", "initial");
        public static readonly CSSProperty_AnimationDuration UNSET = new CSSProperty_AnimationDuration("UNSET", "unset");
        internal CSSProperty_AnimationDuration(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_AnimationDuration, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_AnimationFillMode : SmartEnum<CSSProperty_AnimationFillMode, string>, CSSProperty
    {
        public static readonly CSSProperty_AnimationFillMode list_values = new CSSProperty_AnimationFillMode("list_values", "");
        public static readonly CSSProperty_AnimationFillMode NONE = new CSSProperty_AnimationFillMode("NONE", "none");
        public static readonly CSSProperty_AnimationFillMode FORWARDS = new CSSProperty_AnimationFillMode("FORWARDS", "forwards");
        public static readonly CSSProperty_AnimationFillMode BACKWARDS = new CSSProperty_AnimationFillMode("BACKWARDS", "backwards");
        public static readonly CSSProperty_AnimationFillMode BOTH = new CSSProperty_AnimationFillMode("BOTH", "both");
        public static readonly CSSProperty_AnimationFillMode INHERIT = new CSSProperty_AnimationFillMode("INHERIT", "inherit");
        public static readonly CSSProperty_AnimationFillMode INITIAL = new CSSProperty_AnimationFillMode("INITIAL", "initial");
        public static readonly CSSProperty_AnimationFillMode UNSET = new CSSProperty_AnimationFillMode("UNSET", "unset");
        internal CSSProperty_AnimationFillMode(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_AnimationFillMode, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_AnimationIterationCount : SmartEnum<CSSProperty_AnimationIterationCount, string>, CSSProperty
    {
        public static readonly CSSProperty_AnimationIterationCount number = new CSSProperty_AnimationIterationCount("number", "");
        public static readonly CSSProperty_AnimationIterationCount list_values = new CSSProperty_AnimationIterationCount("list_values", "");
        public static readonly CSSProperty_AnimationIterationCount INFINITE = new CSSProperty_AnimationIterationCount("INFINITE", "infinite");
        public static readonly CSSProperty_AnimationIterationCount INHERIT = new CSSProperty_AnimationIterationCount("INHERIT", "inherit");
        public static readonly CSSProperty_AnimationIterationCount INITIAL = new CSSProperty_AnimationIterationCount("INITIAL", "initial");
        public static readonly CSSProperty_AnimationIterationCount UNSET = new CSSProperty_AnimationIterationCount("UNSET", "unset");

        internal CSSProperty_AnimationIterationCount(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_AnimationIterationCount, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_AnimationName : SmartEnum<CSSProperty_AnimationName, string>, CSSProperty
    {
        public static readonly CSSProperty_AnimationName custom_ident = new CSSProperty_AnimationName("custom_ident", "");
        public static readonly CSSProperty_AnimationName list_values = new CSSProperty_AnimationName("list_values", "");
        public static readonly CSSProperty_AnimationName NONE = new CSSProperty_AnimationName("NONE", "none");
        public static readonly CSSProperty_AnimationName INHERIT = new CSSProperty_AnimationName("INHERIT", "inherit");
        public static readonly CSSProperty_AnimationName INITIAL = new CSSProperty_AnimationName("INITIAL", "initial");
        public static readonly CSSProperty_AnimationName UNSET = new CSSProperty_AnimationName("UNSET", "unset");

        internal CSSProperty_AnimationName(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_AnimationName, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_AnimationPlayState : SmartEnum<CSSProperty_AnimationPlayState, string>, CSSProperty
    {
        public static readonly CSSProperty_AnimationPlayState list_values = new CSSProperty_AnimationPlayState("list_values", "");
        public static readonly CSSProperty_AnimationPlayState RUNNING = new CSSProperty_AnimationPlayState("RUNNING", "running");
        public static readonly CSSProperty_AnimationPlayState PAUSED = new CSSProperty_AnimationPlayState("PAUSED", "paused");
        public static readonly CSSProperty_AnimationPlayState INHERIT = new CSSProperty_AnimationPlayState("INHERIT", "inherit");
        public static readonly CSSProperty_AnimationPlayState INITIAL = new CSSProperty_AnimationPlayState("INITIAL", "initial");
        public static readonly CSSProperty_AnimationPlayState UNSET = new CSSProperty_AnimationPlayState("UNSET", "unset");

        internal CSSProperty_AnimationPlayState(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_AnimationPlayState, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_AnimationTimingFunction : SmartEnum<CSSProperty_AnimationTimingFunction, string>, CSSProperty
    {
        public static readonly CSSProperty_AnimationTimingFunction timing_function = new CSSProperty_AnimationTimingFunction("timing_function", "");
        public static readonly CSSProperty_AnimationTimingFunction list_values = new CSSProperty_AnimationTimingFunction("list_values", "");
        public static readonly CSSProperty_AnimationTimingFunction LINEAR = new CSSProperty_AnimationTimingFunction("LINEAR", "linear");
        public static readonly CSSProperty_AnimationTimingFunction EASE = new CSSProperty_AnimationTimingFunction("EASE", "ease");
        public static readonly CSSProperty_AnimationTimingFunction EASE_IN = new CSSProperty_AnimationTimingFunction("EASE_IN", "ease-in");
        public static readonly CSSProperty_AnimationTimingFunction EASE_OUT = new CSSProperty_AnimationTimingFunction("EASE_OUT", "ease-out");
        public static readonly CSSProperty_AnimationTimingFunction EASE_IN_OUT = new CSSProperty_AnimationTimingFunction("EASE_IN_OUT", "ease-in-out");
        public static readonly CSSProperty_AnimationTimingFunction STEP_START = new CSSProperty_AnimationTimingFunction("STEP_START", "step-start");
        public static readonly CSSProperty_AnimationTimingFunction STEP_END = new CSSProperty_AnimationTimingFunction("STEP_END", "step-end");
        public static readonly CSSProperty_AnimationTimingFunction INHERIT = new CSSProperty_AnimationTimingFunction("INHERIT", "inherit");
        public static readonly CSSProperty_AnimationTimingFunction INITIAL = new CSSProperty_AnimationTimingFunction("INITIAL", "initial");
        public static readonly CSSProperty_AnimationTimingFunction UNSET = new CSSProperty_AnimationTimingFunction("UNSET", "unset");

        internal CSSProperty_AnimationTimingFunction(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_AnimationTimingFunction, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_Transition : SmartEnum<CSSProperty_Transition, string>, CSSProperty
    {
        public static readonly CSSProperty_Transition component_values = new CSSProperty_Transition("component_values", "");
        public static readonly CSSProperty_Transition INHERIT = new CSSProperty_Transition("INHERIT", "inherit");
        public static readonly CSSProperty_Transition INITIAL = new CSSProperty_Transition("INITIAL", "initial");
        public static readonly CSSProperty_Transition UNSET = new CSSProperty_Transition("UNSET", "unset");
        internal CSSProperty_Transition(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_Transition, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_TransitionDelay : SmartEnum<CSSProperty_TransitionDelay, string>, CSSProperty
    {
        public static readonly CSSProperty_TransitionDelay time = new CSSProperty_TransitionDelay("time", "");
        public static readonly CSSProperty_TransitionDelay list_values = new CSSProperty_TransitionDelay("list_values", "");
        public static readonly CSSProperty_TransitionDelay INHERIT = new CSSProperty_TransitionDelay("INHERIT", "inherit");
        public static readonly CSSProperty_TransitionDelay INITIAL = new CSSProperty_TransitionDelay("INITIAL", "initial");
        public static readonly CSSProperty_TransitionDelay UNSET = new CSSProperty_TransitionDelay("UNSET", "unset");

        internal CSSProperty_TransitionDelay(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_TransitionDelay, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_TransitionDuration : SmartEnum<CSSProperty_TransitionDuration, string>, CSSProperty
    {
        public static readonly CSSProperty_TransitionDuration time = new CSSProperty_TransitionDuration("time", "");
        public static readonly CSSProperty_TransitionDuration list_values = new CSSProperty_TransitionDuration("list_values", "");
        public static readonly CSSProperty_TransitionDuration INHERIT = new CSSProperty_TransitionDuration("INHERIT", "inherit");
        public static readonly CSSProperty_TransitionDuration INITIAL = new CSSProperty_TransitionDuration("INITIAL", "initial");
        public static readonly CSSProperty_TransitionDuration UNSET = new CSSProperty_TransitionDuration("UNSET", "unset");
        internal CSSProperty_TransitionDuration(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_TransitionDuration, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_TransitionProperty : SmartEnum<CSSProperty_TransitionProperty, string>, CSSProperty
    {
        public static readonly CSSProperty_TransitionProperty custom_ident = new CSSProperty_TransitionProperty("custom_ident", "");
        public static readonly CSSProperty_TransitionProperty list_values = new CSSProperty_TransitionProperty("list_values", "");
        public static readonly CSSProperty_TransitionProperty ALL = new CSSProperty_TransitionProperty("ALL", "all");
        public static readonly CSSProperty_TransitionProperty NONE = new CSSProperty_TransitionProperty("NONE", "none");
        public static readonly CSSProperty_TransitionProperty INHERIT = new CSSProperty_TransitionProperty("INHERIT", "inherit");
        public static readonly CSSProperty_TransitionProperty INITIAL = new CSSProperty_TransitionProperty("INITIAL", "initial");
        public static readonly CSSProperty_TransitionProperty UNSET = new CSSProperty_TransitionProperty("UNSET", "unset");
        internal CSSProperty_TransitionProperty(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_TransitionProperty, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }
    public sealed class CSSProperty_TransitionTimingFunction : SmartEnum<CSSProperty_TransitionTimingFunction, string>, CSSProperty
    {
        public static readonly CSSProperty_TransitionTimingFunction timing_function = new CSSProperty_TransitionTimingFunction("timing_function", "");
        public static readonly CSSProperty_TransitionTimingFunction list_values = new CSSProperty_TransitionTimingFunction("list_values", "");
        public static readonly CSSProperty_TransitionTimingFunction LINEAR = new CSSProperty_TransitionTimingFunction("LINEAR", "linear");
        public static readonly CSSProperty_TransitionTimingFunction EASE = new CSSProperty_TransitionTimingFunction("EASE", "ease");
        public static readonly CSSProperty_TransitionTimingFunction EASE_IN = new CSSProperty_TransitionTimingFunction("EASE_IN", "ease-in");
        public static readonly CSSProperty_TransitionTimingFunction EASE_OUT = new CSSProperty_TransitionTimingFunction("EASE_OUT", "ease-out");
        public static readonly CSSProperty_TransitionTimingFunction EASE_IN_OUT = new CSSProperty_TransitionTimingFunction("EASE_IN_OUT", "ease-in-out");
        public static readonly CSSProperty_TransitionTimingFunction STEP_START = new CSSProperty_TransitionTimingFunction("STEP_START", "step-start");
        public static readonly CSSProperty_TransitionTimingFunction STEP_END = new CSSProperty_TransitionTimingFunction("STEP_END", "step-end");
        public static readonly CSSProperty_TransitionTimingFunction INHERIT = new CSSProperty_TransitionTimingFunction("INHERIT", "inherit");
        public static readonly CSSProperty_TransitionTimingFunction INITIAL = new CSSProperty_TransitionTimingFunction("INITIAL", "initial");
        public static readonly CSSProperty_TransitionTimingFunction UNSET = new CSSProperty_TransitionTimingFunction("UNSET", "unset");

        internal CSSProperty_TransitionTimingFunction(string name, string value) : base(name, value)
        {
        }
        public bool inherited()
        {
            return false;
        }
        public bool equalsInherit()
        {
            return this == INHERIT;
        }
        public bool equalsInitial()
        {
            return this == INITIAL;
        }
        public bool equalsUnset()
        {
            return this == UNSET;
        }
        public CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        public override bool Equals(SmartEnum<CSSProperty_TransitionTimingFunction, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }

    }

    public class CSSProperty_GenericCSSPropertyProxy : CSSProperty
    {
        internal string text;
        //ORIGINAL LINE: CSSProperty_GenericCSSPropertyProxy(final String thePropertyValue)
        internal CSSProperty_GenericCSSPropertyProxy(string thePropertyValue)
        {
            this.text = thePropertyValue;
        }
        public virtual bool inherited()
        {
            return false;
        }
        public virtual bool equalsInherit()
        {
            return false;
        }
        public virtual bool equalsInitial()
        {
            return false;
        }
        public virtual bool equalsUnset()
        {
            return false;
        }
        public virtual CSSProperty_ValueType ValueType
        {
            get
            {
                return CSSProperty_ValueType.SIMPLE;
            }
        }
        public override string ToString()
        {
            return text;
        }
        /*
        public override bool Equals(SmartEnum<CSSProperty_GenericCSSPropertyProxy, string> other)
        {
            bool areEqualBase = base.Equals(other);

            if (areEqualBase == false)
                return false;

            if (other != null && Name != other.Name)
            {
                return false;
            }
            return areEqualBase;
        }
        */
        /// <summary>
        /// Creates a new instance of the GenericCSSPropertyProxy. This method
        /// simulates the method valueOf(String) of the other CSS attributes that
        /// are implmented as enums.
        /// </summary>
        /// <param name="value"> the property value.
        /// </param>
        /// <returns> a new insance that contains the property value. </returns>
        //ORIGINAL LINE: public static CSSProperty_GenericCSSPropertyProxy valueOf(final String value)
        public static CSSProperty_GenericCSSPropertyProxy valueOf(string value)
        {
            return new CSSProperty_GenericCSSPropertyProxy(string.ReferenceEquals(value, null) ? "" : value.ToLower());
        }
    }
}