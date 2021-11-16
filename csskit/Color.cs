namespace StyleParserCS.csskit
{
    /// <summary>
    /// Simple holder class for color values
    /// </summary>
    public class Color
    {

        internal readonly int value;

        //ORIGINAL LINE: public Color(final int red, final int green, final int blue)
        public Color(int red, int green, int blue) : this(red, green, blue, 255)
        {
        }

        //ORIGINAL LINE: public Color(final int red, final int green, final int blue, final int alpha)
        public Color(int red, int green, int blue, int alpha)
        {
            this.value = ((alpha & 0xFF) << 24) | ((red & 0xFF) << 16) | ((green & 0xFF) << 8) | ((blue & 0xFF));
        }

        /// <summary>
        /// Returns the RGB value representing the color.
        /// </summary>
        public virtual int RGB
        {
            get
            {
                return value;
            }
        }

        /// <summary>
        /// Returns the red value in the range 0-255.
        /// </summary>
        /// <returns> the red value. </returns>
        public virtual int Red
        {
            get
            {
                return (value >> 16) & 0xFF;
            }
        }

        /// <summary>
        /// Returns the green value in the range 0-255.
        /// </summary>
        /// <returns> the green value. </returns>
        public virtual int Green
        {
            get
            {
                return (value >> 8) & 0xFF;
            }
        }

        /// <summary>
        /// Returns the blue value in the range 0-255.
        /// </summary>
        /// <returns> the blue component. </returns>
        public virtual int Blue
        {
            get
            {
                return (value) & 0xFF;
            }
        }

        /// <summary>
        /// Returns the alpha value in the range 0-255.
        /// </summary>
        /// <returns> the alpha value. </returns>
        public virtual int Alpha
        {
            get
            {
                return (value >> 24) & 0xff;
            }
        }

        //ORIGINAL LINE: @Override public boolean equals(final Object o)
        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if (o == null || this.GetType() != o.GetType())
            {
                return false;
            }

            //ORIGINAL LINE: final Color color = (Color) o;
            Color color = (Color)o;

            return value == color.value;
        }

        public override int GetHashCode()
        {
            return value;
        }

    }

}