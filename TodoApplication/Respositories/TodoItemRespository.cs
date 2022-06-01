using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TodoApplication.Models;
using TodoApplication.Services;

namespace TodoApplication.Respositories
{
    internal class TodoItemRespository : ITodoItemRepository
    {
        private readonly IAppConfigService _configService;

        public TodoItemRespository(IAppConfigService configService)
        {
            _configService = configService;
        }

        public List<TodoItem> GetAll()
        {
            if (_configService.TodoItemFile.Exists)
            {
                var todoItemsString = File.ReadAllText(_configService.TodoItemFile.FullName);
                return JsonConvert
                    .DeserializeObject<List<TodoItem>>(todoItemsString);
            }
            else
            {
                return new List<TodoItem>();
            }
        }

        public void Remove(Guid id)
        {
            var items = GetAll();

            var itemToRemove = items.First(item => item.Id == id);
           
            items.Remove(itemToRemove);

            SaveItems(items);
        }


        public void Add(TodoItem item)
        {
            var items = GetAll();
            items.Add(item);
            SaveItems(items);
        }

        public void Update(TodoItem todoItem)
        {
            var items = GetAll();
            // Find the item in the list
            var itemToUpdate = items.First(item => item.Id == todoItem.Id);
            // Update this item
            itemToUpdate.IsDone = todoItem.IsDone;
            itemToUpdate.TagIds = todoItem.TagIds;
            itemToUpdate.TimeStamp = todoItem.TimeStamp;
            itemToUpdate.Name = todoItem.Name;
            // Save all items back to the file
            SaveItems(items);
        }

        private void SaveItems(List<TodoItem> items)
        {
            var todoItemsFile = _configService.TodoItemFile;
            
            if (!todoItemsFile.Directory.Exists)
            {
                todoItemsFile.Directory.Create();
            }

            string todoItemsString = JsonConvert
                .SerializeObject(items, Formatting.Indented);

            File.WriteAllText(todoItemsFile.FullName, todoItemsString);
        }
    }
}
