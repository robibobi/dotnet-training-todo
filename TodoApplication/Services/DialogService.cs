using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using TodoApplication.Dialogs;
using TodoApplication.Respositories;
using TodoApplication.ViewModels;
using TodoApplication.ViewModels.Dialogs;

namespace TodoApplication.Services
{
    internal class DialogService : IDialogService
    {
        private readonly ITagRepository _tagRepository;

        public DialogService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public void ShowManageTagsDialog(
            ObservableCollection<TagViewModel> tags,
            IEnumerable<Guid> referencedIds)
        {
            var manageTagsDialog = new ManageTagsDialog();

            manageTagsDialog.DataContext = new ManageTagsDialogViewModel(tags, referencedIds, _tagRepository);

            SetDialogHostContent(manageTagsDialog);
        }

        public void ShowErrorDailog(string message)
        {
            var errorDialog = new ErrorDialog();

            errorDialog.DataContext = message;

            SetDialogHostContent(errorDialog);
        }

        public async Task<bool> ShowOkCancelDialog(string header, string content)
        {
            var tcs = new TaskCompletionSource<bool>();

            var okCancelDialog = new OkCancelDialog();
            okCancelDialog.HeaderTextBlock.Text = header;
            okCancelDialog.ContentTextBlock.Text = content;

            okCancelDialog.CancelButton.Click += (s, a) => tcs.SetResult(false);
            okCancelDialog.OkButton.Click += (s, a) => tcs.SetResult(true);

            SetDialogHostContent(okCancelDialog);


            var result = await tcs.Task;

            return result;
        }

        private static void SetDialogHostContent(object content)
        {
            DialogHost.Show(content);
        }

    }
}
