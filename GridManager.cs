using Godot;
using System.Collections.Generic;

public partial class GridManager : Node2D
{
	private const int CellSize = 64;
	private const int GridWidth = 10;
	private const int GridHeight = 10;

	private readonly HashSet<Vector2I> _towers = [];

	private PackedScene _towerScene = null!;
	private PackedScene _enemyScene = null!;
	private PackedScene _townCenterScene = null!;


	private Tower _tower = null!;
	private TownCenter _townCenter = null!;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_towerScene = GD.Load<PackedScene>("res://Tower.tscn");
		_enemyScene = GD.Load<PackedScene>("res://Enemy.tscn");
		_townCenterScene = GD.Load<PackedScene>("res://TownCenter.tscn");

		_townCenter = _townCenterScene.Instantiate<TownCenter>();
		_townCenter.Position = new Vector2(320, 320);
		AddChild(_townCenter);
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

		Tower tower = _towerScene.Instantiate<Tower>();

		tower.Position = new Vector2(
			gridX * CellSize + CellSize / 2,
			gridY * CellSize + CellSize
		);

		AddChild(tower);

	}

	public override void _UnhandledInput(InputEvent @event)
{
    if (!@event.IsActionPressed("ui_accept"))
        return;

    Enemy enemy = _enemyScene.Instantiate<Enemy>();

    enemy.Position = new Vector2(0, 320);

	enemy.Init(_townCenter);

    AddChild(enemy);
}
}
