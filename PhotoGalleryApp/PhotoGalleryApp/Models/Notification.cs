namespace PhotoGalleryApp.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsViewed { get; set; }
        public string UserId { get; set; }
    }
}
