using GXPEngine;
using GXPEngine.Core;
using System.Collections.Generic;
using System.Linq;

class World
{
    public static readonly float MinBodySize = 0.01f * 0.01f;
    public static readonly float MaxBodySize = float.MaxValue;

    public static readonly float minBodyDensity = 0.0001f;
    public static readonly float maxBodyDensity = 21.4f;

    private static List<RigidBody> bodyList = new List<RigidBody>();
    private Vector2 gravity;

    public static readonly int minIterations = 1;
    public static readonly int maxIterations = 64;

    private List<CollisionManifold> pointsOfContactList = new List<CollisionManifold>();
    public World()
    {
        this.gravity = new Vector2(0f, 500f);
    }

    // Add a body to the world
    public static void AddBody(RigidBody body)
    {
        bodyList.Add(body);
    }

    // Remove a body from the world
    public bool RemoveBody(RigidBody body)
    {
        body.parent.LateDestroy();
        return bodyList.Remove(body);
    }

    public void Step(float time, int iterations)
    {
        iterations = Mathf.Clamp(iterations, World.minIterations, World.maxIterations);

        for (int it = 0; it < iterations; it++)
        {
            // movement step
            for (int i = 0; i < bodyList.Count; i++)
            {
                bodyList[i].Step(time, gravity, iterations);
            }

            this.pointsOfContactList.Clear();

            // collision step
            for (int i = 0; i < bodyList.Count - 1; i++)
            {
                RigidBody bodyA = bodyList[i];

                for (int j = i + 1; j < bodyList.Count; j++)
                {
                    RigidBody bodyB = bodyList[j];

                    //If both objects are static
                    if (bodyA.isStatic && bodyB.isStatic)
                    {
                        continue;
                    }

                    if (Collide(bodyA, bodyB, out Vector2 normal, out float depth))
                    {
                        HandelCollision(bodyA, bodyB, normal, depth);

                        CollisionManifold contact = new CollisionManifold(bodyA, bodyB, normal, depth, new Vector2(), new Vector2(), 0);
                        this.pointsOfContactList.Add(contact);
                    }
                }
            }

            // Dividing the resolve from the collision step will allow for efficient structure. This way the world can be divided into grids and instead of
            // testing for collision trough all objects in the world I can test for collision in only one node of the grid.
            for (int i = 0; i < this.pointsOfContactList.Count; i++)
            {
                CollisionManifold contact = this.pointsOfContactList[i];
                this.ResolveCollision(contact);
            }
        }
    }

    public void HandelCollision(RigidBody bodyA, RigidBody bodyB, Vector2 normal, float depth)
    {
        // Move the objects out of each other
        if (bodyA.isStatic)
        {
            bodyB.Move(normal * depth);
        }
        else if (bodyB.isStatic)
        {
            bodyA.Move(-normal * depth);
        }
        else
        {
            bodyA.Move(-normal * depth / 2f);
            bodyB.Move(normal * depth / 2f);
        }
    }

    public void ResolveCollision(in CollisionManifold contact)
    {
        RigidBody bodyA = contact.bodyA;
        RigidBody bodyB = contact.bodyB;

        Vector2 normal = contact.normal;
        float depth = contact.depth;

        // Get the relative velocity of the bodies
        Vector2 relativeVelocity = bodyB.LinearVelocity - bodyA.LinearVelocity;

        if (Mathf.Dot(relativeVelocity, normal) > 0)
        {
            return;
        }

        float e = (bodyA.restitution + bodyB.restitution) / 2;

        // Formula for calculating impulse
        float j = -(1f + e) * Mathf.Dot(relativeVelocity, normal);
        j /= bodyA.invMass + bodyB.invMass;
        Vector2 impulse = j * normal;

        // Add an impulse to the bodies 
        bodyA.LinearVelocity -= impulse * bodyA.invMass;
        bodyB.LinearVelocity += impulse * bodyB.invMass;
        
    }

    public bool Collide(RigidBody bodyA, RigidBody bodyB, out Vector2 normal, out float depth)
    {
        normal = new Vector2();
        depth = 0f;


        //Return no collision if at least one of the rigid bodies is in mode 0, or if they are not in the same mode

        if (bodyA.mode != 0 && bodyB.mode != 0)
        {
            if (bodyA.mode != bodyB.mode)
            {
                return false;
            }

        }

        ShapeType shapeTypeA = bodyA.shapeType;
        ShapeType shapeTypeB = bodyB.shapeType;

        if (shapeTypeA is ShapeType.Box)
        {
            if (shapeTypeB is ShapeType.Box)
            {
                return Collisions.IntersectPolygons(bodyA.position,
                        bodyA.GetTransformedVertices(),
                        bodyB.position, bodyB.GetTransformedVertices(),
                        out normal, out depth);
            }
            else if (shapeTypeB is ShapeType.Circle)
            {
                bool result = Collisions.IntersectCirclePolygon(bodyB.position,
                                bodyB.radius, bodyA.position,
                                bodyA.GetTransformedVertices(), out normal, out depth);

                // reverse the normal since i want to pull bodyA out of bodyB
                normal = -normal;

                return result;
            }
        }
        else if (shapeTypeA is ShapeType.Circle)
        {
            if (shapeTypeB is ShapeType.Box)
            {
                return Collisions.IntersectCirclePolygon(bodyA.position, bodyA.radius, bodyB.position, bodyB.GetTransformedVertices(), out normal, out depth);
            }
            else if (shapeTypeB is ShapeType.Circle)
            {
                return Collisions.IntersectCircles(bodyA.position, bodyA.radius, bodyB.position, bodyB.radius, out normal, out depth);
            }
        }

        return false;
    }
}