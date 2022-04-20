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

namespace TodoApplication.ViewModels
{
    internal class TodoItemViewModel
    {
        private readonly ITodoItemRepository _repository;

        public Guid Id { get; }

        public string Name { get; set; }

        public DateTime TimeStamp { get; set; }

        public bool IsDone { get; set; }

        public ObservableCollection<TagViewModel> Tags { get; set; }

        public ActionCommand<TagViewModel> RemoveTagCommand { get; }

        public TodoItemViewModel(
            TodoItem todoItem,
            ITodoItemRepository repository) : this(todoItem.Id)
        {
            Name = todoItem.Name;
            TimeStamp = todoItem.TimeStamp;
            IsDone = todoItem.IsDone;
            _repository = repository;
            RemoveTagCommand = new ActionCommand<TagViewModel>(RemoveTag, CanRemoveTag);
        }

        public TodoItemViewModel(Guid id)
        {
            Id = id;
            Tags = new ObservableCollection<TagViewModel>();
        }


        public void RemoveTag(TagViewModel tagToRemove)
        {
            Tags.Remove(tagToRemove);
            Update();
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

        public void Update()
        {
            var model = CreateModel();
            _repository.Update(model);
        }

        private bool CanRemoveTag(TagViewModel _)
        {
            return true;
        }

    }
}
