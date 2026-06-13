using Godot;

namespace GodotGame.Game.Buildings.TownCenter;

public partial class TownCenter : Node2D
{
	public int Health { get; private set; } = 10;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Damage(int amount)
    {
        Health -= amount;
        GD.Print($"Centre-ville : {Health} PV");

        if (Health <= 0)
        {
            GD.Print("GAME OVER");
        }
    }
}
