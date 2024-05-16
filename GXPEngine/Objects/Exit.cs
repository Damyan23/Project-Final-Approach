using GXPEngine;
using GXPEngine.Core;
using System;

public class Exit : AnimationSprite
{
    LevelManager levelManger;
    GameSettings settings;
    public Exit (Vector2 position, LevelManager levelManager, GameSettings settings) : base ("end point sprite sheet.png", 2, 1)
    {
        this.levelManger = levelManager;
        this.settings = settings;

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
                if (levelManger.currentLevelIndex < 2)
                {
                    levelManger.SwitchToNextLevel();
                    ball.LateDestroy();
                }
                if (levelManger.currentLevelIndex == 1)
                {
                    ball.LateDestroy();
                    settings.isGameOver = true;
                    levelManger.ClearLevel();

                    foreach (GameObject obj in game.GetChildren())
                    {
                        if (obj.name == "hud new.png")
                        {
                            obj.visible = false;
                        }
                    }
                }


   
            }
        }
    }
}