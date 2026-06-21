using Moq;
using PlaylistService.Application.Interfaces;
using PlaylistService.Domain.Entities;
using PlaylistService.Application.Services;

namespace PlaylistService.UnitTests.Tests;

public class PlaylistServiceTests
{
    private readonly Mock<IPlaylistRepository> _repositoryMock;
    private readonly PlaylistService.Application.Services.PlaylistAppService _service;

    public PlaylistServiceTests()
    {
        _repositoryMock = new Mock<IPlaylistRepository>();
        _service = new PlaylistService.Application.Services.PlaylistAppService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreatePlaylistAsync_CreatesPlaylist_AndCallsRepository()
    {
        // Arrange
        var name = "My Playlist";
        var userId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Playlist>()))
            .ReturnsAsync((Playlist p) => p);

        // Act
        var result = await _service.CreatePlaylistAsync(name, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Name);
        Assert.Equal(userId, result.UserId);

        _repositoryMock.Verify(
            r => r.CreateAsync(It.Is<Playlist>(p => p.Name == name && p.UserId == userId)),
            Times.Once);
    }

    [Fact]
    public async Task AddSongToPlaylistAsync_WhenPlaylistAndSongExist_AddsSongAndUpdatesRepository()
    {
        // Arrange
        var playlistId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var songId = Guid.NewGuid();

        var playlist = new Playlist("Test Playlist", userId);
        var song = new Song("Believer", "Imagine Dragons", TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(24)), "Rock");

        _repositoryMock
            .Setup(r => r.GetByIdAndUserIdAsync(playlistId, userId))
            .ReturnsAsync(playlist);

        _repositoryMock
            .Setup(r => r.GetSongByIdAsync(songId))
            .ReturnsAsync(song);

        // Act
        await _service.AddSongToPlaylistAsync(playlistId, userId, songId);

        // Assert
        _repositoryMock.Verify(r => r.UpdateAsync(playlist), Times.Once);
    }

    [Fact]
    public async Task AddSongToPlaylistAsync_WhenPlaylistDoesNotBelongToUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var playlistId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var songId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAndUserIdAsync(playlistId, userId))
            .ReturnsAsync((Playlist?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.AddSongToPlaylistAsync(playlistId, userId, songId));
    }

    [Fact]
    public async Task AddSongToPlaylistAsync_WhenSongDoesNotExist_ThrowsInvalidOperationException()
    {
        // Arrange
        var playlistId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var songId = Guid.NewGuid();

        var playlist = new Playlist("Test Playlist", userId);

        _repositoryMock
            .Setup(r => r.GetByIdAndUserIdAsync(playlistId, userId))
            .ReturnsAsync(playlist);

        _repositoryMock
            .Setup(r => r.GetSongByIdAsync(songId))
            .ReturnsAsync((Song?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.AddSongToPlaylistAsync(playlistId, userId, songId));
    }

    [Fact]
    public async Task AddSongToPlaylistAsync_WhenSongAlreadyExists_ThrowsKeyNotFoundException()
    {
        // Arrange
        var playlistId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var songId = Guid.NewGuid();

        var playlist = new Playlist("Test Playlist", userId);
        var song = new Song("Believer", "Imagine Dragons", TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(24)), "Rock");

        _repositoryMock
            .Setup(r => r.GetByIdAndUserIdAsync(playlistId, userId))
            .ReturnsAsync(playlist);

        _repositoryMock
            .Setup(r => r.GetSongByIdAsync(songId))
            .ReturnsAsync(song);

        playlist.AddSong(songId);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.AddSongToPlaylistAsync(playlistId, userId, songId));
    }
}