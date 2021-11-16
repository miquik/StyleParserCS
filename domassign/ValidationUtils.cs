using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StyleParserCS.domassign
{

    /// 
    /// <summary>
    /// @author Petr Mikulík
    /// </summary>
    public class ValidationUtils
    {
        //  private static readonly Pattern AREA_REGEX = Pattern.compile("\\S+");

        public static string[] getAreas(string areasString)
        {
            ICollection<string> areas = new List<string>();

            foreach (Match m in Regex.Matches(areasString, "\\S+"))
            {
                // matcher.groups();
                // Console.WriteLine("'{0}' found at index {1}.", m.Value, m.Index);
                areas.Add(m.Value);
            }
            return areas.ToArray();
        }

        public static bool containsRectangles(string[][] map)
        {
            int height = map.Length;
            if (height < 1)
            {
                return false;
            }
            int width = map[0].Length;
            ISet<string> knownAreas = new HashSet<string>();
            //ORIGINAL LINE: bool[][] boolMap = new bool[height][width];
            // bool[][] boolMap = RectangularArrays.RectangularBoolArray(height, width);
            bool[][] boolMap = new bool[height][];
            for (int array1 = 0; array1 < height; array1++)
            {
                boolMap[array1] = new bool[width];
            }
            foreach (bool[] column in boolMap)
            {
                Array.Fill(column, false);
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (boolMap[y][x])
                    {
                        continue;
                    }
                    if (knownAreas.Contains(map[y][x]))
                    {
                        return false;
                    }
                    knownAreas.Add(map[y][x]);
                    int currWidth = getWidth(map, x, y);
                    int currHeight = getHeight(map, x, y);
                    if (isValidRectangle(map, x, y, currWidth, currHeight))
                    {
                        validateRectangle(boolMap, x, y, currWidth, currHeight);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool isValidRectangle(string[][] map, int x0, int y0, int width, int height)
        {
            //ORIGINAL LINE: final String super = map[y0][x0];
            string basev = map[y0][x0];
            for (int x = x0 + 1; x < width; x++)
            {
                for (int y = y0; y < height; y++)
                {
                    if (!map[y][x].Equals(basev))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static void validateRectangle(bool[][] map, int x0, int y0, int width, int height)
        {
            for (int x = x0; x < x0 + width; x++)
            {
                for (int y = y0; y < y0 + height; y++)
                {
                    map[y][x] = true;
                }
            }
        }

        private static int getWidth(string[][] map, int x, int y)
        {
            //ORIGINAL LINE: final String super = map[y][x];
            string basev = map[y][x];
            int width = 1;
            while (++x < map[0].Length && map[y][x].Equals(basev))
            {
                width++;
            }
            return width;
        }

        private static int getHeight(string[][] map, int x, int y)
        {
            //ORIGINAL LINE: final String super = map[y][x];
            string basev = map[y][x];
            int height = 1;
            while (++y < map.Length && map[y][x].Equals(basev))
            {
                height++;
            }
            return height;
        }

    }

}