using FCG.Domain.Entity;
using FCG.Domain.Entity.ValueObject;
using FCG.Domain.ValueObject;
using System.Data;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace FCG.Test.Entity;

[TestClass]
public sealed class UserTests
{
    private readonly UserEntity _user = new("Henrique", new EmailAddress("henrique@gmail.com"), new Password("abc123@"));

    private readonly RoleEntity _role = new("Admin", new List<UserEntity>());

    [TestCategory("Domain")]
    [TestMethod("Create User with Valid Data")]
    public void CreateUser_ShouldCreateUserWithValidData()
    {
        Assert.AreEqual("Henrique", _user.Name);
    }

    [TestCategory("Domain")]
    [TestMethod("Attribute User Role with Valid Data")]
    public void AttributeUserRole_ShouldAttributeUserRoleWithValidData()
    {
        _role.Users?.Add(_user);

        Assert.IsTrue(_role!.Users!.Contains(_user));
    }
}
