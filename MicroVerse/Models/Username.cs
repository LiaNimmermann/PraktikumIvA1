namespace MicroVerse.Models
{
    public class Username
    {
        public Username(string name)
        {
            Name = name;
        }

        public Username()
        {
        }

        public String Name { get; set; } = "";

        public override String ToString()
            => "@" + Name;
    }
}
