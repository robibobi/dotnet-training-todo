using System;
using System.Collections.Generic;

namespace TodoApplication.Models
{
    internal class TodoItem : BaseEntity
    {
        public string Name { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsDone { get; set; }

        public List<Guid> TagIds { get; set; } = new List<Guid>();
    }
}
