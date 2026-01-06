using Godot;
using System;

public partial class AiBehavior : Node2D
{
	/// <summary>
	/// radar
	///
	/// if unit find someone
	/// na kogo napodet vazhnost
	///	Player
	/// Player core
	/// Player other building
	/// Different Fraction
	/// Different Fraction buildings
	///
	/// </summary>
	[Export] public int Difficulty = 0;
	private float[] SearchDistance = [300, 600, 1000];

	private float AngleToClosest;
	
	public override void _Ready()
	{
		var timer = new Timer()
		{
			WaitTime = 0.3,
			Autostart = true
		};
		timer.Timeout += AiChoosing;
		AddChild(timer);
	}

	void AiChoosing()
	{
		
	}

	public override void _Process(double delta)
	{
	}
}
