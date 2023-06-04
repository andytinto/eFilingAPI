namespace eFilingAPI.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
        public string PathFile { get; set; }
        public string FileExt { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
