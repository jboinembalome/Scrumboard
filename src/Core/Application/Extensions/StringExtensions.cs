using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrumboard.Application.Extensions
{
    public static class StringExtensions
    {
        public static string GetInitials(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var nameSplit = value.Trim().Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            var initials = "";

            foreach (var item in nameSplit)
                initials += item.Substring(0, 1);

            return initials.ToUpper();
        }
    }
}
