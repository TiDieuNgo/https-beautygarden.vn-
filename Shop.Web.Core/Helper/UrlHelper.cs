using System.Text.RegularExpressions;
using System.Web.Mvc;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Shop.Web.Core.Helper
{
    public static class WeBUrlHelper
    {
        public static string RejectMarks(string text)
        {
            if(string.IsNullOrEmpty(text))
            {
                return "";
            }
            text = text.Trim().ToLower();
            string[] pattern = new string[7];

            pattern[0] = "a|(á|ả|à|ạ|ã|ă|ắ|ẳ|ằ|ặ|ẵ|â|ấ|ẩ|ầ|ậ|ẫ)";

            pattern[1] = "o|(ó|ỏ|ò|ọ|õ|ô|ố|ổ|ồ|ộ|ỗ|ơ|ớ|ở|ờ|ợ|ỡ)";

            pattern[2] = "e|(é|è|ẻ|ẹ|ẽ|ê|ế|ề|ể|ệ|ễ)";

            pattern[3] = "u|(ú|ù|ủ|ụ|ũ|ư|ứ|ừ|ử|ự|ữ)";

            pattern[4] = "i|(í|ì|ỉ|ị|ĩ)";

            pattern[5] = "y|(ý|ỳ|ỷ|ỵ|ỹ)";

            pattern[6] = "d|đ";

            for (int i = 0; i < pattern.Length; i++)
            {
                // kí tự sẽ thay thế

                char replaceChar = pattern[i][0];

                MatchCollection matchs = Regex.Matches(text, pattern[i]);

                foreach (Match m in matchs)
                {
                    text = text.Replace(m.Value[0], replaceChar);
                }
            }
            text = Regex.Replace(text, @"[^\w\.@]", " ").Trim().ToLower();
            text = text.Replace("$", "");
            text = text.Replace("-", "/");
            text = text.Replace("/", "");
            text = text.Replace("^", "");
            text = text.Replace("*", "");
            text = text.Replace(",", "");
            text = text.Replace("'", "");
            text = text.Replace("|", "");
            text = text.Replace(";", "");
            text = text.Replace("#", "");
            text = text.Replace(":", "");
            text = text.Replace(".", "");
            text = text.Replace("?", "");
            text = text.Replace(" ", "-");
            text = text.Replace("&", "-");
            text = text.Replace(".", "");
            bool flag = false;
            string newText = "";
            foreach (char c in text)
            {
                if (c.ToString().Equals("-"))
                {
                    if (!flag)
                    {
                        newText += c;
                        flag = true;
                    }

                }
                else
                {
                    newText += c;
                    flag = false;
                }
            }
            return newText;
        }
        
        static public string Cat_Chuoi(int sl, string Chuoi)
        {
            if (Chuoi.Trim().Length <= sl)
                return Chuoi;
            else
            {
                for (int i = sl; i < Chuoi.Length; i++)
                    if (Chuoi[i].ToString() == " ")
                        return Chuoi.Substring(0, i) + "...";
            }
            return Chuoi;
        }
        
    } 
}
