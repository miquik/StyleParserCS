using System.Collections.Generic;
using System.Text;

namespace StyleParserCS.csskit
{

    using Declaration = StyleParserCS.css.Declaration;
    using PrettyOutput = StyleParserCS.css.PrettyOutput;
    using StyleParserCS.css;
    using RuleMargin = StyleParserCS.css.RuleMargin;
    using RulePage = StyleParserCS.css.RulePage;
    using Selector = StyleParserCS.css.Selector;

    /// <summary>
    /// Wrap of declarations bounded with a page rule 
    /// 
    /// @author kapy
    /// @author Jan Svercl, VUT Brno, 2008
    /// @author Bert Frees, 2012
    /// </summary>
    public class RulePageImpl : AbstractRuleBlock<Rule>, RulePage
    {

        protected internal string name;
        protected internal StyleParserCS.css.Selector_PseudoPage pseudo;

        protected internal RulePageImpl() : base()
        {
            this.name = null;
            this.pseudo = null;
            //ORIGINAL LINE: replaceAll(new java.util.ArrayList<StyleParserCS.css.Rule<?>>());
            replaceAll((IList<Rule>)new List<Rule>());
        }

        /// <summary>
        /// Gets name of the page
        /// </summary>
        public virtual string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Sets name of the page </summary>
        /// <param name="name"> The name to set </param>
        /// <returns> Modified instance </returns>
        public virtual RulePage setName(string name)
        {
            this.name = name;
            return this;
        }

        /// <summary>
        /// Gets pseudo-class of the page
        /// </summary>
        public virtual StyleParserCS.css.Selector_PseudoPage Pseudo
        {
            get
            {
                return pseudo;
            }
        }

        /// <summary>
        /// Sets pseudo-class of the page </summary>
        /// <param name="pseudo"> The pseudo-class to set </param>
        /// <returns> Modified instance </returns>
        public virtual RulePage setPseudo(StyleParserCS.css.Selector_PseudoPage pseudo)
        {
            this.pseudo = pseudo;
            return this;
        }
        
        /*
        TOCHECK:
        public override bool add(Rule element)
        {
            if (element is Declaration || element is RuleMargin)
            {
                return base.add(element);
            }
            else
            {
                throw new System.ArgumentException("Element must be either a Declaration or a RuleMargin");
            }
        }
        */

        public override string ToString()
        {
            return this.ToString(0);
        }

        public virtual string ToString(int depth)
        {

            StringBuilder sb = new StringBuilder();

            sb.Append(OutputUtil.PAGE_KEYWORD);
            if (!string.ReferenceEquals(name, null) && !"".Equals(name))
            {
                sb.Append(OutputUtil.SPACE_DELIM).Append(name);
            }
            if (pseudo != null)
            {
                sb.Append(pseudo.ToString());
            }

            // append declarations and margin rules
            sb.Append(OutputUtil.RULE_OPENING);
            //ORIGINAL LINE: @SuppressWarnings({ "unchecked", "rawtypes" }) java.util.List<StyleParserCS.css.PrettyOutput> rules = (java.util.List)list;
            // TOCHECK
            IList<PrettyOutput> rules = (IList<PrettyOutput>)this;
            sb = OutputUtil.appendList(sb, rules, OutputUtil.EMPTY_DELIM, depth + 1);
            sb.Append(OutputUtil.RULE_CLOSING).Append(OutputUtil.PAGE_CLOSING);

            return sb.ToString();
        }

        /* (non-Javadoc)
		 * @see java.lang.Object#hashCode()
		 */
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = base.GetHashCode();
            result = prime * result + ((pseudo == null) ? 0 : pseudo.GetHashCode());
            return result;
        }

        /* (non-Javadoc)
		 * @see java.lang.Object#equals(java.lang.Object)
		 */
        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (!base.Equals(obj))
            {
                return false;
            }
            if (!(obj is RulePageImpl))
            {
                return false;
            }
            RulePageImpl other = (RulePageImpl)obj;
            if (pseudo == null)
            {
                if (other.pseudo != null)
                {
                    return false;
                }
            }
            else if (!pseudo.Equals(other.pseudo))
            {
                return false;
            }
            return true;
        }



    }

}