namespace ChinaBM.Common
{
    using System;
    using System.Drawing;
    using System.Text.RegularExpressions;

    public static class CssKit
    {
        #region ToColor 将CSS颜色字符串转换为Color对象
        /// <summary>
        /// 将CSS颜色字符串转换为Color对象
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ToColor(string color)
        {
            int red, green, blue;
            char[] rgb;
            color = color.TrimStart('#');
            color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
            switch (color.Length)
            {
                case 3:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[0], 16);
                    green = Convert.ToInt32(rgb[1].ToString() + rgb[1], 16);
                    blue = Convert.ToInt32(rgb[2].ToString() + rgb[2], 16);
                    return Color.FromArgb(red, green, blue);
                case 6:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[1], 16);
                    green = Convert.ToInt32(rgb[2].ToString() + rgb[3], 16);
                    blue = Convert.ToInt32(rgb[4].ToString() + rgb[5], 16);
                    return Color.FromArgb(red, green, blue);
                default:
                    return Color.FromName(color);
            }
        }
        #endregion

        #region CheckColorValue 检查颜色值是否为3/6位的合法颜色
        /// <summary>
        /// 检查颜色值是否为3/6位的合法颜色
        /// </summary>
        /// <param name="targetString">待检查的颜色字符串</param>
        /// <returns></returns>
        public static bool CheckColorValue(string targetString)
        {
            if (string.IsNullOrEmpty(targetString))
            {
                return false;
            }
            targetString = targetString.Trim().Trim('#');
            if (targetString.Length != 3 && targetString.Length != 6)
            {
                return false;
            }
            //不能包含0-9  a-f以外的字符
            if (!Regex.IsMatch(targetString, "[^0-9a-f]", RegexOptions.IgnoreCase))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
