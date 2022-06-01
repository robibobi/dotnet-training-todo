using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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

        public ObservableCollection<TagColor> AvailableColors { get; }

        public AsyncCommand AddTagCommand { get; }
        public AsyncCommand RemoveTagCommand { get; }


        public ManageTagsDialogViewModel(
            ObservableCollection<TagViewModel> tags,
            IEnumerable<Guid> referencedTagIds,
            ITagRepository tagRepository)
        {
            Tags = tags;
            _referencedTagIds = referencedTagIds;
            _tagRepository = tagRepository;
            AddTagCommand = new AsyncCommand(AddTag, CanAddTag);
            RemoveTagCommand = new AsyncCommand(RemoveTag, CanRemoveTag);

            AvailableColors = new ObservableCollection<TagColor>
            {
                TagColor.Default,
                TagColor.Color1,
                TagColor.Color2,
                TagColor.Color3,
                TagColor.Color4
            };
        }

        private bool CanRemoveTag()
        {
            return SelectedTag != null && TagIsNotBeingUsed(SelectedTag);
        }

        private bool TagIsNotBeingUsed(TagViewModel tag)
        {
            return !_referencedTagIds.Contains(tag.Id);
        }

        private async Task RemoveTag()
        {
            if(SelectedTag != null)
            {
                await _tagRepository.Remove(SelectedTag.Id);
                Tags.Remove(SelectedTag);
            }
        }

        private bool CanAddTag()
        {
            return !String.IsNullOrWhiteSpace(TagName);
        }

        private async Task AddTag()
        {
            if (!String.IsNullOrEmpty(TagName))
            {
                var tagModel = new TodoItemTag()
                {
                    Id = Guid.NewGuid(),
                    Name = TagName,
                };
                Tags.Add(new TagViewModel(tagModel, _tagRepository));
            
   
                await _tagRepository.Add(tagModel);
            }
        }
    }
}
