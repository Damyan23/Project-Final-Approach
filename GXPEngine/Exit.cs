using GXPEngine;
using GXPEngine.Core;

public class Exit : Sprite
{ 
    public Exit (Vector2 position, LevelManager levelManager) : base ("exit.png", false, false)
    {
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


    }
}