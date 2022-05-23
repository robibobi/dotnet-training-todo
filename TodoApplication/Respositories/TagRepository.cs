using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TodoApplication.Models;
using TodoApplication.Services;

namespace TodoApplication.Respositories
{
    internal class TagRepository : ITagRepository
    {
        private readonly IAppConfigService _configService;

        public TagRepository(IAppConfigService configService)
        {
            _configService = configService;
        }

        public void Add(TodoItemTag tag)
        {
            var tags = GetAll();
            tags.Add(tag);
            SaveItems(tags);
        }

        public List<TodoItemTag> GetAll()
        {
            var tagFile = _configService.TagItemFile;
            if (tagFile.Exists)
            {
                var tagItemsString = File.ReadAllText(tagFile.FullName);
                return JsonConvert
                    .DeserializeObject<List<TodoItemTag>>(tagItemsString);
            }
            else
            {
                return new List<TodoItemTag>();
            }
        }

        public void Remove(Guid tagId)
        {
            var tags = GetAll();
            var tagToRemove = tags.First(tag => tag.Id == tagId);
            tags.Remove(tagToRemove);
            SaveItems(tags);
        }

        public void Update(TodoItemTag tag)
        {
            // Find the tag  to update
            var tags = GetAll();
            var tagToUpdate = tags.Single(t => t.Id == tag.Id);
            tagToUpdate.Name = tag.Name;
            tagToUpdate.Color = tag.Color;
            SaveItems(tags);
        }

        private void SaveItems(List<TodoItemTag> items)
        {
            var tagFile = _configService.TagItemFile;
            if (!tagFile.Directory.Exists)
            {
                tagFile.Directory.Create();
            }

            var tagItemsString = JsonConvert
                .SerializeObject(items, Formatting.Indented);

            File.WriteAllText(tagFile.FullName, tagItemsString);
        }
    }
}
