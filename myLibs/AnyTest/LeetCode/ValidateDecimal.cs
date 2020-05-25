using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class ValidateDecimal
    {
        public bool IsNumber(string s)
        {
            s = s.Trim();
            if (s.Length == 0)
                return false;
            if (s.Contains(" "))
                return false;
            string[] strs = s.Split('e');
            if (strs.Length > 2)
                return false;

            string RegrexFloat = @"^(\s*[-+]?(\d+\.?\d+|\d+\.?\d*|\d*\.?\d+)(e[-+]?\d+)?\s*)$";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(RegrexFloat);
            System.Text.RegularExpressions.Match match = regex.Match(s);
            if (match.Success && match.Captures[0].Value == s)
                return true;
            return false;
        }
    }
}
