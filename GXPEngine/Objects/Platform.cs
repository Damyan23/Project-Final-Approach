using GXPEngine;
using GXPEngine.Core;

public class Platform : Sprite
{
    Vector2 position;
    public int Level;
    public Platform (Vector2 position) : base ("leaf platform.png")
    {
        this.position = position;
        Box collider = new Box(this.width, this.height, new Vector2 (0, 0), 1f, 0.8f, 1, true);
        this.AddChild (collider);

        this.SetXY (position.x, position.y);
        this.SetOrigin (this.width /2, this.height / 2);
    }
}