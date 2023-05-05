namespace MicroVerse.Models
{
    public class UserModel
    {
        public EMail Id { get; } = new EMail();

        public Username Username { get; } = new Username();

        public String DisplayedName { get; set; } = "";

        public Role Role { get; set; }

        public String Picture { get; set; } = "";

        public String Bio { get; set; } = "";

        public Activation Activation { get; set; }

        public DateTime CreatedAt { get; }

        public String Password { get; set; } = "";
    }
}
