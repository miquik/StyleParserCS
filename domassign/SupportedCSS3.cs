using System;
using System.Collections.Generic;

namespace StyleParserCS.domassign
{
    using CSSFactory = StyleParserCS.css.CSSFactory;
    using CSSProperty = StyleParserCS.css.CSSProperty;
    using SupportedCSS = StyleParserCS.css.SupportedCSS;
    using StyleParserCS.css;
    using TermFactory = StyleParserCS.css.TermFactory;
    using TermList = StyleParserCS.css.TermList;
    using System.Linq;
    using StyleParserCS.utils;

    /// <summary>
    /// Contains default values for properties supported by parser (CSS 3)
    /// 
    /// @author kapy
    /// @author burgetr
    /// </summary>
    public class SupportedCSS3 : SupportedCSS
    {
        private const int TOTAL_SUPPORTED_DECLARATIONS = 177;

        private static readonly TermFactory tf = CSSFactory.TermFactory;

        private static readonly CSSProperty DEFAULT_UA_FONT_FAMILY = StyleParserCS.css.CSSProperty_FontFamily.SANS_SERIF;
        private static readonly CSSProperty DEFAULT_UA_TEXT_ALIGN = StyleParserCS.css.CSSProperty_TextAlign.BY_DIRECTION;
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_UA_COLOR = tf.createColor("#000000");
        private static readonly Term DEFAULT_UA_COLOR = (Term)tf.createColor("#000000");
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_UA_OPACITY = tf.createNumber(1.0f);
        private static readonly Term DEFAULT_UA_OPACITY = (Term)tf.createNumber(1.0f);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_UA_TEXT_IDENT = tf.createLength(0.0f);
        private static readonly Term DEFAULT_UA_TEXT_IDENT = (Term)tf.createLength(0.0f);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_UA_TAB_SIZE = tf.createInteger(8);
        private static readonly Term DEFAULT_UA_TAB_SIZE = (Term)tf.createInteger(8);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_UA_MARGIN = tf.createLength(0.0f);
        private static readonly Term DEFAULT_UA_MARGIN = (Term)tf.createLength(0.0f);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_UA_PADDING = tf.createLength(0.0f);
        private static readonly Term DEFAULT_UA_PADDING = (Term)tf.createLength(0.0f);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_UA_MIN_WIDTH = tf.createLength(0.0f);
        private static readonly Term DEFAULT_UA_MIN_WIDTH = (Term)tf.createLength(0.0f);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_UA_MIN_HEIGHT = tf.createLength(0.0f);
        private static readonly Term DEFAULT_UA_MIN_HEIGHT = (Term)tf.createLength(0.0f);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_BORDER_COLOR = tf.createColor(tf.createIdent("currentColor"));
        private static readonly Term DEFAULT_BORDER_COLOR = (Term)tf.createColor(tf.createIdent("currentColor"));
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_BACKGROUND_COLOR = tf.createColor(tf.createIdent("transparent"));
        private static readonly Term DEFAULT_BACKGROUND_COLOR = (Term)tf.createColor(tf.createIdent("transparent"));
        private static readonly TermList DEFAULT_UA_BACKGROUND_POSITION = tf.createList(2);

        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_FLEX_SHRINK = (Term)tf.createNumber(1.0f);
        private static readonly Term DEFAULT_FLEX_SHRINK = (Term)tf.createNumber(1.0f);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_FLEX_GROW = (Term)tf.createNumber(0.0f);
        private static readonly Term DEFAULT_FLEX_GROW = (Term)tf.createNumber(0.0f);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_ORDER = (Term)tf.createInteger(0);
        private static readonly Term DEFAULT_ORDER = (Term)tf.createInteger(0);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_TIME = (Term)tf.createTime(0f);
        private static readonly Term DEFAULT_TIME = (Term)tf.createTime(0f);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_ITERATION_COUNT = (Term)tf.createInteger(1);
        private static readonly Term DEFAULT_ITERATION_COUNT = (Term)tf.createInteger(1);

        static SupportedCSS3()
        {
            DEFAULT_UA_BACKGROUND_POSITION.Add((Term)tf.createPercent(0.0f));
            DEFAULT_UA_BACKGROUND_POSITION.Add((Term)tf.createPercent(0.0f));
            DEFAULT_UA_BACKGROUND_SIZE.Add((Term)tf.createIdent("auto"));
            DEFAULT_UA_BACKGROUND_SIZE.Add((Term)tf.createIdent("auto"));
            DEFAULT_UA_BORDER_RADIUS.Add((Term)tf.createLength(0.0f));
            DEFAULT_UA_BORDER_RADIUS.Add((Term)tf.createLength(0.0f));
            DEFAULT_UA_BORDER_SPACING.Add((Term)tf.createLength(0.0f));
            DEFAULT_UA_BORDER_SPACING.Add((Term)tf.createLength(0.0f));
            DEFAULT_UA_TRANSFORM_ORIGIN.Add((Term)tf.createPercent(50.0f));
            DEFAULT_UA_TRANSFORM_ORIGIN.Add((Term)tf.createPercent(50.0f));
            DEFAULT_UA_TRANSFORM_ORIGIN.Add((Term)tf.createLength(0.0f));
            instance = new SupportedCSS3();
        }
        private static readonly TermList DEFAULT_UA_BACKGROUND_SIZE = tf.createList(2);
        private static readonly TermList DEFAULT_UA_BORDER_RADIUS = tf.createList(2);
        private static readonly TermList DEFAULT_UA_BORDER_SPACING = tf.createList(2);
        private static readonly TermList DEFAULT_UA_TRANSFORM_ORIGIN = tf.createList(3);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_UA_WIDOWS = (Term)tf.createInteger(2);
        private static readonly Term DEFAULT_UA_WIDOWS = (Term)tf.createInteger(2);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_UA_ORPHANS = (Term)tf.createInteger(2);
        private static readonly Term DEFAULT_UA_ORPHANS = (Term)tf.createInteger(2);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_UA_PAUSE_BEFORE = (Term)tf.createTime(0.0f);
        private static readonly Term DEFAULT_UA_PAUSE_BEFORE = (Term)tf.createTime(0.0f);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_UA_PAUSE_AFTER = (Term)tf.createTime(0.0f);
        private static readonly Term DEFAULT_UA_PAUSE_AFTER = (Term)tf.createTime(0.0f);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_UA_RICHNESS = (Term)tf.createNumber(50.0f);
        private static readonly Term DEFAULT_UA_RICHNESS = (Term)tf.createNumber(50.0f);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_UA_PITCH_RANGE = (Term)tf.createNumber(50.0f);
        private static readonly Term DEFAULT_UA_PITCH_RANGE = (Term)tf.createNumber(50.0f);
        //ORIGINAL LINE: private static final StyleParserCS.css.Term<?> DEFAULT_UA_STRESS = (Term)tf.createNumber(50.0f);
        private static readonly Term DEFAULT_UA_STRESS = (Term)tf.createNumber(50.0f);

        private static readonly CSSProperty DEFAULT_UA_VOICE_FAMILY = StyleParserCS.css.CSSProperty_VoiceFamily.MALE;

        private static readonly SupportedCSS3 instance;

        /// <summary>
        /// Contains names of supported elements and default values according to <a
        /// href="http://www.culturedcode.com/css/reference.html">
        /// http://www.culturedcode.com/css/reference.html</a>
        /// </summary>
        private IDictionary<string, CSSProperty> defaultCSSproperties;
        //ORIGINAL LINE: private java.util.Map<String, StyleParserCS.css.Term<?>> defaultCSSvalues;
        private IDictionary<string, Term> defaultCSSvalues;

        private IDictionary<string, int> ordinals;
        private IDictionary<int, string> ordinalsRev;

        private ISet<string> supportedMedia;

        /// <summary>
        /// Gets instance of SupportedCSS21
        /// </summary>
        /// <returns> Singleton instance reference </returns>
        public static SupportedCSS3 Instance
        {
            get
            {
                return instance;
            }
        }

        public SupportedCSS3()
        {
            this.setSupportedCSS();
            this.setOridinals();
            this.setSupportedAtKeywords();
        }

        public virtual bool isSupportedMedia(string media)
        {
            if (string.ReferenceEquals(media, null))
            {
                return false;
            }

            return supportedMedia.Contains(media.ToLower());
        }

        public bool isSupportedCSSProperty(string property)
        {
            return defaultCSSproperties.GetValue(property) != null;
        }

        public CSSProperty getDefaultProperty(string property)
        {
            CSSProperty value = defaultCSSproperties.GetValue(property);
            // log.debug("Asked for property {}'s default value: {}", property, value);
            return value;
        }

        //ORIGINAL LINE: public final StyleParserCS.css.Term<?> getDefaultValue(String property)
        public Term getDefaultValue(string property)
        {
            return defaultCSSvalues.GetValue(property);
        }

        public int TotalProperties
        {
            get
            {
                return defaultCSSproperties.Count;
            }
        }

        public ISet<string> DefinedPropertyNames
        {
            get
            {
                return defaultCSSproperties.Keys.ToHashSet();
            }
        }

        public virtual string RandomPropertyName
        {
            get
            {
                //ORIGINAL LINE: final java.util.Random generator = new java.util.Random();
                Random generator = new Random();
                int o = generator.Next(TotalProperties);
                return getPropertyName(o);
            }
        }

        public virtual int getOrdinal(string propertyName)
        {
            int? i = ordinals.GetValue(propertyName);
            return (i == null) ? -1 : i.Value;
        }

        public virtual string getPropertyName(int o)
        {
            return ordinalsRev.GetValue(o);
        }

        /// <summary>
        /// Constructs maps of CSSProperties and its values (Term<?) according to CSS
        /// 2.1 definition. Called during construction of object.
        /// </summary>
        private void setSupportedCSS()
        {

            IDictionary<string, CSSProperty> props = new Dictionary<string, CSSProperty>(TOTAL_SUPPORTED_DECLARATIONS);

            //ORIGINAL LINE: java.util.Map<String, StyleParserCS.css.Term<?>> values = new java.util.HashMap<String, StyleParserCS.css.Term<?>>(TOTAL_SUPPORTED_DECLARATIONS, 1.0f);
            IDictionary<string, Term> values = new Dictionary<string, Term>(TOTAL_SUPPORTED_DECLARATIONS);

            // text type
            props["color"] = StyleParserCS.css.CSSProperty_Color.color;
            values["color"] = DEFAULT_UA_COLOR;
            props["opacity"] = StyleParserCS.css.CSSProperty_Opacity.number;
            values["opacity"] = DEFAULT_UA_OPACITY;
            props["font"] = StyleParserCS.css.CSSProperty_Font.component_values;
            props["font-family"] = DEFAULT_UA_FONT_FAMILY;
            props["font-size"] = StyleParserCS.css.CSSProperty_FontSize.MEDIUM;
            props["font-style"] = StyleParserCS.css.CSSProperty_FontStyle.NORMAL;
            props["font-variant"] = StyleParserCS.css.CSSProperty_FontVariant.NORMAL;
            props["font-weight"] = StyleParserCS.css.CSSProperty_FontWeight.NORMAL;
            props["text-decoration"] = StyleParserCS.css.CSSProperty_TextDecoration.NONE;
            props["text-transform"] = StyleParserCS.css.CSSProperty_TextTransform.NONE;

            // text spacing
            props["white-space"] = StyleParserCS.css.CSSProperty_WhiteSpace.NORMAL;
            props["text-align"] = DEFAULT_UA_TEXT_ALIGN;
            props["text-indent"] = StyleParserCS.css.CSSProperty_TextIndent.length;
            values["text-indent"] = DEFAULT_UA_TEXT_IDENT;
            props["line-height"] = StyleParserCS.css.CSSProperty_LineHeight.NORMAL;
            props["word-spacing"] = StyleParserCS.css.CSSProperty_WordSpacing.NORMAL;
            props["letter-spacing"] = StyleParserCS.css.CSSProperty_LetterSpacing.NORMAL;
            props["vertical-align"] = StyleParserCS.css.CSSProperty_VerticalAlign.BASELINE;
            props["direction"] = StyleParserCS.css.CSSProperty_Direction.LTR;
            props["unicode-bidi"] = StyleParserCS.css.CSSProperty_UnicodeBidi.NORMAL;
            props["tab-size"] = StyleParserCS.css.CSSProperty_TabSize.integer;
            values["tab-size"] = DEFAULT_UA_TAB_SIZE;

            // layout box
            props["margin"] = StyleParserCS.css.CSSProperty_Margin.component_values;
            props["margin-top"] = StyleParserCS.css.CSSProperty_Margin.length;
            values["margin-top"] = DEFAULT_UA_MARGIN;
            props["margin-right"] = StyleParserCS.css.CSSProperty_Margin.length;
            values["margin-right"] = DEFAULT_UA_MARGIN;
            props["margin-bottom"] = StyleParserCS.css.CSSProperty_Margin.length;
            values["margin-bottom"] = DEFAULT_UA_MARGIN;
            props["margin-left"] = StyleParserCS.css.CSSProperty_Margin.length;
            values["margin-left"] = DEFAULT_UA_MARGIN;

            props["padding"] = StyleParserCS.css.CSSProperty_Padding.component_values;
            props["padding-top"] = StyleParserCS.css.CSSProperty_Padding.length;
            values["padding-top"] = DEFAULT_UA_PADDING;
            props["padding-right"] = StyleParserCS.css.CSSProperty_Padding.length;
            values["padding-right"] = DEFAULT_UA_PADDING;
            props["padding-bottom"] = StyleParserCS.css.CSSProperty_Padding.length;
            values["padding-bottom"] = DEFAULT_UA_PADDING;
            props["padding-left"] = StyleParserCS.css.CSSProperty_Padding.length;
            values["padding-left"] = DEFAULT_UA_PADDING;

            props["border"] = StyleParserCS.css.CSSProperty_Border.component_values;
            props["border-top"] = StyleParserCS.css.CSSProperty_Border.component_values;
            props["border-right"] = StyleParserCS.css.CSSProperty_Border.component_values;
            props["border-bottom"] = StyleParserCS.css.CSSProperty_Border.component_values;
            props["border-left"] = StyleParserCS.css.CSSProperty_Border.component_values;

            props["border-width"] = StyleParserCS.css.CSSProperty_BorderWidth.component_values;
            props["border-top-width"] = StyleParserCS.css.CSSProperty_BorderWidth.MEDIUM;
            props["border-right-width"] = StyleParserCS.css.CSSProperty_BorderWidth.MEDIUM;
            props["border-bottom-width"] = StyleParserCS.css.CSSProperty_BorderWidth.MEDIUM;
            props["border-left-width"] = StyleParserCS.css.CSSProperty_BorderWidth.MEDIUM;
            props["border-style"] = StyleParserCS.css.CSSProperty_BorderStyle.component_values;
            props["border-top-style"] = StyleParserCS.css.CSSProperty_BorderStyle.NONE;
            props["border-right-style"] = StyleParserCS.css.CSSProperty_BorderStyle.NONE;
            props["border-bottom-style"] = StyleParserCS.css.CSSProperty_BorderStyle.NONE;
            props["border-left-style"] = StyleParserCS.css.CSSProperty_BorderStyle.NONE;

            props["border-color"] = StyleParserCS.css.CSSProperty_BorderColor.component_values;
            props["border-top-color"] = StyleParserCS.css.CSSProperty_BorderColor.color;
            values["border-top-color"] = DEFAULT_BORDER_COLOR;
            props["border-right-color"] = StyleParserCS.css.CSSProperty_BorderColor.color;
            values["border-right-color"] = DEFAULT_BORDER_COLOR;
            props["border-bottom-color"] = StyleParserCS.css.CSSProperty_BorderColor.color;
            values["border-bottom-color"] = DEFAULT_BORDER_COLOR;
            props["border-left-color"] = StyleParserCS.css.CSSProperty_BorderColor.color;
            values["border-left-color"] = DEFAULT_BORDER_COLOR;

            props["border-radius"] = StyleParserCS.css.CSSProperty_BorderRadius.component_values;
            props["border-top-left-radius"] = StyleParserCS.css.CSSProperty_BorderRadius.list_values;
            values["border-top-left-radius"] = (Term)DEFAULT_UA_BORDER_RADIUS;
            props["border-top-right-radius"] = StyleParserCS.css.CSSProperty_BorderRadius.list_values;
            values["border-top-right-radius"] = (Term)DEFAULT_UA_BORDER_RADIUS;
            props["border-bottom-right-radius"] = StyleParserCS.css.CSSProperty_BorderRadius.list_values;
            values["border-bottom-right-radius"] = (Term)DEFAULT_UA_BORDER_RADIUS;
            props["border-bottom-left-radius"] = StyleParserCS.css.CSSProperty_BorderRadius.list_values;
            values["border-bottom-left-radius"] = (Term)DEFAULT_UA_BORDER_RADIUS;

            props["width"] = StyleParserCS.css.CSSProperty_Width.AUTO;
            props["min-width"] = StyleParserCS.css.CSSProperty_MinWidth.length;
            values["min-width"] = DEFAULT_UA_MIN_WIDTH;
            props["max-width"] = StyleParserCS.css.CSSProperty_MaxWidth.NONE;
            props["height"] = StyleParserCS.css.CSSProperty_Height.AUTO;
            props["min-height"] = StyleParserCS.css.CSSProperty_MinHeight.length;
            values["min-height"] = DEFAULT_UA_MIN_HEIGHT;
            props["max-height"] = StyleParserCS.css.CSSProperty_MaxHeight.NONE;
            props["overflow"] = StyleParserCS.css.CSSProperty_Overflow.component_values;
            props["overflow-x"] = StyleParserCS.css.CSSProperty_Overflow.VISIBLE;
            props["overflow-y"] = StyleParserCS.css.CSSProperty_Overflow.VISIBLE;
            props["clip"] = StyleParserCS.css.CSSProperty_Clip.AUTO;
            props["box-sizing"] = StyleParserCS.css.CSSProperty_BoxSizing.CONTENT_BOX;
            props["box-shadow"] = StyleParserCS.css.CSSProperty_BoxShadow.NONE;

            // positioning
            props["display"] = StyleParserCS.css.CSSProperty_Display.INLINE;
            props["position"] = StyleParserCS.css.CSSProperty_Position.STATIC;
            props["top"] = StyleParserCS.css.CSSProperty_Top.AUTO;
            props["right"] = StyleParserCS.css.CSSProperty_Right.AUTO;
            props["bottom"] = StyleParserCS.css.CSSProperty_Bottom.AUTO;
            props["left"] = StyleParserCS.css.CSSProperty_Left.AUTO;
            props["float"] = CSSProperty_Floating.NONE;
            props["clear"] = StyleParserCS.css.CSSProperty_Clear.NONE;
            props["z-index"] = StyleParserCS.css.CSSProperty_ZIndex.AUTO;
            props["visibility"] = StyleParserCS.css.CSSProperty_Visibility.VISIBLE;
            props["transform"] = StyleParserCS.css.CSSProperty_Transform.NONE;
            props["transform-origin"] = StyleParserCS.css.CSSProperty_TransformOrigin.list_values;
            values["transform-origin"] = (Term)DEFAULT_UA_TRANSFORM_ORIGIN;

            // background
            props["background"] = StyleParserCS.css.CSSProperty_Background.component_values;
            props["background-attachment"] = StyleParserCS.css.CSSProperty_BackgroundAttachment.SCROLL;
            props["background-color"] = StyleParserCS.css.CSSProperty_BackgroundColor.color;
            values["background-color"] = DEFAULT_BACKGROUND_COLOR;
            props["background-image"] = StyleParserCS.css.CSSProperty_BackgroundImage.NONE;
            props["background-position"] = StyleParserCS.css.CSSProperty_BackgroundPosition.list_values;
            values["background-position"] = (Term)DEFAULT_UA_BACKGROUND_POSITION;
            props["background-size"] = StyleParserCS.css.CSSProperty_BackgroundSize.list_values;
            values["background-size"] = (Term)DEFAULT_UA_BACKGROUND_SIZE;
            props["background-repeat"] = StyleParserCS.css.CSSProperty_BackgroundRepeat.REPEAT;
            props["background-origin"] = StyleParserCS.css.CSSProperty_BackgroundOrigin.PADDING_BOX;

            // box shadow
            props["box-shadow"] = StyleParserCS.css.CSSProperty_BoxShadow.NONE;

            // elements
            props["list-style"] = StyleParserCS.css.CSSProperty_ListStyle.component_values;
            props["list-style-type"] = StyleParserCS.css.CSSProperty_ListStyleType.DISC;
            props["list-style-position"] = StyleParserCS.css.CSSProperty_ListStylePosition.OUTSIDE;
            props["list-style-image"] = StyleParserCS.css.CSSProperty_ListStyleImage.NONE;

            props["border-collapse"] = StyleParserCS.css.CSSProperty_BorderCollapse.SEPARATE;
            props["border-spacing"] = StyleParserCS.css.CSSProperty_BorderSpacing.list_values;
            values["border-spacing"] = (Term)DEFAULT_UA_BORDER_SPACING;
            props["empty-cells"] = StyleParserCS.css.CSSProperty_EmptyCells.SHOW;
            props["table-layout"] = StyleParserCS.css.CSSProperty_TableLayout.AUTO;
            props["caption-side"] = StyleParserCS.css.CSSProperty_CaptionSide.TOP;

            // other supported by tables (width, vertical-align)
            // are already defined
            props["content"] = StyleParserCS.css.CSSProperty_Content.NORMAL;
            props["quotes"] = StyleParserCS.css.CSSProperty_Quotes.NONE;
            props["counter-increment"] = StyleParserCS.css.CSSProperty_CounterIncrement.NONE;
            props["counter-reset"] = StyleParserCS.css.CSSProperty_CounterReset.NONE;

            // filter
            props["filter"] = StyleParserCS.css.CSSProperty_Filter.NONE;
            props["backdrop-filter"] = StyleParserCS.css.CSSProperty_BackdropFilter.NONE;

            // miscellaneous
            props["cursor"] = StyleParserCS.css.CSSProperty_Cursor.AUTO;
            props["outline"] = StyleParserCS.css.CSSProperty_Outline.component_values;
            props["outline-width"] = StyleParserCS.css.CSSProperty_OutlineWidth.MEDIUM;
            props["outline-style"] = StyleParserCS.css.CSSProperty_OutlineStyle.NONE;
            props["outline-color"] = StyleParserCS.css.CSSProperty_OutlineColor.INVERT;

            props["page-break"] = StyleParserCS.css.CSSProperty_PageBreak.AUTO;
            props["page-break-before"] = StyleParserCS.css.CSSProperty_PageBreak.AUTO;
            props["page-break-after"] = StyleParserCS.css.CSSProperty_PageBreak.AUTO;
            props["page-break-inside"] = StyleParserCS.css.CSSProperty_PageBreakInside.AUTO;

            props["widows"] = StyleParserCS.css.CSSProperty_Widows.integer;
            values["widows"] = DEFAULT_UA_WIDOWS;
            props["orphans"] = StyleParserCS.css.CSSProperty_Orphans.integer;
            values["orphans"] = DEFAULT_UA_ORPHANS;

            // other values according to
            // http://www.w3.org/TR/CSS21/propidx.html

            props["azimuth"] = StyleParserCS.css.CSSProperty_Azimuth.CENTER;
            props["cue"] = StyleParserCS.css.CSSProperty_Cue.component_values;
            props["cue-before"] = StyleParserCS.css.CSSProperty_Cue.NONE;
            props["cue-after"] = StyleParserCS.css.CSSProperty_Cue.NONE;
            props["elevation"] = StyleParserCS.css.CSSProperty_Elevation.LEVEL;
            props["pause"] = StyleParserCS.css.CSSProperty_Pause.component_values;
            props["pause-before"] = StyleParserCS.css.CSSProperty_Pause.time;
            values["pause-before"] = DEFAULT_UA_PAUSE_BEFORE;
            props["pause-after"] = StyleParserCS.css.CSSProperty_Pause.time;
            values["pause-after"] = DEFAULT_UA_PAUSE_AFTER;
            props["pitch-range"] = StyleParserCS.css.CSSProperty_PitchRange.number;
            values["pitch-range"] = DEFAULT_UA_PITCH_RANGE;
            props["pitch"] = StyleParserCS.css.CSSProperty_Pitch.MEDIUM;
            props["play-during"] = StyleParserCS.css.CSSProperty_PlayDuring.AUTO;
            props["richness"] = StyleParserCS.css.CSSProperty_Richness.number;
            values["richness"] = DEFAULT_UA_RICHNESS;
            props["speak-header"] = StyleParserCS.css.CSSProperty_SpeakHeader.ONCE;
            props["speak-numeral"] = StyleParserCS.css.CSSProperty_SpeakNumeral.CONTINUOUS;
            props["speak-punctuation"] = StyleParserCS.css.CSSProperty_SpeakPunctuation.NONE;
            props["speak"] = StyleParserCS.css.CSSProperty_Speak.NORMAL;
            props["speech-rate"] = StyleParserCS.css.CSSProperty_SpeechRate.MEDIUM;
            props["stress"] = StyleParserCS.css.CSSProperty_Stress.number;
            values["stress"] = DEFAULT_UA_STRESS;
            props["voice-family"] = DEFAULT_UA_VOICE_FAMILY;
            props["volume"] = StyleParserCS.css.CSSProperty_Volume.MEDIUM;

            // Flexbox
            props["flex"] = StyleParserCS.css.CSSProperty_Flex.component_values;
            props["flex-flow"] = StyleParserCS.css.CSSProperty_FlexFlow.component_values;
            props["flex-direction"] = StyleParserCS.css.CSSProperty_FlexDirection.ROW;
            props["flex-wrap"] = StyleParserCS.css.CSSProperty_FlexWrap.NOWRAP;
            props["flex-basis"] = StyleParserCS.css.CSSProperty_FlexBasis.AUTO;
            props["flex-grow"] = StyleParserCS.css.CSSProperty_FlexGrow.number;
            values["flex-grow"] = DEFAULT_FLEX_GROW;
            props["flex-shrink"] = StyleParserCS.css.CSSProperty_FlexShrink.number;
            values["flex-shrink"] = DEFAULT_FLEX_SHRINK;
            props["order"] = StyleParserCS.css.CSSProperty_Order.integer;
            values["order"] = DEFAULT_ORDER;
            props["justify-content"] = StyleParserCS.css.CSSProperty_JustifyContent.FLEX_START;
            props["align-content"] = StyleParserCS.css.CSSProperty_AlignContent.STRETCH;
            props["align-items"] = StyleParserCS.css.CSSProperty_AlignItems.STRETCH;
            props["align-self"] = StyleParserCS.css.CSSProperty_AlignSelf.AUTO;

            // grid layout
            props["grid"] = StyleParserCS.css.CSSProperty_Grid.component_values;
            props["grid-gap"] = StyleParserCS.css.CSSProperty_GridGap.component_values;
            props["grid-row-gap"] = StyleParserCS.css.CSSProperty_GridGap.NORMAL;
            props["grid-column-gap"] = StyleParserCS.css.CSSProperty_GridGap.NORMAL;
            props["grid-area"] = StyleParserCS.css.CSSProperty_Grid.component_values;
            props["grid-row"] = StyleParserCS.css.CSSProperty_Grid.component_values;
            props["grid-column"] = StyleParserCS.css.CSSProperty_Grid.component_values;
            props["grid-row-start"] = StyleParserCS.css.CSSProperty_GridStartEnd.AUTO;
            props["grid-column-start"] = StyleParserCS.css.CSSProperty_GridStartEnd.AUTO;
            props["grid-row-end"] = StyleParserCS.css.CSSProperty_GridStartEnd.AUTO;
            props["grid-column-end"] = StyleParserCS.css.CSSProperty_GridStartEnd.AUTO;
            props["grid-template"] = StyleParserCS.css.CSSProperty_Grid.component_values;
            props["grid-template-areas"] = StyleParserCS.css.CSSProperty_GridTemplateAreas.NONE;
            props["grid-template-rows"] = StyleParserCS.css.CSSProperty_GridTemplateRowsColumns.NONE;
            props["grid-template-columns"] = StyleParserCS.css.CSSProperty_GridTemplateRowsColumns.NONE;
            props["grid-auto-flow"] = StyleParserCS.css.CSSProperty_GridAutoFlow.ROW;
            props["grid-auto-rows"] = StyleParserCS.css.CSSProperty_GridAutoRowsColumns.AUTO;
            props["grid-auto-columns"] = StyleParserCS.css.CSSProperty_GridAutoRowsColumns.AUTO;

            // animation
            props["animation"] = StyleParserCS.css.CSSProperty_Animation.component_values;
            props["animation-delay"] = StyleParserCS.css.CSSProperty_AnimationDelay.time;
            values["animation-delay"] = DEFAULT_TIME;
            props["animation-direction"] = StyleParserCS.css.CSSProperty_AnimationDirection.NORMAL;
            props["animation-duration"] = StyleParserCS.css.CSSProperty_AnimationDuration.time;
            values["animation-duration"] = DEFAULT_TIME;
            props["animation-fill-mode"] = StyleParserCS.css.CSSProperty_AnimationFillMode.NONE;
            props["animation-iteration-count"] = StyleParserCS.css.CSSProperty_AnimationIterationCount.number;
            values["animation-iteration-count"] = DEFAULT_ITERATION_COUNT;
            props["animation-name"] = StyleParserCS.css.CSSProperty_AnimationName.NONE;
            props["animation-play-state"] = StyleParserCS.css.CSSProperty_AnimationPlayState.RUNNING;
            props["animation-timing-function"] = StyleParserCS.css.CSSProperty_AnimationTimingFunction.EASE;

            // transition
            props["transition"] = StyleParserCS.css.CSSProperty_Transition.component_values;
            props["transition-delay"] = StyleParserCS.css.CSSProperty_TransitionDelay.time;
            values["transition-delay"] = DEFAULT_TIME;
            props["transition-duration"] = StyleParserCS.css.CSSProperty_TransitionDuration.time;
            values["transition-duration"] = DEFAULT_TIME;
            props["transition-property"] = StyleParserCS.css.CSSProperty_TransitionProperty.ALL;
            props["transition-timing-function"] = StyleParserCS.css.CSSProperty_TransitionTimingFunction.EASE;

            this.defaultCSSproperties = props;
            this.defaultCSSvalues = values;
        }

        private void setOridinals()
        {

            IDictionary<string, int> ords = new Dictionary<string, int>(TotalProperties);
            IDictionary<int, string> ordsRev = new Dictionary<int, string>(TotalProperties);

            int i = 0;
            foreach (string key in defaultCSSproperties.Keys)
            {
                ords[key] = i;
                ordsRev[i] = key;
                i++;
            }

            this.ordinals = ords;
            this.ordinalsRev = ordsRev;

        }

        private void setSupportedAtKeywords()
        {

            ISet<string> set = new HashSet<string>() { "all", "braille", "embossed", "handheld", "print", "projection", "screen", "speech", "tty", "tv" };

            this.supportedMedia = set;
        }

    }

}