using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task Add(TodoItemTag tag)
        {
            var tags = await GetAll();
            tags.Add(tag);
            await SaveItems(tags);
        }

        public async Task<List<TodoItemTag>> GetAll()
        {
            var tagFile = _configService.TagItemFile;

            if (tagFile.Exists)
            {
                var tagItemsString = await ReadFileAsync(tagFile.FullName);
                return JsonConvert
                    .DeserializeObject<List<TodoItemTag>>(tagItemsString);
            }
            else
            {
                return new List<TodoItemTag>();
            }
        }

        public async Task Remove(Guid tagId)
        {
            var tags = await GetAll();
            var tagToRemove = tags.First(tag => tag.Id == tagId);
            tags.Remove(tagToRemove);
            await SaveItems(tags);
        }

        public async Task Update(TodoItemTag tag)
        {
            // Find the tag  to update
            var tags = await GetAll();
            var tagToUpdate = tags.Single(t => t.Id == tag.Id);
            tagToUpdate.Name = tag.Name;
            tagToUpdate.Color = tag.Color;
            await SaveItems(tags);
        }

        private async Task SaveItems(List<TodoItemTag> items)
        {
            var tagFile = _configService.TagItemFile;
            if (!tagFile.Directory.Exists)
            {
                tagFile.Directory.Create();
            }

            var tagItemsString = JsonConvert
                .SerializeObject(items, Formatting.Indented);

            await WriteFileAsync(tagFile.FullName, tagItemsString);
        }


        private async Task<string> ReadFileAsync(string fileName)
        {
            using(var streamReader = new StreamReader(File.OpenRead(fileName)))
            {
                return await streamReader.ReadToEndAsync();
            }
        }

        private async Task WriteFileAsync(string fileName, string content)
        {
            using(var memStream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                using (var fileStream = File.Open(fileName, FileMode.Create))
                {
                    await memStream.CopyToAsync(fileStream);
                }
            }
        }
    }
}
