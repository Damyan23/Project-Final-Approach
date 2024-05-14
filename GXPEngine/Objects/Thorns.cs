using GXPEngine;
using GXPEngine.Core;
using System;
using System.Runtime;

public class Thorns : Sprite
{
    Vector2 position;
    public int Level = 1;
    GameSettings settings;
    public Thorns (Vector2 position, float rotation, GameSettings settings) : base ("thorns.png")
    {
        this.settings = settings;

        this.rotation = rotation;
        this.position = position;
        Box collider = new Box(this.width, this.height /2, new Vector2 (0, 0), 1f, 0.8f, 1, true, rotation);
        this.AddChild (collider);
        collider.visible = false;
        collider.level = Level;

        this.SetXY (position.x, position.y);
        this.SetOrigin (this.width /2, this.height / 2);
    }

    void Update()
    {
        CheckPlayerCollision();
    }

    void CheckPlayerCollision()
    {
        Ball player = ((MyGame)game).GetPlayer();

        if (player == null) return;

        if (HitTest(player))
        {
            settings.phase = 1;
            settings.ghostSpawned = false;
            player.LateDestroy();
        }
    }

}