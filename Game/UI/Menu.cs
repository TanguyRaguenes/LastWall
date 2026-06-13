using Godot;
using System;
using GodotGame.Game.Types;

namespace GodotGame.Game.UI.Menu;


public partial class Menu : CanvasLayer
{

	public event Action<BuildItemType>? BuildItemSelected;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("PanelContainer/GridContainer/TowerButton").Pressed += () =>
			BuildItemSelected?.Invoke(BuildItemType.Tower);

		GetNode<Button>("PanelContainer/GridContainer/TreeButton").Pressed += () =>
			BuildItemSelected?.Invoke(BuildItemType.Tree);

		GetNode<Button>("PanelContainer/GridContainer/GoldButton").Pressed += () =>
			BuildItemSelected?.Invoke(BuildItemType.Gold);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
