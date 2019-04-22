using System;
using System.Text.RegularExpressions;

namespace TCM.Services.Utils
{
    public class IdHelpers
    {
        public static bool IsValid(string id)
        {
            var numChk = new Regex("^[0-9]{1,8}$");
            if (!numChk.IsMatch(id)) return false;
            int clubId = Int32.Parse(id);
            return clubId > 0 ? true : false;
        }

        public static string FormatId(string id)
        {
            if (id.Length == 8) return id;
            string formattedId = id.PadLeft(8).Replace(" ", "0");
            return formattedId;
        }
    }
}
