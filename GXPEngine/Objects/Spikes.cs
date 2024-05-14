using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Spikes : Sprite
    {
        GameSettings settings;

        public Spikes(Vector2 position, string file, GameSettings settings) : base(file)
        {
            this.settings = settings;

            SetScaleXY(0.05f, 0.05f);

            SetXY(position.x, position.y);
        }

        void Update()
        {
            CheckPlayerCollision();
        }

        void CheckPlayerCollision()
        {
            Ball player = ((MyGame)game).GetPlayer();

            if (player == null) return;

            if (HitTest(player))
            {
                settings.phase = 1;
                settings.ghostSpawned = false;
                player.LateDestroy();
            }
        }

    }
}
