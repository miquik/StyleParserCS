using System.Collections.Generic;

namespace StyleParserCS.csskit.antlr4
{
    using CSSFactory = StyleParserCS.css.CSSFactory;
    using CombinedSelector = StyleParserCS.css.CombinedSelector;
    using Declaration = StyleParserCS.css.Declaration;
    using KeyframeBlock = StyleParserCS.css.KeyframeBlock;
    using MediaQuery = StyleParserCS.css.MediaQuery;
    using StyleParserCS.css;
    using RuleFactory = StyleParserCS.css.RuleFactory;
    using RuleFontFace = StyleParserCS.css.RuleFontFace;
    using RuleKeyframes = StyleParserCS.css.RuleKeyframes;
    using RuleMargin = StyleParserCS.css.RuleMargin;
    using RuleMedia = StyleParserCS.css.RuleMedia;
    using RulePage = StyleParserCS.css.RulePage;
    using RuleSet = StyleParserCS.css.RuleSet;
    using RuleViewport = StyleParserCS.css.RuleViewport;
    using Selector = StyleParserCS.css.Selector;
    using AngleSharp.Dom;
    using System.Linq;

    public class SimplePreparator : Preparator
    {
        // protected internal static readonly Logger log = LoggerFactory.getLogger(typeof(SimplePreparator));

        private static RuleFactory rf = CSSFactory.RuleFactory;

        private IElement elem;
        private bool inlinePriority;

        public SimplePreparator(IElement e, bool inlinePriority)
        {
            this.elem = e;
            this.inlinePriority = inlinePriority;
        }

        //ORIGINAL LINE: public StyleParserCS.css.RuleBlock<?> prepareRuleSet(java.util.List<StyleParserCS.css.CombinedSelector> cslist, java.util.List<StyleParserCS.css.Declaration> dlist, boolean wrap, java.util.List<StyleParserCS.css.MediaQuery> media)
        public virtual RuleBlock prepareRuleSet(IList<CombinedSelector> cslist, IList<Declaration> dlist, bool wrap, IList<MediaQuery> media)
        {

            // check emptiness
            if ((cslist == null || cslist.Count == 0) || (dlist == null || dlist.Count == 0))
            {
                // if (// log.DebugEnabled)
                {
                    // log.debug("Empty RuleSet was ommited");
                }
                return null;
            }

            // create rule set
            RuleSet rs = rf.createSet();
            rs.setSelectors(cslist);
            rs.replaceAll(dlist);
            // log.info("Created RuleSet as with:\n{}", rs);

            // wrap
            if (wrap)
            {
                // swap numbers, so RuleMedia is created before RuleSet
                RuleMedia rm = rf.createMedia();
                // if (// log.DebugEnabled)
                {
                    // log.debug("Wrapping RuleSet {} into RuleMedia: {}", rs, media);
                }

                rm.unlock();
                rm.Add(rs);
                rm.setMediaQueries(media);

                // return wrapped block
                //ORIGINAL LINE: return (StyleParserCS.css.RuleBlock<?>) rm;
                return (RuleBlock)rm;
            }

            // return classic rule set
            //ORIGINAL LINE: return (StyleParserCS.css.RuleBlock<?>) rs;
            return (RuleBlock)rs;
        }

        //ORIGINAL LINE: public StyleParserCS.css.RuleBlock<?> prepareRuleMedia(java.util.List<StyleParserCS.css.RuleSet> rules, java.util.List<StyleParserCS.css.MediaQuery> media)
        public virtual RuleBlock prepareRuleMedia(IList<RuleSet> rules, IList<MediaQuery> media)
        {

            if (rules == null || rules.Count == 0)
            {
                // if (// log.DebugEnabled)
                {
                    // log.debug("Empty RuleMedia was ommited");
                }
                return null;
            }

            // create media at position of mark
            RuleMedia rm = rf.createMedia();
            rm.replaceAll(rules);
            if (media != null && media.Count > 0)
            {
                rm.setMediaQueries(media);
            }

            // log.info("Create @media as with:\n{}", rm);

            //ORIGINAL LINE: return (StyleParserCS.css.RuleBlock<?>) rm;
            return (RuleBlock)rm;
        }

        //ORIGINAL LINE: @Override public StyleParserCS.css.RuleBlock<?> prepareRulePage(java.util.List<StyleParserCS.css.Declaration> declarations, java.util.List<StyleParserCS.css.RuleMargin> marginRules, String name, StyleParserCS.css.Selector_PseudoPage pseudo)
        public virtual RuleBlock prepareRulePage(IList<Declaration> declarations, IList<RuleMargin> marginRules, string name, StyleParserCS.css.Selector_PseudoPage pseudo)
        {

            if ((declarations == null || declarations.Count == 0) && (marginRules == null || marginRules.Count == 0))
            {
                // if (// log.DebugEnabled)
                {
                    // log.debug("Empty RulePage was ommited");
                }
                return null;
            }

            RulePage rp = rf.createPage();
            if (declarations != null)
            {
                foreach (Declaration d in declarations)
                {
                    rp.Add((Rule)d);
                }
            }
            if (marginRules != null)
            {
                foreach (RuleMargin m in marginRules)
                {
                    rp.Add((Rule)m);
                }
            }
            rp.setName(name);

            rp.setPseudo(pseudo);
            // log.info("Create @page as with:\n{}", rp);

            //ORIGINAL LINE: return (StyleParserCS.css.RuleBlock<?>) rp;
            return (RuleBlock)rp;
        }

        public virtual RuleMargin prepareRuleMargin(string area, IList<Declaration> decl)
        {

            if ((decl == null || decl.Count == 0))
            {
                // if (// log.DebugEnabled)
                {
                    // log.debug("Empty RuleMargin was ommited");
                }
                return null;
            }

            RuleMargin rm = rf.createMargin(area);
            rm.replaceAll(decl);

            // log.info("Create @" + area + " with:\n" + rm);

            return rm;
        }

        //ORIGINAL LINE: public StyleParserCS.css.RuleBlock<?> prepareRuleViewport(java.util.List<StyleParserCS.css.Declaration> decl)
        public virtual RuleBlock prepareRuleViewport(IList<Declaration> decl)
        {

            if (decl == null || decl.Count == 0)
            {
                // if (// log.DebugEnabled)
                {
                    // log.debug("Empty Viewport was ommited");
                }
                return null;
            }

            RuleViewport rp = rf.createViewport();
            rp.replaceAll(decl);
            // log.info("Create @viewport as {}th with:\n{}", rp);

            //ORIGINAL LINE: return (StyleParserCS.css.RuleBlock<?>) rp;
            return (RuleBlock)rp;
        }

        //ORIGINAL LINE: public StyleParserCS.css.RuleBlock<?> prepareRuleFontFace(java.util.List<StyleParserCS.css.Declaration> decl)
        public virtual RuleBlock prepareRuleFontFace(IList<Declaration> decl)
        {

            if (decl == null || decl.Count == 0)
            {
                // if (// log.DebugEnabled)
                {
                    // log.debug("Empty RuleFontFace was ommited");
                }
                return null;
            }

            RuleFontFace rp = rf.createFontFace();
            rp.replaceAll(decl);
            // log.info("Create @font-face as with:\n{}", rp);

            //ORIGINAL LINE: return (StyleParserCS.css.RuleBlock<?>) rp;
            return (RuleBlock)rp;
        }

        //ORIGINAL LINE: @Override public StyleParserCS.css.RuleBlock<?> prepareInlineRuleSet(java.util.List<StyleParserCS.css.Declaration> dlist, java.util.List<StyleParserCS.css.Selector_SelectorPart> pseudos)
        public virtual RuleBlock prepareInlineRuleSet(IList<Declaration> dlist, IList<StyleParserCS.css.Selector_SelectorPart> pseudos)
        {

            if (dlist == null || dlist.Count == 0)
            {
                // if (// log.DebugEnabled)
                {
                    // log.debug("Empty RuleSet (inline) was ommited");
                }
                return null;
            }

            // create selector with element
            CombinedSelector cs = (CombinedSelector)rf.createCombinedSelector().unlock();
            Selector sel = (Selector)rf.createSelector().unlock();
            sel.Add(rf.createElementDOM(elem, inlinePriority));
            if (pseudos != null)
            {
                foreach (var pseudo in pseudos)
                {
                    sel.Add(pseudo);
                }
            }
            cs.Add(sel);

            RuleSet rs = rf.createSet();
            rs.replaceAll(dlist);
            // rs.setSelectors(cs.Cast<CombinedSelector>().ToList()); // Arrays.asList(cs));
            // rs.setSelectors(cs.asList()); // Arrays.asList(cs));
            rs.setSelectors(new List<CombinedSelector>() { cs });

            // log.info("Create inline ruleset as with:\n{}", rs);

            //ORIGINAL LINE: return (StyleParserCS.css.RuleBlock<?>) rs;
            return (RuleBlock)rs;
        }

        //ORIGINAL LINE: @Override public StyleParserCS.css.RuleBlock<?> prepareRuleKeyframes(java.util.List<StyleParserCS.css.KeyframeBlock> rules, String name)
        public virtual RuleBlock prepareRuleKeyframes(IList<KeyframeBlock> rules, string name)
        {
            if (rules == null || rules.Count == 0)
            {
                // if (// log.DebugEnabled)
                {
                    // log.debug("Empty RuleKeyframes was ommited");
                }
                return null;
            }
            if (string.ReferenceEquals(name, null) || name.Length == 0)
            {
                // if (// log.DebugEnabled)
                {
                    // log.debug("RuleKeyframes with no name was ommited");
                }
                return null;
            }

            // create media at position of mark
            RuleKeyframes rk = rf.createKeyframes();
            rk.replaceAll(rules);
            rk.setName(name);

            // log.info("Create @keyframes as with:\n{}", rk);

            //ORIGINAL LINE: return (StyleParserCS.css.RuleBlock<?>) rk;
            return (RuleBlock)rk;
        }

    }

}