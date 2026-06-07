using Godot;
using System;

public partial class Peasant : Area2D, ISelectable
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Peasant prêt");
		//GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idle");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
	{
		GD.Print($"EVENT = {@event.GetType().Name}");

		if (@event is InputEventMouseButton mouseEvent)
		{
			GD.Print($"BUTTON = {mouseEvent.ButtonIndex}, PRESSED = {mouseEvent.Pressed}");
		}
	}

    public void Select()
    {
        GD.Print("Paysan sélectionné !");
    }
}
