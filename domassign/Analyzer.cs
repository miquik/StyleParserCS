using AngleSharp.Dom;
using Ardalis.SmartEnum;
using StyleParserCS.utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace StyleParserCS.domassign
{
    using CSSFactory = StyleParserCS.css.CSSFactory;
    using CombinedSelector = StyleParserCS.css.CombinedSelector;
    using Declaration = StyleParserCS.css.Declaration;
    using ElementMatcher = StyleParserCS.css.ElementMatcher;
    using MatchCondition = StyleParserCS.css.MatchCondition;
    using MediaSpec = StyleParserCS.css.MediaSpec;
    using NodeData = StyleParserCS.css.NodeData;
    using RuleSet = StyleParserCS.css.RuleSet;
    using Selector = StyleParserCS.css.Selector;
    using Selector_PseudoElementType = StyleParserCS.css.Selector_PseudoElementType;
    using StyleSheet = StyleParserCS.css.StyleSheet;

    /// <summary>
    /// Analyzer allows to apply the given style to any document.
    /// During the initialization, it divides rules of stylesheet into maps accoring to
    /// medias and their type. Afterwards, it is able to return CSS declaration for any
    /// DOM tree and media. It allows to use or not to use inheritance.
    /// 
    /// @author Karel Piwko 2008
    /// @author Radek Burget 2008-2014
    /// 
    /// </summary>
    public class Analyzer
    {
        /// <summary>
        /// The style sheets to be processed. </summary>
        protected internal IList<StyleSheet> sheets;

        /// <summary>
        /// Rule order counter </summary>
        protected internal int currentOrder;

        /// <summary>
        /// Holds maps of declared rules classified into groups of
        /// HolderItem (ID, CLASS, ELEMENT, OTHER).
        /// </summary>
        protected internal Holder rules;

        private MatchCondition matchCond;
        private ElementMatcher matcher;

        /// <summary>
        /// Creates the analyzer for a single style sheet. </summary>
        /// <param name="sheet"> The stylesheet that will be used as the source of rules. </param>
        public Analyzer(StyleSheet sheet)
        {
            sheets = new List<StyleSheet>(1);
            sheets.Add(sheet);
            matchCond = CSSFactory.DefaultMatchCondition;
            matcher = CSSFactory.ElementMatcher;
        }

        /// <summary>
        /// Creates the analyzer for multiple style sheets. </summary>
        /// <param name="sheets"> A list of stylesheets that will be used as the source of rules. </param>
        public Analyzer(IList<StyleSheet> sheets)
        {
            this.sheets = sheets;
            matchCond = CSSFactory.DefaultMatchCondition;
            matcher = CSSFactory.ElementMatcher;
        }

        /// <summary>
        /// Registers a new match condition to be used for matching the elements and
        /// selector parts.
        /// </summary>
        /// <param name="matchCond">
        ///            the new match condition </param>
        public void registerMatchCondition(MatchCondition matchCond)
        {
            this.matchCond = matchCond;
        }

        /// <summary>
        /// Obtains the match condition to be used for matching the elements and
        /// selector parts.
        /// </summary>
        /// <returns> the match condition used by the Analyzer. </returns>
        public MatchCondition MatchCondition
        {
            get
            {
                return this.matchCond;
            }
        }

        /// <summary>
        /// Registers a new element matcher to be used for matching the elements and
        /// selector parts.
        /// </summary>
        /// <param name="matcher">
        ///            the new element matcher </param>
        public void registerElementMatcher(ElementMatcher matcher)
        {
            this.matcher = matcher;
        }

        /// <summary>
        /// Obtains the matcher to be used for matching the elements.
        /// </summary>
        /// <returns> the matcher used by the Analyzer. </returns>
        public ElementMatcher ElementMatcher
        {
            get
            {
                return matcher;
            }
        }

        /// <summary>
        /// Evaluates CSS properties of DOM tree
        /// </summary>
        /// <param name="doc">
        ///            IDocument tree </param>
        /// <param name="media">
        ///            Media </param>
        /// <param name="inherit">
        ///            Use inheritance </param>
        /// <returns> Map where each element contains its CSS properties </returns>
        //ORIGINAL LINE: public StyleMap evaluateDOM(org.w3c.dom.IDocument doc, StyleParserCS.css.MediaSpec media, final boolean inherit)
        public virtual StyleMap evaluateDOM(IDocument doc, MediaSpec media, bool inherit)
        {

            DeclarationMap declarations = assingDeclarationsToDOM(doc, media, inherit);

            StyleMap nodes = new StyleMap(declarations.size());

            // Traversal<StyleMap> traversal = new TraversalAnonymousInnerClass(this, doc, NodeFilter.SHOW_ELEMENT, inherit);
            Traversal<StyleMap> traversal = new TraversalAnonymousInnerClass(this, doc, (int)FilterResult.Accept, inherit, declarations);

            traversal.levelTraversal(nodes);

            return nodes;
        }

        private class TraversalAnonymousInnerClass : Traversal<StyleMap>
        {
            private readonly Analyzer outerInstance;

            private bool inherit;

            public TraversalAnonymousInnerClass(Analyzer outerInstance, IDocument doc, int filter, bool inherit, DeclarationMap declarations) 
                : base(doc, (object)declarations, filter)
            {
                this.outerInstance = outerInstance;
                this.inherit = inherit;
            }


            protected internal override void processNode(StyleMap result, INode current, object source)
            {
                NodeData main = CSSFactory.createNodeData();
                // for all declarations available in the main list (pseudo=null)
                IList<Declaration> declarations = ((DeclarationMap)source).get((IElement)current, null);
                if (declarations != null)
                {
                    foreach (Declaration d in declarations)
                    {
                        main.push(d);
                    }
                    if (inherit)
                    {
                        // INode parentNode = walker.parentNode();
                        // if ( != null)
                        // {
                        main.inheritFrom(result.get((IElement)walker.parentNode(), null));
                        // }
                    }
                }
                // concretize values and store them
                result.put((IElement)current, null, main.concretize());

                //repeat for the pseudo classes (if any)
                foreach (Selector_PseudoElementType pseudo in ((DeclarationMap)source).pseudoSet((IElement)current))
                {
                    NodeData pdata = CSSFactory.createNodeData();
                    declarations = ((DeclarationMap)source).get((IElement)current, pseudo);
                    if (declarations != null)
                    {
                        foreach (Declaration d in declarations)
                        {
                            pdata.push(d);
                        }
                        pdata.inheritFrom(main); //always inherit from the main element style
                    }
                    // concretize values and store them
                    result.put((IElement)current, pseudo, pdata.concretize());
                }

            }
        }

        //ORIGINAL LINE: public StyleMap evaluateDOM(org.w3c.dom.IDocument doc, String media, final boolean inherit)
        public virtual StyleMap evaluateDOM(IDocument doc, string media, bool inherit)
        {
            return evaluateDOM(doc, new MediaSpec(media), inherit);
        }

        /// <summary>
        /// Creates map of declarations assigned to each element of a DOM tree
        /// </summary>
        /// <param name="doc">
        ///            DOM document </param>
        /// <param name="media">
        ///            Media type to be used for declarations </param>
        /// <param name="inherit">
        ///            Inheritance (cascade propagation of values) </param>
        /// <returns> Map of elements as keys and their declarations </returns>
        //ORIGINAL LINE: protected DeclarationMap assingDeclarationsToDOM(org.w3c.dom.IDocument doc, StyleParserCS.css.MediaSpec media, final boolean inherit)
        protected internal virtual DeclarationMap assingDeclarationsToDOM(IDocument doc, MediaSpec media, bool inherit)
        {

            // classify the rules
            classifyAllSheets(media);

            // resulting map
            DeclarationMap declarations = new DeclarationMap();

            // if the holder is empty skip evaluation
            if (rules != null && !rules.Empty)
            {

                Traversal<DeclarationMap> traversal = new TraversalAnonymousInnerClass2(this, doc, (int)FilterResult.Accept); // NodeFilter.SHOW_ELEMENT);

                // list traversal will be enough
                if (!inherit)
                {
                    traversal.listTraversal(declarations);
                }
                // we will do level traversal to economize blind returning
                // in tree
                else
                {
                    traversal.levelTraversal(declarations);
                }
            }

            return declarations;
        }

        private class TraversalAnonymousInnerClass2 : Traversal<DeclarationMap>
        {
            private readonly Analyzer outerInstance;

            public TraversalAnonymousInnerClass2(Analyzer outerInstance, IDocument doc, int filter) : base(doc, (object)outerInstance.rules, filter)
            {
                this.outerInstance = outerInstance;
            }

            protected internal override void processNode(DeclarationMap result, INode current, object source)
            {
                outerInstance.assignDeclarationsToElement(result, walker, (IElement)current, (Holder)source);
            }
        }

        /// <summary>
        /// Assigns declarations to one element.
        /// </summary>
        /// <param name="declarations">
        ///            Declarations of all processed elements </param>
        /// <param name="walker">
        ///            Tree walker </param>
        /// <param name="e">
        ///            DOM IElement </param>
        /// <param name="holder">
        ///            Wrap </param>
        protected internal virtual void assignDeclarationsToElement(DeclarationMap declarations, TreeWalker walker, IElement e, Holder holder)
        {
            // create set of possible candidates applicable to given element
            // set is automatically filtered to not contain duplicates
            List<OrderedRule> candidates = new List<OrderedRule>();
            // match element classes
            foreach (string cname in matcher.elementClasses(e))
            {
                // holder contains rule with given class
                IList<OrderedRule> rules = holder.get(HolderItem.CLASS, cname.ToLower());
                if (rules != null)
                {
                    candidates.AddRange(rules);
                }
            }
            // // log.trace("After CLASSes {} total candidates.", candidates.Count);

            // match IDs
            string id = matcher.elementID(e);
            if (!string.ReferenceEquals(id, null) && id.Length != 0)
            {
                IList<OrderedRule> rules = holder.get(HolderItem.ID, id.ToLower());
                if (rules != null)
                {
                    candidates.AddRange(rules);
                }
            }
            // // log.trace("After IDs {} total candidates.", candidates.Count);

            // match elements
            string name = matcher.elementName(e);
            if (!string.ReferenceEquals(name, null))
            {
                IList<OrderedRule> rules = holder.get(HolderItem.ELEMENT, name.ToLower());
                if (rules != null)
                {
                    candidates.AddRange(rules);
                }
            }
            // // log.trace("After ELEMENTs {} total candidates.", candidates.Count);

            // others
            candidates.AddRange(holder.get(HolderItem.OTHER, null));

            // transform to list to speed up traversal
            // and sort rules in order as they were found in CSS definition
            // IList<OrderedRule> clist = new List<OrderedRule>(candidates);
            candidates.Sort();
            // candidates.Reverse();

            // // log.debug("Totally {} candidates.", candidates.Count);
            // // log.trace("With values: {}", clist);

            // resulting list of declaration for this element with no pseudo-selectors (main list)(local cache)
            IList<Declaration> eldecl = new List<Declaration>();

            // existing pseudo selectors found
            ISet<Selector_PseudoElementType> pseudos = new HashSet<Selector_PseudoElementType>();

            // for all candidates
            foreach (OrderedRule orule in candidates)
            {

                //ORIGINAL LINE: final StyleParserCS.css.RuleSet rule = orule.getRule();
                RuleSet rule = orule.Rule;
                StyleSheet sheet = rule.StyleSheet;
                if (sheet == null)
                {
                    // // log.warn("No source style sheet set for rule: {}", rule.ToString());
                }
                StyleParserCS.css.StyleSheet_Origin origin = (sheet == null) ? StyleParserCS.css.StyleSheet_Origin.AGENT : sheet.Origin;

                // for all selectors inside
                foreach (CombinedSelector s in rule.getSelectors())
                {
                    // this method does automatic rewind of walker
                    if (!matchSelector(s, e, walker))
                    {
                        //  // log.trace("CombinedSelector \"{}\" NOT matched!", s);
                        continue;
                    }

                    // // log.trace("CombinedSelector \"{}\" matched", s);

                    Selector_PseudoElementType pseudo = s.PseudoElementType;
                    StyleParserCS.css.CombinedSelector_Specificity spec = s.computeSpecificity();
                    if (pseudo == null)
                    {
                        // add to main list
                        foreach (Declaration d in rule)
                        {
                            eldecl.Add(new AssignedDeclaration(d, spec, origin));
                        }
                    }
                    else
                    {
                        // remember the pseudo element
                        pseudos.Add(pseudo);
                        // add to pseudo lists
                        foreach (Declaration d in rule)
                        {
                            declarations.addDeclaration(e, pseudo, new AssignedDeclaration(d, spec, origin));
                        }
                    }

                }
            }

            // sort declarations
            eldecl.Sort(); //sort the main list
            // log.debug("Sorted {} declarations.", eldecl.Count);
            // log.trace("With values: {}", eldecl);
            foreach (Selector_PseudoElementType p in pseudos)
            {
                declarations.sortDeclarations(e, p); //sort pseudos
            }

            // set the main list
            declarations.put(e, null, eldecl);
        }

        //ORIGINAL LINE: protected boolean elementSelectorMatches(final StyleParserCS.css.Selector s, final org.w3c.dom.IElement e)
        protected internal virtual bool elementSelectorMatches(Selector s, IElement e)
        {
            return s.matches(e, matcher, matchCond);
        }

        protected internal virtual bool matchSelector(CombinedSelector sel, IElement e, TreeWalker w)
        {

            // store current walker position
            INode current = w.CurrentNode;

            bool retval = false;
            StyleParserCS.css.Selector_Combinator combinator = null;
            // traverse simple selector backwards
            for (int i = sel.Count - 1; i >= 0; i--)
            {
                // last simple selector
                Selector s = sel[i];
                //// log.trace("Iterating loop with selector {}, combinator {}",	s, combinator);

                // decide according to combinator anti-pattern
                if (combinator == null)
                {
                    retval = this.elementSelectorMatches(s, e);
                }
                else if (combinator == StyleParserCS.css.Selector_Combinator.ADJACENT)
                {
                    IElement adjacent = (IElement)w.previousSibling();
                    retval = false;
                    if (adjacent != null)
                    {
                        retval = this.elementSelectorMatches(s, adjacent);
                    }
                }
                else if (combinator == StyleParserCS.css.Selector_Combinator.PRECEDING)
                {
                    IElement preceding;
                    retval = false;
                    while (!retval && (preceding = (IElement)w.previousSibling()) != null)
                    {
                        retval = this.elementSelectorMatches(s, preceding);
                    }
                }
                else if (combinator == StyleParserCS.css.Selector_Combinator.DESCENDANT)
                {
                    IElement ancestor;
                    retval = false;
                    while (!retval && (ancestor = (IElement)w.parentNode()) != null)
                    {
                        retval = this.elementSelectorMatches(s, ancestor);
                    }
                }
                else if (combinator == StyleParserCS.css.Selector_Combinator.CHILD)
                {
                    IElement parent = (IElement)w.parentNode();
                    retval = false;
                    if (parent != null)
                    {
                        retval = this.elementSelectorMatches(s, parent);
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

            // restore walker position
            w.CurrentNode = current;
            return retval;
        }

        /// <summary>
        /// Classifies the rules in all the style sheets. </summary>
        /// <param name="mediaspec"> The specification of the media for evaluating the media queries. </param>
        protected internal virtual void classifyAllSheets(MediaSpec mediaspec)
        {
            rules = new Holder();

            AnalyzerUtil.classifyAllSheets(sheets, rules, mediaspec);
        }

        /// <summary>
        /// Decides about holder item
        /// 
        /// @author kapy
        /// </summary>

        /*
        protected enum HolderItem
        {
            ELEMENT(0), ID(1), CLASS(2), OTHER(3);

        private int type;

        private HolderItem(int type)
        {
            this.type = type;
        }

        public int Value
        {
            return type;
        }
    }
        */


        public sealed class HolderItem : SmartEnum<HolderItem>
        {
            public static readonly HolderItem ELEMENT = new HolderItem(nameof(ELEMENT), 0);
            public static readonly HolderItem ID = new HolderItem(nameof(ID), 1);
            public static readonly HolderItem CLASS = new HolderItem(nameof(CLASS), 2);
            public static readonly HolderItem OTHER = new HolderItem(nameof(OTHER), 3);

            private HolderItem(string name, int value) : base(name, value)
            {
            }
        }

        /// <summary>
        /// Holds holder item type and key value, that is two elements that are about
        /// to be used for storing in holder
        /// 
        /// @author kapy
        /// 
        /// </summary>
        protected internal class HolderSelector
        {
            public HolderItem item;
            public string key;

            public HolderSelector(HolderItem item, string key)
            {
                this.item = item;
                this.key = key;
            }
        }

        /// <summary>
        /// Represents a ruleset and its order in the corresponding style sheet.
        /// 
        /// @author burgetr
        /// </summary>
        public sealed class OrderedRule : IComparable<OrderedRule>
        {
            internal readonly RuleSet rule;
            internal readonly int order;

            public OrderedRule(RuleSet rule, int order)
            {
                this.rule = rule;
                this.order = order;
            }

            public RuleSet Rule
            {
                get
                {
                    return rule;
                }
            }

            public int Order
            {
                get
                {
                    return order;
                }
            }

            public int CompareTo(OrderedRule o)
            {
                return Order - o.Order;
            }

            public override string ToString()
            {
                return "OR" + order + ", " + rule;
            }

        }

        /// <summary>
        /// Holds list of maps of list. This is used to classify rulesets into
        /// structure which is easily accessible by analyzator
        /// 
        /// @author kapy
        /// 
        /// </summary>
        public class Holder
        {

            /// <summary>
            /// HolderItem.* except OTHER are stored there </summary>
            internal IList<IDictionary<string, IList<OrderedRule>>> items;

            /// <summary>
            /// OTHER rules are stored there </summary>
            internal IList<OrderedRule> others;

            public Holder()
            {
                // create list of items
                this.items = new List<IDictionary<string, IList<OrderedRule>>>(HolderItem.List.Count - 1);

                // fill maps in list
                foreach (HolderItem hi in HolderItem.List)
                {
                    // this is special case, it is not map
                    if (hi == HolderItem.OTHER)
                    {
                        others = new List<OrderedRule>();
                    }
                    else
                    {
                        items.Add(new Dictionary<string, IList<OrderedRule>>());
                    }
                }
            }

            public virtual bool Empty
            {
                get
                {
                    foreach (HolderItem hi in HolderItem.List)
                    {
                        if (hi == HolderItem.OTHER)
                        {
                            if (others.Count > 0)
                            {
                                return false;
                            }
                        }
                        else if (items[hi.Value].Count > 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }


            public static Holder union(Holder one, Holder two)
            {

                Holder union = new Holder();
                if (one == null)
                {
                    one = new Holder();
                }
                if (two == null)
                {
                    two = new Holder();
                }

                foreach (HolderItem hi in HolderItem.List)
                {
                    if (hi == HolderItem.OTHER)
                    {
                        ((List<OrderedRule>)union.others).AddRange(one.others);
                        ((List<OrderedRule>)union.others).AddRange(two.others);
                    }
                    else
                    {

                        IDictionary<string, IList<OrderedRule>> oneMap, twoMap, unionMap;
                        oneMap = one.items[hi.Value];
                        twoMap = two.items[hi.Value];
                        unionMap = union.items[hi.Value];

                        //unionMap.putAll(oneMap);
                        foreach (var dp in oneMap)
                        {
                            unionMap.Add(dp);
                        }
                        foreach (string key in twoMap.Keys)
                        {
                            // map already contains this as key, append to list
                            if (unionMap.ContainsKey(key))
                            {
                                ((List<OrderedRule>)unionMap[key]).AddRange(twoMap[key]);
                            }
                            // we could directly add elements
                            else
                            {
                                unionMap[key] = twoMap[key];
                            }
                        }
                    }

                }
                return union;
            }

            /// <summary>
            /// Inserts Ruleset into group identified by HolderType, and optionally
            /// by key value
            /// </summary>
            /// <param name="item">
            ///            Identifier of holder's group </param>
            /// <param name="key">
            ///            Key, used in case other than OTHER </param>
            /// <param name="value">
            ///            Value to be store inside </param>
            public virtual void insert(HolderItem item, string key, OrderedRule value)
            {

                // check others and if so, insert item
                if (item == HolderItem.OTHER)
                {
                    others.Add(value);
                    return;
                }

                // create list if empty
                IDictionary<string, IList<OrderedRule>> map = items[item.Value];
                IList<OrderedRule> list = null;
                if (map.ContainsKey(key))
                {
                    list = map[key];
                } else
                {
                    list = new List<OrderedRule>();
                    map[key] = list;
                }
                /*
                // tocheck
                IList<OrderedRule> list = map[key];
                if (list == null)
                {
                    list = new List<OrderedRule>();
                    map[key] = list;
                }
                */
                list.Add(value);

            }

            /// <summary>
            /// Returns list of rules (ruleset) for given holder and key
            /// </summary>
            /// <param name="item">
            ///            Type of item to be returned </param>
            /// <param name="key">
            ///            Key or <code>null</code> in case of HolderItem.OTHER </param>
            /// <returns> List of rules or <code>null</code> if not found under given
            ///         combination of key and item </returns>
            public virtual IList<OrderedRule> get(HolderItem item, string key)
            {

                // check others
                if (item == HolderItem.OTHER)
                {
                    return others;
                }
                if (items[item.Value].ContainsKey(key))
                {
                    return items[item.Value][key];
                }
                return null;
                // return items[item.Value][key];
            }


            public virtual string contentCount()
            {
                StringBuilder sb = new StringBuilder();

                foreach (HolderItem hi in HolderItem.List)
                {
                    if (hi == HolderItem.OTHER)
                    {
                        sb.Append(hi.Name).Append(": ").Append(others.Count).Append(" ");
                    }
                    else
                    {
                        sb.Append(hi.Name).Append(":").Append(items[hi.Value].Count).Append(" ");
                    }

                }

                return sb.ToString();
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                foreach (HolderItem hi in HolderItem.List)
                {
                    if (hi == HolderItem.OTHER)
                    {
                        sb.Append(hi.Name).Append(" (").Append(others.Count).Append("): ").Append(others).Append("\n");
                    }
                    else
                    {
                        sb.Append(hi.Name).Append(" (").Append(items[hi.Value].Count).Append("): ").Append(items[hi.Value]).Append("\n");
                    }

                }

                return sb.ToString();
            }
        }
    }

}