using log4net;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using TodoApplication.Commands;
using TodoApplication.Models;
using TodoApplication.Respositories;
using TodoApplication.Services;

namespace TodoApplication.ViewModels
{

    internal class MainWindowViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindowViewModel));

        private readonly ITodoItemRepository _todoRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IDialogService _dialogService;
        private string _todoName;
        private TagViewModel _selectedTag;
        private TodoItemViewModel _selectedTodoItem;
        private string _searchText;

        public string TodoName
        {
            get { return _todoName; }
            set 
            { 
                _todoName = value;
                AddTodoCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(TodoName));
            }
        }

        public AsyncCommand AddTodoCommand { get; }
        public AsyncCommand<TodoItemViewModel> RemoveTodoCommand { get; }

        public AsyncCommand AddTagCommand { get; }
        public ActionCommand ShowManageTagsDialogCommand { get; }

        public ObservableCollection<TodoItemViewModel> TodoItems { get; private set; }

        public ICollectionView TodoItemsView { get; private set; }

        public ObservableCollection<TagViewModel> AvailableTags { get; private set; }

        public TagViewModel SelectedTag
        {
            get { return _selectedTag; }
            set 
            { 
                _selectedTag = value;
                AddTagCommand.RaiseCanExecuteChanged(); 
            }
        }

        public TodoItemViewModel SelectedTodoItem
        {
            get { return _selectedTodoItem; }
            set 
            { 
                _selectedTodoItem = value;
                AddTagCommand.RaiseCanExecuteChanged();
            }
        }


        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; TodoItemsView.Refresh(); }
        }


        public MainWindowViewModel(
            ITodoItemRepository repository,
            ITagRepository tagRespository,
            IDialogService dialogService)
        {
            _todoRepository = repository;
            _tagRepository = tagRespository;
            _dialogService = dialogService;

            TodoItems = new ObservableCollection<TodoItemViewModel>();
            AddTodoCommand = new AsyncCommand(AddTodo, CanAddTodo);
            RemoveTodoCommand = new AsyncCommand<TodoItemViewModel>(RemoveTodo, CanRemoveTodo);
            ShowManageTagsDialogCommand = new ActionCommand(ShowManageTagsDialog, () => true);
            AddTagCommand = new AsyncCommand(AddTagToSelectedTodoItem, CanAddTag);

        }

        private bool FilterTodoItems(object obj)
        {
            if (String.IsNullOrWhiteSpace(SearchText))
            {
                return true;
            }

            if(obj is TodoItemViewModel todoItemViewModel)
            {
                return todoItemViewModel.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) != -1;
            }
            return false;
        }

        public override async Task OnAttachedAsync()
        {
            Log.Debug("Attaching main window viemodel...");

            AvailableTags = new ObservableCollection<TagViewModel>();
            var tagsResult = await _tagRepository.GetAll();
            if (!tagsResult.WasSuccessful)
            {
                _dialogService.ShowErrorDailog(tagsResult.Message);
                Log.Error(tagsResult.Message);
                return;
            }

            foreach (var tag in tagsResult.Value)
            {
                AvailableTags.Add(new TagViewModel(tag, _tagRepository));
            }
            RaisePropertyChanged(nameof(AvailableTags));

            var todoItemsResult = await _todoRepository.GetAll();
            if (!todoItemsResult.WasSuccessful)
            {
                _dialogService.ShowErrorDailog(todoItemsResult.Message);
                Log.Error(todoItemsResult.Message);
                return;
            }

            TodoItems = new ObservableCollection<TodoItemViewModel>();
            TodoItemsView = CollectionViewSource.GetDefaultView(TodoItems);
            TodoItemsView.Filter = FilterTodoItems;
            RaisePropertyChanged(nameof(TodoItemsView));
            foreach (var item in todoItemsResult.Value)
            {
                TodoItems.Add(CreateViewModelFromTodoItem(item));
            }
            RaisePropertyChanged(nameof(TodoItems));
        }

        private void ShowManageTagsDialog()
        {
            _dialogService.ShowManageTagsDialog(AvailableTags,
                TodoItems.SelectMany(t => t.CreateModel().TagIds).Distinct());
        }

        private bool CanAddTag()
        {
            return SelectedTodoItem != null &&
                SelectedTag != null;
        }

        private async Task AddTagToSelectedTodoItem()
        {
            SelectedTodoItem.Tags.Add(new TagViewModel(SelectedTag.CreateModel(), _tagRepository));
            var updateResult = await _todoRepository.Update(SelectedTodoItem.CreateModel());
            if (!updateResult.WasSuccessful)
            {
                Log.Error(updateResult.Message);
                _dialogService.ShowErrorDailog($"Failed to add tag to todo item: {updateResult.Message}");
            }
        }


        private async Task AddTodo()
        {
            if (!String.IsNullOrEmpty(TodoName))
            {
                var vm = CreateTodoItemViewModel(TodoName);
                var model = vm.CreateModel();
                TodoItems.Add(vm);
                var addResult = await _todoRepository.Add(model);
                if (!addResult.WasSuccessful)
                {
                    Log.Error(addResult.Message);
                    _dialogService.ShowErrorDailog("Failed to add todo item: " + addResult.Message);
                }
                TodoName = String.Empty;
            }
        }

        private TodoItemViewModel CreateViewModelFromTodoItem(TodoItem item)
        {
            var todoItemViewModel = new TodoItemViewModel(item, _todoRepository);

            var tagViewModels = item.TagIds.Select(tagId => AvailableTags.Single(t => t.Id == tagId));

            todoItemViewModel.Tags = new ObservableCollection<TagViewModel>(tagViewModels);

            return todoItemViewModel;
        }

        private bool CanAddTodo()
        {
            return !String.IsNullOrEmpty(TodoName);
        }

        private async Task RemoveTodo(TodoItemViewModel vmToRemove)
        {
            var userClickedDelete = await _dialogService.ShowOkCancelDialog(
                header: Properties.Resources.DeleteTodoConfirmationDialogHeader,
                content: Properties.Resources.DeleteTodoConfirmationDialogContent);

            if (userClickedDelete && vmToRemove != null)
            {
                TodoItems.Remove(vmToRemove);
                var removeResult = await _todoRepository.Remove(vmToRemove.Id);
                if (!removeResult.WasSuccessful)
                {
                    Log.Error(removeResult.Message);
                    _dialogService.ShowErrorDailog(removeResult.Message);
                }
            }

        }

        private bool CanRemoveTodo(TodoItemViewModel vmToRemove)
        {
            return vmToRemove != null;
        }

        private TodoItemViewModel CreateTodoItemViewModel(string todoName)
        {
            return new TodoItemViewModel(Guid.NewGuid())
            {
                Name = todoName,
                TimeStamp = DateTime.Now,
                IsDone = false
            };
        }
    }
}
