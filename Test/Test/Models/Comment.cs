using System.ComponentModel.DataAnnotations;

public class Comment
{
    [Key]
    public int CommentId { get; set; }
    public string Text { get; set; }
    public string UserId { get; set; }
    public virtual User User { get; set; }
    public virtual Photo Photo { get; set; }
}
