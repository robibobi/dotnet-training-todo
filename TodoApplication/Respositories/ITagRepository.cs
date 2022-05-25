using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApplication.Models;

namespace TodoApplication.Respositories
{
    internal interface ITagRepository
    {
        Task<List<TodoItemTag>> GetAll();
        Task Add(TodoItemTag tag);
        Task Remove(Guid tagId);
        Task Update(TodoItemTag tag);
    }
}
