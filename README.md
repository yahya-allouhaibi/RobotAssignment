# RobotAssignment

# Robot Assignment API

This project is a simple RESTful API for controlling a virtual robot in a room. Users can initialize the robot with specific direction and a start position, and then execute commands to control its movements.

## Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Technologies](#technologies)
- [Setup and Installation](#setup-and-installation)
- [Endpoints](#endpoints)
- [Testing](#testing)

## Overview

The Robot Assignment API lets users initialize a robot in a room, set its start position and direction, and execute commands to move it. The robot’s position and direction can be controlled by sending specific commands, and users receive reports on its new position. If the robot walks out of the room bounds then an error is thrown, and the robot returns to its last position.

## Features

- **Robot Initialization**: Set up a room and place the robot at a specified position with an initial direction.
- **Command Execution**: Control the robot with commands (L, R, F) to turn left, turn right, or move forward.

## Technologies

- **ASP.NET Core**: For building the RESTful API.
- **C#**: The primary language for the application.
- **FluentValidation**: For validation logic on input data.
- **Moq**: Used in unit and integration tests to mock dependencies.
- **Xunit**: Test framework for unit and integration tests.

## Setup and Installation

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual Studio](https://visualstudio.microsoft.com/downloads/) or another code editor

### Steps

1. Start by clonimg the repository.
2. Start the app in Visual studio.
3. Run the application.
4. The API will start on http://localhost:5000 (or https://localhost:5001 for HTTPS).

## Endpoints

POST /api/Robot/InitializeRobot <br/>
Description: Initializes the robot in a defined room.

Parameters:

roomWidth (int): Width of the room <br/>
roomDepth (int): Depth of the room <br/>
startPositionX (int): Initial X-coordinate for the robot <br/>
startPositionY (int): Initial Y-coordinate for the robot <br/>
robotStartDirection (Direction): Initial direction for the robot (N, S, E, W) <br/>

Response:

200 OK: If initialization is successful ("Robot initialized").<br/>
400 Bad Request: If validation fails (e.g., invalid starting position).<br/>

POST /api/Robot/ExecuteCommands<br/>
Description: Executes movement commands for the robot.

Parameters:

commands (string): String of commands (L, R, F)<br/>

Response:

200 OK: If commands are executed successfully, returns the robot’s new position.<br/>
400 Bad Request: If commands are invalid or the input is empty.<br/>

## Testing
The application includes both unit and integration tests using Xunit and Moq.

Running The Tests:

To run all tests, use the following command: dotnet test <br/>
Or simply run the tests in the test explorer in visual studio.
