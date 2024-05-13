using GXPEngine;
using GXPEngine.Core;

public class Exit : Sprite
{ 
    public Exit (Vector2 position) : base ("exit.png")
    {
        this.SetXY(position.x, position.y);
        this.SetOrigin (this.width /2, this.height / 2);
    }
}