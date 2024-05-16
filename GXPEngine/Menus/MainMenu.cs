using GXPEngine;

public class MainMenu : GameObject
{
    MenuManager menuManager;
    GameSettings settings;
    LevelManager levelManager;

    StartButton startButton;

    AnimationSprite center;

    public MainMenu(MenuManager menuManager, GameSettings settings, LevelManager levelManager) : base()
    {
        Sprite background = new Sprite("BACKGROUND FOR UI.png");
        this.AddChild(background);
        background.width = game.width + 40;
        background.height = game.height + 40;
        background.x = -20;
        background.y = -20;

        Sprite logo = new Sprite("LOGO.png");
        logo.SetOrigin(logo.width / 2, logo.height / 2);
        logo.SetXY(game.width / 2, game.height / 2 - logo.height / 2 + 40);
        this.AddChild(logo);

        Sprite dots = new Sprite("DOTS.png");
        dots.SetOrigin(dots.width / 2, dots.height / 2);
        dots.scale = 0.6f;
        dots.SetXY(game.width / 2, game.height / 2 + logo.height / 2 - dots.height / 2 + 40);
        this.AddChild(dots);

        center = new AnimationSprite("start portal sprite sheet.png", 3, 1);
        center.SetOrigin(center.width / 2, center.height / 2);
        center.scale = 0.8f;
        center.SetXY(dots.x, dots.y);
        this.AddChild(center);


        this.menuManager = menuManager;
        this.settings = settings;

        // Creating the start button
        startButton = new StartButton(settings, levelManager);
        startButton.SetXY(dots.x, dots.y - dots.height / 2 - 15);
        AddChild(startButton);

        LevelsButton levelButton = new LevelsButton();
        levelButton.SetXY(dots.x - dots.width / 2 - levelButton.width / 2 + 50, dots.y - 35);
        AddChild(levelButton);

        OptionsButton optionsButton = new OptionsButton();
        optionsButton.SetXY(dots.x - dots.width / 2 - optionsButton.width / 2 + 50, dots.y + 35);
        AddChild(optionsButton);

        LoadButton loadButton = new LoadButton();
        loadButton.SetXY(dots.x + dots.width / 2 + loadButton.width / 2 - 50, dots.y - 35);
        AddChild(loadButton);

        ContinueButton continueButton = new ContinueButton();
        continueButton.SetXY(dots.x + dots.width / 2 + continueButton.width / 2 - 50, dots.y + 35);
        AddChild(continueButton);

        CreditsButton creditsButton = new CreditsButton();
        creditsButton.SetXY(dots.x, dots.y + dots.height / 2 + 15);
        AddChild(creditsButton);
        this.levelManager = levelManager;
    }

    void Update ()
    {
        center.Animate(0.04f);
    }
}

