using System.Collections.Generic;

namespace StyleParserCS.csskit
{

    using TermColor = StyleParserCS.css.TermColor;
    using TermColor_Keyword = StyleParserCS.css.TermColor_Keyword;

    /// <summary>
    /// Holds colors by their names.
    /// Ignores System Colors are they are deprecated in CSS3
    /// TODO Consider possibility of implementing SystemColors
    /// @author kapy
    /// @author burgetr
    /// </summary>
    public class ColorCard
    {

        private static readonly IDictionary<string, TermColor> map = new Dictionary<string, TermColor>(149);

        static ColorCard()
        {
            map["transparent"] = new TermColorKeywordImpl(TermColor_Keyword.TRANSPARENT, 0x00, 0x00, 0x00, 0x00);
            map["currentcolor"] = new TermColorKeywordImpl(TermColor_Keyword.CURRENT_COLOR, 0x00, 0x00, 0x00, 0xff);

            map["aliceblue"] = new TermColorImpl(0xf0, 0xf8, 0xff);
            map["antiquewhite"] = new TermColorImpl(0xfa, 0xeb, 0xd7);
            map["aqua"] = new TermColorImpl(0x00, 0xff, 0xff);
            map["aquamarine"] = new TermColorImpl(0x7f, 0xff, 0xd4);
            map["azure"] = new TermColorImpl(0xf0, 0xff, 0xff);
            map["beige"] = new TermColorImpl(0xf5, 0xf5, 0xdc);
            map["bisque"] = new TermColorImpl(0xff, 0xe4, 0xc4);
            map["black"] = new TermColorImpl(0x00, 0x00, 0x00);
            map["blanchedalmond"] = new TermColorImpl(0xff, 0xeb, 0xcd);
            map["blue"] = new TermColorImpl(0x00, 0x00, 0xff);
            map["blueviolet"] = new TermColorImpl(0x8a, 0x2b, 0xe2);
            map["brown"] = new TermColorImpl(0xa5, 0x2a, 0x2a);
            map["bUriywood"] = new TermColorImpl(0xde, 0xb8, 0x87);
            map["cadetblue"] = new TermColorImpl(0x5f, 0x9e, 0xa0);
            map["chartreuse"] = new TermColorImpl(0x7f, 0xff, 0x00);
            map["chocolate"] = new TermColorImpl(0xd2, 0x69, 0x1e);
            map["coral"] = new TermColorImpl(0xff, 0x7f, 0x50);
            map["cornflowerblue"] = new TermColorImpl(0x64, 0x95, 0xed);
            map["cornsilk"] = new TermColorImpl(0xff, 0xf8, 0xdc);
            map["crimson"] = new TermColorImpl(0xdc, 0x14, 0x3c);
            map["cyan"] = new TermColorImpl(0x00, 0xff, 0xff);
            map["darkblue"] = new TermColorImpl(0x00, 0x00, 0x8b);
            map["darkcyan"] = new TermColorImpl(0x00, 0x8b, 0x8b);
            map["darkgoldenrod"] = new TermColorImpl(0xb8, 0x86, 0x0b);
            map["darkgray"] = new TermColorImpl(0xa9, 0xa9, 0xa9);
            map["darkgreen"] = new TermColorImpl(0x00, 0x64, 0x00);
            map["darkgrey"] = new TermColorImpl(0xa9, 0xa9, 0xa9);
            map["darkkhaki"] = new TermColorImpl(0xbd, 0xb7, 0x6b);
            map["darkmagenta"] = new TermColorImpl(0x8b, 0x00, 0x8b);
            map["darkolivegreen"] = new TermColorImpl(0x55, 0x6b, 0x2f);
            map["darkorange"] = new TermColorImpl(0xff, 0x8c, 0x00);
            map["darkorchid"] = new TermColorImpl(0x99, 0x32, 0xcc);
            map["darkred"] = new TermColorImpl(0x8b, 0x00, 0x00);
            map["darksalmon"] = new TermColorImpl(0xe9, 0x96, 0x7a);
            map["darkseagreen"] = new TermColorImpl(0x8f, 0xbc, 0x8f);
            map["darkslateblue"] = new TermColorImpl(0x48, 0x3d, 0x8b);
            map["darkslategray"] = new TermColorImpl(0x2f, 0x4f, 0x4f);
            map["darkslategrey"] = new TermColorImpl(0x2f, 0x4f, 0x4f);
            map["darkturquoise"] = new TermColorImpl(0x00, 0xce, 0xd1);
            map["darkviolet"] = new TermColorImpl(0x94, 0x00, 0xd3);
            map["deeppink"] = new TermColorImpl(0xff, 0x14, 0x93);
            map["deepskyblue"] = new TermColorImpl(0x00, 0xbf, 0xff);
            map["dimgray"] = new TermColorImpl(0x69, 0x69, 0x69);
            map["dimgrey"] = new TermColorImpl(0x69, 0x69, 0x69);
            map["dodgerblue"] = new TermColorImpl(0x1e, 0x90, 0xff);
            map["firebrick"] = new TermColorImpl(0xb2, 0x22, 0x22);
            map["floralwhite"] = new TermColorImpl(0xff, 0xfa, 0xf0);
            map["forestgreen"] = new TermColorImpl(0x22, 0x8b, 0x22);
            map["fuchsia"] = new TermColorImpl(0xff, 0x00, 0xff);
            map["gainsboro"] = new TermColorImpl(0xdc, 0xdc, 0xdc);
            map["ghostwhite"] = new TermColorImpl(0xf8, 0xf8, 0xff);
            map["gold"] = new TermColorImpl(0xff, 0xd7, 0x00);
            map["goldenrod"] = new TermColorImpl(0xda, 0xa5, 0x20);
            map["gray"] = new TermColorImpl(0x80, 0x80, 0x80);
            map["green"] = new TermColorImpl(0x00, 0x80, 0x00);
            map["greenyellow"] = new TermColorImpl(0xad, 0xff, 0x2f);
            map["grey"] = new TermColorImpl(0x80, 0x80, 0x80);
            map["honeydew"] = new TermColorImpl(0xf0, 0xff, 0xf0);
            map["hotpink"] = new TermColorImpl(0xff, 0x69, 0xb4);
            map["indianred"] = new TermColorImpl(0xcd, 0x5c, 0x5c);
            map["indigo"] = new TermColorImpl(0x4b, 0x00, 0x82);
            map["ivory"] = new TermColorImpl(0xff, 0xff, 0xf0);
            map["khaki"] = new TermColorImpl(0xf0, 0xe6, 0x8c);
            map["lavender"] = new TermColorImpl(0xe6, 0xe6, 0xfa);
            map["lavenderblush"] = new TermColorImpl(0xff, 0xf0, 0xf5);
            map["lawngreen"] = new TermColorImpl(0x7c, 0xfc, 0x00);
            map["lemonchiffon"] = new TermColorImpl(0xff, 0xfa, 0xcd);
            map["lightblue"] = new TermColorImpl(0xad, 0xd8, 0xe6);
            map["lightcoral"] = new TermColorImpl(0xf0, 0x80, 0x80);
            map["lightcyan"] = new TermColorImpl(0xe0, 0xff, 0xff);
            map["lightgoldenrodyellow"] = new TermColorImpl(0xfa, 0xfa, 0xd2);
            map["lightgray"] = new TermColorImpl(0xd3, 0xd3, 0xd3);
            map["lightgreen"] = new TermColorImpl(0x90, 0xee, 0x90);
            map["lightgrey"] = new TermColorImpl(0xd3, 0xd3, 0xd3);
            map["lightpink"] = new TermColorImpl(0xff, 0xb6, 0xc1);
            map["lightsalmon"] = new TermColorImpl(0xff, 0xa0, 0x7a);
            map["lightseagreen"] = new TermColorImpl(0x20, 0xb2, 0xaa);
            map["lightskyblue"] = new TermColorImpl(0x87, 0xce, 0xfa);
            map["lightslategray"] = new TermColorImpl(0x77, 0x88, 0x99);
            map["lightslategrey"] = new TermColorImpl(0x77, 0x88, 0x99);
            map["lightsteelblue"] = new TermColorImpl(0xb0, 0xc4, 0xde);
            map["lightyellow"] = new TermColorImpl(0xff, 0xff, 0xe0);
            map["lime"] = new TermColorImpl(0x00, 0xff, 0x00);
            map["limegreen"] = new TermColorImpl(0x32, 0xcd, 0x32);
            map["linen"] = new TermColorImpl(0xfa, 0xf0, 0xe6);
            map["magenta"] = new TermColorImpl(0xff, 0x00, 0xff);
            map["maroon"] = new TermColorImpl(0x80, 0x00, 0x00);
            map["mediumaquamarine"] = new TermColorImpl(0x66, 0xcd, 0xaa);
            map["mediumblue"] = new TermColorImpl(0x00, 0x00, 0xcd);
            map["mediumorchid"] = new TermColorImpl(0xba, 0x55, 0xd3);
            map["mediumpurple"] = new TermColorImpl(0x93, 0x70, 0xdb);
            map["mediumseagreen"] = new TermColorImpl(0x3c, 0xb3, 0x71);
            map["mediumslateblue"] = new TermColorImpl(0x7b, 0x68, 0xee);
            map["mediumspringgreen"] = new TermColorImpl(0x00, 0xfa, 0x9a);
            map["mediumturquoise"] = new TermColorImpl(0x48, 0xd1, 0xcc);
            map["mediumvioletred"] = new TermColorImpl(0xc7, 0x15, 0x85);
            map["midnightblue"] = new TermColorImpl(0x19, 0x19, 0x70);
            map["mintcream"] = new TermColorImpl(0xf5, 0xff, 0xfa);
            map["mistyrose"] = new TermColorImpl(0xff, 0xe4, 0xe1);
            map["moccasin"] = new TermColorImpl(0xff, 0xe4, 0xb5);
            map["navajowhite"] = new TermColorImpl(0xff, 0xde, 0xad);
            map["navy"] = new TermColorImpl(0x00, 0x00, 0x80);
            map["oldlace"] = new TermColorImpl(0xfd, 0xf5, 0xe6);
            map["olive"] = new TermColorImpl(0x80, 0x80, 0x00);
            map["olivedrab"] = new TermColorImpl(0x6b, 0x8e, 0x23);
            map["orange"] = new TermColorImpl(0xff, 0xa5, 0x00);
            map["orangered"] = new TermColorImpl(0xff, 0x45, 0x00);
            map["orchid"] = new TermColorImpl(0xda, 0x70, 0xd6);
            map["palegoldenrod"] = new TermColorImpl(0xee, 0xe8, 0xaa);
            map["palegreen"] = new TermColorImpl(0x98, 0xfb, 0x98);
            map["paleturquoise"] = new TermColorImpl(0xaf, 0xee, 0xee);
            map["palevioletred"] = new TermColorImpl(0xdb, 0x70, 0x93);
            map["papayawhip"] = new TermColorImpl(0xff, 0xef, 0xd5);
            map["peachpuff"] = new TermColorImpl(0xff, 0xda, 0xb9);
            map["peru"] = new TermColorImpl(0xcd, 0x85, 0x3f);
            map["pink"] = new TermColorImpl(0xff, 0xc0, 0xcb);
            map["plum"] = new TermColorImpl(0xdd, 0xa0, 0xdd);
            map["powderblue"] = new TermColorImpl(0xb0, 0xe0, 0xe6);
            map["purple"] = new TermColorImpl(0x80, 0x00, 0x80);
            map["red"] = new TermColorImpl(0xff, 0x00, 0x00);
            map["rosybrown"] = new TermColorImpl(0xbc, 0x8f, 0x8f);
            map["royalblue"] = new TermColorImpl(0x41, 0x69, 0xe1);
            map["saddlebrown"] = new TermColorImpl(0x8b, 0x45, 0x13);
            map["salmon"] = new TermColorImpl(0xfa, 0x80, 0x72);
            map["sandybrown"] = new TermColorImpl(0xf4, 0xa4, 0x60);
            map["seagreen"] = new TermColorImpl(0x2e, 0x8b, 0x57);
            map["seashell"] = new TermColorImpl(0xff, 0xf5, 0xee);
            map["sienna"] = new TermColorImpl(0xa0, 0x52, 0x2d);
            map["silver"] = new TermColorImpl(0xc0, 0xc0, 0xc0);
            map["skyblue"] = new TermColorImpl(0x87, 0xce, 0xeb);
            map["slateblue"] = new TermColorImpl(0x6a, 0x5a, 0xcd);
            map["slategray"] = new TermColorImpl(0x70, 0x80, 0x90);
            map["slategrey"] = new TermColorImpl(0x70, 0x80, 0x90);
            map["snow"] = new TermColorImpl(0xff, 0xfa, 0xfa);
            map["springgreen"] = new TermColorImpl(0x00, 0xff, 0x7f);
            map["steelblue"] = new TermColorImpl(0x46, 0x82, 0xb4);
            map["tan"] = new TermColorImpl(0xd2, 0xb4, 0x8c);
            map["teal"] = new TermColorImpl(0x00, 0x80, 0x80);
            map["thistle"] = new TermColorImpl(0xd8, 0xbf, 0xd8);
            map["tomato"] = new TermColorImpl(0xff, 0x63, 0x47);
            map["turquoise"] = new TermColorImpl(0x40, 0xe0, 0xd0);
            map["violet"] = new TermColorImpl(0xee, 0x82, 0xee);
            map["wheat"] = new TermColorImpl(0xf5, 0xde, 0xb3);
            map["white"] = new TermColorImpl(0xff, 0xff, 0xff);
            map["whitesmoke"] = new TermColorImpl(0xf5, 0xf5, 0xf5);
            map["yellow"] = new TermColorImpl(0xff, 0xff, 0x00);
            map["yellowgreen"] = new TermColorImpl(0x9a, 0xcd, 0x32);
        }

        /// <summary>
        /// Return color by its name </summary>
        /// <param name="name"> Name of color </param>
        /// <returns> Color if found, <code>null</code> otherwise </returns>
        public static TermColor getTermColor(string name)
        {
            if (map.ContainsKey(name.ToLower()))
            {
                return map[name.ToLower()];
            }
            return null;
        }

    }

}