using GXPEngine;
using GXPEngine.Core;
using System;

public class LogRight : Sprite
{
    Vector2 position;
    public int Level = 1;
    public LogRight (Vector2 position, float rotation) : base ("3rd time.png")
    {
        this.rotation = rotation;
        this.position = position;
        Box collider = new Box(this.width - 90, this.height /2 - 20, new Vector2 (0, 0), 1f, 0.8f, 1, true, rotation);
        this.AddChild (collider);
        collider.visible = false;
        collider.level = Level;


        this.SetXY (position.x, position.y);
        this.SetOrigin (this.width /2, this.height / 2);
    }
}