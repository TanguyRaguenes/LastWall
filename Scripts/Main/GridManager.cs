using Godot;
using System.Collections.Generic;

public partial class GridManager : Node2D
{

	private AStarGrid2D _astar = new();

	//Dimensions de la grille
	private const int CellSize = 64;
	private const int GridWidth = 50;
	private const int GridHeight = 50;

	private readonly HashSet<Vector2I> _towers = [];
	private readonly HashSet<Vector2I> _walls = [];
	private readonly Dictionary<Vector2I, Tree> _trees = [];

	private PackedScene _towerScene = null!;
	private PackedScene _wallScene = null!;
	private PackedScene _treeScene = null!;
	private PackedScene _enemyScene = null!;
	private PackedScene _townCenterScene = null!;


	private Tower _tower = null!;
	private Wall _wall = null!;
	private Tree _tree = null!;
	private TownCenter _townCenter = null!;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		//Import des scenes
		_towerScene = GD.Load<PackedScene>("res://Scenes/Buildings/Tower.tscn");
		_wallScene = GD.Load<PackedScene>("res://Scenes/Buildings/Wall.tscn");
		_treeScene = GD.Load<PackedScene>("res://Scenes/Ressources/Tree.tscn");
		_enemyScene = GD.Load<PackedScene>("res://Scenes/Enemies/Enemy.tscn");
		_townCenterScene = GD.Load<PackedScene>("res://Scenes/Buildings/TownCenter.tscn");

		//Création du centre ville
		_townCenter = _townCenterScene.Instantiate<TownCenter>();
		_townCenter.Position = new Vector2(1000, 1000);
		AddChild(_townCenter);

		_astar.Region = new Rect2I(0, 0, GridWidth, GridHeight);
		_astar.CellSize = new Vector2(CellSize, CellSize);
		_astar.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;
		_astar.Update();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _Draw()
	{

		//Mise en place de la grille
		for (int x = 0; x <= GridWidth; x++)
			DrawLine(new Vector2(x * CellSize, 0), new Vector2(x * CellSize, GridHeight * CellSize), Colors.Gray);

		for (int y = 0; y <= GridHeight; y++)
			DrawLine(new Vector2(0, y * CellSize), new Vector2(GridWidth * CellSize, y * CellSize), Colors.Gray);

	}
	
	public override void _Input(InputEvent @event)
	{

		Vector2 mousePosition = GetGlobalMousePosition();

		int gridX = Mathf.FloorToInt(mousePosition.X / CellSize);
		int gridY = Mathf.FloorToInt(mousePosition.Y / CellSize);

		Vector2I cell = new(gridX, gridY);

		if (@event is not InputEventMouseButton mouseEvent)
        	return;


		if (mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left && !_towers.Contains(cell))
		{
						_towers.Add(cell);

			Tower tower = _towerScene.Instantiate<Tower>();

			tower.Position = new Vector2(
				gridX * CellSize + CellSize / 2,
				gridY * CellSize + CellSize
			);
			AddChild(tower);
		}



		
		if (mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Right && !_trees.ContainsKey(cell))
		{
			Tree tree = _treeScene.Instantiate<Tree>();

			tree.Position = new Vector2(
				gridX * CellSize + CellSize / 2,
				gridY * CellSize + CellSize
			);

			AddChild(tree);

			_trees[cell] = tree;
			_astar.SetPointSolid(cell, true);
			RecalculateAllEnemiesPaths();
		}

		if (mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Middle && _trees.ContainsKey(cell))
		{
			Tree tree = _trees[cell];

			tree.QueueFree();

			_trees.Remove(cell);
			_astar.SetPointSolid(cell, false);
			RecalculateAllEnemiesPaths();
		}

		

	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!@event.IsActionPressed("ui_accept"))
			return;

		Enemy enemy = _enemyScene.Instantiate<Enemy>();

		enemy.Position = new Vector2(0, 320);

		enemy.Init(_townCenter, this);

		AddChild(enemy);
	}


	public Vector2I WorldToCell(Vector2 position)
	{
		return new Vector2I(
			Mathf.FloorToInt(position.X / CellSize),
			Mathf.FloorToInt(position.Y / CellSize)
		);
	}

	public Vector2 CellToWorldCenter(Vector2I cell)
	{
		return new Vector2(
			cell.X * CellSize + CellSize / 2,
			cell.Y * CellSize + CellSize / 2
		);
	}

	public Godot.Collections.Array<Vector2I> GetPath(Vector2 from, Vector2 to)
	{
		Vector2I startCell = WorldToCell(from);
		Vector2I targetCell = WorldToCell(to);

		return _astar.GetIdPath(startCell, targetCell);
	}

	private void RecalculateAllEnemiesPaths()
	{
		foreach (Node node in GetTree().GetNodesInGroup("enemies"))
		{
			if (node is Enemy enemy)
				enemy.RecalculatePath();
		}
	}
}
