using GXPEngine.Core;

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

        float force = 500f;

        public Fan(int x, int y, int width, int height, FanDirection fanDirection)
        {
            this.fanDirection = fanDirection;
            this.position = new Vector2(x, y);
            this.width = width;
            this.height = height;

            box = new Box(width, height, new Vector2(x, y), 1, 1, 0, true);
            AddChild(box);
        }

        public void PushObject(Ball ball)
        {
            if (fanDirection == FanDirection.Right)
            {
                if (ball.y > this.position.y - this.height / 2 && ball.y < this.position.y + this.height / 2)
                {
                    Vector2 difference = new Vector2(ball.x, ball.y) - this.position;
                    float distance = difference.Length();

                    ball.ApplyForce(new Vector2(1, 0) * force / (distance * distance));
                }
            }
        }
    }
}
