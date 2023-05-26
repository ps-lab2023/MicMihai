public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public virtual ICollection<Album> Albums { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
}
