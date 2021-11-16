using System;
using System.Collections.Generic;

namespace StyleParserCS.csskit.antlr4
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using StyleParserCS.css;


    public class CSSParserVisitorImpl : CSSParserExtractor // CSSParserVisitor<object>, CSSParserExtractor
    {
        // factories for building structures
        private RuleFactory rf = CSSFactory.RuleFactory;
        private TermFactory tf = CSSFactory.TermFactory;

        public enum MediaQueryState
        {
            START,
            TYPE,
            AND,
            EXPR,
            TYPEOREXPR

            //logger
        }
        // private org.slf4j.Logger log = org.slf4j.LoggerFactory.getLogger(this.GetType());
        //counter of spaces for pretty debug printing
        private int spacesCounter = 0;

        // block preparator
        private Preparator preparator;

        // list of media queries to wrap rules
        private IList<MediaQuery> wrapMedia;

        // structures after parsing
        private IList<string> importPaths = new List<string>();
        private IList<IList<MediaQuery>> importMedia = new List<IList<MediaQuery>>();
        private RuleList rules = null;
        private IList<MediaQuery> mediaQueryList = null;

        //prevent imports inside the style sheet
        private bool preventImports = false;

        private void logEnter(string entry)
        {
            /*
            // if  (// log.TraceEnabled)
            {
                // log.trace("Enter: {}{}", generateSpaces(spacesCounter), entry);
            }
            */
        }

        private void logEnter(string entry, RuleContext ctx)
        {
            /*
            // if  (// log.TraceEnabled)
            {
                // log.trace("Enter: {}{}: >{}<", generateSpaces(spacesCounter), entry, ctx.GetText());
            }
            */
        }

        private void logLeave(string leaving)
        {
            /*
            // if  (// log.TraceEnabled)
            {
                // log.trace("Leave: {}{}", generateSpaces(spacesCounter), leaving);
            }
            */
        }

        private string extractTextUnescaped(string text)
        {
            return StyleParserCS.unbescape.CssEscape.unescapeCss(text);
        }

        private Declaration_Source extractSource(CSSToken ct)
        {
            // IToken
            return new Declaration_Source(ct.Base, ct.Line, ct.CharPositionInLine);
        }

        /// <summary>
        /// extract base from parse tree node
        /// </summary>
        private Uri extractBase(ITerminalNode node)
        {
            CSSToken ct = (CSSToken)node.Symbol;
            return ct.Base;
        }

        /// <summary>
        /// check if string is valid ID
        /// </summary>
        /// <param name="id"> ID to validate and unescapes </param>
        /// <returns> unescaped id or null </returns>
        private string extractIdUnescaped(string id)
        {
            if (id.Length > 0 && !char.IsDigit(id[0]))
            {
                return StyleParserCS.unbescape.CssEscape.unescapeCss(id);
            }
            return null;
        }

        /// <summary>
        /// generate spaces for pretty debug printing
        /// </summary>
        /// <param name="count"> number of generated spaces </param>
        /// <returns> string with spaces </returns>
        private string generateSpaces(int count)
        {
            string spaces = "";
            for (int i = 0; i < count; i++)
            {
                spaces += " ";
            }
            return spaces;
        }

        /// <summary>
        /// remove terminal node emtpy tokens from input list
        /// </summary>
        /// <param name="inputArrayList"> original list </param>
        /// <returns> list without terminal node type = S (space) </returns>
        private IList<IParseTree> filterSpaceTokens(IList<IParseTree> inputArrayList)
        {
            IList<IParseTree> ret = new List<IParseTree>(inputArrayList.Count);
            foreach (IParseTree item in inputArrayList)
            {
                if (!(item is ITerminalNode) || ((TerminalNodeImpl)item).Symbol.Type != CSSLexer.S)
                {
                    ret.Add(item);
                }
            }
            return ret;
        }

        /// <summary>
        /// check if rule context contains error node
        /// </summary>
        /// <param name="ctx"> rule context </param>
        /// <returns> contains context error node </returns>
        private bool ctxHasErrorNode(ParserRuleContext ctx)
        {
            for (int i = 0; i < ctx.children.Count; i++)
            {
                if (ctx.GetChild(i) is IErrorNode)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Tries to convert generic terms to more specific value types. Currently, colors (TermColor) and
        /// rectangles (TermRect) are supported. </summary>
        /// <param name="term"> the term to be converted </param>
        /// <returns> the corresponding more specific term type or {@code null} when nothing was found. </returns>
        //ORIGINAL LINE: private Term<?> findSpecificType(Term<?> term)
        private Term findSpecificType(Term term)
        {
            TermColor colorTerm = null;
            TermRect rectTerm = null;
            if (term is TermIdent)
            { //idents - try to convert colors
                colorTerm = tf.createColor((TermIdent)term);
            }
            else if (term is TermFunction)
            { // rgba(0,0,0)
                colorTerm = tf.createColor((TermFunction)term);
                if (colorTerm == null)
                {
                    rectTerm = tf.createRect((TermFunction)term);
                }
            }
            //replace with more specific value
            if (colorTerm != null)
            {
                // if (// log.DebugEnabled)
                {
                    // log.debug("term color is OK - creating - " + colorTerm.ToString());
                }

                // Term<Color> --> Term
                // TOCHECK
                return (Term)colorTerm;
            }
            else if (rectTerm != null)
            {
                // if (// log.DebugEnabled)
                {
                    // log.debug("term rect is OK - creating - " + rectTerm.ToString());
                }
                return (Term)rectTerm;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// get parsed rulelist
        /// </summary>
        /// <returns> parsed rules </returns>
        public virtual RuleList Rules
        {
            get
            {
                return rules;
            }
        }

        /// <summary>
        /// get mediaquery list
        /// </summary>
        /// <returns> media query list </returns>
        public virtual IList<MediaQuery> Media
        {
            get
            {
                return mediaQueryList;
            }
        }

        /// <summary>
        /// get import list
        /// </summary>
        /// <returns> list of Uris to import </returns>
        public virtual IList<string> ImportPaths
        {
            get
            {
                return importPaths;
            }
        }

        /// <summary>
        /// get media for imports
        /// </summary>
        /// <returns> list of media for imports </returns>
        public virtual IList<IList<MediaQuery>> ImportMedia
        {
            get
            {
                return importMedia;
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="preparator"> The preparator to be used for creating the rules. </param>
        /// <param name="wrapMedia">  The media queries to be used for wrapping the created rules (e.g. in case
        ///                   of parsing and imported style sheet) or null when no wrapping is required. </param>
        public CSSParserVisitorImpl(Preparator preparator, IList<MediaQuery> wrapMedia)
        {
            this.preparator = preparator;
            this.wrapMedia = wrapMedia;
        }

        //used in parseMediaQuery
        public CSSParserVisitorImpl()
        {

        }
        /// <summary>
        ///****************************************************************
        /// /******************************************************************
        /// /******************************************************************
        /// /******************************************************************
        /// /******************************************************************
        /// /******************************************************************
        /// /******************************************************************
        /// /******************************************************************
        /// /*****************************************************************
        /// </summary>


        /// <param name="ctx"> the parse tree </param>
        /// <returns> RuleList
        /// inlinestyle: S* (declarations | inlineset+ ) </returns>
        public RuleList visitInlinestyle(CSSParser.InlinestyleContext ctx)
        {
            logEnter("inlinestyle");
            this.rules = new RuleArrayList();
            if (ctx.declarations() != null)
            {
                //declarations
                IList<Declaration> decl = visitDeclarations(ctx.declarations());
                //ORIGINAL LINE: StyleParserCS.css.RuleBlock<?> rb = preparator.prepareInlineRuleSet(decl, null);
                StyleParserCS.css.RuleBlock rb = preparator.prepareInlineRuleSet(decl, null);
                if (rb != null)
                {
                    //rb is valid,Add to rules
                    this.rules.Add(rb);
                }
            }
            else
            {
                //inlineset
                foreach (CSSParser.InlinesetContext ctxis in ctx.inlineset())
                {
                    //ORIGINAL LINE: StyleParserCS.css.RuleBlock<?> irs = visitInlineset(ctxis);
                    StyleParserCS.css.RuleBlock irs = visitInlineset(ctxis);
                    if (irs != null)
                    {
                        //irs is valid, Add to rules
                        this.rules.Add(irs);
                    }
                }
            }
            // if (// log.DebugEnabled)
            {
                // log.debug("\n***\n{}\n***\n", this.rules);
            }
            logLeave("inlinestyle");
            return this.rules;
        }

        /// <summary>
        /// Stylesheet, main rule
        /// stylesheet: ( CDO | CDC  | S | nostatement | statement )*
        /// statement* is only processed
        /// </summary>
        public RuleList visitStylesheet(CSSParser.StylesheetContext ctx)
        {
            logEnter("stylesheet: ", ctx);
            this.rules = new RuleArrayList();
            //statement*
            foreach (CSSParser.StatementContext stmt in ctx.statement())
            {
                //ORIGINAL LINE: RuleBlock<?> s = visitStatement(stmt);
                RuleBlock s = visitStatement(stmt);
                if (s != null)
                {
                    //Add statement to rules
                    this.rules.Add(s);
                }
            }
            // if (// log.DebugEnabled)
            {
                // log.debug("\n***\n{}\n***\n", this.rules);
            }
            logLeave("stylesheet");
            return this.rules;
        }

        /// <summary>
        /// scope and stack for statement
        /// - this is for accessing statement scope from inner rules
        /// e.g. - used for invalidate statement from selector
        /// </summary>
        protected internal class statement_scope
        {
            internal bool invalid = false;
        }

        /// <summary>
        /// stack for posibly recursion
        /// </summary>
        protected internal Stack<statement_scope> statement_stack = new Stack<statement_scope>();

        //ORIGINAL LINE: @Override public RuleBlock<?> visitStatement(CSSParser.StatementContext ctx)
        public RuleBlock visitStatement(CSSParser.StatementContext ctx)
        {
            if (ctxHasErrorNode(ctx))
            {
                //context is invalid
                return null;
            }
            logEnter("statement: ", ctx);
            //create new scope and push it to stack
            statement_stack.Push(new statement_scope());
            //ORIGINAL LINE: RuleBlock<?> stmt = null;
            RuleBlock stmt = null;
            if (ctx.ruleset() != null)
            {
                //ruleset
                stmt = visitRuleset(ctx.ruleset());
            }
            else if (ctx.atstatement() != null)
            {
                //atstatement
                stmt = visitAtstatement(ctx.atstatement());
            }
            if (statement_stack.Peek().invalid)
            {
                //stmt == null - is invalid
                // if (// log.DebugEnabled)
                {
                    // log.debug("Statement is invalid");
                }
            }
            statement_stack.Pop();
            logLeave("statement");
            //could be null
            return stmt;
        }

        //ORIGINAL LINE: @Override public RuleBlock<?> visitAtstatement(CSSParser.AtstatementContext ctx)
        public RuleBlock visitAtstatement(CSSParser.AtstatementContext ctx)
        {
            logEnter("atstatement: ", ctx);
            //ORIGINAL LINE: RuleBlock<?> atstmt = null;
            RuleBlock atstmt = null;
            //noinspection StatementWithEmptyBody
            if (ctx.CHARSET() != null)
            {
                //charset is served in lexer
            }
            //import
            else if (ctx.IMPORT() != null)
            {
                IList<StyleParserCS.css.MediaQuery> im = null;
                if (ctx.media() != null)
                {
                    im = visitMedia(ctx.media());
                }
                ctx.import_uri();
                string iuri = visitImport_uri(ctx.import_uri());
                if (!this.preventImports && !string.ReferenceEquals(iuri, null))
                {
                    //// if  (// log.DebugEnabled)
                    {
                        // log.debug("Adding import: {}", iuri);
                    }
                    importMedia.Add(im);
                    importPaths.Add(iuri);
                }
                else
                {
                    //// if  (// log.DebugEnabled)
                    {
                        // log.debug("Ignoring import: {}", iuri);
                    }
                }
            }
            //page
            else if (ctx.page() != null)
            {

                atstmt = visitPage(ctx.page());
            }
            //viewport
            else if (ctx.VIEWPORT() != null)
            {

                IList<StyleParserCS.css.Declaration> declarations = visitDeclarations(ctx.declarations());
                atstmt = preparator.prepareRuleViewport(declarations);
                if (atstmt != null)
                {
                    this.preventImports = true;
                }
            }
            //fontface
            else if (ctx.FONTFACE() != null)
            {

                IList<StyleParserCS.css.Declaration> declarations = visitDeclarations(ctx.declarations());
                atstmt = preparator.prepareRuleFontFace(declarations);
                if (atstmt != null)
                {
                    this.preventImports = true;
                }
            }
            //media
            else if (ctx.MEDIA() != null)
            {

                IList<StyleParserCS.css.MediaQuery> mediaList = null;
                IList<StyleParserCS.css.RuleSet> rules = null;
                if (ctx.media() != null)
                {
                    mediaList = visitMedia(ctx.media());
                }
                if (ctx.media_rule() != null)
                {
                    rules = new List<StyleParserCS.css.RuleSet>();
                    foreach (CSSParser.Media_ruleContext mr in ctx.media_rule())
                    {
                        //ORIGINAL LINE: RuleBlock<?> rs = visitMedia_rule(mr);
                        RuleBlock rs = visitMedia_rule(mr);
                        if (rs != null)
                        {
                            rules.Add((RuleSet)rs);
                        }
                    }
                }
                atstmt = preparator.prepareRuleMedia(rules, mediaList);
                if (atstmt != null)
                {
                    this.preventImports = true;
                }
            }
            //keyframes
            else if (ctx.KEYFRAMES() != null)
            {
                string name = null;
                IList<StyleParserCS.css.KeyframeBlock> keyframes = null;
                if (ctx.keyframes_name() != null)
                {
                    name = visitKeyframes_name(ctx.keyframes_name());
                }
                if (ctx.keyframe_block() != null)
                {
                    keyframes = new List<StyleParserCS.css.KeyframeBlock>();
                    foreach (CSSParser.Keyframe_blockContext kfctx in ctx.keyframe_block())
                    {
                        KeyframeBlock block = visitKeyframe_block(kfctx);
                        if (block != null)
                        {
                            keyframes.Add(block);
                        }
                    }
                }
                atstmt = preparator.prepareRuleKeyframes(keyframes, name);
                if (atstmt != null)
                {
                    this.preventImports = true;
                }
            }
            //unknown
            else
            {
                // if (// log.DebugEnabled)
                {
                    // log.debug("Skipping invalid at statement");
                }
            }
            logLeave("atstatement");
            return atstmt;
        }

        public string visitImport_uri(CSSParser.Import_uriContext ctx)
        {
            if (ctx != null)
            {
                return extractTextUnescaped(ctx.GetText());
            }
            else
            {
                return null;
            }
        }

        //ORIGINAL LINE: @Override public RuleBlock<?> visitPage(CSSParser.PageContext ctx)
        public RuleBlock visitPage(CSSParser.PageContext ctx)
        {
            bool invalid = false;
            string name = null;
            if (ctx.IDENT() != null)
            {
                name = extractTextUnescaped(ctx.IDENT().GetText());
            }
            Selector_PseudoPage pseudo = null;
            if (ctx.pseudo() != null)
            {
                Selector_SelectorPart p = visitPseudo(ctx.pseudo());
                if (p != null && p is Selector_PseudoPage)
                {
                    pseudo = (Selector_PseudoPage)p;
                }
                else
                { // Invalid pseudo
                    //if (// log.DebugEnabled)
                    {
                        // log.debug("skipping RulePage with invalid pseudo-class: " + pseudo);
                    }
                    invalid = true;
                }
            }
            IList<Declaration> declarations = visitDeclarations(ctx.declarations());
            IList<StyleParserCS.css.RuleMargin> margins = null;
            if (ctx.margin_rule() != null)
            {
                margins = new List<StyleParserCS.css.RuleMargin>();
                foreach (CSSParser.Margin_ruleContext mctx in ctx.margin_rule())
                {
                    RuleMargin m = visitMargin_rule(mctx);
                    margins.Add(m);
                    // if (// log.DebugEnabled)
                    {
                        // log.debug("Inserted margin rule #{} into @page", margins.Count + 1);
                    }
                }
            }

            if (invalid)
            {
                return null;
            }
            else
            {
                //ORIGINAL LINE: RuleBlock<?> rb = preparator.prepareRulePage(declarations, margins, name, pseudo);
                RuleBlock rb = preparator.prepareRulePage(declarations, margins, name, pseudo);
                if (rb != null)
                {
                    this.preventImports = true;
                }
                return rb;
            }
        }

        public RuleMargin visitMargin_rule(CSSParser.Margin_ruleContext ctx)
        {
            logEnter("margin_rule");
            RuleMargin m;
            string area = ctx.MARGIN_AREA().GetText();
            IList<Declaration> decl = visitDeclarations(ctx.declarations());
            m = preparator.prepareRuleMargin(extractTextUnescaped(area).Substring(1), decl);
            logLeave("margin_rule");
            return m;
        }

        //ORIGINAL LINE: @Override public StyleParserCS.css.RuleBlock<?> visitInlineset(CSSParser.InlinesetContext ctx)
        public StyleParserCS.css.RuleBlock visitInlineset(CSSParser.InlinesetContext ctx)
        {
            logEnter("inlineset");
            IList<StyleParserCS.css.Selector_SelectorPart> pplist = new List<StyleParserCS.css.Selector_SelectorPart>();
            if (ctx.pseudo() != null)
            {
                foreach (CSSParser.PseudoContext pctx in ctx.pseudo())
                {
                    Selector_SelectorPart p = visitPseudo(pctx);
                    pplist.Add(p);
                }
            }
            IList<Declaration> decl = visitDeclarations(ctx.declarations());
            //ORIGINAL LINE: RuleBlock<?> is = preparator.prepareInlineRuleSet(decl, pplist);
            RuleBlock isv = preparator.prepareInlineRuleSet(decl, pplist);
            logLeave("inlineset");
            return isv;
        }

        public IList<StyleParserCS.css.MediaQuery> visitMedia(CSSParser.MediaContext ctx)
        {
            logEnter("media: ", ctx);
            IList<MediaQuery> queries = mediaQueryList = new List<MediaQuery>();
            foreach (CSSParser.Media_queryContext mqc in ctx.media_query())
            {
                queries.Add(visitMedia_query(mqc));
            }
            // if  (// log.DebugEnabled)
            {
                // log.debug("Totally returned {} media queries.", queries.Count);
            }
            logLeave("media");
            return queries;
        }

        protected internal class mediaquery_scope
        {
            internal StyleParserCS.css.MediaQuery q;
            internal MediaQueryState state;
            internal bool invalid;
        }

        internal mediaquery_scope mq;

        public MediaQuery visitMedia_query(CSSParser.Media_queryContext ctx)
        {
            logEnter("mediaquery: ", ctx);
            mq = new mediaquery_scope();
            mq.q = rf.createMediaQuery();
            mq.q.unlock();
            mq.state = MediaQueryState.START;
            mq.invalid = false;
            logLeave("mediaquery");
            foreach (CSSParser.Media_termContext mtc in ctx.media_term())
            {
                visitMedia_term(mtc);
            }
            if (mq.invalid)
            {
                // log.trace("Skipping invalid rule {}", mq.q);
                mq.q.Type = "all"; //change the malformed media queries to "not all"
                mq.q.Negative = true;
            }
            logLeave("mediaquery");
            return mq.q;
        }

        public object visitMedia_term(CSSParser.Media_termContext ctx)
        {
            //IDENT
            if (ctx.IDENT() != null)
            {
                string m = extractTextUnescaped(ctx.IDENT().GetText());
                MediaQueryState state = mq.state;
                if (m.Equals("ONLY", StringComparison.OrdinalIgnoreCase) && state == MediaQueryState.START)
                {
                    mq.state = MediaQueryState.TYPEOREXPR;
                }
                else if (m.Equals("NOT", StringComparison.OrdinalIgnoreCase) && state == MediaQueryState.START)
                {
                    mq.q.Negative = true;
                    mq.state = MediaQueryState.TYPEOREXPR;
                }
                else if (m.Equals("AND", StringComparison.OrdinalIgnoreCase) && state == MediaQueryState.AND)
                {
                    mq.state = MediaQueryState.EXPR;
                }
                else if (state == MediaQueryState.START || state == MediaQueryState.TYPE || state == MediaQueryState.TYPEOREXPR)
                {
                    mq.q.Type = m;
                    mq.state = MediaQueryState.AND;
                }
                else
                {
                    // log.trace("Invalid media query: found ident: {} state: {}", m, state);
                    mq.invalid = true;
                }
            }
            //media_expression
            else if (ctx.media_expression() != null)
            {
                MediaExpression e = visitMedia_expression(ctx.media_expression());
                if (mq.state == MediaQueryState.START || mq.state == MediaQueryState.EXPR || mq.state == MediaQueryState.TYPEOREXPR)
                {
                    if (e != null && !string.ReferenceEquals(e.Feature, null)) //the expression is valid
                    {
                        mq.q.Add(e);
                        mq.state = MediaQueryState.AND;
                    }
                    else
                    {
                        // log.trace("Invalidating media query for invalud expression");
                        mq.invalid = true;
                    }
                }
                else
                {
                    // log.trace("Invalid media query: found expr, state: {}", mq.state);
                    mq.invalid = true;
                }
            }
            //nomediaquery
            else
            {
                mq.invalid = true;
            }
            return null;
        }

        public MediaExpression visitMedia_expression(CSSParser.Media_expressionContext ctx)
        {
            logEnter("mediaexpression: ", ctx);
            if (ctxHasErrorNode(ctx))
            {
                mq.invalid = true;
                return null;
            }
            MediaExpression expr = rf.createMediaExpression();
            Declaration decl;
            declaration_stack.Push(new declaration_scope());
            declaration_stack.Peek().d = decl = rf.createDeclaration();
            declaration_stack.Peek().invalid = false;

            string property = extractTextUnescaped(ctx.IDENT().GetText());
            decl.Property = property;
            // IToken token = ctx.IDENT().Symbol;
            // Unable to cast object of type 'Antlr4.Runtime.CommonToken' to type 'StyleParserCS.csskit.antlr4.CSSToken'.
            CommonToken  token = (CommonToken)ctx.IDENT().Symbol;
            decl.Source = extractSource((CSSToken)token);
            if (ctx.terms() != null)
            {
                //ORIGINAL LINE: java.util.List<Term<?>> t = visitTerms(ctx.terms());
                IList<Term> t = visitTerms(ctx.terms());
                decl.replaceAll(t);
            }

            if (declaration_stack.Peek().d != null && !declaration_stack.Peek().invalid)
            { //if the declaration is valid
                expr.Feature = decl.Property;
                expr.replaceAll(decl);
            }
            declaration_stack.Pop();

            logLeave("mediaexpression");
            return expr;
        }

        //ORIGINAL LINE: @Override public RuleBlock<?> visitMedia_rule(CSSParser.Media_ruleContext ctx)
        public RuleBlock visitMedia_rule(CSSParser.Media_ruleContext ctx)
        {
            logEnter("media_rule: ", ctx);
            //ORIGINAL LINE: RuleBlock<?> rules = null;
            RuleBlock rules = null;
            if (ctx.ruleset() != null)
            {
                statement_stack.Push(new statement_scope());
                rules = visitRuleset(ctx.ruleset());
                statement_stack.Pop();
            }
            else
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("Skiping invalid statement in media");
                }
            }
            logLeave("media_rule");
            //could be null
            return rules;
        }

        public string visitKeyframes_name(CSSParser.Keyframes_nameContext ctx)
        {
            if (ctx.IDENT() != null)
            {
                return extractTextUnescaped(ctx.IDENT().GetText());
            }
            else if (ctx.stringv() != null)
            {
                return visitString(ctx.stringv());
            }
            else
            {
                return null;
            }
        }

        public KeyframeBlock visitKeyframe_block(CSSParser.Keyframe_blockContext ctx)
        {
            IList<TermPercent> selectors = null;
            if (ctx.keyframe_selector() != null)
            {
                selectors = new List<TermPercent>();
                foreach (var selctx in ctx.keyframe_selector())
                {
                    TermPercent perc = visitKeyframe_selector(selctx);
                    if (perc != null)
                    {
                        selectors.Add(perc);
                    }
                }
            }

            IList<Declaration> declarations = null;
            if (ctx.declarations() != null)
            {
                statement_stack.Push(new statement_scope());
                declarations = visitDeclarations(ctx.declarations());
                statement_stack.Pop();
            }

            if (declarations != null && selectors != null && selectors.Count > 0)
            {
                KeyframeBlock block = rf.createKeyframeBlock();
                block.setPercentages(selectors);
                block.replaceAll(declarations);
                return block;
            }
            else
            {
                return null;
            }
        }

        public TermPercent visitKeyframe_selector(CSSParser.Keyframe_selectorContext ctx)
        {
            if (ctx.IDENT() != null)
            {
                //ORIGINAL LINE: final String idtext = ctx.IDENT().getText();
                string idtext = ctx.IDENT().GetText();
                if (!string.ReferenceEquals(idtext, null))
                {
                    if (idtext.Equals("from", StringComparison.OrdinalIgnoreCase))
                    {
                        return tf.createPercent(0.0f);
                    }
                    else if (idtext.Equals("to", StringComparison.OrdinalIgnoreCase))
                    {
                        return tf.createPercent(100.0f);
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
            else if (ctx.PERCENTAGE() != null)
            {
                return tf.createPercent(ctx.PERCENTAGE().GetText(), 1);
            }
            else
            {
                return null;
            }
        }

        public object visitUnknown_atrule(CSSParser.Unknown_atruleContext ctx)
        {
            //done in atstatement else section
            return null;
        }

        public object visitUnknown_atrule_body(CSSParser.Unknown_atrule_bodyContext ctx)
        {
            //not used - the unknown atrules are skipped
            return null;
        }

        //ORIGINAL LINE: @Override public RuleBlock<?> visitRuleset(CSSParser.RulesetContext ctx)
        public RuleBlock visitRuleset(CSSParser.RulesetContext ctx)
        {
            logEnter("ruleset");
            if (ctxHasErrorNode(ctx) || ctx.norule() != null)
            {
                // log.trace("Leaving ruleset with error {} {}", ctxHasErrorNode(ctx), (ctx.norule() != null));
                return null;
            }
            IList<CombinedSelector> cslist = new List<CombinedSelector>();
            // body
            foreach (CSSParser.Combined_selectorContext csctx in ctx.combined_selector())
            {
                CombinedSelector cs = visitCombined_selector(csctx);
                if (cs != null && cs.Count > 0 && !statement_stack.Peek().invalid)
                {
                    cslist.Add(cs);
                    // if (// log.DebugEnabled)
                    {
                        // log.debug("Inserted combined selector ({}) into ruleset", cslist.Count);
                    }
                }
            }
            IList<StyleParserCS.css.Declaration> decl = visitDeclarations(ctx.declarations());
            //ORIGINAL LINE: RuleBlock<?> stmnt;
            RuleBlock stmnt;
            if (statement_stack.Peek().invalid)
            {
                stmnt = null;
                // if  (// log.DebugEnabled)
                {
                    // log.debug("Ruleset not valid, so not created");
                }
            }
            else
            {
                stmnt = preparator.prepareRuleSet(cslist, decl, (this.wrapMedia != null && this.wrapMedia.Count > 0), this.wrapMedia);
                this.preventImports = true;
            }
            logLeave("ruleset");
            return stmnt;
        }

        public IList<Declaration> visitDeclarations(CSSParser.DeclarationsContext ctx)
        {
            logEnter("declarations");
            IList<Declaration> decl = new List<Declaration>();
            if (ctx != null && ctx.declaration() != null)
            {
                foreach (CSSParser.DeclarationContext declctx in ctx.declaration())
                {
                    Declaration d = visitDeclaration(declctx);
                    if (d != null)
                    {
                        decl.Add(d);
                        // if (// log.DebugEnabled)
                        {
                            // log.debug("Inserted declaration #{} ", decl.Count + 1);
                        }
                    }
                    else
                    {
                        // if (// log.DebugEnabled)
                        {
                            // log.debug("Null declaration was omitted");
                        }
                    }
                }
            }
            logLeave("declarations");
            return decl;
        }

        protected internal class declaration_scope
        {
            internal StyleParserCS.css.Declaration d;
            internal bool invalid;
        }

        protected internal Stack<declaration_scope> declaration_stack = new Stack<declaration_scope>();

        public Declaration visitDeclaration(CSSParser.DeclarationContext ctx)
        {
            logEnter("declaration");
            Declaration decl;
            declaration_stack.Push(new declaration_scope());
            declaration_stack.Peek().d = decl = rf.createDeclaration();
            declaration_stack.Peek().invalid = false;

            if (ctx.noprop() == null && !ctxHasErrorNode(ctx))
            {
                if (ctx.important() != null)
                {
                    visitImportant(ctx.important());
                }
                visitProperty(ctx.property());
                if (ctx.terms() != null)
                {
                    //ORIGINAL LINE: java.util.List<Term<?>> t = visitTerms(ctx.terms());
                    IList<Term> t = visitTerms(ctx.terms());
                    decl.replaceAll(t);
                }
            }
            else
            {
                // if (// log.DebugEnabled)
                {
                    // log.debug("invalidating declaration");
                }
                declaration_stack.Peek().invalid = true;
            }

            if (declaration_stack.Peek().invalid || declaration_stack.Count == 0)
            {
                decl = null;
                // if (// log.DebugEnabled)
                {
                    // log.debug("Declaration was invalidated or already invalid");
                }
            }
            else
            {
                // if (// log.DebugEnabled)
                {
                    // log.debug("Returning declaration: {}.", decl);
                }
            }
            logLeave("declaration");
            declaration_stack.Pop();

            return decl;
        }

        public object visitImportant(CSSParser.ImportantContext ctx)
        {
            if (ctxHasErrorNode(ctx))
            {
                declaration_stack.Peek().invalid = true;
            }
            else
            {
                declaration_stack.Peek().d.Important = true;
                // if (// log.DebugEnabled)
                {
                    // log.debug("IMPORTANT");
                }
            }
            //returns null
            return null;
        }

        public object visitProperty(CSSParser.PropertyContext ctx)
        {
            logEnter("property");

            var ctxi = ctx.IDENT();
            var tp = ctxi.Symbol.GetType();
            string property = extractTextUnescaped(ctxi.GetText());
            if (ctx.MINUS() != null)
            {
                property = ctx.MINUS().GetText() + property;
            }
            declaration_stack.Peek().d.Property = property;
            // IToken token = ctx.IDENT().Symbol;
            CommonToken token = (CommonToken)ctxi.Symbol;
            declaration_stack.Peek().d.Source = extractSource((CSSToken)token);

            // if  (// log.DebugEnabled)
            {
                // log.debug("Setting property: {}", declaration_stack.Peek().d.Property);
            }
            logLeave("property");
            //returns null
            return null;
        }


        protected internal class terms_scope
        {
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> list;
            internal IList<StyleParserCS.css.Term> list;
            //ORIGINAL LINE: StyleParserCS.css.Term<?> term;
            internal StyleParserCS.css.Term term;
            internal StyleParserCS.css.Term_Operator op;
            internal int unary;
            internal bool dash;
        }

        protected internal Stack<terms_scope> terms_stack = new Stack<terms_scope>();

        //ORIGINAL LINE: @Override public java.util.List<Term<?>> visitTerms(CSSParser.TermsContext ctx)
        public IList<Term> visitTerms(CSSParser.TermsContext ctx)
        {
            terms_stack.Push(new terms_scope());
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> tlist;
            IList<StyleParserCS.css.Term> tlist;
            logEnter("terms");
            //ORIGINAL LINE: terms_stack.peek().list = tlist = new java.util.ArrayList<>();
            terms_stack.Peek().list = tlist = new List<StyleParserCS.css.Term>();
            terms_stack.Peek().term = null;
            terms_stack.Peek().op = null;
            terms_stack.Peek().unary = 1;
            terms_stack.Peek().dash = false;
            if (ctx.term() != null)
            {
                foreach (CSSParser.TermContext trmCtx in ctx.term())
                {
                    if (trmCtx is CSSParser.TermValuePartContext)
                    {
                        visitTermValuePart((CSSParser.TermValuePartContext)trmCtx);
                        // set operator, store and create next
                        if (!declaration_stack.Peek().invalid && terms_stack.Peek().term != null)
                        {
                            terms_stack.Peek().term.setOperator(terms_stack.Peek().op);
                            terms_stack.Peek().list.Add(terms_stack.Peek().term);
                            // reinitialization
                            terms_stack.Peek().op = StyleParserCS.css.Term_Operator.SPACE;
                            terms_stack.Peek().unary = 1;
                            terms_stack.Peek().dash = false;
                            terms_stack.Peek().term = null;
                        }
                    }
                    else
                    {
                        visitTermInvalid((CSSParser.TermInvalidContext)trmCtx);
                    }
                }
            }
            // if  (// log.DebugEnabled)
            {
                // log.debug("Totally added {} terms", tlist.Count);
            }
            logLeave("terms");
            terms_stack.Pop();
            return tlist;
        }

        /// <summary>
        /// term
        /// : valuepart #termValuePart
        /// | LCUriY S* (any | SEMICOLON S*)* RCUriY #termInvalid // invalid term
        /// | ATKEYWORD S* #termInvalid // invalid term
        /// ;
        /// </summary>
        //
        public object visitTermValuePart(CSSParser.TermValuePartContext ctx)
        {
            logEnter("term");
            visitValuepart(ctx.valuepart());
            //returns null
            return null;
        }

        public object visitTermInvalid(CSSParser.TermInvalidContext ctx)
        {
            logEnter("term");
            declaration_stack.Peek().invalid = true;
            //returns null
            return null;
        }


        public object visitFunct(CSSParser.FunctContext ctx)
        {
            if (ctx.EXPRESSION() != null)
            {
                // log.warn("Omitting expression " + ctx.GetText() + ", expressions are not supported");
                return null;
            }
            //ORIGINAL LINE: Term<?> ret = null;
            Term ret = null;
            //ORIGINAL LINE: final String fname = extractTextUnescaped(ctx.FUNCTION().getText()).toLowerCase();
            string fname = extractTextUnescaped(ctx.FUNCTION().GetText()).ToLower();
            if (ctx.funct_args() != null)
            {
                //ORIGINAL LINE: java.util.List<Term<?>> t = visitFunct_args(ctx.funct_args());
                IList<Term> t = visitFunct_args(ctx.funct_args());
                if (fname.Equals("Uri"))
                {
                    // the function name is Uri() after escaping - create an URI
                    if (t == null || t.Count != 1)
                    {
                        ret = null;
                    }
                    else
                    {
                        //ORIGINAL LINE: StyleParserCS.css.Term<?> term = t.get(0);
                        StyleParserCS.css.Term term = t[0];
                        if (term is StyleParserCS.css.TermString && term.Operator == null)
                        {
                            ret = (Term)tf.createURI(((StyleParserCS.css.TermString)term).Value, extractBase(ctx.FUNCTION()));
                        }
                        else
                        {
                            ret = null;
                        }
                    }
                }
                else if (fname.Equals("calc"))
                {
                    // create calc() of the given type: <length>, <frequency>, <angle>, <time>, <number>, or <integer>
                    if (t == null || t.Count == 0)
                    {
                        ret = null;
                    }
                    else
                    {
                        ret = (Term)tf.createCalc(t);
                    }

                }
                else
                {
                    // create function
                    ret = (Term)tf.createFunction(fname, t);
                }
            }
            return ret;
        }

        public object visitValuepart(CSSParser.ValuepartContext ctx)
        {
            logEnter("valuepart: ", ctx);
            if (ctxHasErrorNode(ctx))
            {
                // log.error("value part with error");
                terms_stack.Peek().term = null;
                declaration_stack.Peek().invalid = true;
                return null;
            }
            if (ctx.MINUS() != null)
            {
                terms_stack.Peek().unary = -1;
                terms_stack.Peek().dash = true;
            }
            if (ctx.COMMA() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("VP - comma");
                }
                terms_stack.Peek().op = Term_Operator.COMMA;
            }
            else if (ctx.SLASH() != null)
            {
                terms_stack.Peek().op = Term_Operator.SLASH;
            }
            else if (ctx.stringv() != null)
            {
                //string
                // if  (// log.DebugEnabled)
                {
                    // log.debug("VP - string");
                }
                terms_stack.Peek().term = (Term)tf.createString(extractTextUnescaped(ctx.stringv().GetText()));
            }
            else if (ctx.IDENT() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("VP - ident");
                }
                terms_stack.Peek().term = (Term)tf.createIdent(extractTextUnescaped(ctx.IDENT().GetText()), terms_stack.Peek().dash);
            }
            else if (ctx.HASH() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("VP - hash");
                }
                terms_stack.Peek().term = (Term)tf.createColor(ctx.HASH().GetText());
                if (terms_stack.Peek().term == null)
                {
                    declaration_stack.Peek().invalid = true;
                }
            }
            else if (ctx.PERCENTAGE() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("VP - percentage");
                }
                terms_stack.Peek().term = (Term)tf.createPercent(ctx.PERCENTAGE().GetText(), terms_stack.Peek().unary);
            }
            else if (ctx.DIMENSION() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("VP - dimension");
                }
                string dim = ctx.DIMENSION().GetText();
                terms_stack.Peek().term = (Term)tf.createDimension(dim, terms_stack.Peek().unary);
                if (terms_stack.Peek().term == null)
                {
                    // log.info("Unable to create dimension from {}, unary {}", dim, terms_stack.Peek().unary);
                    declaration_stack.Peek().invalid = true;
                }
            }
            else if (ctx.NUMBER() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("VP - number");
                }
                terms_stack.Peek().term = (Term)tf.createNumeric(ctx.NUMBER().GetText(), terms_stack.Peek().unary);
            }
            else if (ctx.UNIRANGE() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("VP - unirange");
                }
                terms_stack.Peek().term = (Term)tf.createUnicodeRange(ctx.UNIRANGE().GetText());
            }
            else if (ctx.URI() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("VP - uri");
                }
                terms_stack.Peek().term = (Term)tf.createURI(extractTextUnescaped(ctx.URI().GetText()), extractBase(ctx.URI()));
            }
            else if (ctx.UNCLOSED_URI() != null && ((CSSToken)ctx.UNCLOSED_URI().Symbol).Valid)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("VP - unclosed_uri");
                }
                terms_stack.Peek().term = (Term)tf.createURI(extractTextUnescaped(ctx.UNCLOSED_URI().GetText()), extractBase(ctx.UNCLOSED_URI()));
            }
            else if (ctx.funct() != null)
            {
                terms_stack.Peek().term = null;
                //ORIGINAL LINE: Term<?> fnterm = (Term<?>) visitFunct(ctx.funct());
                Term fnterm = (Term)visitFunct(ctx.funct());
                if (fnterm != null)
                {
                    if (terms_stack.Peek().unary == -1)
                    {
                        if (fnterm is TermFunction)
                        {
                            //normal function - Add the unary minus to the function name
                            ((TermFunction)fnterm).setFunctionName('-' + ((TermFunction)fnterm).FunctionName);
                            terms_stack.Peek().term = fnterm;
                        }
                        else
                        {
                            //Uri() and calc() - not applicable 
                            declaration_stack.Peek().invalid = true;
                        }
                    }
                    else
                    {
                        terms_stack.Peek().term = fnterm;
                    }
                }
                else
                {
                    declaration_stack.Peek().invalid = true;
                }
            }
            else if (ctx.bracketed_idents() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("VP - bracketed_idents");
                }
                terms_stack.Peek().term = (Term)(TermBracketedIdents)visitBracketed_idents(ctx.bracketed_idents());
                if (terms_stack.Peek().term == null)
                {
                    declaration_stack.Peek().invalid = true; //invalid bracketed ident - invalidate the whole declaration
                }
            }
            else
            {
                // log.error("unhandled valueparts");
                terms_stack.Peek().term = null;
                declaration_stack.Peek().invalid = true;
            }
            //try to convert generic terms to more specific value types
            //ORIGINAL LINE: Term<?> term = terms_stack.peek().term;
            Term term = terms_stack.Peek().term;
            if (term != null)
            {
                term = findSpecificType(term);
                if (term != null)
                {
                    terms_stack.Peek().term = term;
                }
            }
            //returns null
            return null;
        }

        protected internal class funct_args_scope
        {
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> list;
            internal IList<StyleParserCS.css.Term> list;
            //ORIGINAL LINE: StyleParserCS.css.Term<?> term;
            internal StyleParserCS.css.Term term;
        }

        protected internal Stack<funct_args_scope> funct_args_stack = new Stack<funct_args_scope>();

        //ORIGINAL LINE: @Override public java.util.List<Term<?>> visitFunct_args(CSSParser.Funct_argsContext ctx)
        public IList<Term> visitFunct_args(CSSParser.Funct_argsContext ctx)
        {
            funct_args_stack.Push(new funct_args_scope());
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> tlist;
            IList<StyleParserCS.css.Term> tlist;
            funct_args_stack.Peek().term = null;
            logEnter("funct_args");
            //ORIGINAL LINE: funct_args_stack.peek().list = tlist = new java.util.ArrayList<>();
            funct_args_stack.Peek().list = tlist = new List<StyleParserCS.css.Term>();
            if (ctx.funct_argument() != null)
            {
                foreach (CSSParser.Funct_argumentContext argCtx in ctx.funct_argument())
                {
                    visitFunct_argument(argCtx);
                    // set operator, store and create next
                    if (!declaration_stack.Peek().invalid && funct_args_stack.Peek().term != null)
                    {
                        funct_args_stack.Peek().list.Add(funct_args_stack.Peek().term);
                        funct_args_stack.Peek().term = null;
                    }
                }
            }
            // if  (// log.DebugEnabled)
            {
                // log.debug("Totally added {} args", tlist.Count);
            }
            logLeave("funct_args");
            funct_args_stack.Pop();
            return tlist;
        }

        public object visitFunct_argument(CSSParser.Funct_argumentContext ctx)
        {
            logEnter("funct_argument: ", ctx);
            if (ctxHasErrorNode(ctx))
            {
                // log.error("argument with error");
                funct_args_stack.Peek().term = null;
                declaration_stack.Peek().invalid = true;
                return null;
            }
            if (ctx.PLUS() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("FA - plus");
                }
                funct_args_stack.Peek().term = (Term)tf.createOperator('+');
            }
            else if (ctx.MINUS() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("FA - minus");
                }
                funct_args_stack.Peek().term = (Term)tf.createOperator('-');
            }
            else if (ctx.ASTERISK() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("FA - *");
                }
                funct_args_stack.Peek().term = (Term)tf.createOperator('*');
            }
            else if (ctx.SLASH() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("FA - /");
                }
                funct_args_stack.Peek().term = (Term)tf.createOperator('/');
            }
            else if (ctx.LPAREN() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("FA - (");
                }
                funct_args_stack.Peek().term = (Term)tf.createOperator('(');
            }
            else if (ctx.RPAREN() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("FA - )");
                }
                funct_args_stack.Peek().term = (Term)tf.createOperator(')');
            }
            else if (ctx.COMMA() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("FA - comma");
                }
                funct_args_stack.Peek().term = (Term)tf.createOperator(',');
            }
            else if (ctx.stringv() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("FA - string");
                }
                funct_args_stack.Peek().term = (Term)tf.createString(extractTextUnescaped(ctx.stringv().GetText()));
            }
            else if (ctx.IDENT() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("FA - ident");
                }
                funct_args_stack.Peek().term = (Term)tf.createIdent(extractTextUnescaped(ctx.IDENT().GetText()));
            }
            else if (ctx.PERCENTAGE() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("FA - percentage");
                }
                funct_args_stack.Peek().term = (Term)tf.createPercent(ctx.PERCENTAGE().GetText(), 1);
            }
            else if (ctx.DIMENSION() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("FA - dimension");
                }
                string dim = ctx.DIMENSION().GetText();
                funct_args_stack.Peek().term = (Term)tf.createDimension(dim, 1);
                if (funct_args_stack.Peek().term == null)
                {
                    // log.info("Unable to create dimension from {}, unary {}", dim, 1);
                    declaration_stack.Peek().invalid = true;
                }
            }
            else if (ctx.HASH() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("FA - hash");
                }
                funct_args_stack.Peek().term = (Term)tf.createColor(ctx.HASH().GetText());
                if (funct_args_stack.Peek().term == null)
                {
                    declaration_stack.Peek().invalid = true;
                }
            }
            else if (ctx.NUMBER() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("FA - number");
                }
                funct_args_stack.Peek().term = tf.createNumeric(ctx.NUMBER().GetText(), 1);
            }
            else if (ctx.funct() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("FA - funct");
                }
                funct_args_stack.Peek().term = null;
                //ORIGINAL LINE: Term<?> fnterm = (Term<?>) visitFunct(ctx.funct());
                Term fnterm = (Term)visitFunct(ctx.funct());
                if (fnterm != null)
                {
                    funct_args_stack.Peek().term = fnterm;
                }
                else
                {
                    declaration_stack.Peek().invalid = true;
                }
            }
            else
            {
                // log.error("unhandled funct_args");
                funct_args_stack.Peek().term = null;
                declaration_stack.Peek().invalid = true;
            }
            //try convert color from current term
            //ORIGINAL LINE: Term<?> term = funct_args_stack.peek().term;
            Term term = funct_args_stack.Peek().term;
            if (term != null)
            {
                term = findSpecificType(term);
                if (term != null)
                {
                    funct_args_stack.Peek().term = term;
                }
            }
            //returns null
            return null;
        }

        public object visitBracketed_idents(CSSParser.Bracketed_identsContext ctx)
        {
            if (ctx.INVALID_STATEMENT() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("VP - ident invalid");
                }
                return null;
            }
            TermBracketedIdents ret = tf.createBracketedIdents();
            if (ctx.ident_list_item() != null)
            {
                foreach (var ictx in ctx.ident_list_item())
                {
                    TermIdent t = (TermIdent)visitIdent_list_item(ictx);
                    if (t != null)
                    {
                        ret.Add(t);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return ret;
        }

        public object visitIdent_list_item(CSSParser.Ident_list_itemContext ctx)
        {
            bool dash = false;
            if (ctx.INVALID_STATEMENT() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("VP - ident invalid");
                }
                return null;
            }
            if (ctx.MINUS() != null)
            {
                dash = true;
            }
            if (ctx.IDENT() != null)
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("VP - ident item");
                }
                return tf.createIdent(extractTextUnescaped(ctx.IDENT().GetText()), dash);
            }
            else
            {
                return null;
            }
        }

        protected internal class combined_selector_scope
        {
            internal bool invalid;
        }

        protected internal Stack<combined_selector_scope> combined_selector_stack = new Stack<combined_selector_scope>();

        public CombinedSelector visitCombined_selector(CSSParser.Combined_selectorContext ctx)
        {
            logEnter("combined_selector");
            combined_selector_stack.Push(new combined_selector_scope());
            CombinedSelector combinedSelector = (CombinedSelector)rf.createCombinedSelector().unlock();
            Selector_Combinator c;
            Selector s = visitSelector(ctx.selector(0));
            combinedSelector.Add(s);
            for (int i = 1; i < ctx.selector().Length; i++)
            {
                c = visitCombinator(ctx.combinator(i - 1));
                s = visitSelector(ctx.selector(i));
                s.setCombinator(c);
                combinedSelector.Add(s);
            }
            // entire ruleset is not valid when selector is not valid
            // there is no need to parse selector's when already marked as invalid
            if (statement_stack.Peek().invalid || combined_selector_stack.Peek().invalid)
            {
                combinedSelector = null;
                // if  (// log.DebugEnabled)
                {
                    if (statement_stack.Peek().invalid)
                    {
                        // log.debug("Ommiting combined selector, whole statement discarded");
                    }
                    else
                    {
                        // log.debug("Combined selector is invalid");
                    }
                }
                // mark whole ruleset as invalid
                statement_stack.Peek().invalid = true;
            }
            else
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("Returing combined selector: {}.", combinedSelector);
                }
            }
            combined_selector_stack.Pop();
            logLeave("combined_selector");
            return combinedSelector;
        }

        public Selector_Combinator visitCombinator(CSSParser.CombinatorContext ctx)
        {
            logEnter("combinator");
            if (ctx.GREATER() != null)
            {
                return Selector_Combinator.CHILD;
            }
            else if (ctx.PLUS() != null)
            {
                return Selector_Combinator.ADJACENT;
            }
            else if (ctx.TILDE() != null)
            {
                return Selector_Combinator.PRECEDING;
            }
            else
            {
                return Selector_Combinator.DESCENDANT;
            }
        }

        protected internal class selector_scope
        {
            internal StyleParserCS.css.Selector s;
        }

        protected internal Stack<selector_scope> selector_stack = new Stack<selector_scope>();

        /// <summary>
        /// selector
        /// : (IDENT | ASTERISK)  selpart* S*
        /// | selpart+ S*
        /// ;
        /// </summary>
        public virtual Selector visitSelector(CSSParser.SelectorContext ctx)
        {
            if (ctxHasErrorNode(ctx))
            {
                statement_stack.Peek().invalid = true;
                return null;
            }
            selector_stack.Push(new selector_scope());
            StyleParserCS.css.Selector sel;
            logEnter("selector");
            selector_stack.Peek().s = sel = (StyleParserCS.css.Selector)rf.createSelector().unlock();
            if (ctx.IDENT() != null || ctx.ASTERISK() != null)
            {
                StyleParserCS.css.Selector_ElementName en = rf.createElement(StyleParserCS.css.Selector_ElementName_Fields.WILDCARD);
                if (ctx.IDENT() != null)
                {
                    en.setName(extractTextUnescaped(ctx.IDENT().GetText()));
                }
                //// log.debug("Adding element name: {}.", en.getName());
                selector_stack.Peek().s.Add(en);
            }
            foreach (CSSParser.SelpartContext selpartctx in ctx.selpart())
            {
                visitSelpart(selpartctx);
            }

            logLeave("selector");
            selector_stack.Pop();
            return sel;
        }

        /// <summary>
        /// selpart
        /// : HASH
        /// | CLASSKEYWORD
        /// | LBRACKET S* attribute RBRACKET
        /// | pseudo
        /// | INVALID_SELPART // invalid selpart
        /// </summary>
        public object visitSelpart(CSSParser.SelpartContext ctx)
        {
            logEnter("selpart");
            string ident;
            if (ctx.HASH() != null)
            {
                ident = extractIdUnescaped(ctx.HASH().GetText());
                if (!string.ReferenceEquals(ident, null))
                {
                    selector_stack.Peek().s.Add(rf.createID(ident));
                }
                else
                {
                    combined_selector_stack.Peek().invalid = true;
                }

            }
            else if (ctx.CLASSKEYWORD() != null)
            {
                selector_stack.Peek().s.Add(rf.createClass(extractTextUnescaped(ctx.CLASSKEYWORD().GetText())));
            }
            else if (ctx.attribute() != null)
            {
                Selector_ElementAttribute ea = visitAttribute(ctx.attribute());
                selector_stack.Peek().s.Add(ea);

            }
            else if (ctx.pseudo() != null)
            {
                Selector_SelectorPart p = visitPseudo(ctx.pseudo());
                if (p != null)
                {
                    if (p is Selector_PseudoElement && selector_stack.Peek().s.PseudoElementType != null)
                    {
                        // log.warn("Invalid selector with multiple pseudo-elements");
                        combined_selector_stack.Peek().invalid = true;
                    }
                    else
                    {
                        selector_stack.Peek().s.Add(p);
                    }
                }
                else
                {
                    combined_selector_stack.Peek().invalid = true;
                }

            }
            else
            {
                combined_selector_stack.Peek().invalid = true;
            }
            logLeave("selpart");
            //returns null
            return null;
        }


        public Selector_ElementAttribute visitAttribute(CSSParser.AttributeContext ctx)
        {
            //attributes can be like [attr] or [attr operator value]
            // see http://www.w3.org/TR/CSS2/selector.html#attribute-selectors
            logEnter("attribute: ", ctx);
            //initialize attribute
            string attributeName = extractTextUnescaped(ctx.children[0].GetText());
            string value = null;
            bool isStringValue = false;
            Selector_Operator op = Selector_Operator.NO_OPERATOR;
            IList<IParseTree> ctx2 = filterSpaceTokens(ctx.children);
            //is attribute like [attr=value]
            if (ctx2.Count == 3)
            {
                CommonToken opToken = (CommonToken)((TerminalNodeImpl)ctx2[1]).Symbol;
                isStringValue = (ctx2[2] is CSSParser.StringContext);
                if (isStringValue)
                {
                    value = ctx2[2].GetText();
                }
                else
                {

                    value = ctx2[2].GetText();
                }
                value = extractTextUnescaped(value);
                switch (opToken.Type)
                {
                    case CSSParser.EQUALS:
                        {
                            op = Selector_Operator.EQUALS;
                            break;
                        }
                    case CSSParser.INCLUDES:
                        {
                            op = Selector_Operator.INCLUDES;
                            break;
                        }
                    case CSSParser.DASHMATCH:
                        {
                            op = Selector_Operator.DASHMATCH;
                            break;
                        }
                    case CSSParser.CONTAINS:
                        {
                            op = Selector_Operator.CONTAINS;
                            break;
                        }
                    case CSSParser.STARTSWITH:
                        {
                            op = Selector_Operator.STARTSWITH;
                            break;
                        }
                    case CSSParser.ENDSWITH:
                        {
                            op = Selector_Operator.ENDSWITH;
                            break;
                        }
                    default:
                        {
                            op = Selector_Operator.NO_OPERATOR;
                        }
                        break;
                }
            }
            Selector_ElementAttribute elemAttr = null;
            if (!string.ReferenceEquals(attributeName, null))
            {
                elemAttr = rf.createAttribute(value, isStringValue, op, attributeName);
            }
            else
            {
                // if  (// log.DebugEnabled)
                {
                    // log.debug("Invalid attribute element in selector");
                }
                combined_selector_stack.Peek().invalid = true;
            }
            logLeave("attribute");

            return elemAttr;
        }

        public Selector_SelectorPart visitPseudo(CSSParser.PseudoContext ctx)
        {
            logEnter("pseudo: ", ctx);
            bool isPseudoElem = ctx.COLON().Length > 1;
            Selector_SelectorPart pseudo = null;
            string name;
            if (ctx.FUNCTION() != null)
            {
                // function
                name = extractTextUnescaped(ctx.FUNCTION().GetText());
                if (ctx.selector() != null)
                {
                    Selector sel = visitSelector(ctx.selector());
                    pseudo = (isPseudoElem ? rf.createPseudoElement(name, sel) : rf.createPseudoClass(name, sel));
                }
                else
                {
                    string value = (ctx.MINUS() == null ? "" : "-");
                    if (ctx.IDENT() != null)
                    {
                        value += ctx.IDENT().GetText();
                    }
                    else if (ctx.NUMBER() != null)
                    {
                        value += ctx.NUMBER().GetText();
                    }
                    else if (ctx.INDEX() != null)
                    {
                        value += ctx.INDEX().GetText();
                    }
                    else
                    {
                        throw new System.NotSupportedException("unknown state");
                    }
                    pseudo = (isPseudoElem ? rf.createPseudoElement(name, value) : rf.createPseudoClass(name, value));
                }
            }
            else if (ctx.IDENT() != null)
            {
                // ident
                name = extractTextUnescaped(ctx.IDENT().GetText());
                if (ctx.MINUS() != null)
                {
                    name = ctx.MINUS().GetText() + name;
                }
                // Legacy support for :after, :before, :first-line, and :first-letter pseudo-elements
                if (!isPseudoElem && ("after".Equals(name, StringComparison.OrdinalIgnoreCase) || "before".Equals(name, StringComparison.OrdinalIgnoreCase) || "first-line".Equals(name, StringComparison.OrdinalIgnoreCase) || "first-letter".Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    isPseudoElem = true;
                }
                if (isPseudoElem)
                {
                    pseudo = rf.createPseudoElement(name);
                }
                else if (ctx.Parent is CSSParser.PageContext)
                {
                    pseudo = rf.createPseudoPage(name);
                }
                else
                {
                    pseudo = rf.createPseudoClass(name);
                }
            }
            else
            {
                // invalid selpart
                name = "";
            }

            if ((pseudo == null) || (pseudo is Selector_PseudoPage && ((Selector_PseudoPage)pseudo).Type == null) || (pseudo is Selector_PseudoClass && ((Selector_PseudoClass)pseudo).Type == null) || (pseudo is Selector_PseudoElement && ((Selector_PseudoElement)pseudo).Type == null))
            {
                // log.error("invalid pseudo declaration: " + name);
                pseudo = null; // invalid
            }
            logLeave("pseudo");
            return pseudo;
        }

        public string visitString(CSSParser.StringContext ctx)
        {
            if (ctx.INVALID_STRING() != null)
            {
                return null;
            }
            return extractTextUnescaped(ctx.GetText());
        }

        public object visitAny(CSSParser.AnyContext ctx)
        {
            // handled elsewhere
            return null;
        }

        public object visitNostatement(CSSParser.NostatementContext ctx)
        {
            // handled elsewhere
            return null;
        }

        public object visitNoprop(CSSParser.NopropContext ctx)
        {
            // handled elsewhere
            return null;
        }

        public object visitNorule(CSSParser.NoruleContext ctx)
        {
            // handled elsewhere
            return null;
        }

        public object visitNomediaquery(CSSParser.NomediaqueryContext ctx)
        {
            // handled elsewhere
            return null;
        }

        public object visit(IParseTree parseTree)
        {
            logEnter("visit");
            // if  (// log.DebugEnabled)
            {
                // log.debug(parseTree.GetText());
            }
            //        Object o  = visitChildren(parseTree.chi);
            logLeave("visit");
            return null;
        }

        public object visitChildren(IRuleNode ruleNode)
        {
            return null;
        }

        public object visitTerminal(ITerminalNode terminalNode)
        {
            return null;
        }

        public object visitErrorNode(IErrorNode errorNode)
        {
            // log.error("visitErrorNode");
            return null;
        }

    }

}