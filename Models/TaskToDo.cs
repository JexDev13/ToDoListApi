using System.Collections.Generic;
using System.Xml.Linq;

namespace ToDoListApi.Models
{
    public class TaskToDo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
