using GXPEngine;
using GXPEngine.Core;

public class Mushroom : AnimationSprite
{
    Vector2 position;
    public int Level;

    int mode;

    Box boxCollider;

    bool canPlaySound = true;

    Sound sound;
    public Mushroom(Vector2 position, float rotation, int mode) : base("mushroom.png", 2, 1)
    {
        this.rotation = rotation;
        this.position = position;
        this.mode = mode;
        boxCollider = new Box(this.width - 20, this.height - 20, new Vector2(0, 0), 1, 3f, mode, true, rotation);
        this.AddChild(boxCollider);
        boxCollider.visible = false;

        SetFrame(mode - 1);
        

        sound = new Sound("bounce jump.wav");


        this.SetXY(position.x, position.y);
        this.SetOrigin(this.width / 2, this.height / 2);
    }

    void Update()
    {

        boxCollider.level = Level;

        Ball player = ((MyGame)game).GetPlayer();
        if (player == null) return;

        if (HitTest(player))
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

        if(mode == 1)
        {
            if (rb.mode == 1)
            {
                alpha = 1;

            }
            else if (rb.mode == 2)
            {
                alpha = 0.2f;
            }
        }else if(mode == 2)
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