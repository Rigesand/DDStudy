namespace DDStudy2022.DAL.Entities;

public class PostContent : Attach
{
    public Post Post { get; set; } = null!;
}