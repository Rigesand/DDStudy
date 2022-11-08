namespace DDStudy2022.DAL.Entities;

public class Photo : Attach
{
    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;
}