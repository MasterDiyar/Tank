using Godot;
using System;

public partial class SmokeParticles : CpuParticles2D
{
	[Export]public Sprite2D sprite;
	private int times = 2;
	
	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
		Timer timer = GetNode<Timer>("Timer");
		timer.Timeout += TimeOut;
		timer.Start();
		AudioStreamPlayer2D audio = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		audio.Play();
	}

	void TimeOut()
	{
		
		times--;
		GD.Print("TimeOut: ", times);
		if (times == 1)
		{
			sprite.Visible = true;
			sprite.Reparent(GetParent());
		}
		if (times == 0)
			QueueFree();
	}
	
}
