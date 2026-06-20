namespace PlaylistService.Domain.Entities;

public class PlaylistSong
{
    public Guid PlaylistId {get; private set;}
    public Playlist Playlist {get; private set;} = null!;

    public Guid SongId {get; private set;}
    public Song Song {get; private set;} = null!;

    public DateTime DateAdded {get; private set; }


    public PlaylistSong(Guid playlistId, Guid songId)
    {
        PlaylistId = playlistId;
        SongId = songId;
        DateAdded = DateTime.UtcNow;
    }

    protected PlaylistSong(){}

}