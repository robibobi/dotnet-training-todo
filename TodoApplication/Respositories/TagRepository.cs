using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApplication.Models;
using TodoApplication.Services;
using TodoApplication.Util;

namespace TodoApplication.Respositories
{
    internal class TagRepository : ITagRepository
    {
        private readonly IAppConfigService _configService;

        public TagRepository(IAppConfigService configService)
        {
            _configService = configService;
        }

        public async Task<Result<Unit>> Add(TodoItemTag tag)
        {
            var tagsResult = await GetAll();
            if (tagsResult.WasSuccessful)
            {
                var tags = tagsResult.Value;
                tags.Add(tag);
                await SaveItems(tags);
                return Result.CreateSuccess();
            } else
            {
                return tagsResult.Map(_ => Unit.Default);
            }
        }

        public async Task<Result<List<TodoItemTag>>> GetAll()
        {
            var tagFile = _configService.TagItemFile;

            if (tagFile.Exists)
            {
                try
                {

                    var tagItemsString = await FileHelper.ReadFileAsync(tagFile.FullName);
                    var tagList = JsonConvert
                        .DeserializeObject<List<TodoItemTag>>(tagItemsString);

                    return Result.CreateSuccess(tagList);
                } catch (Exception ex)
                {
                    return new Error<List<TodoItemTag>>($"Failed to load tag list from file: {ex.Message}");
                }
            }
            else
            {
                return Result.CreateSuccess(new List<TodoItemTag>());
            }
        }

        public async Task<Result<Unit>> Remove(Guid tagId)
        {
            var tagsResult = await GetAll();
            if (tagsResult.WasSuccessful)
            {
                var tags = tagsResult.Value;
                var tagToRemove = tags.First(tag => tag.Id == tagId);
                tags.Remove(tagToRemove);
                await SaveItems(tags);
                return Result.CreateSuccess();
            } else
            {
                return tagsResult.Map(_ => Unit.Default);
            }
        }

        public async Task<Result<Unit>> Update(TodoItemTag tag)
        {
            // Find the tag  to update
            var tagsResult = await GetAll();
            if (tagsResult.WasSuccessful)
            {
                var tags = tagsResult.Value;
                var tagToUpdate = tags.Single(t => t.Id == tag.Id);
                tagToUpdate.Name = tag.Name;
                tagToUpdate.Color = tag.Color;
                await SaveItems(tags);
                return Result.CreateSuccess();
            } else
            {
                return tagsResult.Map(_ => Unit.Default);
            }

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

            await FileHelper.WriteFileAsync(tagFile.FullName, tagItemsString);
        }
    }
}
