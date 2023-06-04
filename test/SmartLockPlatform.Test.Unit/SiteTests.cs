using FakeItEasy;
using FluentAssertions;
using Moq;
using SmartLockPlatform.Domain.Identity;
using SmartLockPlatform.Domain.Sites;

namespace SmartLockPlatform.Test.Unit;

public class SiteTests
{
    [Fact]
    public void Should_create_site_with_owner_and_specific_timezone()
    {
        //Arrange
        var owner = Mock.Of<User>();
        
        var name = "site 1";

        //Act
        var site = new Site(name, owner);

        //Assert
        site.Should().NotBeNull();
        site.Owner.Should().BeEquivalentTo(owner);
        site.Name.Should().Be(name);
    }

    // [Fact]
    // public void Should_add_lock_for_site()
    // {
    //     //Arrange
    //     var owner = new User();
    //     var name = "human readable id";
    //     var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Amsterdam");
    //     var site = new Site(name, owner, timeZone);
    //     var lockUuid = "11.21.22.34";
    //     var lockName = "Front door";
    //
    //     //Act
    //     var result = site.AddLock(lockName, lockUuid);
    //
    //     //Assert
    //     result.Failed.Should().BeFalse();
    //     site.Locks.Should().HaveCount(1);
    //     site.Locks[0].Uuid.Should().Be(lockUuid);
    //     site.Locks[0].Name.Should().Be(lockName);
    // }
    //
    // [Fact]
    // public void Should_find_a_lock_with_given_uuid()
    // {
    //     //Arrange
    //     var owner = new User();
    //     var uuid = "human readable id";
    //     var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Amsterdam");
    //     var site = new Site(uuid, owner, timeZone);
    //     var lockUuid = "11.21.22.34";
    //     var lockName = "Front door";
    //     site.AddLock(lockName, lockUuid);
    //
    //     //Act
    //     var @lock = site.FindLock(lockUuid);
    //
    //     //Assert
    //     @lock.Should().NotBeNull();
    // }
    //
    // [Fact]
    // public void Should_unlock_the_lock_with_given_uuid()
    // {
    //     //Arrange
    //     var owner = new User();
    //     var uuid = "human readable id";
    //     var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Amsterdam");
    //     var site = new Site(uuid, owner, timeZone);
    //     var lockUuid = "11.21.22.34";
    //     var lockName = "Front door";
    //     site.AddLock(lockName, lockUuid);
    //
    //     //Act
    //     var result = site.UnLock(lockUuid, owner);
    //
    //     //Assert
    //     result.Failed.Should().BeFalse();
    //     site.Locks[0].IsLocked.Should().Be(false);
    // }
    //
    // [Fact]
    // public void Should_put_lock_online()
    // {
    //     //Arrange
    //     var owner = new User();
    //     var uuid = "human readable id";
    //     var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Amsterdam");
    //     var site = new Site(uuid, owner, timeZone);
    //     var lockUuid = "11.21.22.34";
    //     var lockName = "Front door";
    //     site.AddLock(lockName, lockUuid);
    //
    //     //Act
    //     var result = site.PutLockOnline(lockUuid);
    //
    //     //Assert
    //     result.Failed.Should().BeFalse();
    //     site.Locks[0].IsOnline.Should().Be(true);
    // }
    //
    // [Fact]
    // public void Should_take_lock_offline()
    // {
    //     //Arrange
    //     var owner = new User();
    //     var uuid = "human readable id";
    //     var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Amsterdam");
    //     var site = new Site(uuid, owner, timeZone);
    //     var lockUuid = "11.21.22.34";
    //     var lockName = "Front door";
    //     site.AddLock(lockName, lockUuid);
    //     site.PutLockOnline(lockUuid);
    //
    //     //Act
    //     var result = site.TakeLockOffline(lockUuid);
    //
    //     //Assert
    //     result.Failed.Should().BeFalse();
    //     site.Locks[0].IsOnline.Should().Be(false);
    // }
}