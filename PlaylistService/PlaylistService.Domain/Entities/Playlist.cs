
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

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
