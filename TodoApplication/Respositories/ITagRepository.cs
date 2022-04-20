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
        List<TodoItemTag> GetAll();

        void Add(TodoItemTag tag);

        void Remove(Guid tagId);

        void Update(TodoItemTag tag);
    }
}
