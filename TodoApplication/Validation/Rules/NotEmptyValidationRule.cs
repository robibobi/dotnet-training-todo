using System;
using System.Globalization;
using System.Windows.Controls;

namespace TodoApplication.Validation.Rules
{
    internal class NotEmptyValidationRule : ValidationRule
    {
        public string ErrorMessageWhenEmpty { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(value is string str)
            {
                if (!String.IsNullOrWhiteSpace(str))
                {
                    return ValidationResult.ValidResult;
                } else
                {
                    return new ValidationResult(false, ErrorMessageWhenEmpty);
                }
            } else
            {
                return new ValidationResult(false, "Value must be a string.");
            }
        }
    }
}
