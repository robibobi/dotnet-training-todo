using TodoApplication.Models;
using TodoApplication.Services;

namespace TodoApplication.Respositories
{
    internal class TodoItemRespository : FileBasedRepository<TodoItem>, ITodoItemRepository
    {
        public TodoItemRespository(IAppConfigService configService)
            : base(configService.TodoItemFile)
        {
        }
    }
}
