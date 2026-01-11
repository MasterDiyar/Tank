using Godot;
using System;

public partial class Game : Node2D
{
	[Export] private Button solo, duel;

	public override void _Ready()
	{
		solo.Pressed += () => { var a =GD.Load<PackedScene>("res://scene/maps/sujet.tscn").Instantiate(); GetTree().Root.AddChild(a); QueueFree();};
		duel.Pressed += () => { var a =GD.Load<PackedScene>("res://scene/duels.tscn").Instantiate(); GetTree().Root.AddChild(a); QueueFree();};
	}
}
