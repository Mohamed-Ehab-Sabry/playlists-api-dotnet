using System.Net;
using System.Net.Http.Json;

namespace PlaylistService.IntegrationTests;

public class PlaylistEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PlaylistEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUserPlaylists_ReturnsBadRequest_WhenUserIdHeaderIsMissing()
    {
        // Act
        var response = await _client.GetAsync("/api/playlists");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreatePlaylist_ReturnsBadRequest_WhenUserIdHeaderIsMissing()
    {
        // Act
        var response = await _client.PostAsJsonAsync("/api/playlists", new
        {
            name = "Test Playlist"
        });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreatePlaylist_ReturnsCreated_WhenUserIdHeaderIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _client.DefaultRequestHeaders.Add("UserId", userId.ToString());

        // Act
        var response = await _client.PostAsJsonAsync("/api/playlists", new
        {
            name = "My Playlist"
        });

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}