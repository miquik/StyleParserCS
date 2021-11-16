using System;
using System.Collections.Generic;

/// <summary>
/// BaseNodeDataImpl.java
/// Created on 29.12.2016 22:39 by Radek Burget
/// </summary>
namespace StyleParserCS.domassign
{
    using CSSFactory = StyleParserCS.css.CSSFactory;
    using CSSProperty = StyleParserCS.css.CSSProperty;
    using NodeData = StyleParserCS.css.NodeData;
    using SupportedCSS = StyleParserCS.css.SupportedCSS;
    using StyleParserCS.css;
    using TermColor = StyleParserCS.css.TermColor;
    using TermColor_Keyword = StyleParserCS.css.TermColor_Keyword;
    using TermFactory = StyleParserCS.css.TermFactory;
    using TermList = StyleParserCS.css.TermList;
    using TermPropertyValue = StyleParserCS.css.TermPropertyValue;
    using DeclarationTransformer = StyleParserCS.csskit.DeclarationTransformer;

    /// <summary>
    /// A common base for all the NodeData implementations. 
    /// @author burgetr
    /// </summary>
    public abstract class BaseNodeDataImpl : NodeData
    {
        public abstract Declaration getSourceDeclaration(string name, bool includeInherited);
        public abstract Declaration getSourceDeclaration(string name);
        public abstract ICollection<string> PropertyNames { get; }
        public abstract NodeData push(StyleParserCS.css.Declaration d);
        public abstract NodeData concretize();
        public abstract NodeData inheritFrom(NodeData parent);
        public abstract string getAsString(string name, bool includeInherited);
        public abstract T getValue<T>(Type clazz, string name, bool includeInherited);
        public abstract T getValue<T>(Type clazz, string name);
        //ORIGINAL LINE: public abstract StyleParserCS.css.Term<JavaToDotNetGenericWildcard> getValue(String name, boolean includeInherited);
        public abstract Term getValue(string name, bool includeInherited);
        public abstract T getProperty<T>(string name, bool includeInherited);
        public abstract T getProperty<T>(string name);

        protected internal static DeclarationTransformer transformer = CSSFactory.DeclarationTransformer;
        protected internal static SupportedCSS css = CSSFactory.SupportedCSS;

        public virtual T getSpecifiedProperty<T>(string name) // where T : StyleParserCS.css.CSSProperty
        {
            T prop = getProperty<T>(name, true);
            if (prop == null)
            {
                //ORIGINAL LINE: @SuppressWarnings("unchecked") T def = (T) css.getDefaultProperty(name);
                T def = (T)css.getDefaultProperty(name);
                return def;
            }
            else
            {
                return prop;
            }
        }

        //ORIGINAL LINE: @Override public StyleParserCS.css.Term<?> getSpecifiedValue(String name)
        public virtual Term getSpecifiedValue(string name)
        {
            //ORIGINAL LINE: StyleParserCS.css.Term<?> ret = getValue(name, true);
            Term ret = getValue(name, true);
            if (ret == null)
            {
                ret = css.getDefaultValue(name);
            }

            if (ret != null)
            {
                if (ret is TermColor && ((TermColor)ret).Keyword == TermColor_Keyword.CURRENT_COLOR)
                {
                    //clone the value fot setting the current color
                    //ORIGINAL LINE: final StyleParserCS.css.TermFactory tf = StyleParserCS.css.CSSFactory.getTermFactory();
                    TermFactory tf = CSSFactory.TermFactory;
                    ret = (Term)tf.createColor(tf.createIdent("currentColor"));
                    //set the current color value
                    TermColor cvalue = getValue<TermColor>(typeof(TermColor), "color", true);
                    if (cvalue == null)
                    {
                        cvalue = (TermColor)css.getDefaultValue("color");
                    }
                    ((TermColor)ret).setValue(cvalue.Value);
                }
            }

            return ret;
        }

        public virtual T getSpecifiedValue<T>(Type clazz, string name)
        {
            // Object
            // return clazz.cast(getSpecifiedValue(name));
            // TOCHECK! Cast!!
            if (clazz != typeof(T))
            {
                return default;
            }
            return (T)getSpecifiedValue(name);
        }

        public virtual T getProperty<T>(string name, int index) // where T : StyleParserCS.css.CSSProperty
        {
            return getProperty<T>(name, index, true);
        }

        public virtual T getProperty<T>(string name, int index, bool includeInherited) // where T : StyleParserCS.css.CSSProperty
        {
            //ORIGINAL LINE: final StyleParserCS.css.TermList list = getValue(StyleParserCS.css.TermList.class, name, includeInherited);
            TermList list = getValue<TermList>(typeof(TermList), name, includeInherited);
            if (list != null && index < list.Count)
            {
                //ORIGINAL LINE: final StyleParserCS.css.TermPropertyValue pair = (StyleParserCS.css.TermPropertyValue) list.get(index);
                TermPropertyValue pair = (TermPropertyValue)list[index];
                //ORIGINAL LINE: @SuppressWarnings("unchecked") T ret = (T) pair.getKey();
                T ret = (T)pair.Key;
                return ret;
            }
            else
            {
                return default;
            }
        }

        //ORIGINAL LINE: @Override public StyleParserCS.css.Term<?> getValue(String name, int index, boolean includeInherited)
        public virtual Term getValue(string name, int index, bool includeInherited)
        {
            //ORIGINAL LINE: final StyleParserCS.css.TermList list = getValue(StyleParserCS.css.TermList.class, name, includeInherited);
            TermList list = getValue<TermList>(typeof(TermList), name, includeInherited);
            if (list != null && index < list.Count)
            {
                //ORIGINAL LINE: final StyleParserCS.css.TermPropertyValue pair = (StyleParserCS.css.TermPropertyValue) list.get(index);
                TermPropertyValue pair = (TermPropertyValue)list[index];
                return pair.Value;
            }
            else
            {
                return null;
            }
        }

        public virtual T getValue<T>(Type clazz, string name, int index)
        {
            return getValue<T>(clazz, name, index, true);
        }

        public virtual T getValue<T>(Type clazz, string name, int index, bool includeInherited)
        {
            //ORIGINAL LINE: final StyleParserCS.css.TermList list = getValue(StyleParserCS.css.TermList.class, name, includeInherited);
            TermList list = getValue<TermList>(typeof(TermList), name, includeInherited);
            if (list != null && index < list.Count)
            {
                //ORIGINAL LINE: final StyleParserCS.css.TermPropertyValue pair = (StyleParserCS.css.TermPropertyValue) list.get(index);
                TermPropertyValue pair = (TermPropertyValue)list[index];
                if (clazz != typeof(T))
                {
                    return default;
                }
                return (T)pair.Value;// clazz.cast(pair.Value);
            }
            else
            {
                return default;
            }
        }

        public virtual int getListSize(string name, bool includeInherited)
        {
            //ORIGINAL LINE: final StyleParserCS.css.TermList list = getValue(StyleParserCS.css.TermList.class, name, includeInherited);
            TermList list = getValue<TermList>(typeof(TermList), name, includeInherited);
            if (list != null)
            {
                return list.Count;
            }
            else
            {
                return 0;
            }
        }

    }

}