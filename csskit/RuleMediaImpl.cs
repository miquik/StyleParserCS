using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace StyleParserCS.csskit
{

    using MediaQuery = StyleParserCS.css.MediaQuery;
    using RuleMedia = StyleParserCS.css.RuleMedia;
    using RuleSet = StyleParserCS.css.RuleSet;
    using StyleSheet = StyleParserCS.css.StyleSheet;

    /// <summary>
    /// Implementation of RuleMedia
    /// 
    /// @author kapy
    /// @author Jan Svercl, VUT Brno, 2008
    /// 
    /// </summary>
    public class RuleMediaImpl : AbstractRuleBlock<RuleSet>, RuleMedia
    {

        /// <summary>
        /// List of medias </summary>
        protected internal IList<MediaQuery> media;

        /// <summary>
        /// Creates an empty object to be filled by interface methods
        /// </summary>
        protected internal RuleMediaImpl()
        {
            this.media = new List<MediaQuery>();
        }

        public virtual IList<MediaQuery> MediaQueries
        {
            get
            {
                return media;
            }
        }

        public virtual RuleMedia setMediaQueries(IList<MediaQuery> medias)
        {
            this.media = medias;
            return this;
        }

        public override StyleSheet StyleSheet
        {
            set
            {
                base.StyleSheet = value;
                //assign the style sheet recursively to the contained rule sets
                foreach (RuleSet set in this)
                {
                    set.StyleSheet = value;
                }
            }
        }

        public override string ToString()
        {
            return this.ToString(0);
        }

        public virtual string ToString(int depth)
        {

            StringBuilder sb = new StringBuilder();

            // append medias
            sb = OutputUtil.appendTimes(sb, OutputUtil.DEPTH_DELIM, depth);
            sb.Append(OutputUtil.MEDIA_KEYWORD);
            sb = OutputUtil.appendList(sb, media, OutputUtil.MEDIA_DELIM);

            // append rules
            sb = OutputUtil.appendTimes(sb, OutputUtil.DEPTH_DELIM, depth);
            sb.Append(OutputUtil.RULE_OPENING);
            sb = OutputUtil.appendList(sb, this, OutputUtil.RULE_DELIM, depth + 1);
            sb.Append(OutputUtil.RULE_CLOSING);

            return sb.ToString();
        }

        /* (non-Javadoc)
		 * @see java.lang.Object#hashCode()
		 */
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = base.GetHashCode();
            result = prime * result + ((media == null) ? 0 : media.GetHashCode());
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
            if (!(obj is RuleMediaImpl))
            {
                return false;
            }
            RuleMediaImpl other = (RuleMediaImpl)obj;
            if (media == null)
            {
                if (other.media != null)
                {
                    return false;
                }
            }
            //ORIGINAL LINE: else if (!media.equals(other.media))
            else if (!media.SequenceEqual(other.media))
            {
                return false;
            }
            return true;
        }



    }

}