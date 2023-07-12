namespace MicroVerse.Models
{
    public class UserPostingStats
    {
        public UserPostingStats(List<String> labels, List<Int32> allTimePosts, List<Int32> last7DaysPosts)
        {
            Labels = labels;
            Datasets = new List<Dataset>
            {
                new Dataset("Posts all time", allTimePosts),
                new Dataset( "Posts in the last 7 days", last7DaysPosts)
            };
        }

        public List<String> Labels { get; set; }

        public List<Dataset> Datasets { get; set; }

        public class Dataset
        {
            public Dataset(String label, List<Int32> data)
            {
                Label = label;
                Data = data;
            }

            public String Label { get; set; }

            public List<Int32> Data { get; set; } 
        }
    }
}
