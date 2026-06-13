using Godot;

using GodotGame.Game.Interfaces;

public partial class SelectionManager: Node2D
{

	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event is InputEventMouseButton mouseButtonEvent 
		&& mouseButtonEvent.ButtonIndex == MouseButton.Left
		&& mouseButtonEvent.Pressed)
		{

			DetectNodesUnderMouse();

		}
	}
	private void DetectNodesUnderMouse()
	{

		Vector2 position = GetGlobalMousePosition();
		GD.Print(position);

		PhysicsPointQueryParameters2D query = new PhysicsPointQueryParameters2D();
		query.Position=position;
		query.CollideWithAreas=true;
		query.CollideWithBodies=true;

		PhysicsDirectSpaceState2D spaceState = GetWorld2D().DirectSpaceState;

		Godot.Collections.Array<Godot.Collections.Dictionary> results =
			spaceState.IntersectPoint(query, 32);

		foreach (Godot.Collections.Dictionary result in results)
		{
			Variant collider = result["collider"];

			if (collider.Obj is Node node &&  node is ISelectable selection)
			{
				GD.Print($"Objet trouvé : {node.Name} | Path : {node.GetPath()}");

				selection.Select();
			}
		}
		
	}
	
}
