using GXPEngine;
using GXPEngine.Core;
using System;
using TiledMapParser;

public class Log : GameObject
{
    Vector2 position;
    public int Level;
    public Box boxCollider;

    AnimationSprite sprite;

    bool animating = false;

    public int mode;
    public Log(Vector2 position, float rotation, int mode)
    {
        sprite = new AnimationSprite("trunk flower bloom_sprite sheet.png", 3, 2);
        AddChild(sprite);

        this.position = position;
        this.mode = mode;

        SetXY(position.x, position.y);

        boxCollider = new Box(sprite.width - 90, sprite.height / 2 - 20, new Vector2(0, 0), 2f, 0.8f, mode, true, rotation);
        AddChild(boxCollider);
        boxCollider.visible = false;
        boxCollider.level = Level;

        

        sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
        sprite.rotation = rotation;

        if (mode == 1) sprite.currentFrame = 0;
        else sprite.currentFrame = 5;
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
                sprite.alpha = 1;

            }
            else if (rb.mode == 2)
            {
                sprite.alpha = 0.2f;
            }
        }
        else if (mode == 2)
        {
            if (rb.mode == 2)
            {
                sprite.alpha = 1;

            }
            else if (rb.mode == 1)
            {
                sprite.alpha = 0.2f;
            }
        }

        if (mode == 1 && rb.mode == 1 && sprite.HitTest(player))
        {
            animating = true;
        }

        if(animating && sprite.currentFrame < 4)
        {
            sprite.Animate(0.05f);
        }
    }
}