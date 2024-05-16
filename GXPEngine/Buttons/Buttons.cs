// Start button
using GXPEngine;
using System;

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

public class PlayButton : Button
{
    GameSettings settings;
    public PlayButton(GameSettings settings) : base("play button.png", 1, 1)
    {
        this.settings = settings;
    }

    protected override void Update()
    {
        if (hasBeenPressed && settings.startGame)
        {
            settings.levelSetup = true;
            settings.phase = 2;
            hasBeenPressed = false;
        }

        base.Update();
    }
}

public class RestartButton : Button
{
    GameSettings settings;
    public RestartButton (GameSettings settings) : base ("RESTART NEW.png", 1, 2)
    {
        this.settings = settings;
    }

    protected override void Update()
    {
        //if (hasBeenPressed)
        //{
        //    MyGame myGame = (MyGame)game;

        //    foreach (GameObject obj in game.GetChildren())
        //    {
        //        obj.LateDestroy();
        //    }

        //    World.bodyList.Clear();

        //    myGame.SetUp();
        //}

        base.Update();
    }
}

public class ToMenuButton : Button
{
    public ToMenuButton () : base ("TO MENU NEW.png", 1, 2)
    {
    }
}