using System.Text.RegularExpressions;

namespace BeeJee.TestTask.Backend.Extensions
{
    public static class StringExtensions
    {
        private const string EMAIL_PATTERN = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        public static bool IsEmail(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return Regex.Match(value, EMAIL_PATTERN).Success;
        }
    }
}
