namespace MicroVerse.ViewModels
{
    public class FollowButtonModel
    {
        public FollowButtonModel(string user, bool follows)
        {
            User = user;
            Follows = follows;
        }

        public string User { get; set; }
        public bool Follows { get; set; }
    }
}
