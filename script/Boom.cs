using Godot;
using System;

public partial class Boom : Area2D
{
	public float Damage = 10;
	public override void _Ready()
	{
		BodyEntered += Enter;
		GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
		var a = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		a.Play();
		a.AnimationFinished += QueueFree;
	}

	void Enter(Node2D l)
	{
		if (l is Tank tank) {
			tank.TakeDamage(Damage+32-(GlobalPosition-tank.GlobalPosition).Length());
		}
	}
	
}
