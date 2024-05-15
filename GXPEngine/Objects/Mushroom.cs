using GXPEngine;
using GXPEngine.Core;

public class Mushroom : AnimationSprite
{
    Vector2 position;
    public int Level;

    Box boxCollider;

    bool canPlaySound = true;

    Sound sound;
    public Mushroom(Vector2 position, float rotation, int mode) : base("mushroom.png", 2, 1)
    {
        this.rotation = rotation;
        this.position = position;
        boxCollider = new Box(this.width - 20, this.height - 20, new Vector2(0, 0), 1, 3f, mode, true, rotation);
        this.AddChild(boxCollider);
        boxCollider.visible = false;
        boxCollider.level = Level;

        sound = new Sound("bounce jump.wav");


        this.SetXY(position.x, position.y);
        this.SetOrigin(this.width / 2, this.height / 2);
    }

    void Update()
    {
        Ball player = ((MyGame)game).GetPlayer();
        boxCollider.level = Level;
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
    }
}