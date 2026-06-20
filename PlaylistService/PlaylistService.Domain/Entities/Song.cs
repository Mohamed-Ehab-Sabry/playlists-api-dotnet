using System.ComponentModel.DataAnnotations;

namespace PlaylistService.Domain.Entities;

public class Song
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Artist { get; private set; }
    public TimeSpan Duration { get; private set; }
    public string Genre { get; private set; }
    public bool IsDeleted { get; private set; }

    private readonly List<PlaylistSong> _playlistSongs = new();
    public IReadOnlyCollection<PlaylistSong> PlaylistSongs => _playlistSongs.AsReadOnly();

    public Song(string name, string artist, TimeSpan duration, string genre)
    {
        Id = Guid.NewGuid();
        Name = name;
        Artist = artist;
        Duration = duration;
        Genre = genre;
        IsDeleted = false;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }

}
