using GXPEngine;

public class MainMenu : GameObject
{
    MenuManager menuManager;
    GameSettings settings;

    StartButton startButton;


    public MainMenu(MenuManager menuManager, GameSettings settings) : base()
    {

        this.menuManager = menuManager;
        this.settings = settings;

        // Creating the start button
        startButton = new StartButton(settings);
        startButton.SetXY(game.width / 2, game.height / 2 - startButton.width / 2);
        AddChild(startButton);

    }
}

