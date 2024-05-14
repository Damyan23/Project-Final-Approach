using GXPEngine;
using GXPEngine.Core;
using System;

namespace GXPEngine
{

    public class HalfPipe : GameObject
    {
        public int level;
        public HalfPipe(Vector2 position, float width, float height, string fileName)
        {

            //this.SetXY(position.x, position.y);

            Box box1 = new Box(width / 3, height, new Vector2(position.x - width/3, position.y), 1f, -0.8f, 0, true, 90);
            Box box2 = new Box(width / 3, height, new Vector2(position.x + width/3, position.y + height/3), 1f, -0.8f, 1, true, -45);
            Box box3 = new Box(width / 3, height, new Vector2(position.x + width/2, position.y + height), 1f, -0.8f, 1, true, 0);

            this.AddChild(box1);
            this.AddChild (box2); 
            this.AddChild (box3);  

            box1.level = this.level;
            box2.level = this.level;
            box3.level = this.level;

            box1.Rotate(this.rotation);
            box2.Rotate(this.rotation);
            box3.Rotate(this.rotation);

            Sprite sprite = new Sprite(fileName);
            sprite.SetScaleXY(width/285, height/285);
            sprite.SetXY(position.x - width, position.y - height/4);
            AddChild(sprite);

        }
    }
}