using FCG.Domain.Entity;

namespace FCG.Test.Entity;

[TestClass]
public sealed class GameTests
{
    private readonly GameEntity _game = new("The Witcher 3", "Action RPG");


    [TestCategory("Domain")]
    [TestMethod("Create Game with Valid Data")]
    public void CreateGame_ShouldCreateGameWithValidData()
    {
        Assert.AreEqual("The Witcher 3", _game.Name);
    }
}