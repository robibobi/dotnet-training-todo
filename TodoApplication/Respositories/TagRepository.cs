using TodoApplication.Models;
using TodoApplication.Services;

namespace TodoApplication.Respositories
{
    internal class TagRepository : FileBasedRepository<TodoItemTag>, ITagRepository
    {
        public TagRepository(IAppConfigService configService)
            : base(configService.TagItemFile)
        {
        }
    }
}
