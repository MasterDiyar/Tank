using Godot;
using System;
namespace vansur.script;
public partial class Electro : Bullet
{
	private AnimatedSprite2D aSpeite;
	public override void _Ready()
	{
		aSpeite = GetNode<AnimatedSprite2D>("Area2D/AnimatedSprite2D");
		aSpeite.Play("default");
		aSpeite.AnimationFinished += Sanitar;
		
		base._Ready();
	}

	void Sanitar(){
		aSpeite.Play("loop");
		aSpeite.AnimationFinished -= Sanitar;
	}
}
