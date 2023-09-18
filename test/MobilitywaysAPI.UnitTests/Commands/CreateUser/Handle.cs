using AutoFixture;
using FluentAssertions;
using MobilitywayAPI.Shared;
using MobilitywaysAPI.Application.Interfaces;
using MobilitywaysAPI.Application.Result;
using MobilitywaysAPI.Application.Users.Commands.CreateUser;
using MobilitywaysAPI.Domain;
using MobilitywaysAPI.Domain.Entities;
using Moq;
using System.Text;

namespace MobilitywaysAPI.UnitTests.Commands.CreateUser;

public class Handle
{
    private readonly Mock<IRepository<User>> _mockUserRepository = new();
    private readonly Mock<IPasswordService> _mockPasswordService = new();
    private readonly Fixture _autoFixture = new();

    [Fact]
    public async Task Handle_WithNonExistingUser_AddsUserToDatabase()
    {
        // Arrange
        var users = _autoFixture.CreateMany<User>();
        var command = CreateUserCommand();

        _mockUserRepository.Setup(_ => _.GetAll()).Returns(users.AsQueryable());
        _mockPasswordService.Setup(x => x.HashPassword(It.IsAny<string>())).Returns(It.IsAny<string>());

        var sut = new Handler(_mockUserRepository.Object, _mockPasswordService.Object);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        _mockUserRepository.Verify(_ => _.AddAsync(It.IsAny<User>()), Times.Once);
        _mockUserRepository.Verify(_ => _.SaveChangesAsync(), Times.Once);
        result.Type.Should().Be(ResultType.Created);
        result.Message.Should().Be($"{command.User.Name} successfully created");
    }

    [Fact]
    public async Task Handle_WithExistingUser_ReturnsAlreadyExists()
    {
        // Arrange
        var users = _autoFixture.CreateMany<User>();
        var existingUser = _autoFixture.Create<User>();
        var allUsers = users.Append(existingUser);

        var command = CreateUserCommand(existingUser.Email);

        _mockUserRepository.Setup(_ => _.GetAll()).Returns(allUsers.AsQueryable());
        _mockPasswordService.Setup(x => x.HashPassword(It.IsAny<string>())).Returns(It.IsAny<string>());

        var sut = new Handler(_mockUserRepository.Object, _mockPasswordService.Object);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        _mockUserRepository.Verify(_ => _.AddAsync(It.IsAny<User>()), Times.Never);
        _mockUserRepository.Verify(_ => _.SaveChangesAsync(), Times.Never);
        result.Type.Should().Be(ResultType.AlreadyExists);
        result.Message.Should().Be("user already exists");
    }

    [Fact]
    public async Task Handle_WithHashPasswordException_ReturnsException()
    {
        // Arrange
        var users = _autoFixture.CreateMany<User>();
        var command = CreateUserCommand();

        _mockUserRepository.Setup(_ => _.GetAll()).Returns(users.AsQueryable());

        _mockPasswordService.Setup(x => x.HashPassword(It.IsAny<string>())).Throws<Exception>();

        var sut = new Handler(_mockUserRepository.Object, _mockPasswordService.Object);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        _mockUserRepository.Verify(_ => _.AddAsync(It.IsAny<User>()), Times.Never);
        _mockUserRepository.Verify(_ => _.SaveChangesAsync(), Times.Never);
        result.Type.Should().Be(ResultType.Exception);
    }

    private CreateUserCommand CreateUserCommand(string? email = null)
    {
        return new CreateUserCommand
        {
            User = new UserDto
            {
                Name = _autoFixture.Create<string>(),
                Email = email ?? _autoFixture.Create<string>(),
                Password = _autoFixture.Create<string>()
            }
        };
    }
}
