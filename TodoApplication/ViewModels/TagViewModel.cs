using System;
using TodoApplication.Models;
using TodoApplication.Respositories;

namespace TodoApplication.ViewModels
{
    internal class TagViewModel
    {
        private readonly ITagRepository _tagRepository;

        public Guid Id { get; }

        private string _name;

        public string Name
        {
            get { return _name; }
            set 
            { 
                _name = value;
                _tagRepository.Update(CreateModel());
            }
        }


        public TagViewModel(TodoItemTag tag,
            ITagRepository tagRepository)
        {
            Id = tag.Id;
            _name = tag.Name;
            _tagRepository = tagRepository;
        }

        public TodoItemTag CreateModel()
        {
            return new TodoItemTag()
            {
                Id = Id,
                Name = Name,
            };
        }
    }
}
