using GXPEngine;
using GXPEngine.Core;

public class Leaf : AnimationSprite
{
    Vector2 position;
    public int Level;

    Box boxCollider;

    bool canPlaySound = true;

    Sound sound;
    public Leaf(Vector2 position, float rotation, int mode) : base("leaf platform.png", 2, 1)
    {
        this.rotation = rotation;
        this.position = position;
        boxCollider = new Box(this.width - 20, this.height / 2, new Vector2(0, 0),1,  0.8f, mode, true, rotation + 10);
        this.AddChild(boxCollider);
        boxCollider.visible = false;
        boxCollider.level = Level;

        sound = new Sound("leaf jump.wav");

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
    }
}