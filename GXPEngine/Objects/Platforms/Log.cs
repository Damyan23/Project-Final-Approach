using GXPEngine;
using GXPEngine.Core;
using System;
using TiledMapParser;

public class Log : Sprite
{
    Vector2 position;
    public int Level;
    Box boxCollider;

    int mode;
    public Log(Vector2 position, float rotation, int mode) : base("full_log.png")
    {
        this.rotation = rotation;
        this.position = position;
        this.mode = mode;
        boxCollider = new Box(this.width - 90, this.height / 2 - 20, new Vector2(0, 0), 2f, 0.8f, mode, true, rotation);
        this.AddChild(boxCollider);
        boxCollider.visible = false;
        boxCollider.level = Level;




        this.SetOrigin(this.width / 2, this.height / 2);
        this.SetXY(position.x, position.y);
    }

    void Update()
    {

        if (boxCollider.level != this.Level)
        {
            boxCollider.level = this.Level;
        }

        Ball player = ((MyGame)game).GetPlayer();
        if (player == null) return;

        RigidBody rb = player.GetRigidBody();

        if (mode == 1)
        {
            if (rb.mode == 1)
            {
                alpha = 1;

            }
            else if (rb.mode == 2)
            {
                alpha = 0.2f;
            }
        }
        else if (mode == 2)
        {
            if (rb.mode == 2)
            {
                alpha = 1;

            }
            else if (rb.mode == 1)
            {
                alpha = 0.2f;
            }
        }
    }
}