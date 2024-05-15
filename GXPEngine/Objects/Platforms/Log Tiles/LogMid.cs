using GXPEngine;
using GXPEngine.Core;
using System;
using System.Threading;

public class LogMid : Sprite
{
    Vector2 position;
    public int Level = 1;

    bool canPlaySound = true;

    Box boxCollider;

    Sound sound;
    public LogMid (Vector2 position, float rotation) : base ("2nd tile.png")
    {
        this.rotation = rotation;
        this.position = position;
        boxCollider = new Box(this.width, this.height - 10, new Vector2 (0, 0), 1f, 0.8f, 1, true, rotation);
        this.AddChild (boxCollider);
        boxCollider.visible = false;
        boxCollider.level = Level;

        sound = new Sound("normal platform.wav");


        this.SetXY (position.x, position.y);
        this.SetOrigin (this.width /2, this.height / 2);
    }

    void Update()
    {
        Ball player = ((MyGame)game).GetPlayer();

        if (player == null) return;

        if (boxCollider.HitTest(player))
        {
            if (canPlaySound)
            {
                sound.Play();
                canPlaySound = false;
            }
        }
        else
        {
            canPlaySound = true;
        }
    }
}