using GXPEngine;

public class MainMenu : GameObject
{
    MenuManager menuManager;
    GameSettings settings;

    StartButton startButton;
    OptionsButton optionsButton;
    ExitButton exitButton;

    public MainMenu(MenuManager menuManager, GameSettings settings) : base()
    {

        this.menuManager = menuManager;
        this.settings = settings;

        // Creating the start button
        startButton = new StartButton(settings);
        startButton.SetXY(game.width / 2, game.height / 2 - startButton.width / 2);
        AddChild(startButton);

        // Creating the options button
        optionsButton = new OptionsButton(menuManager);
        optionsButton.SetXY(game.width / 2, game.height / 2);
        AddChild(optionsButton);

        // Creating the exit button
        exitButton = new ExitButton();
        exitButton.SetXY(game.width / 2, game.height / 2 + exitButton.width / 2);
        AddChild(exitButton);
    }
}

