using GXPEngine;
using GXPEngine.Core;
using System;

public class LogRight : Sprite
{
    Vector2 position;
    public int Level;

    Box boxCollider;

    bool canPlaySound = true;

    Sound sound;
    public LogRight (Vector2 position, float rotation) : base ("3rd tile.png")
    {
        this.rotation = rotation;
        this.position = position;
        boxCollider = new Box(this.width - 60, this.height /2 - 20, new Vector2 (0, 0), 2f, 0.8f, 1, true, rotation);
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

        if (boxCollider.HitTest(player) && canPlaySound)
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