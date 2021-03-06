using System.Text;

/// 
namespace StyleParserCS.csskit
{
    using TermCalc = StyleParserCS.css.TermCalc;

    /// 
    /// <summary>
    /// @author burgetr
    /// </summary>
    public class TermCalcTimeImpl : TermTimeImpl, TermCalc
    {

        private CalcArgs args;

        public TermCalcTimeImpl(CalcArgs args)
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