namespace PhotoGalleryApp.Models
{
    public class PhotoDetailViewModels
    {
        public Photo Photo { get; set; }
        public Comment NewComment { get; set; }
        public string NewTags { get; set; }
        public List<Album> Albums { get; set; }
    }
}
