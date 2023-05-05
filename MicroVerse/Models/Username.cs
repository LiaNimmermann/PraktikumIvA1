namespace MicroVerse.Models
{
    public class Username
    {
        public String Name { get; set; } = "";

        public override String ToString()
            => "@" + Name;
    }
}
