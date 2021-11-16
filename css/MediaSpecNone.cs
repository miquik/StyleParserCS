using System.Collections.Generic;

/// <summary>
/// MediaSpecNone.java
/// 
/// Created on 8. 7. 2014, 10:51:43 by burgetr
/// </summary>
namespace StyleParserCS.css
{

    /// <summary>
    /// A specific case of media specification that does not match any media query or expression.
    /// @author burgetr
    /// </summary>
    public class MediaSpecNone : MediaSpec
    {

        /// <summary>
        /// Creates the media specification that does not match any media query or expression.
        /// </summary>
        public MediaSpecNone() : base("!")
        {
        }

        public override bool matches(MediaQuery q)
        {
            return false;
        }

        public override bool matches(MediaExpression e)
        {
            return false;
        }

        public override bool matchesOneOf(IList<MediaQuery> queries)
        {
            return false;
        }

        public override bool matchesEmpty()
        {
            return false;
        }

        public override string ToString()
        {
            return "(no media)";
        }

    }

}