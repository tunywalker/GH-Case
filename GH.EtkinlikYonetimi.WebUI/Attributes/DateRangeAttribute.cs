using System;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Attributes
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private readonly string _startDatePropertyName;

        public DateRangeAttribute(string startDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
            ErrorMessage = "Bitiş tarihi, başlangıç tarihine eşit veya daha sonra olmalıdır.";
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var endDate = value as DateTime?;

            var startProp = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            if (startProp == null)
                return new ValidationResult($"'{_startDatePropertyName}' özelliği bulunamadı.");

            var startDate = startProp.GetValue(validationContext.ObjectInstance) as DateTime?;

            if (startDate.HasValue && endDate.HasValue)
            {
                if (endDate.Value < startDate.Value)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
