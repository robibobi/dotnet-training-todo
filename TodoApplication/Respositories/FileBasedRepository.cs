using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TodoApplication.Models;
using TodoApplication.Util;

namespace TodoApplication.Respositories
{
    internal class FileBasedRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileBasedRepository<TEntity>));
        
        private readonly FileInfo _repositoryFile;

        public FileBasedRepository(FileInfo repositoryFile)
        {
            _repositoryFile = repositoryFile;
        }

        public async Task<Result<Unit>> Add(TEntity entity)
        {
            var entitiesResult = await GetAll();
            if (entitiesResult.WasSuccessful)
            {
                var entities = entitiesResult.Value;
                entities.Add(entity);
                return await SaveItems(entities);
            }
            else
            {
                return entitiesResult.Map(_ => Unit.Default);
            }
        }

        public async Task<Result<List<TEntity>>> GetAll()
        {
            Log.Info($"Reading all entities from the file: {_repositoryFile.Name} ...");

            _repositoryFile.Refresh();
            if (_repositoryFile.Exists)
            {
                try
                {
                    var entityItemsStringResult = await FileHelper.ReadFileAsync(_repositoryFile.FullName);

                    return entityItemsStringResult.Map(JsonConvert.DeserializeObject<List<TEntity>>);

                }
                catch (Exception ex)
                {
                    return new Error<List<TEntity>>($"Failed to load entity list from file: {ex.Message}");
                }
            }
            else
            {
                return Result.CreateSuccess(new List<TEntity>());
            }
        }

        public async Task<Result<Unit>> Remove(Guid entityId)
        {
            var entitiesResult = await GetAll();
            if (entitiesResult.WasSuccessful)
            {
                var entities = entitiesResult.Value;
                var entityToRemove = entities.FirstOrDefault(tag => tag.Id == entityId);

                if (entityToRemove == null)
                {
                    return Result.CreateError($"An entity with the given id '{entityId}' does not exist.");
                }

                entities.Remove(entityToRemove);

                return await SaveItems(entities);
            }
            else
            {
                return entitiesResult.Map(_ => Unit.Default);
            }
        }

        public async Task<Result<Unit>> Update(TEntity updatedEntity)
        {
            // Find the tag  to update
            var entitiesResult = await GetAll();
            if (entitiesResult.WasSuccessful)
            {
                var entities = entitiesResult.Value;
                var entityToUpdate = entities.Single(t => t.Id == updatedEntity.Id);
                var indexOfEntityToUpdate = entities.IndexOf(entityToUpdate);

                entities.Remove(entityToUpdate);

                if(indexOfEntityToUpdate < entities.Count)
                {
                    entities.Insert(indexOfEntityToUpdate, updatedEntity);
                } else
                {
                    entities.Add(updatedEntity);
                }
             
                return await SaveItems(entities);
            }
            else
            {
                return entitiesResult.Map(_ => Unit.Default);
            }

        }

        private async Task<Result<Unit>> SaveItems(List<TEntity> items)
        {
            try
            {
                _repositoryFile.Refresh();
                if (!_repositoryFile.Directory.Exists)
                {
                    _repositoryFile.Directory.Create();
                }

                var entityItemsString = JsonConvert
                    .SerializeObject(items, Formatting.Indented);

                return await FileHelper.WriteFileAsync(_repositoryFile.FullName, entityItemsString);
            }
            catch (JsonSerializationException ex)
            {
                return Result.CreateError($"Failed to serialize entities: {ex.Message}");
            }
            catch (IOException ex)
            {
                return Result.CreateError($"Failed to create repository file directory: {ex.Message}");
            }
        }
    }
}
