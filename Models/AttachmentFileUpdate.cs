namespace eFilingAPI.Models
{
    public class AttachmentFileUpdate
    {
        public IFormFile File { get; set; }
        public Int32 Id { get; set; }
        public string FileName { get; set; }
        public Int64 FileSize { get; set; }
        public string Ext { get; set; }
    }
}
