using AutoFixture;
using FluentAssertions;
using MobilitywaysAPI.Application.Result;
using MobilitywaysAPI.Application.Users.Queries.ValidateUser;
using MobilitywaysAPI.Domain;
using MobilitywaysAPI.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilitywaysAPI.UnitTests.Queries.ValidateUser;

public class Handle
{
    private readonly Mock<IRepository<User>> _mockUserRepository = new();
    private readonly Fixture _autoFixture = new();

    [Fact]
    public async Task Handle_Returns_TrueResult_When_UserFound()
    {
        //Arrange
        var existingUser = _autoFixture.Create<User>();
        var users = _autoFixture.CreateMany<User>();
        var allUsers = users.Append(existingUser);

        var query = new ValidateUserQuery(existingUser.Email);

        _mockUserRepository.Setup(_ => _.GetAll()).Returns(allUsers.AsQueryable());

        var sut = new Handler(_mockUserRepository.Object);

        //Act
        var result = await sut.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Type.Should().Be(ResultType.Ok);
    }

    [Fact]
    public async Task Handle_Returns_FalseResult_When_UserNotFound()
    {
        //Arrange
        var users = _autoFixture.CreateMany<User>();

        var query = new ValidateUserQuery(_autoFixture.Create<string>());

        _mockUserRepository.Setup(_ => _.GetAll()).Returns(users.AsQueryable());

        var sut = new Handler(_mockUserRepository.Object);

        //Act
        var result = await sut.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Type.Should().Be(ResultType.FailedValidation);
    }

    [Fact]
    public async Task Handle_Returns_ExceptionResult_When_ErrorOccurs()
    {
        //Arrange
        var users = _autoFixture.CreateMany<User>();

        var query = new ValidateUserQuery(_autoFixture.Create<string>());

        _mockUserRepository.Setup(_ => _.GetAll()).Throws(new Exception());

        var sut = new Handler(_mockUserRepository.Object);

        //Act
        var result = await sut.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Type.Should().Be(ResultType.Exception);
    }
}
