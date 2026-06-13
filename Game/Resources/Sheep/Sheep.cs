using Godot;
using GodotGame.Game.bases;

namespace GodotGame.Game.Resources.Sheep;

public partial class Sheep : Harvestable
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("default");
		Health=2;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
