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

    Ball _ball1;

    Box ground;
    Box slope1;
    Box mode2Box;

    List<Ball> placedObjects;

    EasyDraw _text;

    World world;

    Random rand;

    Player player;

    int phase = 1;

    Sprite square;
    Sprite rect;

    Sprite currentlyHoldingSprite;

    bool holdingBlock = false;

    Fan fan1;

    public MyGame() : base(800, 600, false, false)
    {
        SetUp();
    }

    void SetUp()
    {
        world = new World();
        rand = new Random();

        //_ball1 = new Ball(15, new Vector2(width / 2, height / 2), 0.1f, 0.2f);
        //AddChild(_ball1);

        ground = new Box(width - 100, 60, new Vector2(width / 2, height - 80), 1f, 1f, 0, true);
        AddChild(ground);

        slope1 = new Box(100, 100, new Vector2(width / 2, height / 4 + 50), 1f, 0f, 0, true);
        slope1.Rotate(90);
        AddChild (slope1);

        //mode2Box = new Box(100, 100, new Vector2(width / 2, height - 160), 1, 1, 2, true);
        //AddChild(mode2Box);

        placedObjects = new List<Ball>();

        fan1 = new Fan(width / 4, height - 175, 50, 50, FanDirection.Right);
        AddChild(fan1);

        DrawPlaceableObjects();

        //int numberOfBalls = 10;

        //for (int i = 0; i < numberOfBalls; i++)
        //{
        //    int randomRadius = random.Next(10, 30);
        //    int isStatic = random.Next(0, 2);
        //    Console.WriteLine(isStatic);
        //    bool isStaticBool;

        //    if (isStatic == 0)
        //    {
        //        isStaticBool = false;
        //    }
        //    else
        //    {
        //        isStaticBool = true;
        //    }

        //    Vector2 randomPosition = new Vector2(random.Next(0, width - randomRadius), random.Next(0, height - randomRadius));
        //    Box box = new Box(randomRadius, randomRadius, randomPosition, 0.5f, 0.2f, isStaticBool);
        //    AddChild(box);

        //    randomRadius = random.Next(10, 30);
        //    randomPosition = new Vector2(random.Next(0, width - randomRadius), random.Next(0, height - randomRadius));


        //    Ball ball = new Ball(randomRadius, randomPosition, 0.5f, 0.5f);
        //    AddChild(ball);
        //}   
    }


    void Update()
    {
        this.targetFps = 60;

        world.Step(Time.deltaTimeInSeconds, 20);



        if (Input.GetMouseButtonDown(0) && phase == 2)
        {
            Ball ball = new Ball (rand.Next(15,40), new Vector2 (Input.mouseX, Input.mouseY), 0.001f, 0.8f, 1);
            AddChild(ball);
        }
        else if (Input.GetMouseButtonDown(1) && phase == 2)
        {
            //Box box = new Box(rand.Next(15, 40), rand.Next (15, 40), new Vector2(Input.mouseX, Input.mouseY), 0.005f, 0.5f, 1);
            //AddChild(box);

            Ball ball = new Ball(rand.Next(15, 40), new Vector2(Input.mouseX, Input.mouseY), 0.001f, 0.8f, 2);
            AddChild(ball);
        }

        if (Input.GetKey(Key.R))
        {
            Restart();
        }

        if (Input.GetKey(Key.G))
        {
            phase = 2;

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
                    Box box = new Box(50, 50, new Vector2(Input.mouseX, Input.mouseY), 1, 1, 1, true);
                    AddChild(box);
                }

                else if (currentlyHoldingSprite == rect)
                {
                    Box rect = new Box(100, 50, new Vector2(Input.mouseX, Input.mouseY), 1, 1, 1, true);
                    AddChild(rect);
                }

                holdingBlock = false;

                ShowMouse(true);

                RemoveChild(currentlyHoldingSprite);
                DrawPlaceableObjects();

                currentlyHoldingSprite = null;

            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (PickupPlacableObject(out currentlyHoldingSprite))
            {
                ShowMouse(false);
                holdingBlock = true;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ball ball = new Ball(25, new Vector2(Input.mouseX, Input.mouseY), 1, 0.95f, 1);
            AddChild(ball);

            placedObjects.Add(ball);
        }


        foreach (Ball ball in placedObjects)
        {
            fan1.PushObject(ball);
        }

    }

    bool PickupPlacableObject(out Sprite sprite)
    {
        sprite = null;

        if (square != null && rect != null)
        {
            if (square.HitTestPoint(Input.mouseX, Input.mouseY) && phase == 1)
            {
                sprite = square;

                return true;
            }
            else if (rect.HitTestPoint(Input.mouseX, Input.mouseY) && phase == 1)
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

    void Restart()
    {
        //foreach (GameObject child in this.GetChildren()) 
        //{
        //    if (child is Ball)
        //    {
        //        foreach (RigidBody rigidBody in child.GetChildren()) 
        //        {
        //            rigidBody.LateDestroy();
        //        }
        //    }

        //    child.LateDestroy();
        //}



        //SetUp();
    }
}

