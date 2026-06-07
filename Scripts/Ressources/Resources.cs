using Godot;


public abstract partial class Resource : Node2D
{

    public int Health {get; set;}

    public void Harvest(int damage)
    {
        Health-=damage;
        if (Health <= 0)
        {
            QueueFree();
        }
    }
    
}