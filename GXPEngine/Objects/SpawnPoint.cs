using GXPEngine;
using GXPEngine.Core;

public class SpawnPoint : Sprite
{
    public SpawnPoint (string imageName, Vector2 position) : base (imageName)
    {
        this.SetXY(position.x, position.y);
        this.scale = 0.1f;
        this.SetOrigin (this.width /2, this.height / 2);
    }
}