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

    GameSettings settings;
    MenuManager menuManager;

    Ball _ball1;

    Box ground;
    Box slope1;
    Box slope2;
    Box slope3;
    Box mode2Box;

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

    Fan fan1;

    private bool ghostSpawned = false;

    public MyGame() : base(1280, 720, false, false)
    {
        SetUp();
    }

    void SetUp()
    {
        settings = new GameSettings();
        menuManager = new MenuManager(settings);
        menuManager.SetMainMenu();

        world = new World();
        rand = new Random();

        ground = new Box(width - 100, 60, new Vector2(width / 2, height - 80), 1f, 0.8f, 0, true);

        slope1 = new Box(400, 20, new Vector2(width / 4, height / 4 + 170), 1f, -0.8f, 0, true);
        slope1.Rotate(20);

        slope2 = new Box(400, 20, new Vector2(width / 2, height / 4 + 240), 1f, -0.8f, 0, true);
        slope2.Rotate(0);

        playButton = new StartButton(settings);
        playButton.SetXY(width / 2, height / 2 - 250);

        slope3 = new Box(400, 20, new Vector2(width / 4 * 3, height / 4 + 205), 1f, -0.8f, 0, true);
        slope3.Rotate(-10);

        spawnPoint = new Sprite("spawnPoint.png");
        spawnPoint.scale = 0.1f;
        spawnPoint.SetOrigin(spawnPoint.x + spawnPoint.width / 2, spawnPoint.y + spawnPoint.height / 2);
        spawnPoint.SetXY(width / 2, height / 2 - 200);

        placedObjects = new List<Ball>();

        fan1 = new Fan(width / 4 - 80, height - 290, 50, height, FanDirection.Right, 100000f);
    }


    void Update()
    {
        this.targetFps = 60;

        world.Step(Time.deltaTimeInSeconds, 20);

        if (!settings.isGameOver && settings.startGame && !settings.stuffDrawn)
        {
            menuManager.RemoveCurrentMenu();
            AddChild(ground);
            AddChild(slope1);
            AddChild(slope2);
            AddChild(slope3);
            AddChild(fan1);
            AddChild(spawnPoint);
            AddChild(playButton);

            DrawPlaceableObjects();

            settings.stuffDrawn = true;
        }


        if (settings.phase == 2 && !ghostSpawned && settings.levelSetup)
        {
            Ball ball = new Ball(rand.Next(15, 40), new Vector2(width / 2, height / 2 - 200), 0.001f, 0.8f, 1);
            AddChild(ball);
            placedObjects.Add (ball);
            ghostSpawned = true;
        }

        if (Input.GetMouseButtonDown(2))
        {
            Ball ball = new Ball(rand.Next(15, 40), new Vector2(Input.mouseX, Input.mouseY), 0.001f, 0.8f, 2);
            AddChild(ball);
        }

        if (Input.GetKey(Key.G))
        {
            settings.phase = 2;

            holdingBlock = false;
            ShowMouse(true);
        }

        if (holdingBlock)
        {
            currentlyHoldingSprite.SetXY(Input.mouseX, Input.mouseY);


            if (Input.GetMouseButtonDown(0))
            {
                if (currentlyHoldingSprite == square)
                {
                    Box box = new Box(50, 50, new Vector2(Input.mouseX, Input.mouseY), 1f, 0.8f, 1, true);
                    AddChild(box);
                }

                else if (currentlyHoldingSprite == rect)
                {
                    Box rect = new Box(100, 50, new Vector2(Input.mouseX, Input.mouseY), 1f, 0.8f, 2, true);
                    AddChild(rect);
                }

                holdingBlock = false;

                ShowMouse(true);

                RemoveChild(square);
                RemoveChild(rect);

                DrawPlaceableObjects();

                currentlyHoldingSprite = null;

            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (PickupPlaceableObject(out currentlyHoldingSprite))
            {
                ShowMouse(false);
                holdingBlock = true;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ball ball = new Ball(25, new Vector2(Input.mouseX, Input.mouseY), 1, 0.8f, 1);
            AddChild(ball);

            placedObjects.Add(ball);
        }


        foreach (Ball ball in placedObjects)
        {
            fan1.PushObject(ball);
        }

    }

    bool PickupPlaceableObject(out Sprite sprite)
    {
        sprite = null;

        if (square != null && rect != null)
        {
            if (square.HitTestPoint(Input.mouseX, Input.mouseY) && settings.phase == 1)
            {
                sprite = square;

                return true;
            }
            else if (rect.HitTestPoint(Input.mouseX, Input.mouseY) && settings.phase == 1)
            {
                sprite = rect;

                return true;
            }
        }

        return false;
    }

    void DrawPlaceableObjects()
    {
        square = new Sprite("square.png");
        square.collider.isTrigger = true;
        square.SetOrigin(square.width / 2, square.height / 2);
        square.SetScaleXY(0.05f, 0.05f);
        square.SetXY(width / 3, height - 100);
        AddChild(square);

        rect = new Sprite("rect.png");
        rect.collider.isTrigger = true;
        rect.SetOrigin(rect.width / 2, rect.height / 2);
        rect.SetScaleXY(0.02f, 0.02f);
        rect.SetXY(width / 3 * 2, height - 100);
        AddChild(rect);
    }
}

