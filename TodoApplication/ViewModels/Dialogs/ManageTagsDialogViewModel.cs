using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TodoApplication.Commands;
using TodoApplication.Models;
using TodoApplication.Respositories;

namespace TodoApplication.ViewModels.Dialogs
{
    internal class ManageTagsDialogViewModel
    {
        private readonly IEnumerable<Guid> _referencedTagIds;
        private readonly ITagRepository _tagRepository;

        private string _tagName;
        private TagViewModel _selectedTag;

        public string TagName
        {
            get { return _tagName; }
            set 
            { 
                _tagName = value;
                AddTagCommand.RaiseCanExecuteChanged();
            }
        }

        public TagViewModel SelectedTag
        {
            get { return _selectedTag; }
            set 
            { 
                _selectedTag = value;
                RemoveTagCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<TagViewModel> Tags { get; }

        public ActionCommand AddTagCommand { get; }
        public ActionCommand RemoveTagCommand { get; }


        public ManageTagsDialogViewModel(
            ObservableCollection<TagViewModel> tags,
            IEnumerable<Guid> referencedTagIds,
            ITagRepository tagRepository)
        {
            Tags = tags;
            _referencedTagIds = referencedTagIds;
            _tagRepository = tagRepository;
            AddTagCommand = new ActionCommand(AddTag, CanAddTag);
            RemoveTagCommand = new ActionCommand(RemoveTag, CanRemoveTag);
        }

        private bool CanRemoveTag()
        {
            return SelectedTag != null && TagIsNotBeingUsed(SelectedTag);
        }

        private bool TagIsNotBeingUsed(TagViewModel tag)
        {
            return !_referencedTagIds.Contains(tag.Id);
        }

        private void RemoveTag()
        {
            if(SelectedTag != null)
            {
                _tagRepository.Remove(SelectedTag.Id);
                Tags.Remove(SelectedTag);
            }
        }

        private bool CanAddTag()
        {
            return !String.IsNullOrWhiteSpace(TagName);
        }

        private void AddTag()
        {
            if (!String.IsNullOrEmpty(TagName))
            {
                var tagModel = new TodoItemTag()
                {
                    Id = Guid.NewGuid(),
                    Name = TagName,
                };
                Tags.Add(new TagViewModel(tagModel, _tagRepository));
                _tagRepository.Add(tagModel);
            }
        }
    }
}
