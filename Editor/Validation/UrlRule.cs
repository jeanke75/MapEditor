using System.Globalization;
using System.Windows.Controls;

namespace Editor.Validation
{
    public class UrlRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || value.ToString() == "") return new ValidationResult(false, "No file selected!");
            return new ValidationResult(true, null);
        }
    }
}