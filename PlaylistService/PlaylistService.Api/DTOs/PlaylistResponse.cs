namespace PlaylistService.Api.DTOs;

public class PlaylistResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<SongInPlaylistResponse> Songs { get; set; } = new();
}