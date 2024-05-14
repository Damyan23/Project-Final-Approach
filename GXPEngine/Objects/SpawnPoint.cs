using GXPEngine;
using GXPEngine.Core;

public class SpawnPoint : AnimationSprite
{
    public SpawnPoint (Vector2 position) : base ("start portal sprite sheet.png", 3, 1)
    {
        this.SetXY(position.x, position.y);
        this.scale = 0.8f;
        this.SetOrigin (this.width /2, this.height / 2);
    }

    void Update ()
    {
        this.Animate (0.07f);
    }
}