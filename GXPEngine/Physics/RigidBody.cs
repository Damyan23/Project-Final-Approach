using GXPEngine;
using GXPEngine.Core;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public enum ShapeType
{
    Circle = 0,
    Box = 1
}

public class RigidBody : GameObject
{
    public Vector2 position;
    public Vector2 linearVelocity;
    public float rotationAmmount;
    public float rotationalVelocity;
    public float rigidBodyRotation;

    private Vector2 force;

    public readonly float density;
    public readonly float mass;
    public readonly float invMass;
    public readonly float restitution;
    public readonly float area;
    public readonly float rotationalInertia;
    public readonly float invRotationalInertia;

    public readonly float gravity;
    
    public readonly  bool isStatic;

    public readonly float radius;
    public readonly float width;
    public readonly float height;

    public readonly int mode;

    private readonly Vector2[] vertices;
    // A array to store the transformed vertices since if we transfrom the already transformed ones, the transformation wont be correct
    private Vector2[] transformedVertices;

    private bool transformUpdateRequire;

    public readonly ShapeType shapeType;

    public Vector2 LinearVelocity
    {
        get { return this.linearVelocity; }
        internal set { this.linearVelocity = value; }
    }

    private RigidBody (Vector2 position, float density, float mass, float restitution, float area,
                        bool isStatic, float radius, float width, float height, ShapeType shapeType, int mode) : base ()
    {
        this.position = position;
        this.linearVelocity = new Vector2 ();
        this.rigidBodyRotation = 0f;
        this.rotationalVelocity = 0f;

        this.force = new Vector2 ();

        this.density = density;
        this.mass = mass;
        this.restitution = restitution;
        this.area = area;

        this.isStatic = isStatic;

        this.radius = radius;
        this.width = width;
        this.height = height;

        this.shapeType = shapeType;

        this.mode = mode;

        this.rotationalInertia = this.CalculateRotationalInertial();

        if (!this.isStatic)
        {
            this.invMass = 1f / this.mass;
            this.invRotationalInertia = 1f / this.rotationalInertia;
        }
        else
        {
            this.invMass = 0f;
            this.invRotationalInertia = 0f;
        }


        if (shapeType is ShapeType.Box)
        {
            this.vertices = RigidBody.CreateRectVertices(this.width, this.height);
            this.transformedVertices = new Vector2[4];
        }
        else
        {
            this.vertices = null;
            this.transformedVertices = null;
        }

        this.transformUpdateRequire = true;
    }

    public static Vector2[] CreateRectVertices(float width, float height)
    {
        float left = -width / 2f;
        float right = left + width;
        float bottom = -height / 2;
        float top = bottom + height;

        Vector2[] vertices = new Vector2[4];
        vertices[0] = new Vector2(left, bottom);
        vertices[1] = new Vector2(left, top);
        vertices[2] = new Vector2(right, top);
        vertices[3] = new Vector2(right, bottom);

        return vertices;
    }

    // If has to be updated, loops trough all vertecies, and then transforms one and puts in ito the transformed vertices array
    public Vector2[] GetTransformedVertices()
    {
        if (this.transformUpdateRequire)
        {
            FlatTransform transform = new FlatTransform(this.position, this.rigidBodyRotation);

            for (int i = 0; i < this.vertices.Length; i++)
            {
                Vector2 v = this.vertices[i];
                this.transformedVertices[i] = Vector2.Transform(v, transform);
            }
        }

        this.transformUpdateRequire = false;

        return this.transformedVertices;
    }

    public static bool CreateBoxBody(float width, float height, Vector2 position, float density, bool isStatic, float restitution, int mode, out RigidBody body, out string erroMassage)
    { 
        body = null;
        erroMassage = string.Empty;

        float area = width * height;

        if (area < World.MinBodySize)
        {
            erroMassage = "Area is too small";
            return false;
        }
        else if (area > World.MaxBodySize)
        {
            erroMassage = "Area radius is too big";
            return true;
        }

        if (density < World.minBodyDensity)
        {
            erroMassage = "The density is too small";
            return false;
        }
        else if (density > World.maxBodyDensity)
        {
            erroMassage = "The density is too big";
            return false;
        }

        restitution = Mathf.Clamp(restitution, 0.0f, 1.0f);

        float mass = area * density;

        body = new RigidBody(position, density, mass, restitution, area, isStatic, 0f, width, height, ShapeType.Box, mode);
        World.AddBody(body);
        return true;
    }

    public static bool CreateCircleBody (float radius, Vector2 position, float density, bool isStatic, float restitution, int mode, out RigidBody body, out string erroMassage)
    {
        body = null;
        erroMassage = string.Empty;

        float area = radius / 10 * radius * Mathf.PI;

        if (area < World.MinBodySize) 
        {
            erroMassage = "Curcle radius is too small";
            return false;
        } else if (area > World.MaxBodySize) 
        {
            erroMassage = "Circle radius is too big";
            return true;
        }

        if (density < World.minBodyDensity)
        {
            erroMassage = "The density is too small";
            return false;
        } else if (density > World.maxBodyDensity) 
        {
            erroMassage = "The density is too big";
            return false;
        }

        restitution = Mathf.Clamp (restitution, 0.0f, 1.0f);

        float mass = area * density;

        body = new RigidBody(position, density, mass, restitution, area, isStatic, radius, 0f, 0f, ShapeType.Circle, mode);
        World.AddBody(body);
        return true;
    }

    private float CalculateRotationalInertial ()
    {
        if (this.shapeType is ShapeType.Circle)
        {
            return (1f / 2) * mass * radius * radius;
        } else if (this.shapeType is ShapeType.Box)
        {
            return (1f / 12) * mass * (height * height + width * width);
        } else
        {
            throw new Exception("Shape type is not supported");
        }
    }

    public void Move (Vector2 moveBy)
    {
        this.position += moveBy;
        //this.transformUpdateRequire = true;
    }

    public void Step (float time, Vector2 gravity, int iterations)
    {
        if (this.isStatic) 
        {
            return;
        }

        time /= (float)iterations;

        // F = ma;
        // a= f/m;

        //Vector2 acceleration = this.force / this.mass;
        //this.linearVelocity += acceleration * time;

        this.linearVelocity += gravity * time;

        this.linearVelocity += force;

        this.position += this.linearVelocity * time;

        this.rigidBodyRotation += this.rotationalVelocity * time;

        this.force = new Vector2(0, 0);

        this.transformUpdateRequire = true;
    }

    public void ApplyForce (Vector2 amount) 
    {
        this.force = amount;
        this.transformUpdateRequire = true;
    }

    public void Rotate (float amount)
    {
        this.rigidBodyRotation += amount;
        this.transformUpdateRequire = true;
    }
}
