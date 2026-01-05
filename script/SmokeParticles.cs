using Godot;
using System;

public partial class SmokeParticles : CpuParticles2D
{
	
	public override void _Ready()
	{
		Timer timer = GetNode<Timer>("Timer");
		timer.Timeout += () => CallDeferred(Node.MethodName.QueueFree);
		AudioStreamPlayer2D audio = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		audio.Play();
	}
	
}
