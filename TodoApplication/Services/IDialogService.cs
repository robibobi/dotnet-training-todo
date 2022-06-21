using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using TodoApplication.ViewModels;
using System.Threading.Tasks;

namespace TodoApplication.Services
{
    internal interface IDialogService
    {
        void ShowManageTagsDialog(ObservableCollection<TagViewModel> tags, IEnumerable<Guid> referencedIds);
    
        void ShowErrorDailog(string message);

        Task<bool> ShowOkCancelDialog(string header, string content);
    }
}
