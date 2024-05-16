using GXPEngine;

public class MenuManager : GameObject
{
    // Settings reference
    GameSettings settings;
    LevelManager levelManager;

    private AnimationSprite gameOverText;
    bool animationStarted;

    public MenuManager(GameSettings settings, LevelManager levelManager) : base()
    {

        this.settings = settings;

        // Creating a game over sprite
        gameOverText = new AnimationSprite("gameOver.png", 2, 1);
        gameOverText.SetXY(game.width / 2 - gameOverText.width / 2, game.height / 2 - gameOverText.height / 2);
        this.levelManager = levelManager;
    }

    public void SetCurrentMenu(GameObject menu)
    {
        // Adds the current menu
        game.AddChild(menu);
    }

    public void SetMainMenu()
    {
        // Sets the current menu to the main menu
        SetCurrentMenu(new MainMenu(this, settings, levelManager));

        // Deletes the options menu
        foreach (GameObject child in game.GetChildren())
        {
            if (child is OptionsMenu)
            {
                child.LateDestroy();
            }
        }
    }

    public void SetOptionsMenu()
    {
        // Sets the current menu to options menu
        SetCurrentMenu(new OptionsMenu(this));

        // Deletes the main menu
        foreach (GameObject child in game.GetChildren())
        {
            if (child is MainMenu)
            {
                child.LateDestroy();
            }
        }
    }

    public void SetGameOverMenu()
    {
        SetCurrentMenu(new GameOverMenu(settings, this, levelManager));
        settings.startGame = false;
    }

    public void RemoveCurrentMenu()
    {
        // Deletes the main menu
        foreach (GameObject child in game.GetChildren())
        {
            if (child is MainMenu || child is OptionsMenu)
            {
                child.LateDestroy();
            }
        }
    }
}
