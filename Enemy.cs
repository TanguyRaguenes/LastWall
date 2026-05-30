using Godot;

public partial class Enemy : Node2D
{
    private const float Speed = 100f;

    private TownCenter _townCenter = null!;

	public int Health { get; private set; } = 2;

    public void Init(TownCenter townCenter)
    {
        _townCenter = townCenter;
    }

    public override void _Ready()
    {
		AddToGroup("enemies");

        GetNode<AnimatedSprite2D>("AnimatedSprite2D")
            .Play("idle");
    }

    public override void _Process(double delta)
    {
        Position = Position.MoveToward(
            _townCenter.Position,
            Speed * (float)delta);

        if (Position.DistanceTo(_townCenter.Position) < 5)
        {
            _townCenter.Damage(1);
            QueueFree();
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