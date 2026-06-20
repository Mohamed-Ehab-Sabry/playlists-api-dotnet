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
    public async Task AddSongToPlaylistAsync(Guid playlistId, Guid songId)
    {
        var playlist = await _repository.GetByIdAsync(playlistId);
        if (playlist == null)
            throw new InvalidOperationException($"Playlist with ID {playlistId} not found.");

        var song = await _repository.GetSongByIdAsync(songId);
        if (song == null)
            throw new InvalidOperationException($"Song with ID {songId} not found.");

        // Create the association through the domain constructor so the entity can keep its invariants encapsulated.
        var playlistSong = new PlaylistSong(playlistId, songId);
        playlist.AddSong(playlistSong);

        await _repository.UpdateAsync(playlist);
    }

    /// <summary>
    /// Retrieves all playlists for a specific user.
    /// </summary>
    public async Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(Guid userId)
    {
        return await _repository.GetUserPlaylistsAsync(userId);
    }
}