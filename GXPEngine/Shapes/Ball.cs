using GXPEngine;
using GXPEngine.Core;
using System;
using System.Drawing;

public class Ball : EasyDraw
{
    public int _radius;

    RigidBody _rigidBody;

    Random _random = new Random();

    public float Mass()
    {
        return _rigidBody.mass;
    }

    Vector2 _position;

    public Ball(int pRadius, Vector2 pPosition, float density, float restitution, int mode, bool isStatic = false) : base(pRadius * 2 + 1, pRadius * 2 + 1)
    {
        _radius = pRadius;
        this._position = pPosition;

        string errorMessage;
        bool success = RigidBody.CreateCircleBody(pRadius, pPosition, density, isStatic, restitution, mode, out _rigidBody, out errorMessage);
        if (!success)
        {
            Console.WriteLine("Error creating rigid body: " + errorMessage);
        }

        this.AddChild(_rigidBody);

        this.SetXY(pPosition.x, pPosition.y);

        SetOrigin(_radius, _radius);
        if (!isStatic)
        {
            Draw(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255));
        }
        else
        {
            DrawStatic(40, 40, 40);
        }
    }


    void Draw(int red, int green, int blue)
    {
        Fill(red, green, blue);
        Stroke(red, green, blue);
        Ellipse(_radius, _radius, 2 * _radius, 2 * _radius);
    }

    void DrawStatic(int red, int green, int blue)
    {
        Fill(red, green, blue);
        Stroke(Color.Red);
        Ellipse(_radius, _radius, 2 * _radius, 2 * _radius);
    }

    void Update()
    {
        Step();

        if (Input.GetKeyDown(Key.SPACE))
        {
            if (_rigidBody.mode == 1)
            {
                _rigidBody.mode = 2;
            }
            else if (_rigidBody.mode == 2)
            {
                _rigidBody.mode = 1;
            }
        }
    }

    void UpdateScreenPosition()
    {
        x = _rigidBody.position.x;
        y = _rigidBody.position.y;
    }

    public void ApplyForce(Vector2 amount)
    {
        _rigidBody.ApplyForce(amount);
    }

    public void Step()
    {
        UpdateScreenPosition();
    }
}
