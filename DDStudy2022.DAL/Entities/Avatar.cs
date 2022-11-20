namespace DDStudy2022.DAL.Entities;

public class Avatar : Attach
{
    public User Owner { get; set; } = null!;
    public Guid OwnerId { get; set; }
}