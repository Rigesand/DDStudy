namespace DDStudy2022.Api.Models.Photos;

public class GetPhoto
{
    public Guid Id { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public string Url { get; set; } = null!;
}