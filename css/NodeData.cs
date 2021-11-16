using System;
using System.Collections.Generic;

namespace StyleParserCS.css
{

    /// <summary>
    /// Wrap of CSS properties defined for element. Enumeration values follows this
    /// syntax:
    /// 
    /// <ul style="list-style:none"> <li>
    /// <b>UPPERCASE</b> terminal symbols, direct values present in stylesheet, such
    /// as background-color: transparent;</li> <li>
    /// <b>lowercase</b> non-terminal symbols, just information that concrete value
    /// is stored somewhere else</li> </ul>
    /// 
    /// @author kapy
    /// @author burgetr
    /// </summary>
    public interface NodeData
    {

        /// <summary>
        /// Returns property of given name and supposed type. Inherited properties
        /// are included.
        /// </summary>
        /// @param <T>
        ///            Type of property returned. Let compiler infer returning type 
        ///            from left part of statement, otherwise return just CSSProperty </param>
        /// <param name="name">
        ///            Name of property </param>
        /// <returns> Value of property of type T or <code>null</code> </returns>
        T getProperty<T>(string name);

        /// <summary>
        /// Returns property of given name and supposed type. Inherited properties
        /// can be avoided
        /// </summary>
        /// @param <T>
        ///            Type of property returned. Let compiler infer returning type 
        ///            from left part of statement, otherwise return just CSSProperty </param>
        /// <param name="name">
        ///            Name of property </param>
        /// <param name="includeInherited">
        ///            Whether to include inherited properties or not </param>
        /// <returns> Value of property of type T or <code>null</code> </returns>
        T getProperty<T>(string name, bool includeInherited);

        /// <summary>
        /// Returns n-th property of given name and supposed type. Inherited properties
        /// are included. This is used for properties of the <seealso cref="ValueType.LIST"/> type
        /// which can have multiple values.
        /// </summary>
        /// @param <T>
        ///            Type of property returned. Let compiler infer returning type 
        ///            from left part of statement, otherwise return just CSSProperty </param>
        /// <param name="name">
        ///            Name of property </param>
        /// <param name="index">
        ///            Property value index </param>
        /// <returns> Value of property of type T or <code>null</code> </returns>
        T getProperty<T>(string name, int index);

        /// <summary>
        /// Returns n-th property of given name and supposed type. Inherited properties
        /// are included. This is used for properties of the <seealso cref="ValueType.LIST"/> type
        /// which can have multiple values.
        /// </summary>
        /// @param <T>
        ///            Type of property returned. Let compiler infer returning type 
        ///            from left part of statement, otherwise return just CSSProperty </param>
        /// <param name="name">
        ///            Name of property </param>
        /// <param name="index">
        ///            Property value index </param>
        /// <param name="includeInherited">
        ///            Whether to include inherited properties or not </param>
        /// <returns> Value of property of type T or <code>null</code> </returns>
        T getProperty<T>(string name, int index, bool includeInherited);

        /// <summary>
        /// Returns the property of the given name after applying the defaulting processes.
        /// </summary>
        /// <param name="name">
        ///     Property name </param>
        /// <returns> The specified value of the property of the type T or {@code null} if the
        /// value is not available and no default value is applicable. </returns>
        T getSpecifiedProperty<T>(string name);

        /// <summary>
        /// Returns the <em>cascaded value</em> of property of given name.
        /// Inherited values can be avoided.
        /// </summary>
        /// <param name="name">
        ///            Name of property </param>
        /// <param name="includeInherited">
        ///            Whether to include inherited properties or not </param>
        /// <returns> Value of property or <code>null</code> </returns>
        //ORIGINAL LINE: public Term<?> getValue(String name, boolean includeInherited);
        Term getValue(string name, bool includeInherited);

        /// <summary>
        /// Returns the <em>cascaded value</em> of property of given name and supposed type.
        /// Inherited values can be avoided.
        /// </summary>
        /// @param <T>
        ///            Type of value returned </param>
        /// <param name="clazz">
        ///            Class of type </param>
        /// <param name="name">
        ///            Name of property </param>
        /// <returns> Value of property of type T or <code>null</code> </returns>
        //ORIGINAL LINE: public <T extends Term<?>> T getValue(Class<T> clazz, String name);
        T getValue<T>(Type clazz, string name);

        /// <summary>
        /// Returns the <em>cascaded value</em> of property of given name and supposed type.
        /// Inherited values can be avoided.
        /// </summary>
        /// @param <T>
        ///            Type of value returned </param>
        /// <param name="clazz">
        ///            Class of type </param>
        /// <param name="name">
        ///            Name of property </param>
        /// <param name="includeInherited">
        ///            Whether to include inherited properties or not </param>
        /// <returns> Value of property of type T or <code>null</code> </returns>
        //ORIGINAL LINE: public <T extends Term<?>> T getValue(Class<T> clazz, String name, boolean includeInherited);
        T getValue<T>(Type clazz, string name, bool includeInherited);

        /// <summary>
        /// Returns the <em>cascaded value</em> of a list property of given name.
        /// Inherited values can be avoided.
        /// </summary>
        /// <param name="name">
        ///            Name of property </param>
        /// <param name="index">
        ///            Property value index </param>
        /// <param name="includeInherited">
        ///            Whether to include inherited properties or not </param>
        /// <returns> Value of property or <code>null</code> </returns>
        //ORIGINAL LINE: public Term<?> getValue(String name, int index, boolean includeInherited);
        Term getValue(string name, int index, bool includeInherited);

        /// <summary>
        /// Returns the <em>cascaded value</em> of a list property of given name and supposed type.
        /// Inherited values can be avoided.
        /// </summary>
        /// @param <T>
        ///            Type of value returned </param>
        /// <param name="clazz">
        ///            Class of type </param>
        /// <param name="name">
        ///            Name of property </param>
        /// <param name="index">
        ///            Property value index </param>
        /// <returns> Value of property of type T or <code>null</code> </returns>
        //ORIGINAL LINE: public <T extends Term<?>> T getValue(Class<T> clazz, String name, int index);
        T getValue<T>(Type clazz, string name, int index);

        /// <summary>
        /// Returns the <em>cascaded value</em> of a list property of given name and supposed type.
        /// Inherited values can be avoided.
        /// </summary>
        /// @param <T>
        ///            Type of value returned </param>
        /// <param name="clazz">
        ///            Class of type </param>
        /// <param name="name">
        ///            Name of property </param>
        /// <param name="index">
        ///            Property value index </param>
        /// <param name="includeInherited">
        ///            Whether to include inherited properties or not </param>
        /// <returns> Value of property of type T or <code>null</code> </returns>
        //ORIGINAL LINE: public <T extends Term<?>> T getValue(Class<T> clazz, String name, int index, boolean includeInherited);
        T getValue<T>(Type clazz, string name, int index, bool includeInherited);

        /// <summary>
        /// Returns the <em>specified value</em> of a property which corresponds to the
        /// cascaded value (obtained by <seealso cref="NodeData.getValue(string, bool)"/>) with
        /// applying the defaulting processes. </summary>
        /// <param name="name"> the property name </param>
        /// <returns> the property value or {@code null} when the property value is not defined
        /// and no default value is available. </returns>
        //ORIGINAL LINE: public Term<?> getSpecifiedValue(String name);
        Term getSpecifiedValue(string name);

        /// <summary>
        /// Returns the <em>specified value</em> of a property which corresponds to the
        /// cascaded value (obtained by <seealso cref="NodeData.getValue(string, bool)"/>) with
        /// applying the defaulting processes.
        /// </summary>
        /// <param name="clazz"> the expected class of the result </param>
        /// <param name="name"> the property name </param>
        /// <returns> the property value or {@code null} when the property value is not defined
        /// and no default value is available. </returns>
        //ORIGINAL LINE: public <T extends Term<?>> T getSpecifiedValue(Class<T> clazz, String name);
        T getSpecifiedValue<T>(Type clazz, string name);

        /// <summary>
        /// Returns a string representation of the property value.
        /// </summary>
        /// <param name="name">
        ///             Property name </param>
        /// <param name="includeInherited">
        ///             Whether to include inherited properties or not </param>
        /// <returns> The string representation of the assigned property value
        /// or {@code null} when the property is not defined. </returns>
        string getAsString(string name, bool includeInherited);

        /// <summary>
        /// For list properties, obtains the number of property values in the nested list.
        /// </summary>
        /// <param name="name"> property name </param>
        /// <param name="includeInherited"> Whether to include inherited properties or not </param>
        /// <returns> the number of property values defined in the list </returns>
        int getListSize(string name, bool includeInherited);

        /// <summary>
        /// Accepts values from parent as its own. <code>null</code> parent is
        /// allowed, than instance is returned unchanged.
        /// </summary>
        /// <param name="parent">
        ///            Source of inheritance </param>
        /// <returns> Modified instance </returns>
        /// <exception cref="ClassCastException"> When parent implementation class is not the same </exception>
        //ORIGINAL LINE: public NodeData inheritFrom(NodeData parent) throws ClassCastException;
        NodeData inheritFrom(NodeData parent);

        /// <summary>
        /// Replaces all {@code inherit}, {@code initial} and {@code unset} CSS properties 
        /// with the inherited values or default values of user agent.
        /// </summary>
        /// <returns> Modified property </returns>
        NodeData concretize();

        /// <summary>
        /// Adds data stored in declaration into current instance
        /// </summary>
        /// <param name="d">
        ///            Declaration to be added </param>
        /// <returns> Modified instance
        ///  </returns>
        NodeData push(Declaration d);

        /// <summary>
        /// Returns the names of all the that are available in the current node.
        /// </summary>
        /// <returns> the name of the properties. </returns>
        ICollection<string> PropertyNames { get; }

        /// <summary>
        /// Obtains the source declaration used for the given property. Inherited properties are included. </summary>
        /// <param name="name"> The property name. </param>
        /// <returns> the source declaration </returns>
        Declaration getSourceDeclaration(string name);

        /// <summary>
        /// Obtains the source declaration used for the given property. </summary>
        /// <param name="name"> The property name. </param>
        /// <param name="includeInherited"> whether to include the inherited properties. </param>
        /// <returns> the source declaration </returns>
        Declaration getSourceDeclaration(string name, bool includeInherited);

    }

}