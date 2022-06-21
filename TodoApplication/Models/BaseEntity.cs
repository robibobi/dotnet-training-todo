using System;

namespace TodoApplication.Models
{
    internal abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
