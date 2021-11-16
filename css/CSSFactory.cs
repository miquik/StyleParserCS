using System;
using System.Collections.Generic;

namespace StyleParserCS.css
{
    using DataUriHandler = org.fit.net.DataUriHandler;

    using DeclarationTransformer = StyleParserCS.csskit.DeclarationTransformer;
    using DefaultNetworkProcessor = StyleParserCS.csskit.DefaultNetworkProcessor;
    using MatchConditionImpl = StyleParserCS.csskit.MatchConditionImpl;
    using CSSParserFactory = StyleParserCS.csskit.antlr4.CSSParserFactory;
    using SourceType = StyleParserCS.csskit.antlr4.CSSParserFactory.SourceType;
    using Analyzer = StyleParserCS.domassign.Analyzer;
    using StyleMap = StyleParserCS.domassign.StyleMap;
    using StyleParserCS.domassign;
    using System.IO;
    using AngleSharp.Dom;
    using StyleParserCS.csskit;

    /// <summary>
    /// This class is abstract factory for other factories used during CSS parsing.
    /// Use it, for example, to retrieve current(default) TermFactory,
    /// current(default) SupportedCSS implementation and so on.
    /// 
    /// Factories need to be registered first. By default, CSSFactory uses
    /// automatically for implementations:
    /// <code>StyleParserCS.csskit.TermFactoryImpl</code>
    /// <code>StyleParserCS.domassign.SupportedCSS3</code>
    /// <code>StyleParserCS.csskit.RuleFactoryImpl</code>
    /// <code>StyleParserCS.domassign.SingleMapNodeData</code>
    /// <code>StyleParserCS.csskit.ElementMatcherSafeStd</code>
    /// 
    /// Other usage of this factory is to parse either string or file into
    /// StyleSheet.
    /// 
    /// Whole conversion between DOM tree and mapping of CSS to DOM tree elements
    /// could be done by invoking method {@code assignDOM()}
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public sealed class CSSFactory
    {
        // private static Logger log = LoggerFactory.getLogger(typeof(CSSFactory));

        private const string DEFAULT_TERM_FACTORY = "StyleParserCS.csskit.TermFactoryImpl";
        private const string DEFAULT_SUPPORTED_CSS = "StyleParserCS.domassign.SupportedCSS3";
        private const string DEFAULT_RULE_FACTORY = "StyleParserCS.csskit.RuleFactoryImpl";
        private const string DEFAULT_DECLARATION_TRANSFORMER = "StyleParserCS.domassign.DeclarationTransformerImpl";
        private const string DEFAULT_NODE_DATA_IMPL = "StyleParserCS.domassign.SingleMapNodeData";
        private const string DEFAULT_ELEMENT_MATCHER = "StyleParserCS.csskit.ElementMatcherSafeStd";

        /// <summary>
        /// Default instance of CSSParcerFactory
        /// </summary>
        private static CSSParserFactory pf;

        /// <summary>
        /// Default instance of TermFactory implementation
        /// </summary>
        private static TermFactory tf;

        /// <summary>
        /// Default instance of SupportedCSS implementation
        /// </summary>
        private static SupportedCSS css;

        /// <summary>
        /// Default instance of RuleFactory implementation
        /// </summary>
        private static RuleFactory rf;

        /// <summary>
        /// Default instance of DeclarationTransformer
        /// </summary>
        private static DeclarationTransformer dt;

        /// <summary>
        /// Used ElementMatcher instance.
        /// </summary>
        private static ElementMatcher matcher;

        /// <summary>
        /// Used NodeData class
        /// </summary>
        private static Type ndImpl;

        /// <summary>
        /// Default match condition
        /// </summary>
        private static MatchCondition dcond;

        /// <summary>
        /// Whether to allow lengths with no units and interpret them as pixels.
        /// </summary>
        private static bool implyPixelLengths = false;


        /// <summary>
        /// Media specification used for automatically importing style sheets. 
        /// </summary>
        private static MediaSpec autoImportMedia = null;

        /// 
        /// <summary>
        /// Default network processor
        /// 
        /// </summary>
        private static NetworkProcessor networkProcessor = null;

        /// <summary>
        /// Sets whether to allow lengths with no units and interpret them as pixels. The default value is {@code false}. </summary>
        /// <param name="b"> {@code true} when the lengths with no units should be allowed. </param>
        public static bool ImplyPixelLength
        {
            set
            {
                implyPixelLengths = value;
            }
            get
            {
                return implyPixelLengths;
            }
        }


        /// <summary>
        /// Obtains the media specification used for automatical style import. The parser will
        /// automatically download the style sheets imported using the {@code @import} rules
        /// when they corespond to the given media specification. </summary>
        /// <returns> The media specification. </returns>
        public static MediaSpec AutoImportMedia
        {
            get
            {
                if (autoImportMedia == null)
                {
                    autoImportMedia = new MediaSpecAll();
                }
                return autoImportMedia;
            }
            set
            {
                CSSFactory.autoImportMedia = value;
            }
        }


        public static NetworkProcessor NetworkProcessor
        {
            get
            {
                if (networkProcessor == null)
                {
                    networkProcessor = new DefaultNetworkProcessor();
                }

                return networkProcessor;
            }
            set
            {
                CSSFactory.networkProcessor = value;
            }
        }


        /// <summary>
        /// Registers new CSSParserFactory instance
        /// </summary>
        /// <param name="factory">
        ///            New CSSParserFactory </param>
        public static void registerCSSParserFactory(CSSParserFactory factory)
        {
            pf = factory;
        }

        /// <summary>
        /// Returns CSSParserFactory registered in step above
        /// </summary>
        /// <returns> CSSParserFactory registered </returns>
        private static CSSParserFactory CSSParserFactory
        {
            get
            {
                if (pf == null)
                {
                    pf = CSSParserFactory.Instance;
                }
                return pf;
            }
        }

        /// <summary>
        /// Registers new TermFactory instance
        /// </summary>
        /// <param name="newFactory">
        ///            New TermFactory </param>
        public static void registerTermFactory(TermFactory newFactory)
        {
            tf = newFactory;
        }


        /// <summary>
        /// Returns TermFactory registered in step above
        /// </summary>
        /// <returns> TermFactory registered </returns>
        public static TermFactory TermFactory
        {
            get
            {
                if (tf == null)
                {
                    registerTermFactory(new TermFactoryImpl());
                    /*
                    try
                    {
                        //ORIGINAL LINE: @SuppressWarnings("unchecked") Class clazz = (Class) Class.forName(DEFAULT_TERM_FACTORY);
                        // TOCHECK: NAMESPACE ??? --> const string objectToInstantiate = "SampleProject.Domain.MyNewTestClass, MyTestProject";
                        var objectType = Type.GetType(DEFAULT_TERM_FACTORY);
                        registerTermFactory((TermFactory)Activator.CreateInstance(objectType, true));
                        // Type clazz = (Type)Type.GetType(DEFAULT_TERM_FACTORY);
                        // System.Reflection.MethodInfo m = clazz.GetMethod("getInstance");
                        // registerTermFactory((TermFactory)m.invoke(null));                        
                        // log.debug("Retrived {} as default TermFactory implementation.", DEFAULT_TERM_FACTORY);
                    }
                    catch (Exception e)
                    {
                        // log.error("Unable to get TermFactory from default", e);
                        throw new Exception("No TermFactory implementation registered!");
                    }
                    */
                }
                return tf;                
            }
        }

        /// <summary>
        /// Registers new SupportedCSS instance
        /// </summary>
        /// <param name="newCSS">
        ///            new SupportedCSS </param>
        public static void registerSupportedCSS(SupportedCSS newCSS)
        {
            css = newCSS;
        }

        /// <summary>
        /// Returns registered SupportedCSS
        /// </summary>
        /// <returns> SupportedCSS instance </returns>
        public static SupportedCSS SupportedCSS
        {
            get
            {
                if (css == null)
                {
                    registerSupportedCSS(new SupportedCSS3());
                    // css = ;
                    /*
                    try
                    {
                        //ORIGINAL LINE: @SuppressWarnings("unchecked") Class clazz = (Class) Class.forName(DEFAULT_SUPPORTED_CSS);
                        // TOCHECK
                        // Type clazz = (Type)Type.GetType(DEFAULT_SUPPORTED_CSS);
                        // System.Reflection.MethodInfo m = clazz.GetMethod("getInstance");
                        // registerSupportedCSS((SupportedCSS)m.invoke(null));
                        var objectType = Type.GetType(DEFAULT_SUPPORTED_CSS);
                        registerSupportedCSS((SupportedCSS)Activator.CreateInstance(objectType));

                        // log.debug("Retrived {} as default SupportedCSS implementation.", DEFAULT_SUPPORTED_CSS);
                    }
                    catch (Exception e)
                    {
                        // log.error("Unable to get SupportedCSS from default", e);
                        throw new Exception("No SupportedCSS implementation registered!");
                    }
                    */
                }
                return css;
            }
        }

        /// <summary>
        /// Registers new RuleFactory
        /// </summary>
        /// <param name="newRuleFactory">
        ///            New RuleFactory instance </param>
        public static void registerRuleFactory(RuleFactory newRuleFactory)
        {
            rf = newRuleFactory;
        }

        /// <summary>
        /// Returns registered RuleFactory
        /// </summary>
        /// <returns> RuleFactory instance </returns>
        public static RuleFactory RuleFactory
        {
            get
            {
                if (rf == null)
                {
                    registerRuleFactory(new RuleFactoryImpl());
                    /*
                    try
                    {
                        //ORIGINAL LINE: @SuppressWarnings("unchecked") Class clazz = (Class) Class.forName(DEFAULT_RULE_FACTORY);
                        // TOCHECK
                        // Type clazz = (Type)Type.GetType(DEFAULT_RULE_FACTORY);
                        // System.Reflection.MethodInfo m = clazz.GetMethod("getInstance");
                        // registerRuleFactory((RuleFactory)m.invoke(null));
                        var objectType = Type.GetType(DEFAULT_RULE_FACTORY);
                        registerRuleFactory((RuleFactory)Activator.CreateInstance(objectType, true));

                        // log.debug("Retrived {} as default RuleFactory implementation.", DEFAULT_RULE_FACTORY);
                    }
                    catch (Exception e)
                    {
                        // log.error("Unable to get RuleFactory from default", e);
                        throw new Exception("No RuleFactory implementation registered!");
                    }
                    */
                }

                return rf;
            }
        }

        /// <summary>
        /// Registers new DeclarationTransformer
        /// </summary>
        /// <param name="newDeclarationTransformer">
        ///            New DeclarationTransformer instance </param>
        public static void registerDeclarationTransformer(DeclarationTransformer newDeclarationTransformer)
        {
            dt = newDeclarationTransformer;
        }

        /// <summary>
        /// Returns the registered DeclarationTransformer
        /// </summary>
        /// <returns> DeclarationTransformer instance </returns>
        public static DeclarationTransformer DeclarationTransformer
        {
            get
            {
                if (dt == null)
                {                    
                    registerDeclarationTransformer(new DeclarationTransformerImpl());
                    /*
                    try
                    {
                        //ORIGINAL LINE: @SuppressWarnings("unchecked") Class clazz = (Class) Class.forName(DEFAULT_DECLARATION_TRANSFORMER);
                        // TOCHECK
                        // Type clazz = (Type)Type.GetType(DEFAULT_DECLARATION_TRANSFORMER);
                        // System.Reflection.MethodInfo m = clazz.GetMethod("getInstance");
                        // registerDeclarationTransformer((DeclarationTransformerImpl)m.invoke(null));
                        var objectType = Type.GetType(DEFAULT_DECLARATION_TRANSFORMER);
                        registerDeclarationTransformer((DeclarationTransformerImpl)Activator.CreateInstance(objectType));

                        // log.debug("Retrived {} as default DeclarationTransformer implementation.", DEFAULT_DECLARATION_TRANSFORMER);
                    }
                    catch (Exception e)
                    {
                        // log.error("Unable to get DeclarationTransformer from default", e);
                        throw new Exception("No DeclarationTransformer implementation registered!");
                    }
                    */
                }

                return dt;
            }
        }

        public static void registerElementMatcher(ElementMatcher newElementMatcher)
        {
            matcher = newElementMatcher;
        }

        public static ElementMatcher ElementMatcher
        {
            get
            {
                if (matcher == null)
                {                    
                    registerElementMatcher(new ElementMatcherSafeStd());
                    /*
                    try
                    {
                        //ORIGINAL LINE: @SuppressWarnings("unchecked") Class clazz = (Class) Class.forName(DEFAULT_ELEMENT_MATCHER);
                        // TOCHECK
                        // Type clazz = (Type)Type.GetType(DEFAULT_ELEMENT_MATCHER);
                        // registerElementMatcher(System.Activator.CreateInstance(clazz));
                        var objectType = Type.GetType(DEFAULT_ELEMENT_MATCHER);
                        registerElementMatcher((ElementMatcher)Activator.CreateInstance(objectType));
                        // log.debug("Retrived {} as default ElementMatcher implementation.", DEFAULT_ELEMENT_MATCHER);
                    }
                    catch (Exception e)
                    {
                        // log.error("Unable to get ElementMatcher from default", e);
                        throw new Exception("No ElementMatcher implementation registered!", e);
                    }
                    */
                }
                return matcher;
            }
        }

        /// <summary>
        /// Registers a new default match condition to be used for matching the elements and selector parts. </summary>
        /// <param name="newMatchCondition"> the new match condition </param>
        public static void registerDefaultMatchCondition(MatchCondition newMatchCondition)
        {
            dcond = newMatchCondition;
        }

        /// <summary>
        /// Obtains the default match condition to be used for matching the elements and selector parts. </summary>
        /// <returns> the default match condition used by this factory. </returns>
        public static MatchCondition DefaultMatchCondition
        {
            get
            {
                if (dcond == null)
                {
                    dcond = new MatchConditionImpl(); //use the default match condition when nothing is registered
                }
                return dcond;
            }
        }

        /// <summary>
        /// Registers node data instance. Instance must provide no-argument
        /// Constructor
        /// </summary>
        /// <param name="clazz">
        ///            Instance class </param>
        public static void registerNodeDataInstance(Type clazz)
        {            
            try
            {
                //ORIGINAL LINE: @SuppressWarnings("unused") NodeData test = clazz.newInstance();
                // TOCHECK
                // NodeData test = System.Activator.CreateInstance(clazz);
                NodeData test = (NodeData)Activator.CreateInstance(clazz);
                ndImpl = clazz;
            }
            catch (Exception e)
            {
                throw new Exception("NodeData implemenation (" + clazz.FullName + ") doesn't provide sole constructor", e);
            }            
        }

        /// <summary>
        /// Creates instance of NodeData
        /// </summary>
        /// <returns> Instance of NodeData </returns>
        public static NodeData createNodeData()
        {
            if (ndImpl == null)
            {
                try
                {
                    //ORIGINAL LINE: @SuppressWarnings("unchecked") Class clazz = (Class) Class.forName(DEFAULT_NODE_DATA_IMPL);
                    Type clazz = Type.GetType(DEFAULT_NODE_DATA_IMPL);
                    registerNodeDataInstance(clazz);
                    // log.debug("Registered {} as default NodeData instance.", DEFAULT_NODE_DATA_IMPL);
                }
                catch (Exception)
                {
                }
            }

            try
            {
                // TOCHECK
                // return System.Activator.CreateInstance(ndImpl);
                return (NodeData)Activator.CreateInstance(ndImpl);
            }
            catch (Exception)
            {
                throw new Exception("No NodeData implementation registered");
            }
        }

        /// <summary>
        /// Parses Uri into StyleSheet
        /// </summary>
        /// <param name="Uri">
        ///            Uri of file to be parsed </param>
        /// <param name="encoding">
        ///            Encoding of file </param>
        /// <returns> Parsed StyleSheet </returns>
        /// <exception cref="CSSException">
        ///             When exception during parse occurs </exception>
        /// <exception cref="IOException">
        ///             When file not found </exception>
        //ORIGINAL LINE: public static final StyleSheet parse(java.net.Uri Uri, String encoding) throws CSSException, java.io.IOException
        public static StyleSheet parse(Uri Uri, string encoding)
        {
            return CSSParserFactory.parse(Uri, NetworkProcessor, encoding, CSSParserFactory.SourceType.URI, Uri);
        }

        /// <summary>
        /// Parses Uri into StyleSheet
        /// </summary>
        /// <param name="Uri">
        ///            Uri of file to be parsed </param>
        /// <param name="network">
        ///            Network processor used for handling the Uri connections </param>
        /// <param name="encoding">
        ///            Encoding of file </param>
        /// <returns> Parsed StyleSheet </returns>
        /// <exception cref="CSSException">
        ///             When exception during parse occurs </exception>
        /// <exception cref="IOException">
        ///             When file not found </exception>
        //ORIGINAL LINE: public static final StyleSheet parse(java.net.Uri Uri, NetworkProcessor network, String encoding) throws CSSException, java.io.IOException
        public static StyleSheet parse(Uri Uri, NetworkProcessor network, string encoding)
        {
            return CSSParserFactory.parse(Uri, network, encoding, CSSParserFactory.SourceType.URI, Uri);
        }

        /// <summary>
        /// Parses file into StyleSheet. Internally transforms file to Uri </summary>
        /// <param name="fileName"> Name of file </param>
        /// <param name="encoding"> Encoding used to parse input </param>
        /// <returns> Parsed style sheet </returns>
        /// <exception cref="CSSException"> In case that parsing error occurs </exception>
        /// <exception cref="IOException"> If file is not found or not readable </exception>
        //ORIGINAL LINE: public static final StyleSheet parse(String fileName, String encoding) throws CSSException, java.io.IOException
        public static StyleSheet parse(string fileName, string encoding)
        {

            try
            {
                // File f = new File(fileName);
                // Uri Uri = f.toURI().toUri();
                Uri Uri = new Uri(fileName);
                return parse(Uri, encoding);
            }
            catch (Exception)
            {
                string message = "Unable to construct Uri from fileName: " + fileName;
                // log.error(message);
                throw new FileNotFoundException(message);
            }
        }

        /// <summary>
        /// Parses text into StyleSheet
        /// </summary>
        /// <param name="css">
        ///            Text with CSS declarations </param>
        /// <returns> Parsed StyleSheet </returns>
        /// <exception cref="IOException">
        ///             When exception during read occurs </exception>
        /// <exception cref="CSSException">
        ///             When exception during parse occurs
        /// @deprecated
        ///         This function does not specify the base Uri. <seealso cref="parseString(string, Uri)"/> 
        ///         should be used instead.  </exception>
        //ORIGINAL LINE: @Deprecated public static final StyleSheet parse(String css) throws java.io.IOException, CSSException
        [Obsolete]
        public static StyleSheet parse(string css)
        {
            Uri basev = new Uri("file:///base/Uri/is/not/specified"); //Cannot determine the base URI in this method but we need some base URI for relative Uris
            return CSSParserFactory.parse(css, NetworkProcessor, null, CSSParserFactory.SourceType.EMBEDDED, basev);
        }

        /// <summary>
        /// Parses text into a StyleSheet
        /// </summary>
        /// <param name="css">
        ///            Text with CSS declarations </param>
        /// <param name="base">
        ///            The Uri to be used as a base for loading external resources. Base Uri may
        ///            be {@code null} if there are no external resources in the CSS string
        ///            referenced by relative Uris. </param>
        /// <returns> Parsed StyleSheet </returns>
        /// <exception cref="IOException">
        ///             When exception during read occurs </exception>
        /// <exception cref="CSSException">
        ///             When exception during parse occurs </exception>
        //ORIGINAL LINE: public static final StyleSheet parseString(String css, java.net.Uri super) throws java.io.IOException, CSSException
        public static StyleSheet parseString(string css, Uri basev)
        {
            return parseString(css, basev, NetworkProcessor);
        }

        /// <summary>
        /// Parses text into a StyleSheet
        /// </summary>
        /// <param name="css">
        ///            Text with CSS declarations </param>
        /// <param name="base">
        ///            The Uri to be used as a base for loading external resources. Base Uri may
        ///            be {@code null} if there are no external resources in the CSS string
        ///            referenced by relative Uris. </param>
        /// <param name="network">
        ///            Network processor for retrieving the Uri resources </param>
        /// <returns> Parsed StyleSheet </returns>
        /// <exception cref="IOException">
        ///             When exception during read occurs </exception>
        /// <exception cref="CSSException">
        ///             When exception during parse occurs </exception>
        //ORIGINAL LINE: public static final StyleSheet parseString(String css, java.net.Uri super, NetworkProcessor network) throws java.io.IOException, CSSException
        public static StyleSheet parseString(string css, Uri basev, NetworkProcessor network)
        {
            Uri baseUri = basev;
            if (baseUri == null)
            {
                baseUri = new Uri("file:///base/Uri/is/not/specified"); //prevent errors if there are still some relative Uris used
            }
            return CSSParserFactory.parse(css, network, null, CSSParserFactory.SourceType.EMBEDDED, baseUri);
        }

        /// <summary>
        /// Loads all the style sheets used from the specified DOM tree.
        /// The following style specifications are evaluated:
        /// <ul>
        /// <li>The style sheets included using the <code>link</code> and <code>style</code> tags.
        /// <li>Inline styles specified using the <code>style</code> element attribute.
        /// <li><strong>Proprietary extension:</strong> Default styles defined using the <code>XDefaultStyle</code>
        ///     element attribute. These styles behave the same way as the inline styles but they have the lowest priority
        ///     (the values are used only when not redefined by any other way)
        ///  </ul>   
        /// </summary>
        /// <param name="doc">
        ///            DOM tree </param>
        /// <param name="encoding">
        ///            The default encoding used for the referenced style sheets </param>
        /// <param name="base">
        ///            Base Uri against which all files are searched </param>
        /// <param name="media">
        ///            Selected media for style sheet </param>
        /// <returns> the rules of all the style sheets used in the document including the inline styles </returns>
        public static StyleSheet getUsedStyles(IDocument doc, string encoding, Uri basev, MediaSpec media, NetworkProcessor network)
        {
            SourceData pair = new SourceData(basev, network, media);

            Traversal<StyleSheet> traversal = new CSSAssignTraversal(doc, encoding, pair, (int)FilterResult.Accept);// NodeFilter.SHOW_ELEMENT);

            StyleSheet style = (StyleSheet)RuleFactory.createStyleSheet().unlock();
            traversal.listTraversal(style);
            return style;
        }

        /// <summary>
        /// This is the same as <seealso cref="CSSFactory.getUsedStyles(IDocument, string, Uri, MediaSpec)"/> with
        /// the possibility of specifying a custom network processor.
        /// </summary>
        /// <param name="doc">
        ///            DOM tree </param>
        /// <param name="encoding">
        ///            The default encoding used for the referenced style sheets </param>
        /// <param name="base">
        ///            Base Uri against which all files are searched </param>
        /// <param name="media">
        ///            Selected media for style sheet </param>
        /// <returns> the rules of all the style sheets used in the document including the inline styles </returns>
        public static StyleSheet getUsedStyles(IDocument doc, string encoding, Uri basev, MediaSpec media)
        {
            return getUsedStyles(doc, encoding, basev, media, NetworkProcessor);
        }

        /// <summary>
        /// This is the same as <seealso cref="CSSFactory.getUsedStyles(IDocument, string, Uri, MediaSpec)"/> but only the
        /// media type is provided instead of the complete media specification.
        /// </summary>
        /// <param name="doc">
        ///            DOM tree </param>
        /// <param name="encoding">
        ///            The default encoding used for the referenced style sheets </param>
        /// <param name="base">
        ///            Base Uri against which all files are searched </param>
        /// <param name="media">
        ///            Selected media for style sheet </param>
        /// <returns> the rules of all the style sheets used in the document including the inline styles </returns>
        public static StyleSheet getUsedStyles(IDocument doc, string encoding, Uri basev, string media)
        {
            return getUsedStyles(doc, encoding, basev, new MediaSpec(media), NetworkProcessor);
        }

        [Obsolete]
        public static StyleSheet getUsedStyles(IDocument doc, Uri basev, string media)
        {
            return getUsedStyles(doc, null, basev, media);
        }

        /// <summary>
        /// Goes through a DOM tree and assigns the CSS declarations to the DOM elements.
        /// The following style specifications are evaluated:
        /// <ul>
        /// <li>The style sheets included using the <code>link</code> and <code>style</code> tags.
        /// <li>Inline styles specified using the <code>style</code> element attribute.
        /// <li><strong>Proprietary extension:</strong> Default styles defined using the <code>XDefaultStyle</code>
        ///     element attribute. These styles behave the same way as the inline styles but they have the lowest priority
        ///     (the values are used only when not redefined by any other way)
        ///  </ul>   
        /// </summary>
        /// <param name="doc">
        ///            DOM tree </param>
        /// <param name="encoding">
        ///            The default encoding used for the referenced style sheets </param>
        /// <param name="base">
        ///            Base Uri against which all files are searched </param>
        /// <param name="media">
        ///            Current media specification used for evaluating the media queries </param>
        /// <param name="useInheritance">
        ///            Whether inheritance will be used to determine values </param>
        /// <returns> Map between DOM element nodes and data structure containing CSS
        ///         information </returns>
        public static StyleMap assignDOM(IDocument doc, string encoding, Uri basev, MediaSpec media, bool useInheritance)
        {
            return assignDOM(doc, encoding, basev, media, useInheritance, null);
        }

        /// <summary>
        /// This is the same as <seealso cref="CSSFactory.assignDOM(IDocument, string, Uri, MediaSpec, bool)"/> but only the
        /// media type is provided instead of the complete media specification.
        /// </summary>
        /// <param name="doc">
        ///            DOM tree </param>
        /// <param name="encoding">
        ///            The default encoding used for the referenced style sheets </param>
        /// <param name="base">
        ///            Base Uri against which all files are searched </param>
        /// <param name="media">
        ///            Selected media type for style sheet </param>
        /// <param name="useInheritance">
        ///            Whether inheritance will be used to determine values </param>
        /// <returns> Map between DOM element nodes and data structure containing CSS
        ///         information </returns>
        public static StyleMap assignDOM(IDocument doc, string encoding, Uri basev, string media, bool useInheritance)
        {
            return assignDOM(doc, encoding, basev, new MediaSpec(media), useInheritance);
        }

        /// <summary>
        /// Goes through a DOM tree and assigns the CSS declarations to the DOM elements.
        /// The following style specifications are evaluated:
        /// <ul>
        /// <li>The style sheets included using the <code>link</code> and <code>style</code> tags.
        /// <li>Inline styles specified using the <code>style</code> element attribute.
        /// <li><strong>Proprietary extension:</strong> Default styles defined using the <code>XDefaultStyle</code>
        ///     element attribute. These styles behave the same way as the inline styles but they have the lowest priority
        ///     (the values are used only when not redefined by any other way)
        ///  </ul>
        /// </summary>
        /// <param name="doc">
        ///            DOM tree </param>
        /// <param name="encoding">
        ///            The default encoding used for the referenced style sheets </param>
        /// <param name="base">
        ///            Base Uri against which all files are searched </param>
        /// <param name="media">
        ///            Current media specification used for evaluating the media queries </param>
        /// <param name="useInheritance">
        ///            Whether inheritance will be used to determine values </param>
        /// <param name="matchCond">
        ///            The match condition to match the against. </param>
        /// <returns> Map between DOM element nodes and data structure containing CSS
        ///         information </returns>
        //ORIGINAL LINE: public static final StyleParserCS.domassign.StyleMap assignDOM(org.w3c.dom.IDocument doc, String encoding, java.net.Uri super, MediaSpec media, boolean useInheritance, final MatchCondition matchCond)
        public static StyleMap assignDOM(IDocument doc, string encoding, Uri basev, MediaSpec media, bool useInheritance, MatchCondition matchCond)
        {

            return assignDOM(doc, encoding, NetworkProcessor, basev, media, useInheritance, matchCond);
        }

        /// <summary>
        /// This is the same as <seealso cref="CSSFactory.assignDOM(IDocument, string, Uri, MediaSpec, bool)"/> 
        /// with the possibility of specifying a custom network processor for obtaining data from Uri
        /// resources.
        /// </summary>
        /// <param name="doc">
        ///            DOM tree </param>
        /// <param name="encoding">
        ///            The default encoding used for the referenced style sheets </param>
        /// <param name="network">
        ///            Custom network processor </param>
        /// <param name="base">
        ///            Base Uri against which all files are searched </param>
        /// <param name="media">
        ///            Current media specification used for evaluating the media queries </param>
        /// <param name="useInheritance">
        ///            Whether inheritance will be used to determine values </param>
        /// <param name="matchCond">
        ///            The match condition to match the against. </param>
        /// <returns> Map between DOM element nodes and data structure containing CSS
        ///         information </returns>
        //ORIGINAL LINE: public static final StyleParserCS.domassign.StyleMap assignDOM(org.w3c.dom.IDocument doc, String encoding, NetworkProcessor network, java.net.Uri super, MediaSpec media, boolean useInheritance, final MatchCondition matchCond)
        public static StyleMap assignDOM(IDocument doc, string encoding, NetworkProcessor network, Uri basev, MediaSpec media, bool useInheritance, MatchCondition matchCond)
        {

            SourceData pair = new SourceData(basev, network, media);

            Traversal<StyleSheet> traversal = new CSSAssignTraversal(doc, encoding, pair, (int)FilterResult.Accept); // NodeFilter.SHOW_ELEMENT);

            StyleSheet style = (StyleSheet)RuleFactory.createStyleSheet().unlock();
            traversal.listTraversal(style);

            Analyzer analyzer = new Analyzer(style);
            if (matchCond != null)
            {
                analyzer.registerMatchCondition(matchCond);
            }
            return analyzer.evaluateDOM(doc, media, useInheritance);
        }

        /// <summary>
        /// This is the same as <seealso cref="CSSFactory.assignDOM(IDocument, string, Uri, MediaSpec, bool)"/> but only the
        /// media type is provided instead of the complete media specification.
        /// </summary>
        /// <param name="doc">
        ///            DOM tree </param>
        /// <param name="encoding">
        ///            The default encoding used for the referenced style sheets </param>
        /// <param name="base">
        ///            Base Uri against which all files are searched </param>
        /// <param name="media">
        ///            Selected media type for style sheet </param>
        /// <param name="useInheritance">
        ///            Whether inheritance will be used to determine values </param>
        /// <param name="matchCond">
        ///            The match condition to match the against. </param>
        /// <returns> Map between DOM element nodes and data structure containing CSS
        ///         information </returns>
        //ORIGINAL LINE: public static final StyleParserCS.domassign.StyleMap assignDOM(org.w3c.dom.IDocument doc, String encoding, java.net.Uri super, String media, boolean useInheritance, final MatchCondition matchCond)
        public static StyleMap assignDOM(IDocument doc, string encoding, Uri basev, string media, bool useInheritance, MatchCondition matchCond)
        {

            return assignDOM(doc, encoding, basev, new MediaSpec(media), useInheritance, matchCond);
        }

        [Obsolete]
        public static StyleMap assignDOM(IDocument doc, Uri basev, string media, bool useInheritance)
        {
            return assignDOM(doc, null, basev, media, useInheritance);
        }

        // ========================================================================

        /// <summary>
        /// Walks (X)HTML document and collects style information
        /// 
        /// @author kapy
        /// 
        /// </summary>
        private sealed class CSSAssignTraversal : Traversal<StyleSheet>
        {

            internal static CSSParserFactory pf = CSSParserFactory;
            internal string encoding;
            internal readonly ElementMatcher matcher;

            public CSSAssignTraversal(IDocument doc, string encoding, object source, int whatToShow) : base(doc, source, whatToShow)
            {
                this.encoding = encoding;
                this.matcher = ElementMatcher;
            }

            protected internal override void processNode(StyleSheet result, INode current, object source)
            {
                // base uri
                Uri basev = ((SourceData)source).basev;
                // allowed media
                MediaSpec media = ((SourceData)source).media;
                // network processor
                NetworkProcessor network = ((SourceData)source).network;
                IElement elem = (IElement)current;

                try
                {
                    // embedded style-sheet
                    if (isEmbeddedStyleSheet(elem, media))
                    {
                        result = pf.append(extractElementText(elem), network, null, CSSParserFactory.SourceType.EMBEDDED, result, basev);
                        // log.debug("Matched embedded CSS style");
                    }
                    // linked style-sheet
                    else if (isLinkedStyleSheet(elem, media))
                    {
                        Uri uri = DataUriHandler.createUri(basev, matcher.getAttribute(elem, "href"));
                        result = pf.append(uri, network, encoding, CSSParserFactory.SourceType.URI, result, uri);
                        // log.debug("Matched linked CSS style");
                    }
                    // in-line style and default style
                    else
                    {
                        if (elem.GetAttribute("style") != null && elem.GetAttribute("style").Length > 0)
                        {
                            result = pf.append(elem.GetAttribute("style"), network, null, CSSParserFactory.SourceType.INLINE, elem, true, result, basev);
                            // log.debug("Matched inline CSS style");
                        }
                        if (elem.GetAttribute("XDefaultStyle") != null && elem.GetAttribute("XDefaultStyle").Length > 0)
                        {
                            result = pf.append(elem.GetAttribute("XDefaultStyle"), network, null, CSSParserFactory.SourceType.INLINE, elem, false, result, basev);
                            // log.debug("Matched default CSS style");
                        }
                    }
                }
                catch (CSSException ce)
                {
                    // log.error("THROWN:", ce);
                    int j = 0;
                }
                catch (IOException ioe)
                {
                    // log.error("THROWN:", ioe);
                }

            }

            internal bool isEmbeddedStyleSheet(IElement e, MediaSpec media)
            {
                return "style".Equals(e.NodeName, StringComparison.OrdinalIgnoreCase) && isAllowedMedia(e, media);
            }

            internal bool isLinkedStyleSheet(IElement e, MediaSpec media)
            {
                return e.NodeName.ToLower() == "link" && (matcher.getAttribute(e, "rel").ToLower().Contains("stylesheet")) &&
                    (matcher.getAttribute(e, "type").Length == 0 || "text/css".Equals(matcher.getAttribute(e, "type"), StringComparison.OrdinalIgnoreCase)) && isAllowedMedia(e, media);
            }

            /// <summary>
            /// Extracts element's text, if any
            /// </summary>
            /// <param name="e">
            ///            Element </param>
            /// <returns> Element's text </returns>
            internal static string extractElementText(IElement e)
            {
                INode text = e.FirstChild;
                if (text != null && text.NodeType == NodeType.Text)
                {
                    return ((IText)text).Data;
                }
                return "";
            }

            /// <summary>
            /// Checks allowed media by searching for {@code media} attribute on
            /// element and its content
            /// </summary>
            /// <param name="e">
            ///            (STYLE) Element </param>
            /// <param name="media">
            ///            Current media specification used for parsing </param>
            /// <returns> {@code true} if allowed, {@code false} otherwise </returns>
            internal static bool isAllowedMedia(IElement e, MediaSpec media)
            {
                string attr = e.GetAttribute("media");
                if (!string.ReferenceEquals(attr, null) && attr.Length > 0)
                {
                    attr = attr.Trim();
                    if (attr.Length > 0)
                    {
                        IList<MediaQuery> ql = pf.parseMediaQuery(attr);
                        if (ql != null)
                        {
                            foreach (MediaQuery q in ql)
                            {
                                if (media.matches(q))
                                {
                                    return true; //found a matching media query
                                }
                            }
                            return false; //no matching media query
                        }
                        else
                        {
                            return false; //no usable media queries (malformed string?)
                        }
                    }
                    else
                    {
                        return media.matchesEmpty(); //empty query string
                    }
                }
                else
                {
                    return media.matchesEmpty(); //no media queries
                }
            }
        }

        // holds source description containing the Uri base, network processor and the required media
        private sealed class SourceData
        {
            public Uri basev;
            public NetworkProcessor network;
            public MediaSpec media;

            public SourceData(Uri basev, NetworkProcessor network, MediaSpec media)
            {
                this.basev = basev;
                this.network = network;
                this.media = media;
            }
        }

    }

}