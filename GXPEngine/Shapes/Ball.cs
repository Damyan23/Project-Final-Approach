using GXPEngine;
using GXPEngine.Core;
using System;
using System.Drawing;
using System.Linq.Expressions;

public class Ball : EasyDraw
{
    public int _radius;

    RigidBody _rigidBody;

    Random _random = new Random();

    public float Mass()
    {
        return _rigidBody.mass;
    }

    public RigidBody GetRigidBody()
    {
        return _rigidBody;
    }

    Vector2 _position;

    AnimationSprite sprite;

    public int level;
    bool levelAssigned;

    public Ball(int pRadius, Vector2 pPosition, float density, float restitution, int mode, bool isStatic = false) : base(pRadius * 2, pRadius * 2)
    {
        _radius = pRadius;

        //SetScaleXY(0.5f);

        this._position = pPosition;

        sprite = new AnimationSprite("spirit spritesheet.png", 2, 2, -1, false, false);
        sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
        sprite.SetScaleXY(0.4f);
        AddChild(sprite);

        string errorMessage;
        bool success = RigidBody.CreateCircleBody(pRadius, pPosition, density, isStatic, restitution, mode, out _rigidBody, out errorMessage);
        if (!success)
        {
            Console.WriteLine("Error creating rigid body: " + errorMessage);
        }

        this.AddChild(_rigidBody);

        SetOrigin(_radius, _radius);
        this.SetXY(pPosition.x, pPosition.y);


        //if (!isStatic)
        //{
        //    Draw(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255));
        //}
        //else
        //{
        //    DrawStatic(40, 40, 40);
        //}
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

        if (this.level != _rigidBody.Level && !levelAssigned)
        {
            _rigidBody.Level = this.level;
        }
    }

    void UpdateScreenPosition()
    {
        x = _rigidBody.position.x;
        y = _rigidBody.position.y;

        //sprite.x = 0;
        //sprite.y = _rigidBody.position.y;
    }

    public Vector2 GetVelocity()
    {
        return this._rigidBody.linearVelocity;
    }

    public void ApplyForce(Vector2 amount)
    {
        _rigidBody.ApplyForce(amount);
    }

    public void MovePosition (Vector2 newPos)
    {
        _rigidBody.position = newPos;
    }

    public void SetVelocity (Vector2 velocity)
    {
        _rigidBody.linearVelocity = velocity;
    }

    public void Step()
    {
        UpdateScreenPosition();
        sprite.Animate(0.07f);
    }
}
