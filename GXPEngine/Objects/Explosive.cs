using GXPEngine.Core;

namespace GXPEngine
{
    internal class Explosive : AnimationSprite
    {

        Box box;

        Vector2 position;

        float explosionForce = 1000;

        Sound sound;

        bool AnimationTriggered;

        int mode;

        public Explosive(Vector2 position, int mode) : base ("bomb_sprite sheet.png", 4, 2)
        {
            SetXY(position.x, position.y);

            this.position = position;
            this.mode = mode;

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
            
            RigidBody rb = player.GetRigidBody();

            if (box.HitTest(player))
            {
                sound.Play();

                Vector2 direction = (new Vector2(player.x, player.y) - position).Normalized();

                rb.ApplyForce(direction * explosionForce);

                AnimationTriggered = true;
            }

            if (AnimationTriggered) 
            {
                if (this.currentFrame == 6)
                {
                    this.Destroy();
                }
            }

            if (mode == 1)
            {
                if (rb.mode == 1)
                {
                    alpha = 1;

                }
                else if (rb.mode == 2)
                {
                    alpha = 0.2f;
                }
            }
            else if (mode == 2)
            {
                if (rb.mode == 2)
                {
                    alpha = 1;

                }
                else if (rb.mode == 1)
                {
                    alpha = 0.2f;
                }
            }

        }

    }
}
