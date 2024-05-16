using GXPEngine;

public class GameOverMenu : GameObject
{
    GameSettings settings;
    MenuManager menuManager;

    RestartButton restartButton;
    LevelManager levelManager;
    ToMenuButton toMenuButton;
    public GameOverMenu (GameSettings settings, MenuManager menuManager,LevelManager levelManager) : base ()
    {
        this.settings = settings;
        this.menuManager = menuManager;
        this.levelManager = levelManager;

        Sprite background = new Sprite("BACKGROUND FOR UI.png");
        this.AddChild(background);
        background.width = game.width + 40;
        background.height = game.height + 40;
        background.x = -20;
        background.y = -20;

        Sprite border = new Sprite("banner.png");
        border.SetOrigin(border.width / 2, border.height / 2);
        border.SetXY(game.width /2, game.height /2);
        border.scale = 0.8f;
        this.AddChild (border);

        Sprite gameOver = new Sprite("GAME OVER.png");
        gameOver.SetOrigin (gameOver.width /2, gameOver.height /2);
        gameOver.scale = 0.6f;
        gameOver.SetXY(border.x, border.y - gameOver.height);
        this.AddChild(gameOver);

        Sprite line = new Sprite("LINE.png");
        line.SetOrigin (line.width /2, line.height /2);
        line.scale = 0.8f;
        line.SetXY (game.width /2, game.height /2 + 60);
        this.AddChild(line);

        restartButton = new RestartButton(settings);
        restartButton.SetOrigin(restartButton.width / 2, restartButton.height / 2);
        restartButton.scale = 0.8f;
        restartButton.SetXY(line.x, line.y - restartButton.height /2 + 20);
        this.AddChild(restartButton);

        toMenuButton = new ToMenuButton();
        toMenuButton.SetOrigin (toMenuButton.width /2, toMenuButton.height / 2);
        toMenuButton.scale = 0.8f;
        toMenuButton.SetXY (line.x, line.y + toMenuButton.height /2 - 20);
        this.AddChild (toMenuButton);
    }

    void Update ()
    {
        if (restartButton.hasBeenPressed && settings.isGameOver)
        {
            levelManager.currentLevelIndex = 0;
            levelManager.LoadLevel(levelManager.currentLevelIndex);
            //World.bodyList.Clear();

            settings.stuffDrawn = false;
            settings.isGameOver = false;
            settings.startGame = true;
            settings.phase = 1;
            settings.ghostSpawned = false;
            settings.levelSetup = false;

            this.LateDestroy();
        }
        else if (toMenuButton.hasBeenPressed)
        {
            levelManager.currentLevelIndex = 0;
            levelManager.LoadLevel(levelManager.currentLevelIndex);

            settings.stuffDrawn = false;
            settings.isGameOver = false;
            settings.startGame = false;
            settings.ghostSpawned = false;
            settings.levelSetup = false;

            menuManager.SetMainMenu();
            this.LateDestroy ();
        }
    }
}