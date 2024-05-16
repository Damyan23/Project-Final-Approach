using GXPEngine;

public class Button : AnimationSprite
{
    public bool hasBeenPressed { get; protected set; }

    public Button(string image, int cols, int rows) : base(image, cols, rows)
    {

        this.SetOrigin(width / 2, height / 2);
    }

    protected virtual void Update()
    {
        ButtonUpdate();
    }

    protected virtual void ButtonUpdate()
    {
        // If the mouse is over the button
        if (HitTestPoint(Input.mouseX, Input.mouseY))
        {
            this.SetCycle(1);
            // If the button has been clicked
            if (Input.GetMouseButtonDown(0))
            {
                hasBeenPressed = true;
            }
        }
        else
        {
            this.SetCycle(0);
        }
    }
}