using System.Collections;
using System.Collections.Generic;

namespace StyleParserCS.css
{
    public interface Rule
    {
        Rule replaceAll(IList replacement);
        Rule unlock();
        IList asList();
    }


    public interface Rule<T> : IList<T>, Rule
    {
        /// <summary>
        /// Replaces all elements stored inside. Replaces changes
        /// whole collection, so it can be used to unlock immutable object.
        /// </summary>
        /// <param name="replacement"> New list </param>
        /// <returns> Modified collection </returns>
        Rule<T> replaceAll(IList<T> replacement);

        /// <summary>
        /// Unlocks immutable object by changing collection from
        /// immutable to mutable </summary>
        /// <returns> Modified collection </returns>
        new Rule<T> unlock();

        /// <summary>
        /// Returns underlying collection as list. This list is shared
        /// with Rule, so can be directly modified </summary>
        /// <returns> Underlying collection </returns>
        new IList<T> asList();
    }

}