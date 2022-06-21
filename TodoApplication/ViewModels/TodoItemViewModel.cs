using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoApplication.Commands;
using TodoApplication.Models;
using TodoApplication.Respositories;
using TodoApplication.Util;

namespace TodoApplication.ViewModels
{
    internal class TodoItemViewModel
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TodoItemViewModel));

        private readonly ITodoItemRepository _repository;

        public Guid Id { get; }

        public string Name { get; set; }

        public DateTime TimeStamp { get; set; }

        private bool _isDone;

        public bool IsDone
        {
            get { return _isDone; }
            set 
            { 
                if(_isDone != value)
                {
                    _isDone = value;
                    AsyncVoidHelper.TryThrowOnDispatcher(Update);
                }
            }
        }


        public ObservableCollection<TagViewModel> Tags { get; set; }

        public AsyncCommand<TagViewModel> RemoveTagCommand { get; }

        public TodoItemViewModel(
            TodoItem todoItem,
            ITodoItemRepository repository) : this(todoItem.Id)
        {
            _repository = repository;

            Name = todoItem.Name;
            TimeStamp = todoItem.TimeStamp;
            _isDone = todoItem.IsDone;
            RemoveTagCommand = new AsyncCommand<TagViewModel>(RemoveTag, CanRemoveTag);
        }

        public TodoItemViewModel(Guid id)
        {
            Id = id;
            Tags = new ObservableCollection<TagViewModel>();
        }


        public async Task RemoveTag(TagViewModel tagToRemove)
        {
            Tags.Remove(tagToRemove);
            await Update();
        }


        public TodoItem CreateModel()
        {
            return new TodoItem()
            {
                Name = Name,
                TimeStamp = TimeStamp,
                IsDone = IsDone,
                Id = Id,
                TagIds = Tags.Select(tagVm => tagVm.Id).ToList()
            };
        }

        public async Task Update()
        {
            var model = CreateModel();
            var updateResult = await _repository.Update(model);
            if (!updateResult.WasSuccessful)
            {
                Log.Error(updateResult.Message);
            }
        }

        private bool CanRemoveTag(TagViewModel _)
        {
            return true;
        }

    }
}
