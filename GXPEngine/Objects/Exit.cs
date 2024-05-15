using GXPEngine;
using GXPEngine.Core;
using System;

public class Exit : AnimationSprite
{
    LevelManager levelManger;
    public Exit (Vector2 position, LevelManager levelManager) : base ("end point sprite sheet.png", 2, 1)
    {
        this.levelManger = levelManager;

        this.SetXY(position.x, position.y);
        this.SetOrigin (this.width /2, this.height / 2);
        this.scale = 1.5f;
    }

    void Update ()
    {
        DetectCollision ();

        this.Animate(0.03f);
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