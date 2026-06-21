using PlaylistService.Domain.Entities;

namespace PlaylistService.Application.Interfaces;

public interface IPlaylistRepository
{
    // Create operations
    Task<Playlist> CreateAsync(Playlist playlist);

    // Fetch operations
    Task<Playlist?> GetByIdAsync(Guid id);
    Task<Playlist?> GetByIdAndUserIdAsync(Guid id, Guid userId);

    Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(Guid userId);
    Task<Song?> GetSongByIdAsync(Guid songId);

    // Update operation
    Task UpdateAsync(Playlist playlist);

}