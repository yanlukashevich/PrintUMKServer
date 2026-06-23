namespace PrintUMKServer.Models
{
    public class PrintJob
    {
        public Guid JobId { get; set; }
        public string? UserId { get; set; } // null = guest
        public string BlobPath { get; set; }
        public string PrinterName { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
