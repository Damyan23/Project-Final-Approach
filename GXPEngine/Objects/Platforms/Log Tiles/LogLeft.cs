using GXPEngine;
using GXPEngine.Core;

public class LogLeft : Sprite
{
    Vector2 position;
    public int Level;

    int mode;

    bool canPlaySound = true;

    Box boxCollider;

    Sound sound;
    public LogLeft(Vector2 position, float rotation) : base("1st tile.png")
    {
        this.rotation = rotation;
        this.position = position;
        this.mode = 1;
        boxCollider = new Box(this.width - 115, this.height / 2 - 20, new Vector2(0, 0), 2f, 0.8f, 1, true, rotation);
        this.AddChild(boxCollider);
        boxCollider.visible = false;
        boxCollider.level = Level;

        sound = new Sound("normal platform.wav");


        this.SetXY(position.x, position.y);
        this.SetOrigin(this.width / 2, this.height / 2);
    }

    void Update()
    {
        Ball player = ((MyGame)game).GetPlayer();

        if (player == null) return;

        if (boxCollider.HitTest(player))
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

        RigidBody rb = player.GetRigidBody();

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