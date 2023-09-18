using MobilitywayAPI.Shared;
using MobilitywaysAPI.Application.Interfaces;
using MobilitywaysAPI.Application.Users.Queries.Login;
using MobilitywaysAPI.Domain.Entities;
using MobilitywaysAPI.Domain;
using System.Text;
using AutoFixture;
using Moq;
using FluentAssertions;

namespace MobilitywaysAPI.UnitTests.Queries.Login;

public class Handle
{
    private readonly Mock<IRepository<User>> _mockUserRepository = new();
    private readonly Mock<IPasswordService> _mockPasswordService = new();
    private readonly Mock<IJwtService> _mockJwtService = new();
    private readonly Fixture _autoFixture = new();

    [Fact]
    public async Task Handle_ValidLogin_ReturnsToken()
    {
        //Arrange
        var existingUser = _autoFixture.Create<User>();
        var users = _autoFixture.CreateMany<User>();
        var allUsers = users.Append(existingUser);
        var query = new LoginUserQuery
        {
            UserLogin = new UserLoginDto
            {
                Email = existingUser.Email,
                Password = _autoFixture.Create<string>()
            }
        };

        var token = _autoFixture.Create<string>();

        _mockUserRepository.Setup(_ => _.GetAll()).Returns(allUsers.AsQueryable());
        _mockPasswordService.Setup(_ => _.VerifyPassword(query.UserLogin.Password, existingUser.PasswordHash)).Returns(true);
        _mockJwtService.Setup(_ => _.GenerateToken(query.UserLogin)).Returns(token);

        var sut = new Handler(_mockUserRepository.Object, _mockPasswordService.Object, _mockJwtService.Object);

        //Act
        var result = await sut.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(token);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsFailure()
    {
        //Arrange
        var users = _autoFixture.CreateMany<User>();
        var query = new LoginUserQuery
        {
            UserLogin = new UserLoginDto
            {
                Email = _autoFixture.Create<string>(),
                Password = _autoFixture.Create<string>()
            }

        };

        _mockUserRepository.Setup(_ => _.GetAll()).Returns(users.AsQueryable());

        var sut = new Handler(_mockUserRepository.Object, _mockPasswordService.Object, _mockJwtService.Object);

        //Act
        var result = await sut.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("User not found or password incorrect");
    }

    [Fact]
    public async Task Handle_IncorrectPassword_ReturnsFailure()
    {
        //Arrange
        var existingUser = _autoFixture.Create<User>();
        var users = _autoFixture.CreateMany<User>();
        var allUsers = users.Append(existingUser);
        var query = new LoginUserQuery
        {
            UserLogin = new UserLoginDto
            {
                Email = existingUser.Email,
                Password = _autoFixture.Create<string>()
            }
        };

        _mockUserRepository.Setup(_ => _.GetAll()).Returns(allUsers.AsQueryable());
        _mockPasswordService.Setup(_ => _.VerifyPassword(query.UserLogin.Password, existingUser.PasswordHash)).Returns(false);

        var sut = new Handler(_mockUserRepository.Object, _mockPasswordService.Object, _mockJwtService.Object);

        //Act
        var result = await sut.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("User not found or password incorrect");
    }
}
