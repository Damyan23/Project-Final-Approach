using GXPEngine;
using GXPEngine.Core;
using System.Configuration.Assemblies;

public class Teleporper : GameObject
{
    public Sprite Entrence;
    public Sprite Exit;

    public Teleporper (Vector2 enterPoint, Vector2 exitPoint) : base ()
    {
        Entrence = new Sprite("teleporterEntrence.png");
        Entrence.SetXY(enterPoint.x, enterPoint.y);
        this.AddChild(Entrence);

        Exit = new Sprite("teleporterExit.png");
        Exit.SetXY (exitPoint.x, exitPoint.y);
        this.AddChild (Exit);
    }
}