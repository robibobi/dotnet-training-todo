using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApplication.Models;
using TodoApplication.Util;

namespace TodoApplication.Respositories
{
    internal interface ITagRepository
    {
        Task<Result<List<TodoItemTag>>> GetAll();
        Task<Result<Unit>> Add(TodoItemTag tag);
        Task<Result<Unit>> Remove(Guid tagId);
        Task<Result<Unit>> Update(TodoItemTag tag);
    }
}
