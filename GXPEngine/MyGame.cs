using System;
using GXPEngine;
using System.Drawing;
using GXPEngine.Core;

public class MyGame : Game
{

    static void Main()
    {
        new MyGame().Start();
    }
    Ball _ball1;

    Box ground ;
    Box slope1;
    Box slope2;

    EasyDraw _text;

    World world;

    Random rand;

    public MyGame() : base(800, 600, false, false)
    {
        SetUp ();
    }

    void SetUp()
    {
        world = new World();
        rand = new Random();

        //_ball1 = new Ball(15, new Vector2(width / 2, height / 2), 0.1f, 0.2f);
        //AddChild(_ball1);

        ground = new Box(width - 100, 60, new Vector2(width / 2, height - 80), 1f, 0.5f, true);
        AddChild(ground);

        slope1 = new Box(400, 30, new Vector2(width / 2, height / 4), 1f, 0.5f, true);
        slope1.Rotate(45);
        AddChild (slope1);

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
        Console.WriteLine(targetFps);

        world.Step(Time.deltaTimeInSeconds, 20);

        //float dx = 0f;
        //float dy = 0f;
        //float forceMagnitude = 500;

        //if (Input.GetKey(Key.A)) { dx--; }
        //else if (Input.GetKey(Key.D)) { dx++; }

        //if (Input.GetKey(Key.W)) { dy--; }
        //else if (Input.GetKey(Key.S)) { dy++; }

        //if (dx != 0f || dy != 0f)
        //{
        //    Vector2 forceDirection = new Vector2(dx, dy).Normalized();

        //    Vector2 force = forceDirection * forceMagnitude;

        //    box1.ApplyForce(force);
        //}

        if (Input.GetMouseButtonDown (0))
        {
            Ball ball = new Ball (rand.Next(15,40), new Vector2 (Input.mouseX, Input.mouseY), 0.005f, 0.5f);
            AddChild(ball);
        }
        else if (Input.GetMouseButtonDown(1)) 
        {
            Box box = new Box(rand.Next(15, 40), rand.Next (15, 40), new Vector2(Input.mouseX, Input.mouseY), 0.005f, 0.5f);
            AddChild(box);
        }

        if (Input.GetKey(Key.R)) 
        {
            Restart ();
        }
    }

    void Restart ()
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

