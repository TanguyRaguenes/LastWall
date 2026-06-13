using Godot;

namespace GodotGame.Game.bases;

public abstract partial class Harvestable : Node2D
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