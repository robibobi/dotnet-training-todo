using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using TodoApplication.ViewModels;

namespace TodoApplication.Services
{
    internal interface IDialogService
    {
        void ShowManageTagsDialog(ObservableCollection<TagViewModel> tags, IEnumerable<Guid> referencedIds);
    }
}
