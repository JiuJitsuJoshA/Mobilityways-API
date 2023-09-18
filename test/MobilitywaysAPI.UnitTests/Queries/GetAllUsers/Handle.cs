using AutoFixture;
using FluentAssertions;
using MobilitywayAPI.Shared;
using MobilitywaysAPI.Application.Result;
using MobilitywaysAPI.Application.Users.Queries.GetAllUsers;
using MobilitywaysAPI.Domain;
using MobilitywaysAPI.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilitywaysAPI.UnitTests.Queries.GetAllUsers;

public class Handle
{
    private readonly Mock<IRepository<User>> _mockUserRepository = new();
    private readonly Fixture _autoFixture = new();

    [Fact]
    public async Task Handle_ReturnsOk_WithUsers()
    {
        // Arrange
        var users = _autoFixture.CreateMany<User>();
        var usersToCheck = users.Select(_ => new UserViewDto
        {
            Name = _.Name,
            Email = _.Email
        });

        var query = _autoFixture.Create<GetAllUsersQuery>();

        _mockUserRepository.Setup(_ => _.GetAll()).Returns(users.AsQueryable());

        var sut = new Handler(_mockUserRepository.Object);

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.Type.Should().Be(ResultType.Ok);
        result.Value.Should().BeEquivalentTo(usersToCheck);
    }

    [Fact]
    public async Task Handle_ReturnExceptionType_When_ErrorOccurs()
    {
        // Arrange
        var query = _autoFixture.Create<GetAllUsersQuery>();
        _mockUserRepository.Setup(_ => _.GetAll()).Throws(new Exception());

        var sut = new Handler(_mockUserRepository.Object);

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.Type.Should().Be(ResultType.Exception);
    }
}
