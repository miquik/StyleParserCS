using System;
using System.Collections.Generic;

// https://github.com/antlr/antlr4/tree/master/runtime/CSharp/src

namespace StyleParserCS.csskit.antlr4
{
    using AngleSharp.Dom;
    using Antlr4.Runtime;
    using StyleParserCS.css;
    using System.IO;
    using DataUriHandler = org.fit.net.DataUriHandler;

    /// <summary>
    /// Handles construction of parser
    /// 
    /// @author sedlakr
    /// </summary>
    public class CSSParserFactory
    {
        // private static readonly Logger log = LoggerFactory.getLogger(typeof(CSSParserFactory));

        /// <summary>
        /// Source types.
        /// http://www.w3schools.com/css/css_howto.asp
        /// </summary>
        public enum SourceType
        {
            INLINE, // example: <a style="color:red; margin:0 auto;">hyperlink</a>
            EMBEDDED, // example <style> a{ color:red;} i{font-style: italic} </style>
            URI // file example:<link rel="stylesheet" type="text/css" href="mystyle.css">
        }

        /// <summary>
        /// singleton instance
        /// </summary>
        private static CSSParserFactory instance;

        /// <summary>
        /// dummy constructor for singleton
        /// </summary>
        protected internal CSSParserFactory()
        {
        }

        /// <summary>
        /// get instance singleton method
        /// </summary>
        /// <returns> CSS Parser Factory </returns>
        public static CSSParserFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CSSParserFactory();
                }
                return instance;
            }
        }

        /// <summary>
        /// Parses source of given type
        /// </summary>
        /// <param name="source">         Source, interpretation depends on {@code type} </param>
        /// <param name="type">           Type of source provided </param>
        /// <param name="inline">         InlineElement </param>
        /// <param name="inlinePriority"> True when the rule should have an 'inline' (greater) priority </param>
        /// <returns> Created StyleSheet </returns>
        /// <exception cref="IOException">  When problem with input stream occurs </exception>
        /// <exception cref="CSSException"> When unrecoverable exception during parsing occurs </exception>
        //ORIGINAL LINE: public StyleSheet parse(Object source, NetworkProcessor network, String encoding, SourceType type, org.w3c.dom.Element inline, boolean inlinePriority, java.net.Uri super) throws java.io.IOException, CSSException
        public virtual StyleSheet parse(object source, NetworkProcessor network, string encoding, SourceType type, IElement inline, bool inlinePriority, Uri basev)
        {

            StyleSheet sheet = (StyleSheet)CSSFactory.RuleFactory.createStyleSheet().unlock();

            Preparator preparator = new SimplePreparator(inline, inlinePriority);
            return parseAndImport(source, network, encoding, type, sheet, preparator, basev, null);
        }


        /// <summary>
        /// Parses source of given type. Uses no element.
        /// </summary>
        /// <param name="source"> Source, interpretation depends on {@code type} </param>
        /// <param name="type">   Type of source provided </param>
        /// <param name="base">   The base Uri </param>
        /// <returns> Created StyleSheet </returns>
        /// <exception cref="IOException">              When problem with input stream occurs </exception>
        /// <exception cref="CSSException">             When unrecoverable exception during parsing occurs </exception>
        /// <exception cref="IllegalArgumentException"> When type of source is INLINE </exception>
        //ORIGINAL LINE: public StyleSheet parse(Object source, NetworkProcessor network, String encoding, SourceType type, java.net.Uri super) throws java.io.IOException, CSSException
        public virtual StyleSheet parse(object source, NetworkProcessor network, string encoding, SourceType type, Uri basev)
        {
            if (type == SourceType.INLINE)
            {
                throw new System.ArgumentException("Missing element for INLINE input");
            }

            return parse(source, network, encoding, type, null, false, basev);
        }

        /// <summary>
        /// Appends parsed source to passed style sheet. This style sheet must be
        /// IMPERATIVELY parsed by this factory to guarantee proper appending
        /// </summary>
        /// <param name="source">         Source, interpretation depends on {@code type} </param>
        /// <param name="type">           Type of source provided </param>
        /// <param name="inline">         Inline element </param>
        /// <param name="inlinePriority"> True when the rule should have an 'inline' (greater) priority </param>
        /// <param name="sheet">          StyleSheet to be modified </param>
        /// <returns> Modified StyleSheet </returns>
        /// <exception cref="IOException">  When problem with input stream occurs </exception>
        /// <exception cref="CSSException"> When unrecoverable exception during parsing occurs </exception>

        //ORIGINAL LINE: public StyleSheet append(Object source, NetworkProcessor network, String encoding, SourceType type, org.w3c.dom.Element inline, boolean inlinePriority, StyleSheet sheet, java.net.Uri super) throws java.io.IOException, CSSException
        public virtual StyleSheet append(object source, NetworkProcessor network, string encoding, SourceType type, IElement inline, bool inlinePriority, StyleSheet sheet, Uri basev)
        {

            Preparator preparator = new SimplePreparator(inline, inlinePriority);
            return parseAndImport(source, network, encoding, type, sheet, preparator, basev, null);
        }

        /// <summary>
        /// Appends parsed source to passed style sheet. This style sheet must be
        /// IMPERATIVELY parsed by this factory to guarantee proper appending. Uses
        /// no inline element
        /// </summary>
        /// <param name="source"> Source, interpretation depends on {@code type} </param>
        /// <param name="type">   Type of source provided </param>
        /// <param name="base">   Base Uri </param>
        /// <param name="sheet">  StyleSheet to be modified </param>
        /// <returns> Modified StyleSheet </returns>
        /// <exception cref="IOException">              When problem with input stream occurs </exception>
        /// <exception cref="CSSException">             When unrecoverable exception during parsing occurs </exception>
        /// <exception cref="IllegalArgumentException"> When type of source is INLINE </exception>
        //ORIGINAL LINE: public StyleSheet append(Object source, NetworkProcessor network, String encoding, SourceType type, StyleSheet sheet, java.net.Uri super) throws java.io.IOException, CSSException
        public virtual StyleSheet append(object source, NetworkProcessor network, string encoding, SourceType type, StyleSheet sheet, Uri basev)
        {
            if (type == SourceType.INLINE)
            {
                throw new System.ArgumentException("Missing element for INLINE input");
            }

            return append(source, network, encoding, type, null, false, sheet, basev);
        }

        /// <summary>
        /// Parses the source using the given infrastructure and returns the resulting style sheet.
        /// The imports are handled recursively.
        /// </summary>
        //ORIGINAL LINE: protected StyleSheet parseAndImport(Object source, NetworkProcessor network, String encoding, SourceType type, StyleSheet sheet, Preparator preparator, java.net.Uri super, java.util.List<MediaQuery> media) throws CSSException, java.io.IOException
        protected internal virtual StyleSheet parseAndImport(object source, NetworkProcessor network, string encoding, SourceType type, StyleSheet sheet, Preparator preparator, Uri basev, IList<MediaQuery> media)
        {
            CSSParser parser = createParser(source, network, encoding, type, basev);
            CSSParserExtractor extractor = parse(parser, type, preparator, media);

            for (int i = 0; i < extractor.ImportPaths.Count; i++)
            {
                string path = extractor.ImportPaths[i];
                IList<MediaQuery> imedia = extractor.ImportMedia[i];

                if (((imedia == null || imedia.Count == 0) && CSSFactory.AutoImportMedia.matchesEmpty()) || CSSFactory.AutoImportMedia.matchesOneOf(imedia)) //or some media query matches to the autoload media spec
                {
                    Uri Uri = DataUriHandler.createUri(basev, path);
                    try
                    {
                        parseAndImport(Uri, network, encoding, SourceType.URI, sheet, preparator, Uri, imedia);
                    }
                    catch (IOException e)
                    {
                        // log.warn("Couldn't read imported style sheet: {}", e.Message);
                    }
                }
                else
                {
                    // log.trace("Skipping import {} (media not matching)", path);
                }
            }

            return addRulesToStyleSheet(extractor.Rules, sheet);
        }

        // creates the parser
        //ORIGINAL LINE: private static CSSParser createParser(Object source, NetworkProcessor network, String encoding, SourceType type, java.net.Uri super) throws java.io.IOException, CSSException
        private static CSSParser createParser(object source, NetworkProcessor network, string encoding, SourceType type, Uri basev)
        {

            CSSInputStream input = getInput(source, network, encoding, type);
            input.Base = basev;
            return createParserForInput(input);
        }
        private static CSSParser createParserForInput(CSSInputStream input)
        {
            // error listener
            CSSErrorListener errorListener = new CSSErrorListener();
            // lexer
            CSSLexer lexer = new CSSLexer(input);
            lexer.init();
            lexer.RemoveErrorListeners();
            // TOCHECK!
            // lexer.AddErrorListener((IAntlrErrorListener<int>)errorListener);
            // IAntlrErrorListener<IToken>
            // token stream
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            // parser
            CSSParser parser = new CSSParser(tokens);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(errorListener);
            parser.ErrorHandler = new CSSErrorStrategy();
            return parser;
        }

        //ORIGINAL LINE: private static CSSParserExtractor parse(CSSParser parser, SourceType type, Preparator preparator, java.util.List<MediaQuery> media) throws CSSException
        private static CSSParserExtractor parse(CSSParser parser, SourceType type, Preparator preparator, IList<MediaQuery> media)
        {
            ParserRuleContext tree;
            CSSParserVisitorImpl visitor = new CSSParserVisitorImpl(preparator, media);
            switch (type)
            {
                case StyleParserCS.csskit.antlr4.CSSParserFactory.SourceType.INLINE:
                    tree = parser.inlinestyle();
                    visitor.visitInlinestyle((CSSParser.InlinestyleContext)tree);
                    break;
                case StyleParserCS.csskit.antlr4.CSSParserFactory.SourceType.EMBEDDED:
                    tree = parser.stylesheet();
                    visitor.visitStylesheet((CSSParser.StylesheetContext)tree);
                    break;
                case StyleParserCS.csskit.antlr4.CSSParserFactory.SourceType.URI:
                    tree = parser.stylesheet();
                    visitor.visitStylesheet((CSSParser.StylesheetContext)tree);
                    break;
                default:
                    throw new Exception("Coding error");
            }
            return visitor;
        }


        /// <summary>
        /// Parses a media query from a string (e.g. the 'media' HTML attribute).
        /// </summary>
        /// <param name="query"> The query string </param>
        /// <returns> List of media queries found. </returns>
        public virtual IList<MediaQuery> parseMediaQuery(string query)
        {
            try
            {
                // input from string
                CSSInputStream input = CSSInputStream.stringStream(query);
                input.Base = new Uri("file://media/query/Uri"); //this Uri should not be used, just for safety
                                                                // create parser
                CSSParser parser = createParserForInput(input);
                // visitor
                CSSParserVisitorImpl visitor = new CSSParserVisitorImpl();
                return visitor.visitMedia(parser.media());
            }
            catch (IOException e)
            {
                // log.error("I/O error during media query parsing: {}", e.Message);
                return null;
            } /* catch (CSSException e) {
	            log.warn("Malformed media query {}", query);
	            return null;
	        } */
            catch (RecognitionException)
            {
                // log.warn("Malformed media query {}", query);
                return null;
            }
        }


        /// <summary>
        /// Creates new CSSException which encapsulates cause
        /// </summary>
        /// <param name="t">   Cause </param>
        /// <param name="msg"> Message </param>
        /// <returns> New CSSException </returns>
        protected internal static CSSException encapsulateException(Exception t, string msg)
        {
            // log.error("THROWN:", t);
            return new CSSException(msg, t);
        }

        protected internal static StyleSheet addRulesToStyleSheet(RuleList rules, StyleSheet sheet)
        {
            if (rules != null)
            {
                //ORIGINAL LINE: for (RuleBlock<?> rule : rules)
                foreach (RuleBlock rule in rules)
                {
                    sheet.Add(rule);
                }
            }
            return sheet;
        }

        //ORIGINAL LINE: protected static CSSInputStream getInput(Object source, NetworkProcessor network, String encoding, SourceType type) throws java.io.IOException
        protected internal static CSSInputStream getInput(object source, NetworkProcessor network, string encoding, SourceType type)
        {
            switch (type)
            {
                case StyleParserCS.csskit.antlr4.CSSParserFactory.SourceType.INLINE:
                case StyleParserCS.csskit.antlr4.CSSParserFactory.SourceType.EMBEDDED:
                    return CSSInputStream.stringStream((string)source);
                case StyleParserCS.csskit.antlr4.CSSParserFactory.SourceType.URI:
                    return CSSInputStream.UriStream((Uri)source, network, encoding);
                default:
                    throw new Exception("Coding error");
            }
        }
    }
}