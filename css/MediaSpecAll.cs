using System.Collections.Generic;

/// <summary>
/// MediaSpecAll.java
/// 
/// Created on 8. 7. 2014, 10:46:22 by burgetr
/// </summary>
namespace StyleParserCS.css
{

    /// <summary>
    /// A special case of media specification that matches all media queries.
    /// 
    /// @author burgetr
    /// </summary>
    public class MediaSpecAll : MediaSpec
    {

        /// <summary>
        /// Creates the media specification that matches to all media queries and expressions.
        /// </summary>
        public MediaSpecAll() : base("*")
        {
        }

        public override bool matches(MediaQuery q)
        {
            return true;
        }

        public override bool matches(MediaExpression e)
        {
            return true;
        }

        public override bool matchesOneOf(IList<MediaQuery> queries)
        {
            return queries.Count > 0; //we don't match an empty list (to be consistent)
        }

        public override string ToString()
        {
            return "(all media)";
        }

    }

}