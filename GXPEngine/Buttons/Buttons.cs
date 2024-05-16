// Start button
public class StartButton : Button
{
    GameSettings settings;
    public StartButton(GameSettings settings) : base("NEW GAME.png", 1, 2)
    {
        this.settings = settings;
        this.scale = 0.6f;
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

public class LevelsButton : Button
{
    public LevelsButton() : base("LEVELS.png", 1, 2)
    {
        this.scale = 0.6f;
    }
}

public class OptionsButton : Button
{ 
    public OptionsButton () : base ("OPTIONS.png", 1, 2)
    {
        this.scale = 0.6f;
    }
}

public class LoadButton : Button
{
    public LoadButton () : base ("LOAD GAME.png", 1, 2)
    {
        this.scale = 0.6f;
    }
}

public class ContinueButton : Button 
{
    public ContinueButton () : base ("CONTINUE.png", 1, 2)
    {
        this.scale = 0.6f;
    }
}


public class CreditsButton : Button
{ 
    public CreditsButton () : base ("CREDITS.png", 1, 2)
    {
        this.scale = 0.6f;
    }
}