using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PlaylistService.Domain.Entities;

namespace PlaylistService.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Playlist> Playlists => Set<Playlist>();
    public DbSet<Song> Songs => Set<Song>();
    public DbSet<PlaylistSong> PlaylistSongs => Set<PlaylistSong>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PlaylistSong>().HasKey(ps => new {ps.PlaylistId, ps.SongId});

        modelBuilder.Entity<PlaylistSong>()
        .HasOne(ps => ps.Playlist)
        .WithMany(p => p.PlaylistSongs)
        .HasForeignKey(ps => ps.PlaylistId);

        modelBuilder.Entity<PlaylistSong>()
        .HasOne(ps => ps.Song)
        .WithMany(p => p.PlaylistSongs)
        .HasForeignKey(ps => ps.SongId);

        modelBuilder.Entity<Playlist>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Song>().HasQueryFilter(s => !s.IsDeleted);
    }
}