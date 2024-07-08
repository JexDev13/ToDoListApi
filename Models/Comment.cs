
namespace ToDoListApi.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsUpdated { get; set; }
        public int TaskToDoId { get; set; }
        public TaskToDo TaskToDo { get; set; }
        public int? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
