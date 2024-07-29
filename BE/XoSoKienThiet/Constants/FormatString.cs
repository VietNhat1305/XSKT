﻿using System.Globalization;
using System.Text;

namespace XoSoKienThiet.Constants
{
    public static class FormatterString
    {
        public static string ConvertToUnsign(string text)
        {
            if (text == null)
                return "";
            text = text.ToLower().Trim();
            string[] arr1 = new string[]
            {
                "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é", "è", "ẻ", "ẽ", "ẹ", "ê", "ế", "ề", "ể", "ễ", "ệ",
                "í", "ì", "ỉ", "ĩ", "ị",
                "ó", "ò", "ỏ", "õ", "ọ", "ô", "ố", "ồ", "ổ", "ỗ", "ộ", "ơ", "ớ", "ờ", "ở", "ỡ", "ợ",
                "ú", "ù", "ủ", "ũ", "ụ", "ư", "ứ", "ừ", "ử", "ữ", "ự",
                "ý", "ỳ", "ỷ", "ỹ", "ỵ","\t", "\n","," ,"\t" , "\n"
            };
            string[] arr2 = new string[]
            {
                "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e", "e", "e", "e", "e", "e", "e", "e", "e", "e", "e",
                "i", "i", "i", "i", "i",
                "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o",
                "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u",
                "y", "y", "y", "y", "y","","","","",""
            };
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
            }
            return text.ToUpper().Replace(" ", "");
        }
        
        
        
        public static string HandlerSortBy(string text)
        {
            if (text == null)
                return null;
            return text.Substring(0, 1).ToUpper() + text.Substring(1);
        }

        public static bool HasDiacritics(string value)
        {
            if (value == null) return false;

            var normalize = value.Normalize(NormalizationForm.FormD);

            var sb = new StringBuilder();

            foreach (var t in normalize.Where(t => CharUnicodeInfo.GetUnicodeCategory(t) != UnicodeCategory.NonSpacingMark))
                sb.Append(t);

            return (sb.ToString() != value);
        }






    }
}