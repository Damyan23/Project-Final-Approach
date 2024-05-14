using GXPEngine;
using GXPEngine.Core;

public class Leaf : Sprite
{
    Vector2 position;
    public int Level = 1;
    public Leaf (Vector2 position, float rotation) : base ("leaf platform.png")
    {
        this.rotation = rotation;
        this.position = position;
        Box collider = new Box(this.width - 20, this.height /2, new Vector2 (0, 0), 1f, 0.8f, 1, true, rotation + 10);
        this.AddChild (collider);
        collider.visible = false;
        collider.level = Level;

        this.SetXY (position.x, position.y);
        this.SetOrigin (this.width /2, this.height / 2);
    }
}