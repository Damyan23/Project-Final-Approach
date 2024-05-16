// Start button
public class StartButton : Button
{
    GameSettings settings;
    public StartButton(GameSettings settings) : base("NEW GAME.png", 1, 2)
    {
        this.settings = settings;
    }

    protected override void Update()
    {
        if (hasBeenPressed && !settings.startGame)
        {
            settings.startGame = true;
            settings.isGameOver = false;
            hasBeenPressed = false;
        }
        else if (hasBeenPressed && settings.startGame)
        {
            settings.levelSetup = true;
            settings.phase = 2;
            hasBeenPressed = false;
        }

        base.Update();
    }
}
