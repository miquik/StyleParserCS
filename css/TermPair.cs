namespace StyleParserCS.css
{
    public interface TermPair<K, V> : Term<V>
    {

        /// <summary>
        /// Gets key </summary>
        /// <returns> Key stored in pair </returns>
        K Key { get; }

        /// <summary>
        /// Sets key </summary>
        /// <param name="key"> Key to be stored in pair </param>
        /// <returns> Modified object to allow chaining </returns>
        TermPair<K, V> setKey(K key);
    }

}