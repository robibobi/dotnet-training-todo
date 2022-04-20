using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApplication.Models;

namespace TodoApplication.Respositories
{
    internal interface ITodoItemRepository
    {
        List<TodoItem> GetAll();

        void Add(TodoItem item);

        void Remove(Guid id);

        void Update(TodoItem item);
    }
}
