using Godot;


namespace GodotGame.Game.GridDrawer;

public partial class GridDrawer : Node2D
{

	private const int CellSize = 128;
    private const int GridWidth = 50;
    private const int GridHeight = 50;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ZIndex = -5;
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
}
