using GXPEngine.Core;
using System;

namespace GXPEngine
{
    public enum FanDirection
    {
        Right, Left, Up, Down
    }

    internal class Fan : GameObject
    {

        Box box;

        Vector2 position;

        int width;
        int height;

        FanDirection fanDirection;

        float force;

        public Fan(int x, int y, int width, int height, FanDirection fanDirection, float force)
        {
            this.fanDirection = fanDirection;
            this.position = new Vector2(x, y);
            this.width = width;
            this.height = height;
            this.force = force;

            box = new Box(width, height, new Vector2(x, y), 1, 0.01f, 0, true);
            AddChild(box);
        }

        public void PushObject(Ball ball)
        {
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
                        Console.WriteLine(force);
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
