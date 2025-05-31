using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace MiejskiDomKultury.Helpers
{
    public static class ValidationHelpers
    {
        public static void UpdateValidation(DependencyObject control, string propertyName, INotifyDataErrorInfo vm)
        {
            var errors = vm.GetErrors(propertyName) as IEnumerable<string>;

            if (errors != null && errors.Cast<string>().Any())
            {
                var error = errors.Cast<string>().First();
                var bindingExpression = BindingOperations.GetBindingExpression(control, PasswordBox.TagProperty);

                if (bindingExpression != null)
                {
                    Validation.MarkInvalid(bindingExpression, new ValidationError(new ExceptionValidationRule(), bindingExpression, error, null));
                }
            }
            else
            {
                var bindingExpression = BindingOperations.GetBindingExpression(control, PasswordBox.TagProperty);
                if (bindingExpression != null)
                {
                    Validation.ClearInvalid(bindingExpression);
                }
            }
        }
    }
}
