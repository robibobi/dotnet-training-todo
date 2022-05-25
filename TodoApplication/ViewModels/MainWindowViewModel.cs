using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TodoApplication.Commands;
using TodoApplication.Models;
using TodoApplication.Respositories;
using TodoApplication.Services;

namespace TodoApplication.ViewModels
{

    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly ITodoItemRepository _todoRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IDialogService _dialogService;
        private string _todoName;
        private TagViewModel _selectedTag;
        private TodoItemViewModel _selectedTodoItem;

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

        public ActionCommand AddTodoCommand { get; }
        public ActionCommand<TodoItemViewModel> RemoveTodoCommand { get; }

        public ActionCommand AddTagCommand { get; }
        public ActionCommand ShowManageTagsDialogCommand { get; }

        public ObservableCollection<TodoItemViewModel> TodoItems { get; private set; }

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

        public MainWindowViewModel(
            ITodoItemRepository repository,
            ITagRepository tagRespository,
            IDialogService dialogService,
            ILoggingService loggingService)
        {
            _todoRepository = repository;
            _tagRepository = tagRespository;
            _dialogService = dialogService;

            AddTodoCommand = new ActionCommand(AddTodo, CanAddTodo);
            RemoveTodoCommand = new ActionCommand<TodoItemViewModel>(RemoveTodo, CanRemoveTodo);
            ShowManageTagsDialogCommand = new ActionCommand(ShowManageTagsDialog, () => true);
            AddTagCommand = new ActionCommand(AddTagToSelectedTodoItem, CanAddTag);

        }

        public override async Task OnAttachedAsync()
        {
            AvailableTags = new ObservableCollection<TagViewModel>();
            var tags = await _tagRepository.GetAll();
            foreach (var tag in tags)
            {
                AvailableTags.Add(new TagViewModel(tag, _tagRepository));
            }
            RaisePropertyChanged(nameof(AvailableTags));

            TodoItems = new ObservableCollection<TodoItemViewModel>();
            foreach (var item in _todoRepository.GetAll())
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

        private void AddTagToSelectedTodoItem()
        {
            SelectedTodoItem.Tags.Add(new TagViewModel(SelectedTag.CreateModel(), _tagRepository));
            _todoRepository.Update(SelectedTodoItem.CreateModel());
        }


        private void AddTodo()
        {
            if (!String.IsNullOrEmpty(TodoName))
            {
                var vm = CreateTodoItemViewModel(TodoName);
                var model = vm.CreateModel();
                TodoItems.Add(vm);
                _todoRepository.Add(model);
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

        private void RemoveTodo(TodoItemViewModel vmToRemove)
        {
            if (vmToRemove != null)
            {
                TodoItems.Remove(vmToRemove);
                _todoRepository.Remove(vmToRemove.Id);
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
