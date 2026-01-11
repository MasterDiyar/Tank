using Godot;
using System;

public partial class Spawner : Timer
{
	[Export] public int Count = 1, Team = 0, Difficulty = 0;
	[Export] public PackedScene[] Mob;
	[Export] public Vector2 SpawnOffset = Vector2.Zero, SpawnSize = Vector2.One;
	private Node2D Parent;
	[Export] public Vector2 SpawnRandomRange = new Vector2(50, 50); 
	[Export] public PackedScene AiScene = GD.Load<PackedScene>("res://scene/ai_behavior.tscn");
	public override void _Ready()
	{
		Parent = GetParent<Node2D>();
		Timeout += Work;
	}

	void Work()
	{
		if (Mob.Length == 0) return;
		
		for (int i = 0; i < Count; i++){
			int rI = GD.RandRange(0, Mob.Length - 1);
			var mob = Mob[rI].Instantiate<Node2D>();
			Vector2 randomPos = new Vector2(
				(float)GD.RandRange(-SpawnRandomRange.X, SpawnRandomRange.X),
				(float)GD.RandRange(-SpawnRandomRange.Y, SpawnRandomRange.Y)
			);
			GetTree().CurrentScene.AddChild(mob);
            
			mob.GlobalPosition = GetParent<Node2D>().GlobalPosition + SpawnOffset + randomPos;
			var ai = AiScene.Instantiate<AiBehavior>();
			ai.Difficulty = Difficulty;
			mob.AddChild(ai);

			if (mob is Tank tank)
				tank.Team = Team;
		}
	}
}
