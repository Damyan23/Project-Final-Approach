using GXPEngine.Core;
using System.Collections.Generic;

namespace GXPEngine
{

    //public class HalfPipe : GameObject
    //{
    //    public int level;
    //    public HalfPipe(Vector2 position, float width, float height, string fileName)
    //    {
    //        this.SetXY(position.x, position.y);

    //        Box box1 = new Box(width / 3, height, new Vector2(position.x - width/3, position.y), 1f, -0.8f, 0, true, 90);
    //        Box box2 = new Box(width / 3, height, new Vector2(position.x + width/3, position.y + height/3), 1f, -0.8f, 1, true, -45);
    //        Box box3 = new Box(width / 3, height, new Vector2(position.x + width/2, position.y + height), 1f, -0.8f, 1, true, 0);

    //        this.AddChild(box1);
    //        this.AddChild (box2); 
    //        this.AddChild (box3);  

    //        box1.level = this.level;
    //        box2.level = this.level;
    //        box3.level = this.level;

    //        box1.Rotate(this.rotation);
    //        box2.Rotate(this.rotation);
    //        box3.Rotate(this.rotation);

    //        Sprite sprite = new Sprite(fileName);
    //        sprite.SetScaleXY(width/285, height/285);
    //        sprite.SetXY(position.x - width, position.y - height/4);
    //        sprite.rotation = 90;
    //        AddChild(sprite);
    //    }
    //}

    public class HalfPipeRight : AnimationSprite
    {
        public int level = 1;
        GameSettings settings;
        bool collidersAdded;

        bool canPlaySound = true;

        Box collider1;
        Box collider2;
        Box collider3;

        Sound sound;

        public List<Box> boxColliders = new List<Box>();
        public HalfPipeRight(Vector2 position, GameSettings settings, int mode) : base("half pipe right.png", 2, 1)
        {
            //this.scale = 0.5f;
            this.settings = settings;

            this.SetXY(position.x, position.y);
            this.SetOrigin(this.width / 2, this.height / 2);

            collider1 = new Box(width / 7, height / 2, new Vector2(position.x + 125, position.y - 50), 1, -0.8f, mode, true, 0);
            collider2 = new Box(width / 7, height / 2, new Vector2(position.x + 65, position.y + 40), 1, -0.8f, mode, true, 45);
            collider3 = new Box(width / 7, height / 2 + 55, new Vector2(position.x - 70, position.y + 95), 1, -0.8f, mode, true, 87);

            collider1.level = this.level;
            collider2.level = this.level;
            collider3.level = this.level;

            sound = new Sound("normal platform.wav");
        }

        void Update()
        {
            if (!collidersAdded && settings.startGame)
            {
                game.AddChild(collider1);
                game.AddChild(collider2);
                game.AddChild(collider3);
                collidersAdded = true;

                collider1.visible = false;
                collider2.visible = false;
                collider3.visible = false;

                this.boxColliders.Add(collider1);
                this.boxColliders.Add(collider2);
                this.boxColliders.Add(collider3);
            }

            Ball player = ((MyGame)game).GetPlayer();

            if (player == null) return;

            if (collider1.HitTest(player) || collider2.HitTest(player) || collider3.HitTest(player))
            {
                if (canPlaySound)
                {
                    sound.Play();
                    canPlaySound = false;
                }
            }
            else
            {
                canPlaySound = true;
            }
        }
    }
}