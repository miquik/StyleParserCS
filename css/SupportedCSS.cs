using System.Collections.Generic;

namespace StyleParserCS.css
{


    /// <summary>
    /// Encapsulates supported CSS properties, their types and default values.
    /// Implementations should provide reasonable User Agent defaults.
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public interface SupportedCSS
    {

        /// <summary>
        /// Returns total number of properties defined </summary>
        /// <returns> total number of properties defined </returns>
        int TotalProperties { get; }

        /// <summary>
        /// Returns names of defined properties </summary>
        /// <returns> Set with name of defined properties </returns>
        ISet<string> DefinedPropertyNames { get; }


        /// <summary>
        /// Checks whether media type is supported </summary>
        /// <param name="media"> Name of media, such as <code>screen</code> </param>
        /// <returns> <code>true</code> if supported, <code>false</code> otherwise </returns>
        bool isSupportedMedia(string media);

        /// <summary>
        /// Checks whether property is supported </summary>
        /// <param name="property"> Name of CSS property </param>
        /// <returns> <code>true</code> if supported, <code>false</code> otherwise </returns>
        bool isSupportedCSSProperty(string property);

        /// <summary>
        /// Return default value of CSSProperty under given name </summary>
        /// <param name="propertyName"> Name of CSSProperty </param>
        /// <returns> Default value of CSSProperty or <code>null</code>
        /// when property with this name is not found </returns>
        CSSProperty getDefaultProperty(string propertyName);

        /// <summary>
        /// Some CSSProperties have even additional values. When this 
        /// happens, this functions could be used to retrieve that value.
        /// 
        /// Example: property <code>background-position</code>, 
        /// has default value of list with two percentages 0% 0%
        /// </summary>
        /// <param name="propertyName"> Name of CSSProperty </param>
        /// <returns> Default value or CSSProperty's value or <code>null</code>
        /// when property with this name is not found </returns>
        //ORIGINAL LINE: Term<?> getDefaultValue(String propertyName);
        Term getDefaultValue(string propertyName);

        /// <summary>
        /// For testing, this should get name of randomly chosen CssKeywords. </summary>
        /// <returns> Name of CSSProperty </returns>
        string RandomPropertyName { get; }

        /// <summary>
        /// Returns ordinal number of propertyName. This number must be positive integer. 
        /// This value should be unique with supported properties </summary>
        /// <param name="propertyName"> Name of property </param>
        /// <returns> ordinal number in property was found, <code>-1</code> elsewhere </returns>
        int getOrdinal(string propertyName);

        /// <summary>
        /// Returns property name according to ordinal number </summary>
        /// <param name="o"> Ordinal number previously retrieved by <code>getOrdinal()</code> </param>
        /// <returns> Name of property or <code>null</code> if not found </returns>
        string getPropertyName(int o);

    }

}