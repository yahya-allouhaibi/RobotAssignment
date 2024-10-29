using Microsoft.AspNetCore.Mvc.Testing;
using RobotAssignment.Enums;
namespace RobotAssignment.Tests;

public class RobotControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public RobotControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async void InitializeRobot_With_Valid_Input_ReturnsOK()
    {
        // Arrange
        var requestUrl = "/api/Robot/InitializeRobot";
        var roomWidth = 5;
        var roomDepth = 5;
        var startPositionX = 1;
        var startPositionY = 1;
        var robotStartDirection = Direction.N;

        var queryParams = $"?roomWidth={roomWidth}&roomDepth={roomDepth}&startPositionX={startPositionX}&startPositionY={startPositionY}&robotStartDirection={robotStartDirection}";

        // Act
        var response = await _client.PostAsync(requestUrl + queryParams, null);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Robot initialized", result);
    }

    [Fact]
    public async void InitializeRobot_With_Negative_Values_Returns_BadRequest()
    {
        // Arrange
        var requestUrl = "/api/Robot/InitializeRobot";
        var roomWidth = -5;
        var roomDepth = 5;
        var startPositionX = 1;
        var startPositionY = 1;
        var robotStartDirection = Direction.N;

        var queryParams = $"?roomWidth={roomWidth}&roomDepth={roomDepth}&startPositionX={startPositionX}&startPositionY={startPositionY}&robotStartDirection={robotStartDirection}";

        // Act
        var response = await _client.PostAsync(requestUrl + queryParams, null);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Width can not be negative", result);
    }
    
    [Fact]
    public async void InitializeRobot_With_TheRobot_Outside_TheRoom_Returns_BadRequest()
    {
        // Arrange
        var requestUrl = "/api/Robot/InitializeRobot";
        var roomWidth = 5;
        var roomDepth = 5;
        var startPositionX = 8;
        var startPositionY = 8;
        var robotStartDirection = Direction.N;

        var queryParams = $"?roomWidth={roomWidth}&roomDepth={roomDepth}&startPositionX={startPositionX}&startPositionY={startPositionY}&robotStartDirection={robotStartDirection}";

        // Act
        var response = await _client.PostAsync(requestUrl + queryParams, null);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("The robot can not start from outside the room", result);
    }

    [Fact]
    public async Task ExecuteCommands_Should_ReturnOk_WithNewPosition_WhenCommandsAreValid()
    {
        // Arrange
        var requestUrl = "/api/Robot/ExecuteCommands";
        var commands = "FLLR";
        var queryparams = $"?commands={commands}";

        await InitializeRobot();

        // Act
        var response = await _client.PostAsync(requestUrl + queryparams, null);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("Report: 1 2 W", result);
    }

    [Fact]
    public async Task ExecuteCommands_Should_ReturnBadRequest_When_Robot_Walks_OutOfThe_Room()
    {
        // Arrange
        var requestUrl = "/api/Robot/ExecuteCommands";
        var commands = "FLLRFFFFFFFFFFF";
        var queryparams = $"?commands={commands}";

        await InitializeRobot();

        // Act
        var response = await _client.PostAsync(requestUrl + queryparams, null);

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("The robot walked outside the room bounds.", result);
    }
    
    [Fact]
    public async Task ExecuteCommands_Should_ReturnBadRequest_When_Input_IsInvalid()
    {
        // Arrange
        var requestUrl = "/api/Robot/ExecuteCommands";
        var commands = "FLLRFFFFKK";
        var queryparams = $"?commands={commands}";

        await InitializeRobot();

        // Act
        var response = await _client.PostAsync(requestUrl + queryparams, null);

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("The commands can not contain characters other than L, R and F", result);
    }

    public async Task InitializeRobot()
    {
        var roomWidth = 5;
        var roomDepth = 5;
        var startPositionX = 1;
        var startPositionY = 1;
        var robotStartDirection = Direction.N;
        var requestUrl = "/api/Robot/InitializeRobot";
        var queryParams = $"?roomWidth={roomWidth}&roomDepth={roomDepth}&startPositionX={startPositionX}&startPositionY={startPositionY}&robotStartDirection={robotStartDirection}";

        await _client.PostAsync(requestUrl + queryParams, null);
    }
}
