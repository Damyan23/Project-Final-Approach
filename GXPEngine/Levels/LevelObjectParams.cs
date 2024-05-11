// Contains all the parameters an object can have
using GXPEngine.Core;

public class LevelObjectParams
{
    public LevelObjectType type;
    public Vector2 position;
    public float width;
    public float height;
    public float rotation;
    public float density;
    public float bounciness;
    public int mode;
    public bool isStatic;

    public string imageName;

    public LevelObjectParams(LevelObjectType type, string imageName, Vector2 position, float width, float height, float rotation, float density, float bounciness, int mode, bool isStatic)
    {
        this.type = type;
        this.imageName = imageName;
        this.position = position;
        this.width = width;
        this.height = height;
        this.rotation = rotation;
        this.density = density;
        this.bounciness = bounciness;
        this.mode = mode;
        this.isStatic = isStatic;
    }
}

public enum LevelObjectType
{
    Box,
    Spawnpoint,
}
