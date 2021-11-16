using StyleParserCS.utils;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// MultiMap.java
/// 
/// Created on 31.1.2010, 21:20:00 by radek
/// </summary>
namespace StyleParserCS.domassign
{

    /// <summary>
    /// This is a general map that assigns the data D to two keys E (Element) and P (Pseudo element).
    /// The element is mandatory, the PseudoElement key value may be null. When P is null, the structure 
    /// behaves as a simple map called a main map. Other values of PseudoElement create so-called pseudo
    /// maps. The map is optimized to provide the best performance for the main map.
    /// 
    /// @author burgetr
    /// </summary>
    public abstract class MultiMap<E, P, D>
    {
        private Dictionary<E, D> mainMap; //main map for no pseudo-elements
        private Dictionary<E, Dictionary<P, D>> pseudoMaps; //maps for the individual pseudo-elements

        /// <summary>
        /// Creates an empty map
        /// </summary>
        public MultiMap()
        {
            mainMap = new Dictionary<E, D>();
            pseudoMaps = new Dictionary<E, Dictionary<P, D>>();
        }

        /// <summary>
        /// Creates an empty map
        /// </summary>
        public MultiMap(int initialSize)
        {
            mainMap = new Dictionary<E, D>(initialSize);
            pseudoMaps = new Dictionary<E, Dictionary<P, D>>();
        }

        /// <summary>
        /// Creates a new instance of the data value.
        /// This is only used by <seealso cref="MultiMap.getOrCreate(object, object)"/>.
        /// </summary>
        protected internal abstract D createDataInstance();

        /// <summary>
        /// Obtains the size of the main map (where P is null) </summary>
        /// <returns> the number of elements in the main map. </returns>
        public virtual int size()
        {
            return mainMap.Count;
        }

        /// <summary>
        /// Gets the data for the given element and pseudo-element. </summary>
        /// <param name="el"> the element </param>
        /// <param name="pseudo"> a pseudo-element or null, if no pseudo-element is required </param>
        /// <returns> the stored data </returns>
        public virtual D get(E el, P pseudo)
        {
            if (el == null)
            {
                return default(D);
            }
            D ret;
            if (pseudo == null)
            {
                ret = mainMap.GetValue(el);
            }
            else
            {
                Dictionary<P, D> map = pseudoMaps.GetValue(el);
                if (map == null)
                {
                    ret = default(D);
                }
                else
                {
                    ret = map.GetValue(pseudo);
                }
            }
            return ret;
        }

        /// <summary>
        /// Gets the data for the given element and no pseudo-element </summary>
        /// <param name="el"> the element </param>
        /// <returns> the stored data </returns>
        public virtual D get(E el)
        {
            return mainMap.GetValue(el);
        }

        /// <summary>
        /// Gets the data or creates an empty list if it does not exist yet. </summary>
        /// <param name="el"> the element </param>
        /// <param name="pseudo"> a pseudo-element or null, if no pseudo-element is required </param>
        /// <returns> the stored data </returns>
        public virtual D getOrCreate(E el, P pseudo)
        {
            D ret;
            if (pseudo == null)
            {
                ret = mainMap.GetValue(el);
                if (ret == null)
                {
                    ret = createDataInstance();
                    mainMap[el] = ret;
                }
            }
            else
            {
                Dictionary<P, D> map = pseudoMaps.GetValue(el);
                if (map == null)
                {
                    map = new Dictionary<P, D>();
                    pseudoMaps[el] = map;
                }
                ret = map.GetValue(pseudo);
                if (ret == null)
                {
                    ret = createDataInstance();
                    map[pseudo] = ret;
                }
            }
            return ret;
        }



        /// <summary>
        /// Sets the data for the specified element and pseudo-element. </summary>
        /// <param name="el"> the element to which the data belongs </param>
        /// <param name="pseudo"> a pseudo-element or null of none is required </param>
        /// <param name="data"> data to be set </param>
        public virtual void put(E el, P pseudo, D data)
        {
            if (pseudo == null)
            {
                mainMap[el] = data;
            }
            else
            {
                Dictionary<P, D> map = pseudoMaps.GetValue(el);
                if (map == null)
                {
                    map = new Dictionary<P, D>();
                    pseudoMaps[el] = map;
                }
                map[pseudo] = data;
            }
        }

        /// <summary>
        /// Gets all the keys (elements) of the main map. </summary>
        /// <returns> A set of elements contained in the map. </returns>
        public virtual ISet<E> keySet()
        {
            return mainMap.Keys.ToHashSet();
        }


        /// <summary>
        /// Gets all the pseudo elements that are available for the given element. </summary>
        /// <param name="el"> The given element </param>
        /// <returns> A set of all pseudo elements available for the element </returns>
        public virtual ISet<P> pseudoSet(E el)
        {
            Dictionary<P, D> map = pseudoMaps.GetValue(el);
            if (map == null)
            {
                return new HashSet<P>(); // Collections.emptySet();
            }
            else
            {
                return map.Keys.ToHashSet();
            }
        }

        /// <summary>
        /// Checks if the given pseudo element is available for the given element </summary>
        /// <param name="el"> The element </param>
        /// <param name="pseudo"> The tested pseudo element </param>
        /// <returns> true when there is some value associated with the given pair </returns>
        public virtual bool hasPseudo(E el, P pseudo)
        {
            Dictionary<P, D> map = pseudoMaps.GetValue(el);
            if (map == null)
            {
                return false;
            }
            else
            {
                return map.ContainsKey(pseudo);
            }
        }

    }

}