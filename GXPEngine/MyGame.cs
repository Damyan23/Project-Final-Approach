using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;

public class MyGame : Game
{
    static void Main()
    {
        new MyGame().Start();
    }

    public Ball GetPlayer()
    {
        return ball;
    }

    GameSettings settings;
    MenuManager menuManager;
    LevelManager levelManager;

    Ball ball;

    Box ground;
    Box slope1;
    Box slope2;
    Box slope3;
    Box mode2Box;

    Explosive explosive;

    StartButton playButton;

    Sprite spawnPoint;

    List<Ball> placedObjects;

    EasyDraw _text;

    World world;

    Random rand;

    Player player;

    Sprite square;
    Sprite rect;

    Sprite currentlyHoldingSprite;

    bool holdingBlock = false;

    private bool playButtonAdded;

    string squareFile = "square.png";
    string rectFile = "rect.png";

    string fileOfHoldingSprite;

    HalfPipeLeft pipe;

    Mushroom platform;

    Sound backgroundMusic;
    public MyGame() : base(1280, 720, false, false)
    {
        SetUp();
    }

    void SetUp()
    {
        settings = new GameSettings();
        menuManager = new MenuManager(settings);
        menuManager.SetMainMenu();

        levelManager = new LevelManager(settings);
        this.AddChild(levelManager);

        world = new World(this);
        rand = new Random();

        playButton = new StartButton(settings);
        playButton.SetXY(width / 2, height / 2 - 250);

        placedObjects = new List<Ball>();

        backgroundMusic = new Sound("game music5.mp3", true);
    }


    void Update()
    {
        this.targetFps = 60;

        world.Step(Time.deltaTimeInSeconds, 20);

        if (!settings.isGameOver && settings.startGame && !settings.stuffDrawn)
        {
            menuManager.RemoveCurrentMenu();
            levelManager.Start();

            backgroundMusic.Play();

            this.AddChild(platform);
            DrawPlaceableObjects();

            settings.stuffDrawn = true;
        }

        if (settings.phase == 2 && !settings.ghostSpawned && settings.stuffDrawn)
        {
            ball = new Ball(rand.Next(15, 40), new Vector2(levelManager.currentLevelSpawnPoint.x, levelManager.currentLevelSpawnPoint.y), 0.001f, 0.8f, 1);
            ball.level = levelManager.currentLevelIndex + 1;
            AddChild(ball);
            placedObjects.Add(ball);
            this.RemoveChild(playButton);

            playButtonAdded = false;
            settings.ghostSpawned = true;
        }

        if (!playButtonAdded && settings.phase == 1 && settings.stuffDrawn)
        {
            this.AddChild(playButton);
            playButtonAdded = true;
        }

        if (settings.ghostSpawned)
        {
            if (ball.y > this.height - ball.height / 2)
            {
                ball.LateDestroy();
                settings.phase = 1;
                settings.ghostSpawned = false;
            }
            else if (ball.y < ball.height / 2)
            {
                ball.LateDestroy();
                settings.phase = 1;
                settings.ghostSpawned = false;
            }

            if (ball.x > this.width - ball.width / 2)
            {
                ball.LateDestroy();
                settings.phase = 1;
                settings.ghostSpawned = false;
            }
            else if (ball.x < ball.width / 2)
            {
                ball.LateDestroy();
                settings.phase = 1;
                settings.ghostSpawned = false;
            }
        }

        RemoveAHoveredPlatform();

        if (Input.GetKey(Key.G))
        {
            settings.phase = 2;

            holdingBlock = false;
            ShowMouse(true);
        }

        if (holdingBlock)
        {
            PlaceBlock();
        }

        if (Input.GetMouseButtonDown(0) && !holdingBlock && !settings.ghostSpawned)
        {
            if (PickupPlaceableObject(out fileOfHoldingSprite))
            {
                ShowMouse(false);
                holdingBlock = true;
            }
        }
    }

    //Shit function, did not cook
    bool PickupPlaceableObject(out string file)
    {
        file = null;

        // Create a temporary Box object to check if it can be added
        Box tempBox = new Box(1, 1, new Vector2(), 1, 0, 0, true, 0);

        if (levelManager.levels[levelManager.currentLevelIndex].CanAddObject(tempBox))
        {
            if (square != null && rect != null)
            {
                if (square.HitTestPoint(Input.mouseX, Input.mouseY) && settings.phase == 1)
                {
                    file = squareFile;

                    currentlyHoldingSprite = new Sprite(file);
                    currentlyHoldingSprite.SetOrigin(currentlyHoldingSprite.width / 2, currentlyHoldingSprite.height / 2);
                    currentlyHoldingSprite.SetScaleXY(0.05f);
                    AddChild(currentlyHoldingSprite);

                    return true;
                }
                else if (rect.HitTestPoint(Input.mouseX, Input.mouseY) && settings.phase == 1)
                {
                    file = rectFile;

                    currentlyHoldingSprite = new Sprite(file);
                    currentlyHoldingSprite.SetOrigin(currentlyHoldingSprite.width / 2, currentlyHoldingSprite.height / 2);
                    currentlyHoldingSprite.SetScaleXY(0.02f);
                    AddChild(currentlyHoldingSprite);

                    return true;
                }
            }
        }
        return false;
    }

    void PlaceBlock()
    { 
        currentlyHoldingSprite.SetXY(Input.mouseX, Input.mouseY);

        if (Input.GetMouseButtonDown(0))
        {

            int levelIndex = levelManager.currentLevelIndex;

            Level level = levelManager.levels[levelIndex];

            if (fileOfHoldingSprite == squareFile)
            {
                bool success = true;

                
                Box box = new Box(50, 50, new Vector2(Input.mouseX, Input.mouseY), 1f, 0.8f, 1, true);

                foreach (GameObject obj in level.objects)
                {
                    if (obj.HitTest(box))
                    {
                        success = false;
                    }
                }


                foreach(Teleporter obj in level.teleporters)
                {
                    if (!success) break;

                    if(obj.Entrance.HitTest(box) || obj.Exit.HitTest(box))
                    {
                        success = false;
                    }
                }

                if (success)
                {
                    levelManager.AddObject(box);
                }
                else
                {
                    box.LateDestroy();
                }
                
            }



            else if (fileOfHoldingSprite == rectFile)
            {
                bool success = true;

                Box rect = new Box(100, 50, new Vector2(Input.mouseX, Input.mouseY), 1f, 0.8f, 1, true);

                foreach (GameObject obj in level.objects)
                {
                    if (obj.HitTest(rect))
                    {
                        success = false;
                    }
                }

                foreach (Teleporter obj in level.teleporters)
                {
                    if (obj.Entrance.HitTest(rect) || obj.Exit.HitTest(rect))
                    {
                        success = false;
                    }
                }

                if (success)
                {
                    levelManager.AddObject(rect);
                }
                else
                {
                    rect.LateDestroy();
                }
            }

            holdingBlock = false;

            ShowMouse(true);

            RemoveChild(square);
            RemoveChild(rect);

            DrawPlaceableObjects();

            currentlyHoldingSprite.LateDestroy();
        }
    }

    void RemoveAHoveredPlatform()
    {
        if (Input.GetMouseButtonDown(1))
        {
            foreach (GameObject obj in levelManager.playerAddedObjects)
            {
                if (obj.HitTestPoint(Input.mouseX, Input.mouseY) && settings.phase == 1)
                {
                    levelManager.RemoveObject(obj);
                    break;
                }
            }
        }
    }

    void DrawPlaceableObjects()
    {
        square = new Sprite(squareFile);
        square.collider.isTrigger = true;
        square.SetOrigin(square.width / 2, square.height / 2);
        square.SetScaleXY(0.05f, 0.05f);
        square.SetXY(width / 3, height - 100);
        AddChild(square);

        rect = new Sprite(rectFile);
        rect.collider.isTrigger = true;
        rect.SetOrigin(rect.width / 2, rect.height / 2);
        rect.SetScaleXY(0.02f, 0.02f);
        rect.SetXY(width / 3 * 2, height - 100);
        AddChild(rect);
    }
}

