using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Editor.Validation
{
    public class NumberRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string pattern = "^[0-9]*$";
            if (value == null || value.ToString() == "" || !new Regex(pattern).Match(value.ToString()).Success) return new ValidationResult(false, "Must be a number!");
            return new ValidationResult(true, null);
        }
    }
}