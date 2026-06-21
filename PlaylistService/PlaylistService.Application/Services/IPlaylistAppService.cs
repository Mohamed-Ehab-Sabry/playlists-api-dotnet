using PlaylistService.Domain.Entities;

namespace PlaylistService.Application.Services;

public interface IPlaylistAppService
{
    Task<Playlist> CreatePlaylistAsync(string name, Guid userId);
    Task AddSongToPlaylistAsync(Guid playlistId, Guid userId, Guid songId);
    Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(Guid userId);
}