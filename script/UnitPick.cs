using Godot;
using System;

using Godot;
using System;

public partial class UnitPick : Control
{
	[Export] public MenuButton MainButton;
	[Export] public TextureRect MainTexture;
	
	[Export] public MenuButton[] NumberButtons; 
	[Export] public TextureRect[] NumberTextures;

	public Label InfoLabel;
	Button startButton;

	private string[] info =
	[
		"tank 1\nspeed: 160px/s\naccel: 100px/s\nhp: 100\narmor: 100u\nrotation speed: 4rad/sec",
		"tank LA\nspeed: 180px/s\naccel: 80px/s\nhp: 120\narmor: 90u\nrotation speed: 3rad/sec"
	];
	
	public int[] SelectedUnitIds = new int[5];

	public override void _Ready()
	{
		startButton = GetNode<Button>("Button");
		InfoLabel = GetNode<Label>("Label");
		SetupMenuButton(MainButton, MainTexture, 0);

		for (var i = 0; i < NumberButtons.Length; i++)
			SetupMenuButton(NumberButtons[i], NumberTextures[i], i + 1);
		
	}

	private void SetupMenuButton(MenuButton btn, TextureRect rect, int saveIndex)
	{
		btn.ExpandIcon = true;

		var popup = btn.GetPopup();
		popup.IdPressed += (id) => {
			int itemIndex = popup.GetItemIndex((int)id);
			Texture2D selectedIcon = popup.GetItemIcon(itemIndex);
			SelectedUnitIds[saveIndex] = (int)id;
			rect.Texture = selectedIcon;
			btn.Icon = selectedIcon;
			if (saveIndex == 0) 
				UpdateInfoText((int)id);
			
		};
	}
	private void UpdateInfoText(int unitId)
	{
		if (unitId < info.Length) 
			InfoLabel.Text = info[unitId];
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		InfoLabel.Text = info[SelectedUnitIds[0]];
	}
}
