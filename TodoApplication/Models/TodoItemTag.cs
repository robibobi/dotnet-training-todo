using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication.Models
{
    internal class TodoItemTag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TagColor Color { get; set; }
    }
}
