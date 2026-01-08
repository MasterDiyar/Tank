using Godot;
using System;

using Godot;
using System;
using vansur.script.Singles;

public partial class UnitPick : Control
{
	[Export] public MenuButton MainButton,
		SecondaryButton;
	[Export] public TextureRect MainTexture,
		SecondaryTexture;
	
	
	[Export] public MenuButton[] NumberButtons,
		SecondNumberButtons; 
	[Export] public TextureRect[] NumberTextures,
		SecondNumberTextures;

	public Label InfoLabel, SecondaryInfoLabel;
	[Export] Button startButton;

	private string[] info =
	[
		"tank FH\nspeed: 160px/s\naccel: 100px/s\nhp: 100\narmor: 100u\nrotation speed: 4rad/sec",
		"tank LA\nspeed: 180px/s\naccel: 80px/s\nhp: 120\narmor: 90u\nrotation speed: 3rad/sec"
	];
	
	public int[,] SelectedUnitIds = new int[2,5];

	public override void _Ready()
	{
		startButton = GetNode<Button>("Button");
		startButton.Pressed += Start;
		InfoLabel = GetNode<Label>("Label");
		SecondaryInfoLabel = GetNode<Label>("SecondaryInfoLabel");
		SetupMenuButton(MainButton, MainTexture, 0, 0);

		for (var i = 0; i < NumberButtons.Length; i++)
			SetupMenuButton(NumberButtons[i], NumberTextures[i], i + 1, 0);
		for  (var i = 0; i < SecondNumberButtons.Length; i++)
			SetupMenuButton(SecondNumberButtons[i], SecondNumberTextures[i], i + 1, 1);
		
	}

	private void SetupMenuButton(MenuButton btn, TextureRect rect, int saveIndex, int tankIndex)
	{
		btn.ExpandIcon = true;

		var popup = btn.GetPopup();
		popup.IdPressed += (id) => {
			int itemIndex = popup.GetItemIndex((int)id);
			Texture2D selectedIcon = popup.GetItemIcon(itemIndex);
			SelectedUnitIds[tankIndex,saveIndex] = (int)id;
			rect.Texture = selectedIcon;
			btn.Icon = selectedIcon;
			if (saveIndex == 0) 
				UpdateInfoText((int)id, tankIndex);
			
		};
	}
	private void UpdateInfoText(int unitId, int tankIndex)
	{
		if (unitId < info.Length)
		{
			if (tankIndex == 0)
				InfoLabel.Text = info[unitId];
			else
				SecondaryInfoLabel.Text = info[unitId];
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		InfoLabel.Text = info[SelectedUnitIds[0, 0]];
		SecondaryInfoLabel.Text = info[SelectedUnitIds[1, 0]];
	}

	void Start()
	{
		var map = GD.Load<PackedScene>("res://scene/duels.tscn").Instantiate<Duels>();
		var tankScene = GD.Load<PackedScene>("res://scene/tank.tscn");

		for (int o = 0; o < 2; o++)
		{
			var newTank = tankScene.Instantiate<Tank>();
			newTank.Position = new Vector2((o%2==0) ? 300 : 1200, 100 + 100 * o/2f);
			for (int i = 0; i < 5; i++)
			{
				if (SelectedUnitIds[o, i] == 0) continue;
				newTank.AddHead(StatInfo.HeadScenes[SelectedUnitIds[o, i]]);
			}
			
			map.AddChild(newTank);
		}
	}
}
