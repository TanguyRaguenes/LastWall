using Godot;

namespace Game.Units.Archer;

public partial class Archer : CharacterBody2D, ISelectable
{

	public override void _Ready()
	{
	   GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idle");
	}


	public override void _PhysicsProcess(double delta)
	{
		
	}

	private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
	{

		GD.Print("TEST");

		if (@event is InputEventMouseButton mouseEvent 
			&& mouseEvent.ButtonIndex == MouseButton.Left 
			&& mouseEvent.Pressed)
		{
			GD.Print("Sélection archer");
		}
		
	}

    public void Select()
    {
        GD.Print("Archer sélectionné !");
    }
}
