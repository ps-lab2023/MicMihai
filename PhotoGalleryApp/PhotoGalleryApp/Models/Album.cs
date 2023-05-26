using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class Album
{
    public int AlbumId { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
    public virtual User? User { get; set; }
    public virtual ICollection<Photo>? Photos { get; set; }
}
