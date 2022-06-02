using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TodoApplication.Models;
using TodoApplication.Services;
using TodoApplication.Util;

namespace TodoApplication.Respositories
{
    internal class TagRepository : ITagRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TagRepository));

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
                return await SaveItems(tags);
            } else
            {
                return tagsResult.Map(_ => Unit.Default);
            }
        }

        public async Task<Result<List<TodoItemTag>>> GetAll()
        {
            Log.Info("Reading all tags from the tag file...");

            var tagFile = _configService.TagItemFile;

            if (tagFile.Exists)
            {
                try
                {
                    var tagItemsStringResult = await FileHelper.ReadFileAsync(tagFile.FullName);
   
                    return tagItemsStringResult.Map(JsonConvert.DeserializeObject<List<TodoItemTag>>);

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
                var tagToRemove = tags.FirstOrDefault(tag => tag.Id == tagId);

                if(tagToRemove == null)
                {
                    return Result.CreateError($"A tag with the given id '{tagId}' does not exist.");
                }

                tags.Remove(tagToRemove);

                return await SaveItems(tags);
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
                return await SaveItems(tags);
            } else
            {
                return tagsResult.Map(_ => Unit.Default);
            }

        }

        private async Task<Result<Unit>> SaveItems(List<TodoItemTag> items)
        {
            var tagFile = _configService.TagItemFile;
            try
            {
                if (!tagFile.Directory.Exists)
                {
                    tagFile.Directory.Create();
                }

                var tagItemsString = JsonConvert
                    .SerializeObject(items, Formatting.Indented);

                return await FileHelper.WriteFileAsync(tagFile.FullName, tagItemsString);
            } 
            catch(JsonSerializationException ex)
            {
                return Result.CreateError($"Failed to serialize tag items: {ex.Message}");
            }
            catch (IOException ex)
            {
                return Result.CreateError($"Failed to create tag file directory: {ex.Message}");
            }
        }
    }
}
