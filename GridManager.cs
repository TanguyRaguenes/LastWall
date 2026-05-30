using Godot;
using System.Collections.Generic;

public partial class GridManager : Node2D
{
	private const int CellSize = 64;
	private const int GridWidth = 10;
	private const int GridHeight = 10;

	private readonly HashSet<Vector2I> _towers = [];

	private PackedScene _towerScene = null!;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_towerScene = GD.Load<PackedScene>("res://Tower.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _Draw()
	{
		for (int x = 0; x <= GridWidth; x++)
			DrawLine(new Vector2(x * CellSize, 0), new Vector2(x * CellSize, GridHeight * CellSize), Colors.Gray);

		for (int y = 0; y <= GridHeight; y++)
			DrawLine(new Vector2(0, y * CellSize), new Vector2(GridWidth * CellSize, y * CellSize), Colors.Gray);

		

			
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is not InputEventMouseButton mouseEvent)
        return;

		if (!mouseEvent.Pressed || mouseEvent.ButtonIndex != MouseButton.Left)
			return;

		Vector2 mousePosition = GetGlobalMousePosition();

		int gridX = Mathf.FloorToInt(mousePosition.X / CellSize);
		int gridY = Mathf.FloorToInt(mousePosition.Y / CellSize);

		Vector2I cell = new(gridX, gridY);

		if (_towers.Contains(cell))
			return;

		_towers.Add(cell);

		Node2D tower = _towerScene.Instantiate<Node2D>();

		tower.Position = new Vector2(
			gridX * CellSize + CellSize / 2,
			gridY * CellSize + CellSize
		);

		AddChild(tower);

	}
}
