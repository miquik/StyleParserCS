using System;

namespace StyleParserCS.css
{

    /// <summary>
    /// Contains imports associated with medias. Acts as collection
    /// of associated medias
    /// 
    /// @author kapy
    /// </summary>
    [Obsolete]
    public interface RuleImport : RuleBlock<string>, PrettyOutput
    {

        /// <summary>
        /// Gets URI of import rule file </summary>
        /// <returns> URI of file to be imported </returns>
        string URI { get; }

        /// <summary>
        /// Sets URI of import rule </summary>
        /// <param name="uri"> URI of file to be imported </param>
        RuleImport setURI(string uri);

    }

}