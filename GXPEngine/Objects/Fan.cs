using GXPEngine.Core;
using System;

namespace GXPEngine
{
    public enum FanDirection
    {
        Right, Left, Up, Down, None
    }

    public class Fan : GameObject
    {
        Box box;

        Vector2 position;

        float width;
        float height;

        FanDirection fanDirection;

        float force = 100000f;

        public int level;

        Ball player;

        int mode;

        public Fan(Vector2 position, float width, float height, float density, float restitution, FanDirection fanDirection, int mode)
        {
            this.fanDirection = fanDirection;
            this.position = position;
            this.width = width;
            this.height = height;
            this.mode = mode;

            box = new Box(width, height, position, density, restitution, mode, true);
            AddChild(box);
            box.level = -1;

            this.box.level = level;
        }

        void Update()
        {
            player = ((MyGame)game).GetPlayer();

            if(player != null) PushObject(player);
        }

        public void PushObject(Ball ball)
        {
            if (ball.GetRigidBody().mode != mode) return;

            if (fanDirection == FanDirection.Right)
            {
                if (ball.y > this.position.y - this.height / 2 && ball.y < this.position.y + this.height / 2)
                {
                    if (ball.x - ball._radius > position.x + width / 2)
                    {
                        Vector2 difference = new Vector2(ball.x, ball.y) - this.position;
                        float distance = difference.Length();

                        float force = Mathf.Clamp(this.force / (distance * distance), 0, 1000f);

                        ball.ApplyForce(new Vector2(force, 0));
                    }
                }
            }

            if (fanDirection == FanDirection.Left)
            {
                if (ball.y > this.position.y - this.height / 2 && ball.y < this.position.y + this.height / 2)
                {
                    if (ball.x + ball._radius < position.x - width / 2)
                    {
                        Vector2 difference = new Vector2(ball.x, ball.y) - this.position;
                        float distance = difference.Length();

                        float force = Mathf.Clamp(this.force / (distance * distance), 0, 1000f);

                        ball.ApplyForce(new Vector2(-force, 0));
                    }
                }
            }

            if (fanDirection == FanDirection.Up)
            {
                if (ball.x > this.position.x - this.width / 2 && ball.x < this.position.x + this.width / 2)
                {
                    if (ball.y + ball._radius < position.y - height / 2)
                    {
                        Vector2 difference = new Vector2(ball.x, ball.y) - this.position;
                        float distance = difference.Length();

                        float force = Mathf.Clamp(this.force / (distance * distance), 0, 1000f);

                        ball.ApplyForce(new Vector2(0, -force));
                    }
                }
            }

            if (fanDirection == FanDirection.Down)
            {
                if (ball.x > this.position.x - this.width / 2 && ball.x < this.position.x + this.width / 2)
                {
                    if (ball.y - ball._radius > position.y + height / 2)
                    {
                        Vector2 difference = new Vector2(ball.x, ball.y) - this.position;
                        float distance = difference.Length();

                        float force = Mathf.Clamp(this.force / (distance * distance), 0, 1000f);

                        ball.ApplyForce(new Vector2(0, force));
                    }
                }
            }

        }
    }
}
