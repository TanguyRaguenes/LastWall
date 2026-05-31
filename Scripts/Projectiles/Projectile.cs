using Godot;
using System;

public partial class Projectile : Node2D
{

	private const float Speed = 300f;
    private Enemy _target = null!;
    private int _damage;

	public void Init(Enemy target, int damage)
	{
		_target = target;
        _damage = damage;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		 if (!IsInstanceValid(_target))
        {
            QueueFree();
            return;
        }

        Position = Position.MoveToward(
            _target.Position,
            Speed * (float)delta
        );

        if (Position.DistanceTo(_target.Position) < 10f)
        {
            _target.Damage(_damage);
            QueueFree();
        }
	}


}
