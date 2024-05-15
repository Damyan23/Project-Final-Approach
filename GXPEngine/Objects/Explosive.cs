using GXPEngine.Core;

namespace GXPEngine
{
    internal class Explosive : GameObject
    {

        Box box;

        Vector2 position;

        float explosionForce = 1000;

        Sound sound;

        public Explosive(Vector2 position, int mode)
        {
            SetXY(position.x, position.y);

            this.position = position;

            sound = new Sound("explosion.wav");

            box = new Box(50, 50, new Vector2(0, 0), 1, 0, mode, true);
            AddChild(box);
        }



        void Update()
        {
            CheckPlayerCollision();
        }

        public void CheckPlayerCollision()
        {
            Ball player = ((MyGame)game).GetPlayer();

            if (player == null) return;

            if (box.HitTest(player))
            {
                sound.Play();

                Vector2 direction = (new Vector2(player.x, player.y) - position).Normalized();

                RigidBody rb = player.GetRigidBody();
                rb.ApplyForce(direction * explosionForce);

                this.Destroy();
            }

        }

    }
}
