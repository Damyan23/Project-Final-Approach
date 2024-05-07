using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    internal class Player : AnimationSprite
    {

        public Box box;

        public int mode;

        public Player() : base("barry.png", 7, 1)
        {
            SetOrigin(width/2, height/2);
            SetXY(game.width/2, game.height/2);

            box = new Box(width, height, new Vector2(0, 0), 1, 1, 1);
            AddChild(box);

            mode = 1;
        }

        void Update()
        {
            Move();

            box.Step();

            this.x = box.x;
            this.y = box.y;

            if (Input.GetKeyDown(Key.SPACE))
            {
                if (mode == 1) mode = 2;
                else if (mode == 2) mode = 1;
            }
        }

        void Move()
        {
            float dx = 0f;
            float dy = 0f;
            float forceMagnitude = 500;

            if (Input.GetKey(Key.A)) { dx--; }
            else if (Input.GetKey(Key.D)) { dx++; }

            if (Input.GetKey(Key.W)) { dy--; }
            else if (Input.GetKey(Key.S)) { dy++; }

            if (dx != 0f || dy != 0f)
            {
                Vector2 forceDirection = new Vector2(dx, dy).Normalized();

                Vector2 force = forceDirection * forceMagnitude;

                box.ApplyForce(force);
            }
        }
    }
}
