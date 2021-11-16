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
    /// Implementation of NodeData by single HashMap. Is more space efficient at the cost of 
    /// speed.
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public class SingleMapNodeData : BaseNodeDataImpl
    {

        private const int COMMON_DECLARATION_SIZE = 7;

        private IDictionary<string, Quadruple> map;

        public SingleMapNodeData()
        {
            this.map = new Dictionary<string, Quadruple>(css.TotalProperties);
        }

        public override T getProperty<T>(string name) // where T : StyleParserCS.css.CSSProperty
        {
            // until java 7 compiler is not able to infer correct type 
            // this is an ugly workaround
            return this.getProperty<T>(name, true);
        }

        public override T getProperty<T>(string name, bool includeInherited) // where T : StyleParserCS.css.CSSProperty
        {

            Quadruple q = map.GetValue(name);
            if (q == null)
            {
                return default(T);
            }

            CSSProperty tmp;

            if (includeInherited)
            {
                if (q.curProp != null)
                {
                    tmp = q.curProp;
                }
                else
                {
                    tmp = q.inhProp;
                }
            }
            else
            {
                tmp = q.curProp;
            }

            // this will cast to inferred type
            // if there is no inferred type, cast to CSSProperty is safe
            // otherwise the possibility having wrong left side of assignment
            // is roughly the same as use wrong dynamic class cast 
            //ORIGINAL LINE: @SuppressWarnings("unchecked") T retval = (T) tmp;
            // T retval = (T)tmp;
            if (tmp is T)
            {
                return (T)tmp;
            }
            return default(T);
        }

        //ORIGINAL LINE: public StyleParserCS.css.Term<?> getValue(String name, boolean includeInherited)
        public override Term getValue(string name, bool includeInherited)
        {

            Quadruple q = map.GetValue(name);
            if (q == null)
            {
                return null;
            }

            if (includeInherited)
            {
                if (q.curProp != null)
                {
                    return q.curValue;
                }
                else
                {
                    return q.inhValue;
                }
            }
            else
            {
                return q.curValue;
            }
        }

        public override T getValue<T>(Type clazz, string name)
        {
            return getValue<T>(clazz, name, true);
        }

        public override string getAsString(string name, bool includeInherited)
        {
            Quadruple q = map.GetValue(name);
            if (q == null)
            {
                return null;
            }

            CSSProperty prop = q.curProp;
            //ORIGINAL LINE: StyleParserCS.css.Term<?> value = q.curValue;
            Term value = q.curValue;
            if (prop == null && includeInherited)
            {
                prop = q.inhProp;
                value = q.inhValue;
            }
            return (value == null ? prop.ToString() : value.ToString());
        }

        public override T getValue<T>(Type clazz, string name, bool includeInherited)
        {
            Quadruple q = map.GetValue(name);
            if (q == null)
            {
                return default(T);
            }

            if (clazz != typeof(T))
            {
                return default;
            }

            if (includeInherited)
            {
                if (q.curProp != null)
                {
                    return (T)q.curValue; // clazz.cast(q.curValue);
                }
                else
                {
                    return (T)q.inhValue; // clazz.cast(q.inhValue);
                }
            }
            else
            {
                return (T)q.curValue; // clazz.cast(q.curValue);
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

            foreach (KeyValuePair<string, CSSProperty> entry in properties) //.SetOfKeyValuePairs())
            {
                //ORIGINAL LINE: final String key = entry.getKey();
                string key = entry.Key;
                Quadruple q = map.GetValue(key);
                if (q == null) 
                {
                    q = new Quadruple();
                }
                q.curProp = entry.Value;
                q.curValue = terms.GetValue(key, null);
                q.curSource = d;
                // remove operator
                if ((q.curValue != null) && (q.curValue.Operator != null))
                {
                    q.curValue = q.curValue.shallowClone().setOperator(null);
                }
                map[key] = q;
            }
            return this;

        }

        public override NodeData concretize()
        {

            foreach (KeyValuePair<string, Quadruple> entry in map) //.SetOfKeyValuePairs())
            {
                //ORIGINAL LINE: final String key = entry.getKey();
                string key = entry.Key;
                //ORIGINAL LINE: final Quadruple q = entry.getValue();
                Quadruple q = entry.Value;

                // replace current with inherited or defaults
                if (q.curProp != null)
                {
                    if (q.curProp.equalsInherit())
                    {
                        if (q.inhProp == null)
                        {
                            q.curProp = css.getDefaultProperty(key);
                        }
                        else
                        {
                            q.curProp = q.inhProp;
                            q.curSource = q.inhSource;
                        }

                        if (q.inhValue == null)
                        {
                            q.curValue = css.getDefaultValue(key);
                        }
                        else
                        {
                            q.curValue = q.inhValue;
                        }

                        map[key] = q;
                    }
                    else if (q.curProp.equalsInitial())
                    {
                        q.curProp = css.getDefaultProperty(key);
                        q.curValue = css.getDefaultValue(key);
                        map[key] = q;
                    }
                    else if (q.curProp.equalsUnset())
                    {
                        if (q.curProp.inherited())
                        {
                            if (q.inhProp == null)
                            {
                                q.curProp = css.getDefaultProperty(key);
                            }
                            else
                            {
                                q.curProp = q.inhProp;
                            }
                            if (q.inhValue == null)
                            {
                                q.curValue = css.getDefaultValue(key);
                            }
                            else
                            {
                                q.curValue = q.inhValue;
                            }
                        }
                        else
                        {
                            q.curProp = css.getDefaultProperty(key);
                            q.curValue = css.getDefaultValue(key);
                        }
                        map[key] = q;
                    }
                }
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

            if (!(parent is SingleMapNodeData))
            {
                throw new System.InvalidCastException("Cant't inherit from NodeData different from " + this.GetType().FullName + "(" + parent.GetType().FullName + ")");
            }

            SingleMapNodeData nd = (SingleMapNodeData)parent;

            // inherit values
            foreach (KeyValuePair<string, Quadruple> entry in nd.map) //.SetOfKeyValuePairs())
            {
                //ORIGINAL LINE: final String key = entry.getKey();
                string key = entry.Key;
                //ORIGINAL LINE: final Quadruple qp = entry.getValue();
                Quadruple qp = entry.Value;
                Quadruple q = map.GetValue(key);

                // create new quadruple if this do not contain one
                // for this property
                if (q == null)
                {
                    q = new Quadruple();
                }

                bool forceInherit = (q.curProp != null && q.curProp.equalsInherit());
                bool changed = false;

                //try the inherited value of the parent
                if (qp.inhProp != null && (qp.inhProp.inherited() || forceInherit))
                {
                    q.inhProp = qp.inhProp;
                    q.inhValue = qp.inhValue;
                    q.inhSource = qp.inhSource;
                    changed = true;
                }

                //try the declared property of the parent
                if (qp.curProp != null && (qp.curProp.inherited() || forceInherit))
                {
                    q.inhProp = qp.curProp;
                    q.inhValue = qp.curValue;
                    q.inhSource = qp.curSource;
                    changed = true;
                }
                // insert/replace only if contains inherited/original 
                // value			
                if (changed && !q.Empty)
                {
                    map[key] = q;
                }
            }
            return this;
        }


        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            List<string> keys = new List<string>(map.Keys);
            keys.Sort(StringComparer.Ordinal);

            foreach (string key in keys)
            {
                // always use own value if exists
                Quadruple q = map.GetValue(key);

                CSSProperty prop = q.curProp;
                if (prop == null)
                {
                    prop = q.inhProp;
                }

                //ORIGINAL LINE: StyleParserCS.css.Term<?> value = q.curValue;
                Term value = q.curValue;
                if (value == null)
                {
                    value = q.inhValue;
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
                //ORIGINAL LINE: final java.util.List<String> keys = new java.util.ArrayList<String>();
                IList<string> keys = new List<string>();
                ((List<string>)keys).AddRange(map.Keys);
                return keys;
            }
        }

        public override Declaration getSourceDeclaration(string name)
        {
            return getSourceDeclaration(name, true);
        }

        public override Declaration getSourceDeclaration(string name, bool includeInherited)
        {
            Quadruple q = map.GetValue(name);
            if (q == null)
            {
                return null;
            }
            else
            {
                if (includeInherited)
                {
                    if (q.curSource != null)
                    {
                        return q.curSource;
                    }
                    return q.inhSource;
                }
                else
                {
                    return q.curSource;
                }
            }
        }

        internal class Quadruple
        {
            internal CSSProperty inhProp = null;
            internal CSSProperty curProp = null;
            //ORIGINAL LINE: StyleParserCS.css.Term<?> inhValue = null;
            internal Term inhValue = null;
            //ORIGINAL LINE: StyleParserCS.css.Term<?> curValue = null;
            internal Term curValue = null;
            internal Declaration inhSource = null;
            internal Declaration curSource = null;

            public Quadruple()
            {
            }

            public virtual bool Empty
            {
                get
                {
                    return inhProp == null && curProp == null && inhValue == null && curValue == null;
                }
            }
        }

    }




}