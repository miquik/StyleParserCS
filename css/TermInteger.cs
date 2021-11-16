namespace StyleParserCS.css
{
    /// <summary>
    /// Holds integer value 
    /// @author kapy
    /// 
    /// </summary>
    public interface TermInteger : TermLength
    {

        int IntValue { get; }

        TermInteger setValue(int value);

    }

}