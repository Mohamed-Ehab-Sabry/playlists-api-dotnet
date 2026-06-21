using PlaylistService.Application.Interfaces;
using PlaylistService.Domain.Entities;

namespace PlaylistService.Application.Services;

public class PlaylistAppService : IPlaylistAppService
{
    private readonly IPlaylistRepository _repository;

    public PlaylistAppService(IPlaylistRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Creates a new playlist for a user.
    /// </summary>
    public async Task<Playlist> CreatePlaylistAsync(string name, Guid userId)
    {
        var playlist = new Playlist(name, userId);
        return await _repository.CreateAsync(playlist);
    }

    /// <summary>
    /// Adds a song to an existing playlist.
    /// </summary>
    public async Task AddSongToPlaylistAsync(Guid playlistId, Guid userId, Guid songId)
    {
        var playlist = await _repository.GetByIdAndUserIdAsync(playlistId, userId)
            ?? throw new InvalidOperationException("Playlist not found or does not belong to the current user.");

        var song = await _repository.GetSongByIdAsync(songId)
            ?? throw new InvalidOperationException("Song not found.");

        playlist.AddSong(songId);

        await _repository.UpdateAsync(playlist);
    }

    /// <summary>
    /// Retrieves all playlists for a specific user.
    /// </summary>
    public async Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(Guid userId)
    {
        return await _repository.GetUserPlaylistsAsync(userId);
    }

    /// <summary>
    /// Updates the name of a playlist that belongs to the given user.
    /// </summary>
    public async Task<Playlist> UpdatePlaylistAsync(Guid playlistId, Guid userId, string newName)
    {
        var playlist = await _repository.GetByIdAndUserIdAsync(playlistId, userId)
            ?? throw new InvalidOperationException("Playlist not found or does not belong to the current user.");

        playlist.UpdateName(newName);
        await _repository.UpdateAsync(playlist);

        return playlist;
    }

    /// <summary>
    /// Soft-deletes a playlist that belongs to the given user.
    /// </summary>
    public async Task DeletePlaylistAsync(Guid playlistId, Guid userId)
    {
        var playlist = await _repository.GetByIdAsync(playlistId);

        if (playlist is null || playlist.UserId != userId)
        {
            throw new InvalidOperationException("Playlist not found or does not belong to the current user.");
        }

        playlist.SoftDelete();
        await _repository.UpdateAsync(playlist);
    }

    /// <summary>
    /// Removes a single song from a playlist that belongs to the given user.
    /// </summary>
    public async Task RemoveSongFromPlaylistAsync(Guid playlistId, Guid userId, Guid songId)
    {
        var playlist = await _repository.GetByIdAndUserIdAsync(playlistId, userId)
            ?? throw new InvalidOperationException("Playlist not found or does not belong to the current user.");

        playlist.RemoveSong(songId); // throws KeyNotFoundException if song not present

        await _repository.UpdateAsync(playlist);
    }
}