

using System.ComponentModel.DataAnnotations;

public class Tag
{
    public int TagId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<TagPhoto>? TagPhotos { get; set; }
}
