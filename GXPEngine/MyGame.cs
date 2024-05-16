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

    Sprite bouncyPlatform;
    string bouncyPlatformFile = "spirit bouncy platform.png";

    Sprite halfPipeLeft;
    string halfPipeLeftFile = "spirit half pipe left.png";

    Sprite halfPipeRight;
    string halfPipeRightFile = "spirit half pipe right.png";

    Sprite leafPlatform;
    string leafPlatformFile = "spirit leaf platform.png";

    Sprite trunkPlatform;
    string trunkPlatformFile = "spirit trunk platform.png";

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

        world.Step(Time.deltaTimeInSeconds, 50);

        if (!settings.isGameOver && settings.startGame && !settings.stuffDrawn)
        {
            menuManager.RemoveCurrentMenu();
            levelManager.Start();

            backgroundMusic.Play();

            DrawPlaceableObjects();



            settings.stuffDrawn = true;
        }

        if (settings.phase == 2 && !settings.ghostSpawned && settings.stuffDrawn)
        {
            ball = new Ball(20, new Vector2(levelManager.currentLevelSpawnPoint.x, levelManager.currentLevelSpawnPoint.y), 1.5f, 0.8f, 1);
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
        Level currentLevel = levelManager.levels[levelManager.currentLevelIndex];
        Vector2 mousePos = new Vector2(Input.mouseX, Input.mouseY);

        file = null;

        if (bouncyPlatform != null)
        {
            if (bouncyPlatform.HitTestPoint(mousePos.x, mousePos.y) && settings.phase == 1)
            {
                Mushroom temp = new Mushroom(new Vector2(), 0, 2);

                if (currentLevel.CanAddObject(temp))
                {

                    file = bouncyPlatformFile;

                    currentlyHoldingSprite = new Sprite(file);
                    currentlyHoldingSprite.SetOrigin(currentlyHoldingSprite.width / 2, currentlyHoldingSprite.height / 2);
                    currentlyHoldingSprite.SetScaleXY(0.6f);
                    AddChild(currentlyHoldingSprite);

                    return true;
                }
            }
        }

        if (halfPipeLeft != null)
        {
            if (halfPipeLeft.HitTestPoint(mousePos.x, mousePos.y) && settings.phase == 1)
            {
                HalfPipeLeft temp = new HalfPipeLeft(new Vector2(), settings, 2);

                if (currentLevel.CanAddObject(temp))
                {
                    file = halfPipeLeftFile;

                    currentlyHoldingSprite = new Sprite(file);
                    currentlyHoldingSprite.SetOrigin(currentlyHoldingSprite.width / 2, currentlyHoldingSprite.height / 2);
                    currentlyHoldingSprite.SetScaleXY(0.5f);
                    AddChild(currentlyHoldingSprite);

                    return true;

                }
            }
        }

        if (halfPipeRight != null)
        {
            if (halfPipeRight.HitTestPoint(mousePos.x, mousePos.y) && settings.phase == 1)
            {
                HalfPipeRight temp = new HalfPipeRight(new Vector2(), settings, 2);

                if (currentLevel.CanAddObject(temp))
                {
                    file = halfPipeRightFile;

                    currentlyHoldingSprite = new Sprite(file);
                    currentlyHoldingSprite.SetOrigin(currentlyHoldingSprite.width / 2, currentlyHoldingSprite.height / 2);
                    currentlyHoldingSprite.SetScaleXY(0.5f);
                    AddChild(currentlyHoldingSprite);

                    return true;

                }
            }
        }

        if (leafPlatform != null)
        {
            if (leafPlatform.HitTestPoint(mousePos.x, mousePos.y) && settings.phase == 1)
            {
                Leaf temp = new Leaf(new Vector2(), 0, 2);

                if (currentLevel.CanAddObject(temp))
                {
                    file = leafPlatformFile;

                    currentlyHoldingSprite = new Sprite(file);
                    currentlyHoldingSprite.SetOrigin(currentlyHoldingSprite.width / 2, currentlyHoldingSprite.height / 2);
                    currentlyHoldingSprite.SetScaleXY(0.5f);
                    AddChild(currentlyHoldingSprite);

                    return true;

                }
            }
        }

        if (trunkPlatform != null)
        {
            if (trunkPlatform.HitTestPoint(mousePos.x, mousePos.y) && settings.phase == 1)
            {
                Log temp = new Log(new Vector2(), 0, 2);

                if (currentLevel.CanAddObject(temp))
                {
                    file = trunkPlatformFile;

                    currentlyHoldingSprite = new Sprite(file);
                    currentlyHoldingSprite.SetOrigin(currentlyHoldingSprite.width / 2, currentlyHoldingSprite.height / 2);
                    currentlyHoldingSprite.SetScaleXY(0.8f);
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

            if (fileOfHoldingSprite == bouncyPlatformFile)
            {
                bool success = true;


                Mushroom mushroom = new Mushroom(new Vector2(Input.mouseX, Input.mouseY), 0, 2);

                foreach (GameObject obj in level.objects)
                {
                    if (obj.HitTest(mushroom))
                    {
                        success = false;
                        break;
                    }
                }

                if (success)
                {
                    levelManager.AddObject(mushroom);
                }
                else
                {
                    mushroom.LateDestroy();
                }

            }

            else if (fileOfHoldingSprite == halfPipeLeftFile)
            {
                bool success = true;


                HalfPipeLeft halfpipe = new HalfPipeLeft(new Vector2(Input.mouseX, Input.mouseY), settings, 2);

                foreach (GameObject obj in level.objects)
                {
                    if (obj.HitTest(halfpipe))
                    {
                        success = false;
                        break;
                    }
                }

                if (success)
                {
                    levelManager.AddObject(halfpipe);
                }
                else
                {
                    halfpipe.LateDestroy();
                }

            }
            else if (fileOfHoldingSprite == halfPipeRightFile)
            {
                bool success = true;

                HalfPipeRight halfpipe = new HalfPipeRight(new Vector2(Input.mouseX, Input.mouseY), settings, 2);

                foreach (GameObject obj in level.objects)
                {
                    if (obj.HitTest(halfpipe))
                    {
                        success = false;
                        break;
                    }
                }

                if (success)
                {
                    levelManager.AddObject(halfpipe);
                }
                else
                {
                    halfpipe.LateDestroy();
                }

            }

            else if (fileOfHoldingSprite == leafPlatformFile)
            {
                bool success = true;


                Leaf leaf = new Leaf(new Vector2(Input.mouseX, Input.mouseY), 0, 2);

                foreach (GameObject obj in level.objects)
                {
                    if (obj.HitTest(leaf))
                    {
                        success = false;
                        break;
                    }
                }

                if (success)
                {
                    levelManager.AddObject(leaf);
                }
                else
                {
                    leaf.LateDestroy();
                }

            }

            else if (fileOfHoldingSprite == trunkPlatformFile)
            {
                bool success = true;


                Log log = new Log(new Vector2(Input.mouseX, Input.mouseY), 0, 2);

                foreach (GameObject obj in level.objects)
                {
                    if (obj.HitTest(log))
                    {
                        success = false;
                        break;
                    }
                }

                if (success)
                {
                    levelManager.AddObject(log);
                }
                else
                {
                    log.LateDestroy();
                }

            }

            holdingBlock = false;

            ShowMouse(true);

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

                    DrawPlaceableObjects();
                    break;
                }
            }
        }
    }

    void DrawPlaceableObjects()
    {
        bouncyPlatform = new Sprite(bouncyPlatformFile);
        bouncyPlatform.collider.isTrigger = true;
        bouncyPlatform.SetOrigin(bouncyPlatform.width / 2, bouncyPlatform.height / 2);
        bouncyPlatform.SetScaleXY(0.6f);
        bouncyPlatform.SetXY(width / 6, height - 100);
        AddChild(bouncyPlatform);

        halfPipeLeft = new Sprite(halfPipeLeftFile);
        halfPipeLeft.collider.isTrigger = true;
        halfPipeLeft.SetOrigin(halfPipeLeft.width / 2, halfPipeLeft.height / 2);
        halfPipeLeft.SetScaleXY(0.5f);
        halfPipeLeft.SetXY(width / 6 * 2, height - 100);
        AddChild(halfPipeLeft);

        halfPipeRight = new Sprite(halfPipeRightFile);
        halfPipeRight.collider.isTrigger = true;
        halfPipeRight.SetOrigin(halfPipeRight.width / 2, halfPipeRight.height / 2);
        halfPipeRight.SetXY(width / 6 * 3, height - 100);
        halfPipeRight.SetScaleXY(0.5f);
        AddChild(halfPipeRight);

        leafPlatform = new Sprite(leafPlatformFile);
        leafPlatform.collider.isTrigger = true;
        leafPlatform.SetOrigin(leafPlatform.width / 2, leafPlatform.height / 2);
        leafPlatform.SetXY(width / 6 * 4, height - 100);
        leafPlatform.SetScaleXY(0.5f);
        AddChild(leafPlatform);

        trunkPlatform = new Sprite(trunkPlatformFile);
        trunkPlatform.collider.isTrigger = true;
        trunkPlatform.SetOrigin(trunkPlatform.width / 2, trunkPlatform.height / 2);
        trunkPlatform.SetXY(width / 6 * 5, height - 100);
        trunkPlatform.SetScaleXY(0.8f);
        AddChild(trunkPlatform);
    }
}

