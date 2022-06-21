namespace TodoApplication.Models
{
    internal class TodoItemTag : BaseEntity
    {
        public string Name { get; set; }
        public TagColor Color { get; set; }
    }
}
