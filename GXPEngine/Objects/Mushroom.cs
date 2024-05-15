using GXPEngine;
using GXPEngine.Core;

public class Mushroom : Sprite
{
    Vector2 position;
    public int Level = 1;

    Box collider;

    bool canPlaySound = true;

    Sound sound;
    public Mushroom(Vector2 position, float rotation) : base("mushroom.png")
    {
        this.rotation = rotation;
        this.position = position;
        collider = new Box(this.width - 20, this.height - 20, new Vector2(0, 0), 1f, 3f, 1, true, rotation);
        this.AddChild(collider);
        collider.visible = false;
        collider.level = Level;

        sound = new Sound("bounce jump.wav");


        this.SetXY(position.x, position.y);
        this.SetOrigin(this.width / 2, this.height / 2);
    }

    void Update()
    {
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
    }
}