namespace StyleParserCS.csskit
{
    using StyleParserCS.css;
    using StyleSheet = StyleParserCS.css.StyleSheet;

    /// <summary>
    /// CSS style sheet, entry point.
    /// Allows 
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public class StyleSheetImpl : AbstractRule<RuleBlock>, StyleSheet
    {

        private StyleParserCS.css.StyleSheet_Origin origin;

        protected internal StyleSheetImpl()
        {
            this.origin = StyleParserCS.css.StyleSheet_Origin.AUTHOR;
        }

        public virtual StyleParserCS.css.StyleSheet_Origin Origin
        {
            set
            {
                this.origin = value;
            }
            get
            {
                return origin;
            }
        }


        protected override void InsertItem(int index, RuleBlock item)
        {
            hash = 0;
            item.StyleSheet = this;
            base.InsertItem(index, item);
        }
        /*
        public virtual void add(int index, RuleBlock element)
        {
            element.StyleSheet = this;
            base.insert(index, element);
        }

        public override bool add(RuleBlock o)
        {
            o.StyleSheet = this;
            return base.add(o);
        }
        */

    }

}