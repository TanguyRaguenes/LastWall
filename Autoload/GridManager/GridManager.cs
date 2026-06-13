using Godot;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq.Expressions;

public partial class GridManager : Node2D
{

	private AStarGrid2D _astar = new();

	//Dimensions de la grille
	private const int CellSize = 128;
	private const int GridWidth = 50;
	private const int GridHeight = 50;

	private readonly Dictionary<Vector2I, Node2D> _placedItems = [];

	private Dictionary<BuildItemType, PackedScene> _buildScenes = [];

	private PackedScene _towerScene = null!;
	private PackedScene _treeScene = null!;
	private PackedScene _sheepScene = null!;
	private PackedScene _goldScene = null!;
	private PackedScene _peasantScene = null!;
	private PackedScene _enemyScene = null!;
	private PackedScene _townCenterScene = null!;

	private Tower _tower = null!;
	private Tree _tree = null!;
	private TownCenter _townCenter = null!;

	 private BuildItemType _selectedItem = BuildItemType.None;

	 private Node2D? _preview;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		//Import des scenes
		_enemyScene = GD.Load<PackedScene>("res://Scenes/Enemies/Enemy.tscn");
		_townCenterScene = GD.Load<PackedScene>("res://Scenes/Buildings/TownCenter.tscn");
		_towerScene = GD.Load<PackedScene>("res://Scenes/Buildings/Tower.tscn");
		_treeScene = GD.Load<PackedScene>("res://Scenes/Ressources/Tree.tscn");
		_sheepScene = GD.Load<PackedScene>("res://Scenes/Ressources/Sheep.tscn");
		_goldScene = GD.Load<PackedScene>("res://Scenes/Ressources/Gold.tscn");
		_peasantScene = GD.Load<PackedScene>("res://Scenes/PlayerUnits/Peasant.tscn");

		//Création du centre ville
		_townCenter = _townCenterScene.Instantiate<TownCenter>();
		_townCenter.Position = new Vector2(1000, 1000);
		AddChild(_townCenter);

		_astar.Region = new Rect2I(0, 0, GridWidth, GridHeight);
		_astar.CellSize = new Vector2(CellSize, CellSize);
		_astar.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;
		_astar.Update();

		// ConstructionMenu menu = GetNode<ConstructionMenu>("ConstructionMenu");

		Menu menu = GetNode<Menu>("Menu");

		//Mise en place de l'abonnement
        menu.BuildItemSelected += OnBuildItemSelected;

		Texture2D cursor = GD.Load<Texture2D>(
        "res://Assets/Sprites/Cursors/Cursor_02.png"
		);

		Input.SetCustomMouseCursor(cursor);

		_buildScenes = new()
		{
			{ BuildItemType.Tower, _towerScene },
			{ BuildItemType.Tree, _treeScene },
			{ BuildItemType.Sheep, _sheepScene },
			{ BuildItemType.Peasant, _peasantScene },
			{ BuildItemType.Gold, _goldScene }
		};

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if (_selectedItem == BuildItemType.None || _preview == null)
        return;

		Vector2I cell = WorldToCell(GetGlobalMousePosition());
		_preview.Position = CellToWorldCenter(cell);
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

		if (@event is InputEventKey keyEvent
        && keyEvent.Pressed
        && keyEvent.Keycode == Key.Space)
		{
						Enemy enemy = _enemyScene.Instantiate<Enemy>();
				enemy.Position = new Vector2(0, 320);
				enemy.Init(_townCenter, this);
				AddChild(enemy);
		}

	}

	public override void _UnhandledInput(InputEvent @event)
	{

		if (@event is not InputEventMouseButton mouseEvent || !mouseEvent.Pressed)
        	return;

		if (mouseEvent.ButtonIndex == MouseButton.Right)
		{
			CancelBuildSelection();
			return;
		}

		if (mouseEvent.ButtonIndex == MouseButton.Left)
		{
			if (_selectedItem == BuildItemType.None)
				return;

			Vector2I cell = WorldToCell(GetGlobalMousePosition());
			PlaceSelectedItem(cell);
		}

	}


	public static Vector2I WorldToCell(Vector2 position)
	{
		return new Vector2I(
			Mathf.FloorToInt(position.X / CellSize),
			Mathf.FloorToInt(position.Y / CellSize)
		);
	}

	public static Vector2 CellToWorldCenter(Vector2 cell)
	{
		return new Vector2(
			cell.X * CellSize + CellSize / 2,
			cell.Y * CellSize + CellSize
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

	private void OnBuildItemSelected(BuildItemType item)
    {
		_selectedItem = item;
		CreatePreview(item);

        GD.Print($"Sélection : {item}");

		//  Input.MouseMode = Input.MouseModeEnum.Hidden;

    }

	private void PlaceSelectedItem(Vector2I cell)
	{
		PackedScene? scene = GetSceneForItem(_selectedItem);

		if (scene == null)
			return;

		PlaceItem(cell, scene);
	}
	
	private void PlaceItem(Vector2I cell, PackedScene scene)
	{
		if (_placedItems.ContainsKey(cell))
			return;

		Node2D item = scene.Instantiate<Node2D>();

		item.Position = CellToWorldCenter(cell);

		AddChild(item);

		_placedItems[cell] = item;

		_astar.SetPointSolid(cell, true);
		RecalculateAllEnemiesPaths();
	}

	private void CreatePreview(BuildItemType item)
	{
		_preview?.QueueFree();
		_preview = null;

		PackedScene? scene = GetSceneForItem(item);

		if (scene == null)
			return;

		_preview = scene.Instantiate<Node2D>();
		_preview.Modulate = new Color(1, 1, 1, 0.5f);
		_preview.ZIndex = 1000;

		AddChild(_preview);
	}

	private PackedScene? GetSceneForItem(BuildItemType item)
	{
		return _buildScenes.GetValueOrDefault(item);
	}

	private void CancelBuildSelection()
	{
		_selectedItem = BuildItemType.None;

		_preview?.QueueFree();
		_preview = null;

		// Input.MouseMode = Input.MouseModeEnum.Visible;

		GD.Print("Construction annulée");
	}
}

