// Contains all the parameters an object can have
using GXPEngine;
using GXPEngine.Core;

public class LevelObjectParams
{
    public LevelObjectType type;
    public Vector2 position;
    public Vector2 position2;
    public FanDirection fanDirection;
    public float width;
    public float height;
    public float rotation;
    public float density;
    public float bounciness;
    public int mode;
    public bool isStatic;

    public string imageName;

    public LevelObjectParams(LevelObjectType type, Vector2 position, string imageName = null, Vector2 position2 = new Vector2 () , float width = 0, float height = 0, float rotation = 0, float density = 0, float bounciness = 0, int mode = 0, bool isStatic = false, FanDirection fanDirection = FanDirection.None)
    {
        this.type = type;
        this.imageName = imageName;
        this.position = position;
        this.position2 = position2;
        this.width = width;
        this.height = height;
        this.rotation = rotation;
        this.density = density;
        this.bounciness = bounciness;
        this.mode = mode;
        this.isStatic = isStatic;
        this.fanDirection = fanDirection;
        
    }
}

public enum LevelObjectType
{
    Spawnpoint,
    Teleporter,
    Explosive,
    Thorns,
    Fan,
    Exit,
    HalfpipeRight,
    HalfpipeLeft,
    Log,
    LogLeft,
    LogRight,
    LogMid,
    Leaf,
    Mushroom,
}
