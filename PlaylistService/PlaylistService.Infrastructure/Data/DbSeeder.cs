using Microsoft.EntityFrameworkCore;
using PlaylistService.Domain.Entities;

namespace PlaylistService.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Songs.AnyAsync())
        {
            return;
        }

        var songs = new List<Song>
        {
            new Song("Believer", "Imagine Dragons", TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(24)), "Rock"),
            new Song("Shape of You", "Ed Sheeran", TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(53)), "Pop"),
            new Song("Numb", "Linkin Park", TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(7)), "Rock"),
            new Song("Blinding Lights", "The Weeknd", TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(20)), "Pop"),
            new Song("Viva La Vida", "Coldplay", TimeSpan.FromMinutes(4).Add(TimeSpan.FromSeconds(2)), "Alternative"),
            new Song("Lose Yourself", "Eminem", TimeSpan.FromMinutes(5).Add(TimeSpan.FromSeconds(26)), "Hip-Hop"),
            new Song("Bohemian Rhapsody", "Queen", TimeSpan.FromMinutes(5).Add(TimeSpan.FromSeconds(55)), "Rock"),
            new Song("Levitating", "Dua Lipa", TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(23)), "Pop"),
            new Song("Someone Like You", "Adele", TimeSpan.FromMinutes(4).Add(TimeSpan.FromSeconds(45)), "Soul"),
            new Song("Counting Stars", "OneRepublic", TimeSpan.FromMinutes(4).Add(TimeSpan.FromSeconds(17)), "Pop Rock")
        };

        await context.Songs.AddRangeAsync(songs);
        await context.SaveChangesAsync();
    }
}