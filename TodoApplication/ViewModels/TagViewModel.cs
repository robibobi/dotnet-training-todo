using System;
using TodoApplication.Models;
using TodoApplication.Respositories;

namespace TodoApplication.ViewModels
{
    internal class TagViewModel
    {
        private readonly ITagRepository _tagRepository;
        
        private string _name;
        private TagColor _color;

        public Guid Id { get; }

        public string Name
        {
            get { return _name; }
            set 
            { 
                _name = value;
                _tagRepository.Update(CreateModel());
            }
        }


        public TagColor Color
        {
            get { return _color; }
            set 
            { 
                _color = value;
                _tagRepository.Update(CreateModel());
            }
        }

        public TagViewModel(TodoItemTag tag,
            ITagRepository tagRepository)
        {
            Id = tag.Id;
            _name = tag.Name;
            _color = tag.Color;
            _tagRepository = tagRepository;
        }

        public TodoItemTag CreateModel()
        {
            return new TodoItemTag()
            {
                Id = Id,
                Name = Name,
                Color = Color,
            };
        }
    }
}
