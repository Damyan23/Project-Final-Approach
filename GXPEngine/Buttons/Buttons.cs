
using GXPEngine;
using System;

// Start button
public class StartButton : Button
{
    GameSettings settings;
    public StartButton (GameSettings settings) : base ("PlayButton.png", 2, 1)
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
        } else if (hasBeenPressed && settings.startGame)
        {
            settings.levelSetup = true;
            settings.phase = 2;
        }

        base.Update();
    }
}

// Options button
public class OptionsButton : Button
{
    MenuManager menuManager;
    public OptionsButton (MenuManager menuManager) : base ("OptionsButton.png", 2, 1)
    {
        this.menuManager = menuManager;
    }

    protected override void Update()
    {
        if (hasBeenPressed)
        {
            menuManager.SetOptionsMenu();
        }

        base.Update();
    }
}

// Exit button
public class ExitButton : Button
{
    public ExitButton () : base ("ExitButton.png", 2, 1)
    {

    }

    protected override void Update ()
    {
        if (hasBeenPressed)
        {
            System.Environment.Exit(0);
        }

        base.Update ();
    }
}

// Back button
public class BackButton : Button
{
    MenuManager menuManager;

    public BackButton (MenuManager menuManager) : base ("BackButton.png", 2, 1)
    {
        this.menuManager= menuManager;
    }

    protected override void Update()
    {
        if (hasBeenPressed)
        {
            menuManager.SetMainMenu ();
        }

        base.Update();
    }
}
