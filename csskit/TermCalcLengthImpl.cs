using System.Text;

/// 
namespace StyleParserCS.csskit
{
    using TermCalc = StyleParserCS.css.TermCalc;

    /// 
    /// <summary>
    /// @author burgetr
    /// </summary>
    public class TermCalcLengthImpl : TermLengthImpl, TermCalc
    {

        private CalcArgs args;

        public TermCalcLengthImpl(CalcArgs args)
        {
            this.args = args;
        }

        public virtual CalcArgs Args
        {
            get
            {
                return args;
            }
        }

        public override float Value
        {
            get
            {
                return 0f;
            }
        }

        public override string ToString()
        {
            return OutputUtil.appendCalcArgs(new StringBuilder(OutputUtil.CALC_KEYWORD), args).ToString();
        }

    }

}