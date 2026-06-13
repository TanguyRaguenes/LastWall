using Godot;
using System;
using GodotGame.Game.Types;

namespace GodotGame.Game.UI.ConstructionMenu;

public partial class ConstructionMenu : CanvasLayer
{


	public event Action<BuildItemType>? BuildItemSelected;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<TextureButton>("PanelContainer/GridContainer/TowerButton").Pressed += () =>
			BuildItemSelected?.Invoke(BuildItemType.Tower);

		GetNode<TextureButton>("PanelContainer/GridContainer/TreeButton").Pressed += () =>
			BuildItemSelected?.Invoke(BuildItemType.Tree);

		GetNode<TextureButton>("PanelContainer/GridContainer/GoldButton").Pressed += () =>
			BuildItemSelected?.Invoke(BuildItemType.Gold);

		GetNode<TextureButton>("PanelContainer/GridContainer/SheepButton").Pressed += () =>
			BuildItemSelected?.Invoke(BuildItemType.Sheep);

		GetNode<TextureButton>("PanelContainer/GridContainer/PeasantButton").Pressed += () =>
			BuildItemSelected?.Invoke(BuildItemType.Peasant);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
