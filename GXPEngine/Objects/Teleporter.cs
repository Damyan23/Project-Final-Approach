using GXPEngine;
using GXPEngine.Core;

public class Teleporter : GameObject
{
    public Sprite Entrance;
    public Sprite Exit;

    Sound sound;

    GameSettings settings;

    Vector2 exitPoint;

    bool timerStarted = false;

    float ballTimer;
    Vector2 ballVelocity;

    int teleportDelay = 200;
    public Teleporter(Vector2 enterPoint, Vector2 exitPoint, GameSettings settings) : base()
    {
        Entrance = new Sprite("teleporterEntrence.png");
        Entrance.SetXY(enterPoint.x, enterPoint.y);
        Entrance.SetOrigin(Entrance.width / 2, Entrance.height / 2);
        this.AddChild(Entrance);

        Exit = new Sprite("teleporterExit.png");
        Exit.SetXY(exitPoint.x, exitPoint.y);
        Exit.SetOrigin(Exit.width / 2, Exit.height / 2);
        this.AddChild(Exit);

        this.exitPoint = exitPoint;
        this.settings = settings;

        sound = new Sound("teleport.wav");
    }

    void Update()
    {
        CheckPlayerCollision();
    }

    void CheckPlayerCollision()
    {
        Ball ball = ((MyGame)game).GetPlayer();

        if (ball == null) return;

        // Check if the ball overlaps with the teleporter entrance
        if (ball.HitTest(Entrance) && ball.GetRigidBody().mode == 1)
        {
            //Store the balls velocity, make it zero and make the ball invisible
            if (!timerStarted)
            {
                sound.Play();
                ball.visible = false;
                ballTimer = Time.time;
                ballVelocity = ball.GetVelocity();
                ball.SetVelocity(new Vector2(0, 0));
                timerStarted = true;
            }

            // After the delay move the ball to the exit, set its velocity to the sorted one and make it visible again
            if (Time.time - ballTimer > teleportDelay)
            {
                ball.MovePosition(exitPoint);
                ball.SetVelocity(ballVelocity);
                ball.visible = true;
                timerStarted = false;
            }

        }
    }
}