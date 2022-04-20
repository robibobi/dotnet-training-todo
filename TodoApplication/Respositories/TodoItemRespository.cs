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
    internal class TodoItemRespository : ITodoItemRepository
    {
        const string TodoFileName = "todoItems.json";


        public TodoItemRespository()
        {

        }

        public List<TodoItem> GetAll()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = Path.Combine(path, "TodoApplication", TodoFileName);
            if (File.Exists(path))
            {
                var todoItemsString = File.ReadAllText(path);
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
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            path = Path.Combine(path, "TodoApplication");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, TodoFileName);

            string todoItemsString = JsonConvert
                .SerializeObject(items, Formatting.Indented);

            File.WriteAllText(path, todoItemsString);
        }
    }
}
