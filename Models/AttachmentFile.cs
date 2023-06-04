namespace eFilingAPI.Models
{
    public class AttachmentFile
    {
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public Int64 FileSize { get; set; }
        public string Ext { get; set; }
    }
}
