using Godot;
using System;

using Godot;
using System;
using vansur.script;
using vansur.script.Singles;

public partial class UnitPick : Control
{
	[Export] public Button[] ChooseButtons = []; 
	
	public Label InfoLabel, SecondaryInfoLabel;
	[Export] Button startButton;

	private string[] info =
	[
		"tank FH\nspeed: 160px/s\naccel: 100px/s\nhp: 100\narmor: 400u\nrotation speed: 4rad/sec",
		"tank LA\nspeed: 180px/s\naccel: 80px/s\nhp: 120\narmor: 290u\nrotation speed: 3rad/sec",
		"tank Emir\nspeed: 220px/s\naccel: 60px/s\nhp:200\narmor: 200u\nrotation speed: 2.5rad/sec"
	];
	[Export] Texture2D tankTextures;
	
	public int[] SelectedUnitIds = new int[2];

	private Tank[] tanks;
	private Sprite2D[] bodies;
	private int[] tankIndex = [0, 0];
	

	private (Texture2D bodyTexture, PackedScene[] heads, float speed, float accel, float hp, float armor, float rotationSpeed)
		[] tankinfo;

	void LoadTI() => tankinfo =
		[
			(GD.Load<Texture2D>("res://assets/tank.png"), 
				[GD.Load<PackedScene>("res://scene/heads/0l_head.tscn")],
				160, 100, 100, 400, 4)
		];
	

	public override void _Ready()
	{
		LoadTI();
		startButton = GetNode<Button>("Button");
		startButton.Pressed += Start;
		InfoLabel = GetNode<Label>("Label");
		SecondaryInfoLabel = GetNode<Label>("SecondaryInfoLabel");
		tanks = [GetNode<Tank>("Control/tank"), GetNode<Tank>("Control2/tank")];
		bodies = [tanks[0].GetNode<Sprite2D>("Tank"), tanks[1].GetNode<Sprite2D>("Tank")];
		
		

		for (int i = 0; i < ChooseButtons.Length; i++)
		{
			var btn = ChooseButtons[i];
			var body = bodies[i];
			int index = i; 
			btn.Pressed += () => SetButton(btn, body, (index % 2 == 0) ? -1 : 1, index/2);
		}
	}

	void SetButton(Button btn, Sprite2D sprite, int index, int absI)
	{
		
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
		InfoLabel.Text = info[SelectedUnitIds[0]];
		SecondaryInfoLabel.Text = info[SelectedUnitIds[0]];
		throw new NotImplementedException("do it plz!!!");
	}

	void Start()
	{
		var map = GD.Load<PackedScene>("res://scene/duels.tscn").Instantiate<Duels>();
		var tankScene = GD.Load<PackedScene>("res://scene/tank.tscn");

		for (int o = 0; o < 2; o++) {
			var newTank = tankScene.Instantiate<Tank>();
			newTank.Team = 0 % 2;
			newTank.Position = new Vector2((newTank.Team==0) ? 300 : 1200, 100 + 100 * o/2f);
			for (int i = 0; i < 4; i++) {
				if (SelectedUnitIds[i] == 0) continue;
				newTank.AddHead(StatInfo.HeadScenes[SelectedUnitIds[i]]);
			}
			
			map.AddChild(newTank);
		}
		GetTree().Root.AddChild(map);
		CallDeferred("queue_free");
	}
}
