using GXPEngine;
using GXPEngine.Core;
using System;

public class Log : Sprite
{
    Vector2 position;
    public int Level;
    Box boxCollider;
    public Log(Vector2 position, float rotation, int mode) : base("full_log.png")
    {
        this.rotation = rotation;
        this.position = position;
        boxCollider = new Box(this.width - 90, this.height / 2 - 20, new Vector2(0, 0), 2f, 0.8f, mode, true, rotation);
        this.AddChild(boxCollider);
        boxCollider.visible = false;
        boxCollider.level = Level;


        this.SetXY(position.x, position.y);
        this.SetOrigin(this.width / 2, this.height / 2);
    }

    void Update()
    {
        if (boxCollider.level != this.Level)
        {
            boxCollider.level = this.Level;
        }
    }

    void Update ()
    {
       if (boxCollider.level != this.Level)
        {
            boxCollider.level = this.Level;
        }
    }
}