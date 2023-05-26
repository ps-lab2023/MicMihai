using System.ComponentModel.DataAnnotations;

public class TagPhoto
{
    public int TagId { get; set; }
    public virtual Tag? Tag { get; set; }
    public int PhotoId { get; set; }
    public virtual Photo? Photo { get; set; }
}
