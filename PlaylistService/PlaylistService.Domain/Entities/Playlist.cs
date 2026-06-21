using System.Data;

namespace PlaylistService.Domain.Entities;

public class Playlist
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    private readonly List<PlaylistSong> _playlistSongs = new();
    public IReadOnlyCollection<PlaylistSong> PlaylistSongs => _playlistSongs.AsReadOnly();

    public Playlist(string name, Guid userId)
    {
        Id = Guid.NewGuid();
        Name = name;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public void AddSong(Guid songId)
    {
        if (_playlistSongs.Any(ps => ps.SongId == songId))
        {
            throw new KeyNotFoundException("Song is already in this playlist.");
        }

        _playlistSongs.Add(new PlaylistSong(Id, songId));
    }

    /// <summary>
    /// Removes a song from this playlist.
    /// </summary>
    /// <exception cref="KeyNotFoundException">Thrown when the song is not in the playlist.</exception>
    public void RemoveSong(Guid songId)
    {
        var entry = _playlistSongs.FirstOrDefault(ps => ps.SongId == songId);
        if (entry is null)
        {
            throw new KeyNotFoundException("Song is not in this playlist.");
        }

        _playlistSongs.Remove(entry);
    }

    /// <summary>
    /// Updates the playlist's display name.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the new name is null or whitespace.</exception>
    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Playlist name cannot be empty.", nameof(name));
        }

        Name = name;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
