using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using TodoApplication.Converters;

namespace TodoApplication.MarkupExtensions
{
    internal class TagColorConverterExtension : MarkupExtension
    {
        private static readonly TagColorConverter Converter = new TagColorConverter();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Converter;
        }
    }
}
