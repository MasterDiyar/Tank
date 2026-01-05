using Godot;
using System;
using vansur.script;

public partial class BreakableObject : StaticBody2D, DamagingObject
{
	public float Hp { get; set; } = 1000;
	[Export] float MaxHp { get; set; } = 1000;
	[Export] PackedScene smokeParticles;
	public override void _Ready()
	{
		Hp = MaxHp;
	}

	public virtual void TakeDamage(float damage)
	{
		Hp -= damage;
		
		
		if (Hp <= 0)CallDeferred(MethodName.DefferedDie);
	}

	protected void DefferedDie()
	{
		var smk = smokeParticles.Instantiate<CpuParticles2D>();
		smk.GlobalPosition = GlobalPosition;
		GetTree().Root.CallDeferred("add_child", smk);
		QueueFree();
	}
}
