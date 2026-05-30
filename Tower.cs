using Godot;
using System;

public partial class Tower : Node2D
{

	private const float Range = 150f;
	private PackedScene _projectileScene = null!;
	private double _cooldown;
	private const double FireRate = 1.0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Tower prête");
		_projectileScene = GD.Load<PackedScene>("res://Projectile.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// On réduit le temps d'attente à chaque frame
		_cooldown -= delta;

		Enemy? target = FindTarget();

		if (target == null)
			return;

		// Si le cooldown n'est pas terminé, on ne tire pas
		if (_cooldown > 0)
			return;

		Projectile projectile = _projectileScene.Instantiate<Projectile>();

		projectile.Position = Position;
		projectile.Init(target, 1);

		GetParent().AddChild(projectile);

		// On remet le cooldown à 1 seconde
		_cooldown = FireRate;

	}

	 private Enemy? FindTarget()
    {
        foreach (Node node in GetTree().GetNodesInGroup("enemies"))
        {
            Enemy enemy = (Enemy)node;

            if (Position.DistanceTo(enemy.Position) <= Range)
            {
                return enemy;
            }
        }

        return null;
    }
}
