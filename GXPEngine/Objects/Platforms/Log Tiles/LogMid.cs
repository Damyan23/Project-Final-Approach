using GXPEngine;
using GXPEngine.Core;
using System;

public class LogMid : Sprite
{
    Vector2 position;
    public int Level = 1;
    public LogMid (Vector2 position, float rotation) : base ("2nd tile.png")
    {
        this.rotation = rotation;
        this.position = position;
        Box collider = new Box(this.width, this.height - 10, new Vector2 (0, 0), 1f, 0.8f, 1, true, rotation);
        this.AddChild (collider);
        collider.visible = false;
        collider.level = Level;


        this.SetXY (position.x, position.y);
        this.SetOrigin (this.width /2, this.height / 2);
    }
}