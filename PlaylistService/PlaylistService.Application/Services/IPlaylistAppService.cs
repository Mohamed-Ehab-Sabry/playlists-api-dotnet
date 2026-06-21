using PlaylistService.Domain.Entities;

namespace PlaylistService.Application.Services;

public interface IPlaylistAppService
{
    Task<Playlist> CreatePlaylistAsync(string name, Guid userId);
    Task AddSongToPlaylistAsync(Guid playlistId, Guid userId, Guid songId);
    Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(Guid userId);

    /// <summary>
    /// Updates the name of a playlist that belongs to the given user.
    /// </summary>
    Task<Playlist> UpdatePlaylistAsync(Guid playlistId, Guid userId, string newName);

    /// <summary>
    /// Soft-deletes a playlist that belongs to the given user.
    /// </summary>
    Task DeletePlaylistAsync(Guid playlistId, Guid userId);

    /// <summary>
    /// Removes a single song from a playlist that belongs to the given user.
    /// </summary>
    Task RemoveSongFromPlaylistAsync(Guid playlistId, Guid userId, Guid songId);
}