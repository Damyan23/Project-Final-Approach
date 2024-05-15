using GXPEngine;
using GXPEngine.Core;
using System;
using System.Runtime;

public class Thorns : Sprite
{
    Vector2 position;
    public int Level;
    GameSettings settings;

    bool canPlaySound = true;

    Sound sound;
    public Thorns (Vector2 position, float rotation, GameSettings settings) : base ("thorns.png")
    {
        this.settings = settings;

        this.rotation = rotation;
        this.position = position;
        boxCollider = new Box(this.width, this.height /2, new Vector2 (0, 0), 1f, 0.8f, 1, true, rotation);
        this.AddChild (boxCollider);
        boxCollider.visible = false;
        boxCollider.level = Level;

        sound = new Sound("hurtbyspikes2.wav");

        this.SetXY (position.x, position.y);
        this.SetOrigin (this.width /2, this.height / 2);
    }

    void Update()
    {
        CheckPlayerCollision();

        if (boxCollider.level != this.Level)
        {
            boxCollider.level = Level;
        }
    }

    void CheckPlayerCollision()
    {
        Ball player = ((MyGame)game).GetPlayer();

        if (player == null) return;

        if (HitTest(player))
        {
            if (canPlaySound)
            {
                sound.Play();

                canPlaySound = false;
            }

            settings.phase = 1;
            settings.ghostSpawned = false;
            player.LateDestroy();
        }
        else
        {
            canPlaySound = true;
        }
    }

}