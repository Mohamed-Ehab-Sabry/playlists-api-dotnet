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

    // ──────────────────────────────────────────────────────────────
    // Helpers
    // ──────────────────────────────────────────────────────────────

    private bool TryGetUserId(out Guid userId)
    {
        var header = HttpContext.Request.Headers["UserId"].ToString();
        return Guid.TryParse(header, out userId);
    }

    // ──────────────────────────────────────────────────────────────
    // POST  /api/playlists
    // ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Creates a new playlist for the authenticated user.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PlaylistResponse>> CreatePlaylist([FromBody] CreatePlaylistRequest request)
    {
        if (!TryGetUserId(out var userId))
            return BadRequest("Invalid or missing UserId header.");

        var playlist = await _playlistAppService.CreatePlaylistAsync(request.Name, userId);

        var response = MapToResponse(playlist);
        return CreatedAtAction(nameof(GetUserPlaylists), new { }, response);
    }

    // ──────────────────────────────────────────────────────────────
    // GET   /api/playlists
    // ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Retrieves all playlists for the authenticated user.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlaylistResponse>>> GetUserPlaylists()
    {
        if (!TryGetUserId(out var userId))
            return BadRequest("Invalid or missing UserId header.");

        var playlists = await _playlistAppService.GetUserPlaylistsAsync(userId);
        return Ok(playlists.Select(MapToResponse));
    }

    // ──────────────────────────────────────────────────────────────
    // POST  /api/playlists/{id}/songs
    // ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Adds a song to a specific playlist.
    /// </summary>
    [HttpPost("{id}/songs")]
    public async Task<IActionResult> AddSongToPlaylist(Guid id, [FromBody] AddSongRequest request)
    {
        if (!TryGetUserId(out var userId))
            return BadRequest("Invalid or missing UserId header.");

        try
        {
            await _playlistAppService.AddSongToPlaylistAsync(id, userId, request.SongId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // ──────────────────────────────────────────────────────────────
    // PATCH /api/playlists/{id}   (bonus)
    // ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Updates the name of a playlist that belongs to the authenticated user.
    /// </summary>
    [HttpPatch("{id}")]
    public async Task<ActionResult<PlaylistResponse>> UpdatePlaylist(Guid id, [FromBody] UpdatePlaylistRequest request)
    {
        if (!TryGetUserId(out var userId))
            return BadRequest("Invalid or missing UserId header.");

        try
        {
            var updated = await _playlistAppService.UpdatePlaylistAsync(id, userId, request.Name);
            return Ok(MapToResponse(updated));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // ──────────────────────────────────────────────────────────────
    // DELETE /api/playlists/{id}   (bonus)
    // ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Soft-deletes a playlist that belongs to the authenticated user.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlaylist(Guid id)
    {
        if (!TryGetUserId(out var userId))
            return BadRequest("Invalid or missing UserId header.");

        try
        {
            await _playlistAppService.DeletePlaylistAsync(id, userId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // ──────────────────────────────────────────────────────────────
    // DELETE /api/playlists/{id}/songs/{songId}   (bonus)
    // ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Removes a song from a playlist that belongs to the authenticated user.
    /// </summary>
    [HttpDelete("{id}/songs/{songId}")]
    public async Task<IActionResult> RemoveSongFromPlaylist(Guid id, Guid songId)
    {
        if (!TryGetUserId(out var userId))
            return BadRequest("Invalid or missing UserId header.");

        try
        {
            await _playlistAppService.RemoveSongFromPlaylistAsync(id, userId, songId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // ──────────────────────────────────────────────────────────────
    // Private mapping helper
    // ──────────────────────────────────────────────────────────────

    private static PlaylistResponse MapToResponse(PlaylistService.Domain.Entities.Playlist playlist) =>
        new()
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
}