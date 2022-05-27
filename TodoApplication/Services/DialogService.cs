using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private static void SetDialogHostContent(object content)
        {
            DialogHost.Show(content);
        }
    }
}
