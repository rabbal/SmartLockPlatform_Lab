using FluentAssertions;
using Moq;
using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Domain.Identity;

namespace SmartLockPlatform.Test.Unit;

public class UserTests
{
    private readonly Mock<IPasswordHashAlgorithm> _passwordHashAlgorithm = new();

    [Fact]
    public void Should_create_admin_with_credentials()
    {
        //Arrange
        var email = Email.Create("admin@example.com").Value!;
        const string plainPassword = "@dmIn123";
        var password = Password.Create(plainPassword, PasswordOptions.CreateUnrestraint()).Value!;
        var firstName = new FirstName("first name1");
        var lastName = new LastName("last name1");

        //Act
        var user = new User(firstName, lastName, email, true);
        user.SetPassword(password, _passwordHashAlgorithm.Object);

        //Assert
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Email.Should().Be(email);
        user.IsActive.Should().BeFalse();
        _passwordHashAlgorithm.Verify(a => a.HashPassword(plainPassword), Times.Once);
        user.PasswordHash.Should().NotBeEmpty();
    }
}