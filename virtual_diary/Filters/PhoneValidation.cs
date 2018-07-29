using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace virtual_diary.Filters
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    sealed public class PhoneValidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Regex regex = new Regex(@"^(\+3816|06)?([0-6]|[8-9])[0-9]{6,7}$");

            return !string.IsNullOrEmpty((string)value) && regex.IsMatch((string)value);
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        }
    }
}