using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TodoApplication.UserControls
{
    public partial class TodoItemTag : UserControl
    {
        //public string TagText { get; set; }

        public static readonly DependencyProperty TagTextProperty =
            DependencyProperty.Register("TagText", typeof(string), typeof(TodoItemTag),
                new PropertyMetadata("- default -", (obj, args) => ((TodoItemTag)obj).TagTextChanged()));

        private void TagTextChanged()
        {
            if (TagText.StartsWith("?"))
            {
                TagTextBlock.Text = TagText.ToUpper();
            } else
            {
                TagTextBlock.Text = TagText;
            }
        }

        public string TagText
        {
            get
            {
                return (string)GetValue(TagTextProperty);
            }
            set
            {
                SetValue(TagTextProperty, value);
            }
        }


        public TodoItemTag()
        {
            InitializeComponent();
        }
    }
}
