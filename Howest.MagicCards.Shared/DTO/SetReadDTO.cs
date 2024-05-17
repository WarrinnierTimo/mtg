namespace Howest.MagicCards.Shared.DTO;

public record SetReadDTO
{
    public long Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }
}