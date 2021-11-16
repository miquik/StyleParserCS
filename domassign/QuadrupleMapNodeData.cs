using System;
using System.Collections.Generic;
using System.Text;

namespace StyleParserCS.domassign
{

    using CSSProperty = StyleParserCS.css.CSSProperty;
    using Declaration = StyleParserCS.css.Declaration;
    using NodeData = StyleParserCS.css.NodeData;
    using StyleParserCS.css;
    using OutputUtil = StyleParserCS.csskit.OutputUtil;
    using StyleParserCS.utils;

    /// <summary>
    /// Implementation of NodeData by four distinct HashMaps. According to tests,
    /// it is about 25% faster then SingleDataMap when retrieving values and inheriting 
    /// but occupies up to 100% more memory.
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public class QuadrupleMapNodeData : BaseNodeDataImpl
    {

        private const int COMMON_DECLARATION_SIZE = 7;

        private IDictionary<string, CSSProperty> propertiesOwn;
        private IDictionary<string, CSSProperty> propertiesInh;
        //ORIGINAL LINE: private java.util.Map<String,StyleParserCS.css.Term<?>> valuesOwn;
        private IDictionary<string, Term> valuesOwn;
        //ORIGINAL LINE: private java.util.Map<String,StyleParserCS.css.Term<?>> valuesInh;
        private IDictionary<string, Term> valuesInh;
        private IDictionary<string, Declaration> sourcesOwn;
        private IDictionary<string, Declaration> sourcesInh;

        public QuadrupleMapNodeData()
        {
            this.propertiesOwn = new Dictionary<string, CSSProperty>(css.TotalProperties);
            this.propertiesInh = new Dictionary<string, CSSProperty>(css.TotalProperties);
            //ORIGINAL LINE: this.valuesOwn = new java.util.HashMap<String, StyleParserCS.css.Term<?>>(css.getTotalProperties(), 1.0f);
            this.valuesOwn = new Dictionary<string, Term>(css.TotalProperties);
            //ORIGINAL LINE: this.valuesInh = new java.util.HashMap<String, StyleParserCS.css.Term<?>>(css.getTotalProperties(), 1.0f);
            this.valuesInh = new Dictionary<string, Term>(css.TotalProperties);
            this.sourcesOwn = new Dictionary<string, Declaration>(css.TotalProperties);
            this.sourcesInh = new Dictionary<string, Declaration>(css.TotalProperties);
        }


        public override T getProperty<T>(string name) // where T : StyleParserCS.css.CSSProperty
        {
            return this.getProperty<T>(name, true);
        }

        public override T getProperty<T>(string name, bool includeInherited) // where T : StyleParserCS.css.CSSProperty
        {

            CSSProperty inh = null, tmp = null;

            if (includeInherited)
            {
                inh = propertiesInh.GetValue(name);
            }

            tmp = propertiesOwn.GetValue(name);
            if (tmp == null)
            {
                tmp = inh;
            }

            // this will cast to inferred type
            // if there is no inferred type, cast to CSSProperty is safe
            // otherwise the possibility having wrong left side of assignment
            // is roughly the same as use wrong dynamic class cast 
            //ORIGINAL LINE: @SuppressWarnings("unchecked") T retval = (T) tmp;
            T retval = (T)tmp;
            return retval;
        }

        //ORIGINAL LINE: public StyleParserCS.css.Term<?> getValue(String name, boolean includeInherited)
        public override Term getValue(string name, bool includeInherited)
        {

            if (includeInherited)
            {
                //ORIGINAL LINE: final StyleParserCS.css.Term<?> own = valuesOwn.get(name);
                Term own = valuesOwn.GetValue(name);
                if (own != null)
                {
                    return own;
                }
                else
                {
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> inherited = null;
                    Term inherited = null;
                    if (!propertiesOwn.ContainsKey(name))
                    {
                        inherited = valuesInh.GetValue(name);
                    }
                    return inherited;
                }
            }
            else
            {
                return valuesOwn.GetValue(name);
            }
        }

        public override T getValue<T>(Type clazz, string name, bool includeInherited)
        {

            if (includeInherited)
            {
                //ORIGINAL LINE: final T own = clazz.cast(valuesOwn.get(name));
                if (clazz != typeof(T))
                {
                    return default;
                }
                T own = (T)valuesOwn.GetValue(name); // clazz.cast(valuesOwn[name]);
                if (own != null)
                {
                    return own;
                }
                else
                {
                    T inherited = default;
                    if (!propertiesOwn.ContainsKey(name))
                    {
                        inherited = (T)valuesInh.GetValue(name); // clazz.cast(valuesInh[name]);
                    }
                    return inherited;
                }
            }
            else
            {
                return (T)valuesOwn.GetValue(name); // clazz.cast(valuesOwn[name]);
            }
        }

        public override T getValue<T>(Type clazz, string name)
        {
            return getValue<T>(clazz, name, true);
        }

        public override string getAsString(string name, bool includeInherited)
        {
            bool usedInherited = false;
            CSSProperty prop = propertiesOwn.GetValue(name);
            if (prop == null && includeInherited)
            {
                prop = propertiesInh.GetValue(name);
                usedInherited = true;
            }
            if (prop == null)
            {
                return null;
            }
            else if (prop.ToString().Length > 0)
            {
                return prop.ToString();
            }
            else
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> val = usedInherited ? valuesInh.get(name) : valuesOwn.get(name);
                Term val = usedInherited ? valuesInh.GetValue(name) : valuesOwn.GetValue(name);
                return (val == null) ? null : val.ToString();
            }
        }

        public override NodeData push(Declaration d)
        {

            IDictionary<string, CSSProperty> properties = new Dictionary<string, CSSProperty>(COMMON_DECLARATION_SIZE);
            //ORIGINAL LINE: java.util.Map<String,StyleParserCS.css.Term<?>> terms = new java.util.HashMap<String, StyleParserCS.css.Term<?>>(COMMON_DECLARATION_SIZE);
            IDictionary<string, Term> terms = new Dictionary<string, Term>(COMMON_DECLARATION_SIZE);

            bool result = transformer.parseDeclaration(d, properties, terms);

            // in case of false do not insert anything
            if (!result)
            {
                return this;
            }

            //set the sources and store the properties
            foreach (KeyValuePair<string, CSSProperty> entry in properties) //.SetOfKeyValuePairs())
            {
                propertiesOwn[entry.Key] = entry.Value;
                sourcesOwn[entry.Key] = d;
            }

            // remove operators from terms and store the values
            //ORIGINAL LINE: for(java.util.Map.Entry<String,StyleParserCS.css.Term<?>> entry: terms.entrySet())
            foreach (KeyValuePair<string, Term> entry in terms) // .SetOfKeyValuePairs())
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = entry.getValue();
                Term t = entry.Value;
                if (t.Operator != null)
                {
                    t = t.shallowClone().setOperator(null);
                }
                valuesOwn[entry.Key] = t;
            }

            return this;

        }

        //ORIGINAL LINE: public StyleParserCS.css.NodeData inheritFrom(StyleParserCS.css.NodeData parent) throws ClassCastException
        public override NodeData inheritFrom(NodeData parent)
        {

            if (parent == null)
            {
                return this;
            }

            if (!(parent is QuadrupleMapNodeData))
            {
                throw new System.InvalidCastException("Cant't inherit from NodeData different from " + this.GetType().FullName + "(" + parent.GetType().FullName + ")");
            }

            QuadrupleMapNodeData nd = (QuadrupleMapNodeData)parent;

            // inherit values
            foreach (string key in nd.propertiesInh.Keys)
            {
                CSSProperty value = nd.propertiesInh.GetValue(key);
                CSSProperty cur = this.propertiesOwn.GetValue(key);
                if (value.inherited() || (cur != null && cur.equalsInherit()))
                {
                    this.propertiesInh[key] = value;
                    // remove old value to be sure
                    this.valuesInh.Remove(key);
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term = nd.valuesInh.get(key);
                    Term term = nd.valuesInh.GetValue(key);
                    if (term != null)
                    {
                        this.valuesInh[key] = term;
                    }
                    Declaration src = nd.sourcesInh.GetValue(key);
                    if (src != null)
                    {
                        this.sourcesInh[key] = src;
                    }
                }
            }

            foreach (string key in nd.propertiesOwn.Keys)
            {
                CSSProperty value = nd.propertiesOwn.GetValue(key);
                CSSProperty cur = this.propertiesOwn.GetValue(key);
                if (value.inherited() || (cur != null && cur.equalsInherit()))
                {
                    this.propertiesInh[key] = value;
                    // remove old value to be sure
                    this.valuesInh.Remove(key);
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term = nd.valuesOwn.get(key);
                    Term term = nd.valuesOwn.GetValue(key);
                    if (term != null)
                    {
                        this.valuesInh[key] = term;
                    }
                    Declaration src = nd.sourcesOwn.GetValue(key);
                    if (src != null)
                    {
                        this.sourcesInh[key] = src;
                    }
                }
            }

            return this;
        }

        public override NodeData concretize()
        {

            // inherited firstly, replace them with defaults
            foreach (string key in propertiesInh.Keys)
            {
                CSSProperty p = propertiesInh.GetValue(key);
                if (p.equalsInherit())
                {
                    propertiesInh[key] = css.getDefaultProperty(key);
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> value = css.getDefaultValue(key);
                    Term value = css.getDefaultValue(key);
                    if (value != null)
                    {
                        valuesInh[key] = value;
                    }
                }

            }

            // own after, replace them with inherited or default
            foreach (string key in propertiesOwn.Keys)
            {
                CSSProperty p = propertiesOwn.GetValue(key);
                if (p.equalsInherit())
                {
                    CSSProperty rp = propertiesInh.GetValue(key);
                    if (rp == null)
                    {
                        rp = css.getDefaultProperty(key);
                    }

                    propertiesOwn[key] = rp;

                    //ORIGINAL LINE: StyleParserCS.css.Term<?> value = valuesInh.get(key);
                    Term value = valuesInh.GetValue(key);
                    if (value == null)
                    {
                        value = css.getDefaultValue(key);
                    }
                    if (value != null)
                    {
                        valuesOwn[key] = value;
                    }

                    Declaration source = sourcesInh.GetValue(key);
                    if (source != null)
                    {
                        sourcesOwn[key] = source;
                    }
                }
                else if (p.equalsInitial())
                {
                    CSSProperty rp = css.getDefaultProperty(key);
                    propertiesOwn[key] = rp;
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> value = css.getDefaultValue(key);
                    Term value = css.getDefaultValue(key);
                    if (value != null)
                    {
                        valuesOwn[key] = value;
                    }
                }
                else if (p.equalsUnset())
                {
                    if (p.inherited())
                    {
                        CSSProperty rp = propertiesInh.GetValue(key);
                        if (rp == null)
                        {
                            rp = css.getDefaultProperty(key);
                        }
                        propertiesOwn[key] = rp;

                        //ORIGINAL LINE: StyleParserCS.css.Term<?> value = valuesInh.get(key);
                        Term value = valuesInh.GetValue(key);
                        if (value == null)
                        {
                            value = css.getDefaultValue(key);
                        }
                        if (value != null)
                        {
                            valuesOwn[key] = value;
                        }
                    }
                    else
                    {
                        CSSProperty rp = css.getDefaultProperty(key);
                        propertiesOwn[key] = rp;
                        //ORIGINAL LINE: StyleParserCS.css.Term<?> value = css.getDefaultValue(key);
                        Term value = css.getDefaultValue(key);
                        if (value != null)
                        {
                            valuesOwn[key] = value;
                        }
                    }
                }
            }

            return this;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            List<string> keys = new List<string>();
            keys.AddRange(propertiesInh.Keys);
            keys.AddRange(propertiesOwn.Keys);
            // ISet<string> tmp = new LinkedHashSet<string>();
            // tmp.addAll(propertiesInh.Keys);
            // tmp.addAll(propertiesOwn.Keys);

            // IList<string> keys = new List<string>(tmp);
            keys.Sort(StringComparer.Ordinal);

            foreach (string key in keys)
            {
                // always use own value if exists
                CSSProperty prop = propertiesOwn.GetValue(key);
                if (prop == null)
                {
                    prop = propertiesInh.GetValue(key);
                }

                //ORIGINAL LINE: StyleParserCS.css.Term<?> value = valuesOwn.get(key);
                Term value = valuesOwn.GetValue(key);
                if (value == null)
                {
                    value = valuesInh.GetValue(key);
                }

                sb.Append(key).Append(OutputUtil.PROPERTY_OPENING);

                if (value != null)
                {
                    sb.Append(value.ToString());
                }
                else
                {
                    sb.Append(prop.ToString());
                }

                sb.Append(OutputUtil.PROPERTY_CLOSING);

            }

            return sb.ToString();
        }

        public override ICollection<string> PropertyNames
        {
            get
            {
                List<string> keys = new List<string>();
                keys.AddRange(propertiesInh.Keys);
                keys.AddRange(propertiesOwn.Keys);
                // ISet<string> tmp = new LinkedHashSet<string>();
                // tmp.addAll(propertiesInh.Keys);
                // tmp.addAll(propertiesOwn.Keys);

                // IList<string> keys = new List<string>(tmp);
                keys.Sort(StringComparer.Ordinal);

                return keys;
            }
        }

        public override Declaration getSourceDeclaration(string name)
        {
            return sourcesOwn.GetValue(name);
        }

        public override Declaration getSourceDeclaration(string name, bool includeInherited)
        {
            Declaration ret = sourcesOwn.GetValue(name);
            if (includeInherited && ret == null)
            {
                ret = sourcesInh.GetValue(name);
            }
            return ret;
        }


        /* (non-Javadoc)
		 * @see java.lang.Object#hashCode()
		 */
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + ((propertiesInh == null) ? 0 : propertiesInh.GetHashCode());
            result = prime * result + ((propertiesOwn == null) ? 0 : propertiesOwn.GetHashCode());
            result = prime * result + ((valuesInh == null) ? 0 : valuesInh.GetHashCode());
            result = prime * result + ((valuesOwn == null) ? 0 : valuesOwn.GetHashCode());
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
            if (obj == null)
            {
                return false;
            }
            if (!(obj is QuadrupleMapNodeData))
            {
                return false;
            }
            QuadrupleMapNodeData other = (QuadrupleMapNodeData)obj;
            if (propertiesInh == null)
            {
                if (other.propertiesInh != null)
                {
                    return false;
                }
            }
            else if (!propertiesInh.Equals(other.propertiesInh))
            {
                return false;
            }
            if (propertiesOwn == null)
            {
                if (other.propertiesOwn != null)
                {
                    return false;
                }
            }
            else if (!propertiesOwn.Equals(other.propertiesOwn))
            {
                return false;
            }
            if (valuesInh == null)
            {
                if (other.valuesInh != null)
                {
                    return false;
                }
            }
            else if (!valuesInh.Equals(other.valuesInh))
            {
                return false;
            }
            if (valuesOwn == null)
            {
                if (other.valuesOwn != null)
                {
                    return false;
                }
            }
            else if (!valuesOwn.Equals(other.valuesOwn))
            {
                return false;
            }
            return true;
        }



    }

}