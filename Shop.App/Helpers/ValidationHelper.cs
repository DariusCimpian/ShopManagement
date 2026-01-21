using System.Text.RegularExpressions;

namespace Shop.App.Helpers
{
    public static class ValidationHelper
    {
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        private static readonly Regex StrongPasswordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$");

        public static bool AreFieldsEmpty(params string[] fields)
        {
            foreach (var field in fields)
            {
                if (string.IsNullOrWhiteSpace(field))
                    return true;
            }
            return false;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return EmailRegex.IsMatch(email);
        }

        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            return StrongPasswordRegex.IsMatch(password);
        }
    }
}