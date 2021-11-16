using System;
using System.Collections.Generic;
using System.Linq;

namespace StyleParserCS.domassign
{
    using CSSFactory = StyleParserCS.css.CSSFactory;
    using CombinedSelector = StyleParserCS.css.CombinedSelector;
    using Declaration = StyleParserCS.css.Declaration;
    using ElementMatcher = StyleParserCS.css.ElementMatcher;
    using MatchCondition = StyleParserCS.css.MatchCondition;
    using MediaQuery = StyleParserCS.css.MediaQuery;
    using MediaSpec = StyleParserCS.css.MediaSpec;
    using NodeData = StyleParserCS.css.NodeData;
    using StyleParserCS.css;
    using RuleMedia = StyleParserCS.css.RuleMedia;
    using RuleSet = StyleParserCS.css.RuleSet;
    using Selector = StyleParserCS.css.Selector;
    using Selector_PseudoClassType = StyleParserCS.css.Selector_PseudoClassType;
    using Selector_PseudoElementType = StyleParserCS.css.Selector_PseudoElementType;
    using StyleSheet = StyleParserCS.css.StyleSheet;
    using Holder = StyleParserCS.domassign.Analyzer.Holder;
    using HolderItem = StyleParserCS.domassign.Analyzer.HolderItem;
    using HolderSelector = StyleParserCS.domassign.Analyzer.HolderSelector;
    using OrderedRule = StyleParserCS.domassign.Analyzer.OrderedRule;
    using AngleSharp.Dom;
    using StyleParserCS.utils;

    /// <summary>
    /// A pure (state-less) Analyser.
    /// 
    /// Can be used by clients that need more control over what computation is cached.
    /// 
    /// </summary>
    public sealed class AnalyzerUtil
    {
        /// <summary>
        /// Returns all applicable rules for an element
        /// </summary>
        /// <param name="sheets"> </param>
        /// <param name="element"> </param>
        /// <param name="mediaspec">
        /// @return </param>
        //ORIGINAL LINE: public static StyleParserCS.domassign.Analyzer.OrderedRule[] getApplicableRules(final java.util.List<StyleParserCS.css.StyleSheet> sheets, final org.w3c.dom.IElement element, final StyleParserCS.css.MediaSpec mediaspec)
        public static OrderedRule[] getApplicableRules(IList<StyleSheet> sheets, IElement element, MediaSpec mediaspec)
        {
            //ORIGINAL LINE: final StyleParserCS.domassign.Analyzer.Holder rules = getClassifiedRules(sheets, mediaspec);
            Holder rules = getClassifiedRules(sheets, mediaspec);
            return getApplicableRules(element, rules, null);
        }

        //ORIGINAL LINE: public static StyleParserCS.domassign.Analyzer.Holder getClassifiedRules(final java.util.List<StyleParserCS.css.StyleSheet> sheets, final StyleParserCS.css.MediaSpec mediaspec)
        public static Holder getClassifiedRules(IList<StyleSheet> sheets, MediaSpec mediaspec)
        {
            //ORIGINAL LINE: final StyleParserCS.domassign.Analyzer.Holder rules = new StyleParserCS.domassign.Analyzer.Holder();
            Holder rules = new Holder();
            AnalyzerUtil.classifyAllSheets(sheets, rules, mediaspec);
            return rules;
        }

        //ORIGINAL LINE: public static StyleParserCS.css.NodeData getElementStyle(org.w3c.dom.IElement el, StyleParserCS.css.Selector_PseudoElementType pseudo, final StyleParserCS.css.ElementMatcher matcher, StyleParserCS.css.MatchCondition matchCond, StyleParserCS.domassign.Analyzer.OrderedRule[] applicableRules)
        public static NodeData getElementStyle(IElement el, Selector_PseudoElementType pseudo, ElementMatcher matcher, MatchCondition matchCond, OrderedRule[] applicableRules)
        {
            return makeNodeData(computeDeclarations(el, pseudo, applicableRules, matcher, matchCond));
        }

        //ORIGINAL LINE: public static StyleParserCS.domassign.Analyzer.OrderedRule[] getApplicableRules(final org.w3c.dom.IElement e, final StyleParserCS.domassign.Analyzer.Holder holder, final StyleParserCS.css.RuleSet[] elementRuleSets)
        public static OrderedRule[] getApplicableRules(IElement e, Holder holder, RuleSet[] elementRuleSets)
        {
            // create set of possible candidates applicable to given element
            // set is automatically filtered to not contain duplicates
            //ORIGINAL LINE: final java.util.Set<StyleParserCS.domassign.Analyzer.OrderedRule> candidates = new java.util.HashSet<StyleParserCS.domassign.Analyzer.OrderedRule>();
            // ISet<OrderedRule> candidates = new HashSet<OrderedRule>();
            // TOCHECK
            List<OrderedRule> candidates = new List<OrderedRule>();

            // match element classes
            foreach (String cname in CSSFactory.ElementMatcher.elementClasses(e))
            {
                // holder contains rule with given class
                //ORIGINAL LINE: final java.util.List<StyleParserCS.domassign.Analyzer.OrderedRule> classRules = holder.get(StyleParserCS.domassign.Analyzer.HolderItem.CLASS, cname.toLowerCase());
                IList<OrderedRule> classRules = holder.get(HolderItem.CLASS, cname.ToLower());
                if (classRules != null)
                {
                    candidates.AddRange(classRules);
                }
            }
            // // log.trace("After CLASSes {} total candidates.", candidates.size());

            // match IDs
            //ORIGINAL LINE: final String id = StyleParserCS.css.CSSFactory.getElementMatcher().elementID(e);
            string id = CSSFactory.ElementMatcher.elementID(e);
            if (!string.ReferenceEquals(id, null) && id.Length != 0)
            {
                //ORIGINAL LINE: final java.util.List<StyleParserCS.domassign.Analyzer.OrderedRule> idRules = holder.get(StyleParserCS.domassign.Analyzer.HolderItem.ID, id.toLowerCase());
                IList<OrderedRule> idRules = holder.get(HolderItem.ID, id.ToLower());
                if (idRules != null)
                {
                    candidates.AddRange(idRules);
                }
            }
            // // log.trace("After IDs {} total candidates.", candidates.size());

            // match elements
            //ORIGINAL LINE: final String name = StyleParserCS.css.CSSFactory.getElementMatcher().elementName(e);
            string name = CSSFactory.ElementMatcher.elementName(e);
            if (!string.ReferenceEquals(name, null))
            {
                //ORIGINAL LINE: final java.util.List<StyleParserCS.domassign.Analyzer.OrderedRule> nameRules = holder.get(StyleParserCS.domassign.Analyzer.HolderItem.ELEMENT, name.toLowerCase());
                IList<OrderedRule> nameRules = holder.get(HolderItem.ELEMENT, name.ToLower());
                if (nameRules != null)
                {
                    candidates.AddRange(nameRules);
                }
            }
            // // log.trace("After ELEMENTs {} total candidates.", candidates.size());

            // others            
            candidates.AddRange(holder.get(HolderItem.OTHER, null));

            //ORIGINAL LINE: final int totalCandidates = candidates.size();
            int totalCandidates = candidates.Count;
            //ORIGINAL LINE: final int netCandidates = elementRuleSets == null ? totalCandidates : totalCandidates + elementRuleSets.length;
            int netCandidates = elementRuleSets == null ? totalCandidates : totalCandidates + elementRuleSets.Length;

            // // log.debug("Totally {} candidates.", totalCandidates);

            // transform to array to speed up traversal
            // and sort rules in order as they were found in CSS definition
            //ORIGINAL LINE: final StyleParserCS.domassign.Analyzer.OrderedRule[] clist = (StyleParserCS.domassign.Analyzer.OrderedRule[]) candidates.toArray(new StyleParserCS.domassign.Analyzer.OrderedRule[netCandidates]);
            OrderedRule[] clist = (OrderedRule[])candidates.ToArray();
            Array.Sort(clist, 0, totalCandidates);

            // Append the element rules
            if (elementRuleSets != null)
            {
                //ORIGINAL LINE: final int lastOrder = totalCandidates > 0 ? clist[totalCandidates-1].getOrder() : 0;
                int lastOrder = totalCandidates > 0 ? clist[totalCandidates - 1].Order : 0;
                for (int i = 0; i < elementRuleSets.Length; i++)
                {
                    clist[totalCandidates + i] = new OrderedRule(elementRuleSets[i], lastOrder + i);
                }
            }

            // NOTE: The following trace statement creates a lot of memory pressure
            // // log.trace("With values: {}", Arrays.ToString(clist));

            return clist;
        }

        //ORIGINAL LINE: static StyleParserCS.css.NodeData makeNodeData(final java.util.List<StyleParserCS.css.Declaration> decls)
        internal static NodeData makeNodeData(IList<Declaration> decls)
        {
            //ORIGINAL LINE: final StyleParserCS.css.NodeData main = StyleParserCS.css.CSSFactory.createNodeData();
            NodeData main = CSSFactory.createNodeData();
            foreach (Declaration d in decls)
            {
                main.push(d);
            }

            return main;
        }

        /// <summary>
        /// Classifies the rules in all the style sheets. </summary>
        /// <param name="mediaspec"> The specification of the media for evaluating the media queries. </param>
        //ORIGINAL LINE: static void classifyAllSheets(final java.util.List<StyleParserCS.css.StyleSheet> sheets, final StyleParserCS.domassign.Analyzer.Holder rules, final StyleParserCS.css.MediaSpec mediaspec)
        internal static void classifyAllSheets(IList<StyleSheet> sheets, Holder rules, MediaSpec mediaspec)
        {
            //ORIGINAL LINE: final Counter orderCounter = new Counter();
            Counter orderCounter = new Counter();
            foreach (StyleSheet sheet in sheets)
            {
                classifyRules(sheet, mediaspec, rules, orderCounter);
            }
        }

        //ORIGINAL LINE: static boolean elementSelectorMatches(final StyleParserCS.css.Selector s, final org.w3c.dom.IElement e, final StyleParserCS.css.ElementMatcher matcher, final StyleParserCS.css.MatchCondition matchCond)
        internal static bool elementSelectorMatches(Selector s, IElement e, ElementMatcher matcher, MatchCondition matchCond)
        {
            return s.matches(e, matcher, matchCond);
        }

        //ORIGINAL LINE: private static boolean nodeSelectorMatches(final StyleParserCS.css.Selector s, final org.w3c.dom.INode n, final StyleParserCS.css.ElementMatcher matcher, final StyleParserCS.css.MatchCondition matchCond)
        private static bool nodeSelectorMatches(Selector s, INode n, ElementMatcher matcher, MatchCondition matchCond)
        {
            if (n.NodeType == NodeType.Element)
            {
                return s.matches((IElement)n, matcher, matchCond);
            }
            else
            {
                return false;
            }
        }

        //ORIGINAL LINE: static java.util.List<StyleParserCS.css.Declaration> computeDeclarations(final org.w3c.dom.IElement e, final StyleParserCS.css.Selector_PseudoElementType pseudo, final StyleParserCS.domassign.Analyzer.OrderedRule[] clist, final StyleParserCS.css.ElementMatcher matcher, final StyleParserCS.css.MatchCondition matchCond)
        internal static IList<Declaration> computeDeclarations(IElement e, Selector_PseudoElementType pseudo, OrderedRule[] clist, ElementMatcher matcher, MatchCondition matchCond)
        {
            // resulting list of declaration for this element with no pseudo-selectors (main list)(local cache)
            //ORIGINAL LINE: final java.util.List<StyleParserCS.css.Declaration> eldecl = new java.util.ArrayList<StyleParserCS.css.Declaration>();
            List<Declaration> eldecl = new List<Declaration>();

            // for all candidates
            foreach (OrderedRule orule in clist)
            {

                //ORIGINAL LINE: final StyleParserCS.css.RuleSet rule = orule.getRule();
                RuleSet rule = orule.Rule;
                //ORIGINAL LINE: final StyleParserCS.css.StyleSheet sheet = rule.getStyleSheet();
                StyleSheet sheet = rule.StyleSheet;
                //ORIGINAL LINE: final StyleParserCS.css.StyleSheet_Origin origin = (sheet == null) ? StyleParserCS.css.StyleSheet_Origin.AGENT : sheet.getOrigin();
                StyleParserCS.css.StyleSheet_Origin origin = (sheet == null) ? StyleParserCS.css.StyleSheet_Origin.AGENT : sheet.Origin;

                // for all selectors inside
                foreach (CombinedSelector s in rule.getSelectors())
                {

                    if (!AnalyzerUtil.matchSelector(s, e, matcher, matchCond))
                    {
                        // log.trace("CombinedSelector \"{}\" NOT matched!", s);
                        continue;
                    }

                    // log.trace("CombinedSelector \"{}\" matched", s);

                    //ORIGINAL LINE: final StyleParserCS.css.Selector_PseudoElementType ptype = s.getPseudoElementType();
                    Selector_PseudoElementType ptype = s.PseudoElementType;
                    if (ptype == pseudo)
                    {
                        // add to the resulting list
                        //ORIGINAL LINE: final StyleParserCS.css.CombinedSelector_Specificity spec = s.computeSpecificity();
                        StyleParserCS.css.CombinedSelector_Specificity spec = s.computeSpecificity();
                        foreach (Declaration d in rule)
                        {
                            eldecl.Add(new AssignedDeclaration(d, spec, origin));
                        }
                    }
                }
            }

            // sort declarations
            eldecl.Sort(); //sort the main list
            // log.debug("Sorted {} declarations.", eldecl.Count);
            // log.trace("With values: {}", eldecl);

            return eldecl;
        }

        //ORIGINAL LINE: public static boolean hasPseudoSelector(final StyleParserCS.domassign.Analyzer.OrderedRule[] rules, final org.w3c.dom.IElement e, final StyleParserCS.css.MatchCondition matchCond, StyleParserCS.css.Selector_PseudoClassType pd)
        public static bool hasPseudoSelector(OrderedRule[] rules, IElement e, MatchCondition matchCond, Selector_PseudoClassType pd)
        {
            foreach (OrderedRule rule in rules)
            {
                foreach (CombinedSelector cs in rule.Rule.getSelectors())
                {
                    //ORIGINAL LINE: final StyleParserCS.css.Selector lastSelector = cs.get(cs.size() - 1);
                    Selector lastSelector = cs[cs.Count - 1];
                    if (lastSelector.hasPseudoClass(pd))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //ORIGINAL LINE: public static boolean hasPseudoSelectorForAncestor(final StyleParserCS.domassign.Analyzer.OrderedRule[] rules, final org.w3c.dom.IElement e, final org.w3c.dom.IElement targetAncestor, final StyleParserCS.css.ElementMatcher matcher, final StyleParserCS.css.MatchCondition matchCond, StyleParserCS.css.Selector_PseudoClassType pd)
        public static bool hasPseudoSelectorForAncestor(OrderedRule[] rules, IElement e, IElement targetAncestor, ElementMatcher matcher, MatchCondition matchCond, Selector_PseudoClassType pd)
        {
            foreach (OrderedRule rule in rules)
            {
                foreach (CombinedSelector cs in rule.Rule.getSelectors())
                {
                    if (hasPseudoSelectorForAncestor(cs, e, targetAncestor, matcher, matchCond, pd))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //ORIGINAL LINE: private static boolean hasPseudoSelectorForAncestor(final StyleParserCS.css.CombinedSelector sel, final org.w3c.dom.IElement e, final org.w3c.dom.IElement targetAncestor, final StyleParserCS.css.ElementMatcher matcher, final StyleParserCS.css.MatchCondition matchCond, StyleParserCS.css.Selector_PseudoClassType pd)
        private static bool hasPseudoSelectorForAncestor(CombinedSelector sel, IElement e, IElement targetAncestor, ElementMatcher matcher, MatchCondition matchCond, Selector_PseudoClassType pd)
        {
            bool retval = false;
            StyleParserCS.css.Selector_Combinator combinator = null;
            IElement current = e;
            // traverse simple selector backwards
            for (int i = sel.Count - 1; i >= 0; i--)
            {
                // last simple selector
                //ORIGINAL LINE: final StyleParserCS.css.Selector s = sel.get(i);
                Selector s = sel[i];

                // decide according to combinator anti-pattern
                if (combinator == null)
                {
                    retval = elementSelectorMatches(s, current, matcher, matchCond);
                }
                else if (combinator == StyleParserCS.css.Selector_Combinator.ADJACENT)
                {
                    INode adjacent = current;
                    do
                    {
                        adjacent = adjacent.PreviousSibling;
                    } while (adjacent != null && adjacent.NodeType != NodeType.Element);
                    retval = false;
                    if (adjacent != null && adjacent.NodeType == NodeType.Element)
                    {
                        current = (IElement)adjacent;
                        retval = elementSelectorMatches(s, current, matcher, matchCond);
                    }
                }
                else if (combinator == StyleParserCS.css.Selector_Combinator.PRECEDING)
                {
                    INode preceding = current.PreviousSibling;
                    retval = false;
                    do
                    {
                        if (preceding != null)
                        {
                            if (nodeSelectorMatches(s, preceding, matcher, matchCond))
                            {
                                current = (IElement)preceding;
                                retval = true;
                            }
                            else
                            {
                                preceding = preceding.PreviousSibling;
                            }
                        }
                    } while (!retval && preceding != null);
                }
                else if (combinator == StyleParserCS.css.Selector_Combinator.DESCENDANT)
                {
                    INode ancestor = current.Parent;
                    retval = false;
                    do
                    {
                        if (ancestor != null)
                        {
                            if (nodeSelectorMatches(s, ancestor, matcher, matchCond))
                            {
                                current = (IElement)ancestor;
                                retval = true;
                            }
                            else
                            {
                                ancestor = ancestor.Parent;
                            }
                        }
                    } while (!retval && ancestor != null);
                }
                else if (combinator == StyleParserCS.css.Selector_Combinator.CHILD)
                {
                    //ORIGINAL LINE: final org.w3c.dom.INode parent = current.getParentNode();
                    INode parent = current.Parent;
                    retval = false;
                    if (parent != null && parent.NodeType == NodeType.Element)
                    {
                        current = (IElement)parent;
                        retval = elementSelectorMatches(s, current, matcher, matchCond);
                    }
                }

                // set combinator for next loop
                combinator = s.Combinator;

                // leave loop if not matched
                if (!retval)
                {
                    break;
                }
                else if (current == targetAncestor)
                {
                    return s.hasPseudoClass(pd);
                }
            }
            return false;
        }

        //ORIGINAL LINE: protected static boolean matchSelector(final StyleParserCS.css.CombinedSelector sel, final org.w3c.dom.IElement e, final StyleParserCS.css.ElementMatcher matcher, final StyleParserCS.css.MatchCondition matchCond)
        private static bool matchSelector(CombinedSelector sel, IElement e, ElementMatcher matcher, MatchCondition matchCond)
        {
            bool retval = false;
            StyleParserCS.css.Selector_Combinator combinator = null;
            IElement current = e;
            // traverse simple selector backwards
            for (int i = sel.Count - 1; i >= 0; i--)
            {
                // last simple selector
                //ORIGINAL LINE: final StyleParserCS.css.Selector s = sel.get(i);
                Selector s = sel[i];
                // log.trace("Iterating loop with selector {}, combinator {}", s, combinator);

                // decide according to combinator anti-pattern
                if (combinator == null)
                {
                    retval = elementSelectorMatches(s, current, matcher, matchCond);
                }
                else if (combinator == StyleParserCS.css.Selector_Combinator.ADJACENT)
                {
                    INode adjacent = current;
                    do
                    {
                        adjacent = adjacent.PreviousSibling;
                    } while (adjacent != null && adjacent.NodeType != NodeType.Element);
                    retval = false;
                    if (adjacent != null && adjacent.NodeType == NodeType.Element)
                    {
                        current = (IElement)adjacent;
                        retval = elementSelectorMatches(s, current, matcher, matchCond);
                    }
                }
                else if (combinator == StyleParserCS.css.Selector_Combinator.PRECEDING)
                {
                    INode preceding = current.PreviousSibling;
                    retval = false;
                    do
                    {
                        if (preceding != null)
                        {
                            if (nodeSelectorMatches(s, preceding, matcher, matchCond))
                            {
                                current = (IElement)preceding;
                                retval = true;
                            }
                            else
                            {
                                preceding = preceding.PreviousSibling;
                            }
                        }
                    } while (!retval && preceding != null);
                }
                else if (combinator == StyleParserCS.css.Selector_Combinator.DESCENDANT)
                {
                    INode ancestor = current.Parent;
                    retval = false;
                    do
                    {
                        if (ancestor != null)
                        {
                            if (nodeSelectorMatches(s, ancestor, matcher, matchCond))
                            {
                                current = (IElement)ancestor;
                                retval = true;
                            }
                            else
                            {
                                ancestor = ancestor.Parent;
                            }
                        }
                    } while (!retval && ancestor != null);
                }
                else if (combinator == StyleParserCS.css.Selector_Combinator.CHILD)
                {
                    //ORIGINAL LINE: final org.w3c.dom.INode parent = current.getParentNode();
                    INode parent = current.Parent;
                    retval = false;
                    if (parent != null && parent.NodeType == NodeType.Element)
                    {
                        current = (IElement)parent;
                        retval = elementSelectorMatches(s, current, matcher, matchCond);
                    }
                }

                // set combinator for next loop
                combinator = s.Combinator;

                // leave loop if not matched
                if (!retval)
                {
                    break;
                }
            }
            return retval;
        }

        /// <summary>
        /// Classify CSS rule according its selector for to be of specified item(s)
        /// </summary>
        /// <param name="selector">
        ///            CombinedSelector of rules </param>
        /// <returns> List of HolderSelectors to which selectors conforms </returns>
        //ORIGINAL LINE: private static java.util.List<StyleParserCS.domassign.Analyzer.HolderSelector> classifySelector(final StyleParserCS.css.CombinedSelector selector)
        private static IList<HolderSelector> classifySelector(CombinedSelector selector)
        {

            //ORIGINAL LINE: final java.util.List<StyleParserCS.domassign.Analyzer.HolderSelector> hs = new java.util.ArrayList<StyleParserCS.domassign.Analyzer.HolderSelector>();
            IList<HolderSelector> hs = new List<HolderSelector>();

            try
            {
                // last simple selector decided about all selector
                //ORIGINAL LINE: final StyleParserCS.css.Selector last = selector.getLastSelector();
                Selector last = selector.LastSelector;

                // is element or other (wildcard)
                //ORIGINAL LINE: final String element = last.getElementName();
                string element = last.ElementName;
                if (!string.ReferenceEquals(element, null))
                {
                    // wildcard
                    if (StyleParserCS.css.Selector_ElementName_Fields.WILDCARD.Equals(element))
                    {
                        hs.Add(new HolderSelector(HolderItem.OTHER, null));
                    }
                    // element
                    else
                    {
                        hs.Add(new HolderSelector(HolderItem.ELEMENT, element.ToLower()));
                    }
                }

                // is class name
                //ORIGINAL LINE: final String className = last.getClassName();
                string className = last.ClassName;
                if (!string.ReferenceEquals(className, null))
                {
                    hs.Add(new HolderSelector(HolderItem.CLASS, className.ToLower()));
                }

                // is id
                //ORIGINAL LINE: final String id = last.getIDName();
                string id = last.IDName;
                if (!string.ReferenceEquals(id, null))
                {
                    hs.Add(new HolderSelector(HolderItem.ID, id.ToLower()));
                }

                // is in others
                if (hs.Count == 0)
                {
                    hs.Add(new HolderSelector(HolderItem.OTHER, null));
                }

                return hs;

            }
            //ORIGINAL LINE: catch (final UnsupportedOperationException e)
            catch (Exception)
            {
                // log.error("CombinedSelector does not include any selector, this should not happen!");
                return new List<HolderSelector>();
            }
        }

        private class Counter
        {
            internal int count = 0;
            public virtual int AndIncrement
            {
                get
                {
                    return count++;
                }
            }
        }

        //ORIGINAL LINE: private static void insertClassified(final StyleParserCS.domassign.Analyzer.Holder holder, final java.util.List<StyleParserCS.domassign.Analyzer.HolderSelector> hs, final StyleParserCS.css.RuleSet value, final Counter orderCounter)
        private static void insertClassified(Holder holder, IList<HolderSelector> hs, RuleSet value, Counter orderCounter)
        {
            foreach (HolderSelector h in hs)
            {
                holder.insert(h.item, h.key, new OrderedRule(value, orderCounter.AndIncrement));
            }
        }

        /// <summary>
        /// Divides rules in sheet into different categories to be easily and more
        /// quickly parsed afterward
        /// </summary>
        /// <param name="sheet"> The style sheet to be classified </param>
        /// <param name="mediaspec"> The specification of the media for evaluating the media queries. </param>
        /// <param name="orderCounter">  </param>
        //ORIGINAL LINE: private static void classifyRules(final StyleParserCS.css.StyleSheet sheet, final StyleParserCS.css.MediaSpec mediaspec, final StyleParserCS.domassign.Analyzer.Holder rules, final Counter orderCounter)
        private static void classifyRules(StyleSheet sheet, MediaSpec mediaspec, Holder rules, Counter orderCounter)
        {

            //ORIGINAL LINE: for (final StyleParserCS.css.Rule<?> rule : sheet)
            int curRule = 0;
            foreach (Rule rule in sheet)
            {
                // this rule conforms to all media
                if (rule is RuleSet)
                {
                    //ORIGINAL LINE: final StyleParserCS.css.RuleSet ruleset = (StyleParserCS.css.RuleSet) rule;
                    RuleSet ruleset = (RuleSet)rule;
                    foreach (CombinedSelector s in ruleset.getSelectors())
                    {
                        insertClassified(rules, classifySelector(s), ruleset, orderCounter);
                    }
                }
                // this rule conforms to different media
                else if (rule is RuleMedia)
                {
                    //ORIGINAL LINE: final StyleParserCS.css.RuleMedia rulemedia = (StyleParserCS.css.RuleMedia) rule;
                    RuleMedia rulemedia = (RuleMedia)rule;

                    bool mediaValid = false;
                    if (rulemedia.MediaQueries == null || rulemedia.MediaQueries.Count == 0)
                    {
                        //no media queries actually
                        mediaValid = mediaspec.matchesEmpty();
                    }
                    else
                    {
                        //find a matching query
                        foreach (MediaQuery media in rulemedia.MediaQueries)
                        {
                            if (mediaspec.matches(media))
                            {
                                mediaValid = true;
                                break;
                            }
                        }
                    }

                    if (mediaValid)
                    {
                        // for all rules in media set
                        foreach (RuleSet ruleset in rulemedia)
                        {
                            // for all selectors in there
                            foreach (CombinedSelector s in ruleset.getSelectors())
                            {
                                insertClassified(rules, classifySelector(s), ruleset, orderCounter);
                            }
                        }
                    }
                }
                curRule++;
            }

            // logging
//            if (// log.DebugEnabled)
            {
                // log.debug("For media \"{}\" we have {} rules", mediaspec, rules.contentCount());
  //              if (// log.TraceEnabled)
                {
                    // log.trace("Detailed view: \n{}", rules);
                }
            }
        }

    }

}