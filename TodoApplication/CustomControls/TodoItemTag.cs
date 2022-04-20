using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TodoApplication.Exceptions;

namespace TodoApplication.CustomControls
{
    [TemplatePart(Name = TagTextBlockName, Type = typeof(TextBlock))]
    internal class TodoItemTag : Control
    {
        private const string TagTextBlockName = "CustomTagTextBlock";

        public static readonly DependencyProperty TagTextProperty =
            DependencyProperty.Register("TagText", typeof(string),
                typeof(TodoItemTag), new PropertyMetadata("- default -", OnTagTextChanged));

        public static readonly DependencyProperty RemoveTagCommandProperty =
            DependencyProperty.Register("RemoveTagCommand", typeof(ICommand),
                typeof(TodoItemTag));

        public static readonly DependencyProperty RemoveTagCommandParameterProperty =
            DependencyProperty.Register("RemoveTagCommandParameter", typeof(object),
                typeof(TodoItemTag));


        private static void OnTagTextChanged(
            DependencyObject todoItemTag,
            DependencyPropertyChangedEventArgs e)
        {
            ((TodoItemTag)todoItemTag).UpdateTagTextBlock();
        }

        private TextBlock _tagTextBlock;

        public string TagText
        {
            get { return (string)GetValue(TagTextProperty); }
            set { SetValue(TagTextProperty, value); }
        }
        public ICommand RemoveTagCommand
        {
            get { return (ICommand)GetValue(RemoveTagCommandProperty); }
            set { SetValue(RemoveTagCommandProperty, value); }
        }
        public object RemoveTagCommandParameter
        {
            get { return (object)GetValue(RemoveTagCommandParameterProperty); }
            set { SetValue(RemoveTagCommandParameterProperty, value); }
        }

        static TodoItemTag()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TodoItemTag),
                new FrameworkPropertyMetadata(typeof(TodoItemTag)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _tagTextBlock = Template.FindName(TagTextBlockName, this) as TextBlock;
            if(_tagTextBlock == null)
            {
                throw new MissingTemplatePartException($"Could not find element with name '{TagTextBlockName}' of type Textblock in the template.");
            }
            UpdateTagTextBlock();
        }


        private void UpdateTagTextBlock()
        {
            if(_tagTextBlock == null)
            {
                return;
            }


            if (TagText.StartsWith("?"))
            {
                _tagTextBlock.Text = TagText.Trim('?').ToUpper();
            }
            else
            {
                _tagTextBlock.Text = TagText;
            }
        }

    }
}
