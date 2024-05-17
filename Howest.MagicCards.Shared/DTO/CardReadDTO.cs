namespace Howest.MagicCards.Shared.DTO;

public record CardReadDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string OriginalImageUrl { get; set; }
}
