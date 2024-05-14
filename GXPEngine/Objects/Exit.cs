using GXPEngine;
using GXPEngine.Core;
using System;

public class Exit : Sprite
{
    LevelManager levelManger;
    public Exit (Vector2 position, LevelManager levelManager) : base ("exit.png")
    {
        this.levelManger = levelManager;

        this.SetXY(position.x, position.y);
        this.SetOrigin (this.width /2, this.height / 2);
    }

    void Update ()
    {
        DetectCollision ();
    }

    void DetectCollision ()
    {
        Ball ball = ((MyGame)game).GetPlayer();

        if (ball != null) 
        {
            if (this.HitTest(ball))
            {
                levelManger.SwitchToNextLevel();
                ball.LateDestroy();
             }
        }
    }
}