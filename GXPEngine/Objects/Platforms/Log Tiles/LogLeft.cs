using GXPEngine;
using GXPEngine.Core;

public class LogLeft : Sprite
{
    Vector2 position;
    public int Level = 1;

    bool canPlaySound = true;

    Box collider;

    Sound sound;
    public LogLeft(Vector2 position, float rotation) : base("1st tile.png")
    {
        this.rotation = rotation;
        this.position = position;
        collider = new Box(this.width - 90, this.height / 2 - 20, new Vector2(0, 0), 1f, 0.8f, 1, true, rotation);
        this.AddChild(collider);
        collider.visible = true;
        collider.level = Level;

        sound = new Sound("normal platform.wav");


        this.SetXY(position.x, position.y);
        this.SetOrigin(this.width / 2, this.height / 2);
    }

    void Update()
    {
        Ball player = ((MyGame)game).GetPlayer();

        if (player == null) return;

        if (collider.HitTest(player))
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