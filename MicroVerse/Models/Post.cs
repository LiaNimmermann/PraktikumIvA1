namespace MicroVerse.Models
{
    //not finished
    public class Post
    {
        public int Id { get; set; }
        public string Body { get; set; }    
        public UserModel Author { get; set; }
        public Post ReactsTo { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
