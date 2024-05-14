using GXPEngine;
using GXPEngine.Core;
using System;

public class HalfPipe : Sprite
{
    public int level;
    public HalfPipe(Vector2 position) : base ("halp pipe.png")
    {
  
        this.SetOrigin (this.width /2, this.height / 2);

        this.scale = 0.5f;

        this.SetXY(position.x, position.y);

        Box box1 = new Box(this.width / 3, this.height + 50, new Vector2 (- 100, -200), 1f, -0.8f, 1, true, 90);
        Box box2 = new Box(this.width / 3, this.height + 50, new Vector2(), 1f, -0.8f, 1, true, -45);
        Box box3 = new Box(this.width / 3, this.height - 50, new Vector2(), 1f, -0.8f, 1, true, 0);

        this.AddChild (box1);
        this.AddChild (box2); 
        this.AddChild (box3);  

        box1.level = this.level;
        box2.level = this.level;
        box3.level = this.level;

        box1.GetRigidBody().position = new Vector2 (box1.x, box1.y);

        //this.rotation = 45;

        box1.Rotate(this.rotation);
        box2.Rotate (this.rotation);
        box3.Rotate (this.rotation);

        Console.WriteLine (box1.GetRigidBody().position);
        Console.WriteLine(new Vector2(box1.x, box1.y));
    }
}