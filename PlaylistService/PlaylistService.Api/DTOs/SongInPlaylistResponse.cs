namespace PlaylistService.Api.DTOs;

public class SongInPlaylistResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public string Genre { get; set; } = string.Empty;
}