using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApplication.Models;

namespace TodoApplication.Respositories
{
    internal class TagRepository : ITagRepository
    {
        const string TagFileName = "tags.json";

        public TagRepository()
        {

        }

        public void Add(TodoItemTag tag)
        {
            var tags = GetAll();
            tags.Add(tag);
            SaveItems(tags);
        }

        public List<TodoItemTag> GetAll()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = Path.Combine(path, "TodoApplication", TagFileName);
            if (File.Exists(path))
            {
                var tagItemsString = File.ReadAllText(path);
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

            SaveItems(tags);
        }

        private void SaveItems(List<TodoItemTag> items)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = Path.Combine(path, "TodoApplication");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, TagFileName);

            string tagItemsString = JsonConvert
                .SerializeObject(items, Formatting.Indented);

            File.WriteAllText(path, tagItemsString);
        }
    }
}
