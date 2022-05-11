using System;
using System.Windows.Markup;

namespace TodoApplication.MarkupExtensions
{
    internal class PlaceholderTextExtension : MarkupExtension
    {
        private readonly string _placeholderString;

        public int TextLength { get; set; }


        public PlaceholderTextExtension()
        {
            _placeholderString = "Lorem ipsum dolor sit amet, consectetur adipiscing elit," +
                " sed do eiusmod tempor incididunt ut labore et dolore magna" +
                " aliqua. Ut enim ad minim veniam, quis nostrud exercitation " +
                "ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                " Duis aute irure dolor in reprehenderit in voluptate velit " +
                "esse cillum dolore eu fugiat nulla pariatur. Excepteur sint " +
                "occaecat cupidatat non proident, sunt in culpa qui officia " +
                "deserunt mollit anim id est laborum.";

            TextLength = _placeholderString.Length;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _placeholderString.Substring(0, TextLength);
        }
    }
}
