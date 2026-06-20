using Microsoft.EntityFrameworkCore;
using PlaylistService.Application.Interfaces;
using PlaylistService.Domain.Entities;
using PlaylistService.Infrastructure.Data;

namespace PlaylistService.Infrastructure.Repositories;

public class PlaylistRepository : IPlaylistRepository
{
    private readonly ApplicationDbContext _context;

    public PlaylistRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Playlist> CreateAsync(Playlist playlist)
    {
        await _context.Playlists.AddAsync(playlist);
        await _context.SaveChangesAsync();
        return playlist;
    }

    public async Task<Playlist?> GetByIdAsync(Guid id)
    {
        return await _context.Playlists.Include(p => p.PlaylistSongs).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(Guid userId)
    { 
        return await _context.Playlists
        .Include(p => p.PlaylistSongs)
        .ThenInclude(ps => ps.Song)
        .Where(p => p.UserId == userId)
        .ToListAsync();
    }

    public async Task<Song?> GetSongByIdAsync(Guid songId)
    {
        return await _context.Songs.FindAsync(songId);
    }

    public async Task UpdateAsync(Playlist playlist)
    {
        _context.Playlists.Update(playlist);
        await _context.SaveChangesAsync();
    }
}