using Godot;

public partial class Enemy : Node2D
{
    private const float Speed = 100f;

    private TownCenter _townCenter = null!;
    private GridManager _gridManager = null!;
    private Godot.Collections.Array<Vector2I> _path = new();
    private int _pathIndex = 0;

    public int Health { get; private set; } = 2;

    public void Init(TownCenter townCenter, GridManager gridManager)
    {
        _townCenter = townCenter;
        _gridManager = gridManager;

        RecalculatePath();
    }

    public override void _Ready()
    {
        AddToGroup("enemies");

        GetNode<AnimatedSprite2D>("AnimatedSprite2D")
            .Play("idle");
    }

    public void RecalculatePath()
    {
        _path = _gridManager.GetPath(Position, _townCenter.Position);

        if (_path.Count <= 1)
        {
            _pathIndex = 0;
            return;
        }

        _pathIndex = 1;
    }

    public override void _Process(double delta)
    {
        MoveAlongPath(delta);
    }

    private void MoveAlongPath(double delta)
    {
        if (_path.Count == 0)
            return;

        Vector2 targetPosition = _gridManager.CellToWorldCenter(_path[_pathIndex]);

        Position = Position.MoveToward(
            targetPosition,
            Speed * (float)delta
        );

        if (Position.DistanceTo(targetPosition) < 5)
        {
            _pathIndex++;

            if (_pathIndex >= _path.Count)
            {
                _townCenter.Damage(1);
                QueueFree();
            }
        }
    }

    public void Damage(int amount)
    {
        Health -= amount;
        GD.Print($"Enemy : {Health} PV");

        if (Health <= 0)
        {
            GD.Print("Arg!!!!");
            QueueFree();
        }
    }
}