using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TodoApplication.ViewModels
{
    internal class ValidationViewModelBase : ViewModelBase, IDataErrorInfo
    {
        private Dictionary</*propertyName*/string, /*msg*/ string> _errors = new Dictionary<string, string>();

        public string this[string propertyName]
        {
            get
            {
                if (_errors.ContainsKey(propertyName))
                {
                    return _errors[propertyName];
                } else
                {
                    return null;
                }
            }
        }

        public string Error
        {
            get
            {
                return String.Join(" ", _errors.Values);
            }
        }


        protected void SetError(string propertyName, string errorMessage)
        {
            _errors[propertyName] = errorMessage;
        }

        protected void ResetError(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
            }
        }

    }
}
