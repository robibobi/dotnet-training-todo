﻿using System;
using System.ComponentModel;
using System.Linq;
using TodoApplication.Models;
using TodoApplication.Properties;
using TodoApplication.Respositories;

namespace TodoApplication.ViewModels
{
    internal class TagViewModel : ValidationViewModelBase
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
                if (ValidateName())
                {
                    _tagRepository.Update(CreateModel());
                }
            }
        }

        private bool ValidateName()
        {
            // Validate that name is not empty
            if (String.IsNullOrWhiteSpace(Name))
            {
                SetError(nameof(Name), Resources.TagEmptyError);
                return false;
            }
            // Validate that name is unique
            else if(NameIsNotUnique())
            {
                SetError(nameof(Name), Resources.TagNotUniqueError);
                return false;
            } else
            {
                ResetError(nameof(Name));
                return true;
            }
        }

        private bool NameIsNotUnique()
        {
            var otherTagNames = _tagRepository
                .GetAll()
                .Where(tag => tag.Id != this.Id)
                .Select(tag => tag.Name);

            return otherTagNames.Contains(Name);
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
