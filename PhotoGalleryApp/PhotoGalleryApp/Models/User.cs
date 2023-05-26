using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public virtual ICollection<Album> Albums { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
}
