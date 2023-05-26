public class Photo
{
    public int PhotoId { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public int AlbumId { get; set; }
    public virtual Album Album { get; set; }
    public virtual ICollection<TagPhoto> TagPhotos { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
}
