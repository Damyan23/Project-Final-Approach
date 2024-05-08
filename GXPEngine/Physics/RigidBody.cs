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
    public float rotationAmount;
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

    public int mode;

    private readonly Vector2[] vertices;
    // A array to store the transformed vertices since if we transform the already transformed ones, the transformation wont be correct
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

    //If has to be updated, loops trough all vertices, and then transforms one and puts in ito the transformed vertices array
    //public Vector2[] GetTransformedVertices()
    //{
    //    if (this.transformUpdateRequire)
    //    {
    //        // Calculate the center of the object
    //        Vector2 center = new Vector2(this.position.x + this.width / 2, this.position.y + this.height / 2);

    //        // Create a FlatTransform instance with the center position and rigid body rotation
    //        FlatTransform transform = new FlatTransform(center, this.rigidBodyRotation);

    //        for (int i = 0; i < this.vertices.Length; i++)
    //        {
    //            // Calculate the vertex position relative to the object's center
    //            Vector2 v = this.vertices[i] - center;

    //            // Rotate the vertex around the object's center
    //            this.transformedVertices[i] = Vector2.Transform(v, transform);

    //            // Offset the vertex back to its absolute position
    //            this.transformedVertices[i] += center;
    //        }
    //    }

    //    this.transformUpdateRequire = false;

    //    return this.transformedVertices;
    //}


    public Vector2[] GetTransformedVertices()
    {
        // Ensure that the object has a parent
        if (parent != null)
        {
            // Get the parent's position, width, and height
            float parentX = parent.x;
            float parentY = parent.y;
            float parentWidth = this.width;
            float parentHeight = this.height;

            // Calculate the corners of the parent object relative to its center
            Vector2[] parentCorners = new Vector2[4];
            parentCorners[0] = new Vector2(-parentWidth / 2, -parentHeight / 2); // Top-left corner
            parentCorners[1] = new Vector2(parentWidth / 2, -parentHeight / 2); // Top-right corner
            parentCorners[2] = new Vector2(parentWidth / 2, parentHeight / 2); // Bottom-right corner
            parentCorners[3] = new Vector2(-parentWidth / 2, parentHeight / 2); // Bottom-left corner

            // Apply the parent's rotation to the corners
            float parentRotation = parent.rotation * Mathf.PI / 180.0f;
            for (int i = 0; i < parentCorners.Length; i++)
            {
                float rotatedX = parentCorners[i].x * Mathf.Cos(parentRotation) - parentCorners[i].y * Mathf.Sin(parentRotation);
                float rotatedY = parentCorners[i].x * Mathf.Sin(parentRotation) + parentCorners[i].y * Mathf.Cos(parentRotation);
                parentCorners[i] = new Vector2(rotatedX, rotatedY);
            }

            // Calculate the transformed vertices by adding the parent's position
            for (int i = 0; i < parentCorners.Length; i++)
            {
                vertices[i] = new Vector2(parentX + parentCorners[i].x, parentY + parentCorners[i].y);
                this.transformedVertices[i] = vertices[i];
            }

            // Indicate that the vertices have been updated
            this.transformUpdateRequire = true;
        }
        return this.transformedVertices;
    }




    public static bool CreateBoxBody(float width, float height, Vector2 position, float density, bool isStatic, float restitution, int mode, out RigidBody body, out string errorMessage)
    { 
        body = null;
        errorMessage = string.Empty;

        float area = width * height;

        if (area < World.MinBodySize)
        {
            errorMessage = "Area is too small";
            return false;
        }
        else if (area > World.MaxBodySize)
        {
            errorMessage = "Area radius is too big";
            return true;
        }

        if (density < World.minBodyDensity)
        {
            errorMessage = "The density is too small";
            return false;
        }
        else if (density > World.maxBodyDensity)
        {
            errorMessage = "The density is too big";
            return false;
        }

        //restitution = Mathf.Clamp(restitution, 0.0f, 1.0f);

        float mass = area * density;

        body = new RigidBody(position, density, mass, restitution, area, isStatic, 0f, width, height, ShapeType.Box, mode);
        World.AddBody(body);
        return true;
    }

    public static bool CreateCircleBody (float radius, Vector2 position, float density, bool isStatic, float restitution, int mode, out RigidBody body, out string errorMessage)
    {
        body = null;
        errorMessage = string.Empty;

        float area = radius / 10 * radius * Mathf.PI;

        if (area < World.MinBodySize) 
        {
            errorMessage = "Circle radius is too small";
            return false;
        } else if (area > World.MaxBodySize) 
        {
            errorMessage = "Circle radius is too big";
            return true;
        }

        if (density < World.minBodyDensity)
        {
            errorMessage = "The density is too small";
            return false;
        } else if (density > World.maxBodyDensity) 
        {
            errorMessage = "The density is too big";
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
