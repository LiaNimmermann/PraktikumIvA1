namespace MicroVerse.Models
{
    public class EMail
    {
        public String Name { get; set; } = "";

        public String Host { get; set; } = "";

        public override String ToString()
            => Name + "@" + Host;
    }
}
