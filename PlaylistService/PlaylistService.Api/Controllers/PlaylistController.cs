using Microsoft.AspNetCore.Mvc;
using PlaylistService.Api.DTOs;
using PlaylistService.Application.Services;

namespace PlaylistService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaylistsController : ControllerBase
{
    private readonly IPlaylistAppService _playlistAppService;

    public PlaylistsController(IPlaylistAppService playlistAppService)
    {
        _playlistAppService = playlistAppService;
    }

    /// <summary>
    /// Creates a new playlist for the authenticated user.
    /// </summary>
    /// <param name="request">The playlist creation request containing the name.</param>
    /// <returns>The created playlist.</returns>
    [HttpPost]
    public async Task<ActionResult<PlaylistResponse>> CreatePlaylist([FromBody] CreatePlaylistRequest request)
    {
        // Extract UserId from the header (mocked for demonstration)
        var userIdHeader = HttpContext.Request.Headers["UserId"].ToString();
        if (!Guid.TryParse(userIdHeader, out var userId))
        {
            return BadRequest("Invalid or missing UserId header.");
        }

        var playlist = await _playlistAppService.CreatePlaylistAsync(request.Name, userId);

        var response = new PlaylistResponse
        {
            Id = playlist.Id,
            Name = playlist.Name,
            UserId = playlist.UserId,
            CreatedAt = playlist.CreatedAt,
            Songs = playlist.PlaylistSongs
                .Select(ps => new SongInPlaylistResponse
                {
                    Id = ps.Song.Id,
                    Name = ps.Song.Name,
                    Artist = ps.Song.Artist,
                    Duration = ps.Song.Duration,
                    Genre = ps.Song.Genre
                })
                .ToList()
        };

        return CreatedAtAction(nameof(GetUserPlaylists), new { }, response);
    }

    /// <summary>
    /// Retrieves all playlists for the authenticated user.
    /// </summary>
    /// <returns>A list of the user's playlists.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlaylistResponse>>> GetUserPlaylists()
    {
        // Extract UserId from the header (mocked for demonstration)
        var userIdHeader = HttpContext.Request.Headers["UserId"].ToString();
        if (!Guid.TryParse(userIdHeader, out var userId))
        {
            return BadRequest("Invalid or missing UserId header.");
        }

        var playlists = await _playlistAppService.GetUserPlaylistsAsync(userId);

        var responses = playlists.Select(p => new PlaylistResponse
        {
            Id = p.Id,
            Name = p.Name,
            UserId = p.UserId,
            CreatedAt = p.CreatedAt,
            Songs = p.PlaylistSongs
                .Select(ps => new SongInPlaylistResponse
                {
                    Id = ps.Song.Id,
                    Name = ps.Song.Name,
                    Artist = ps.Song.Artist,
                    Duration = ps.Song.Duration,
                    Genre = ps.Song.Genre
                })
                .ToList()
        }).ToList();

        return Ok(responses);
    }

    /// <summary>
    /// Adds a song to a specific playlist.
    /// </summary>
    /// <param name="id">The playlist ID.</param>
    /// <param name="request">The song ID to add.</param>
    [HttpPost("{id}/songs")]
    public async Task<IActionResult> AddSongToPlaylist(Guid id, [FromBody] AddSongRequest request)
    {
        try
        {
            await _playlistAppService.AddSongToPlaylistAsync(id, request.SongId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}