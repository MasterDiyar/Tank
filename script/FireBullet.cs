using Godot;
using System;
using System.Collections.Generic;

namespace vansur.script;
	
	
public partial class FireBullet : Bullet
{
	Node2D parent;
	private Timer DamageTimer;
	private List<DamagingObject> enemyTanks = [];
	public override void _Ready()
	{
		area = GetNode<Area2D>("Area2D");
		area.TopLevel = true;
		area.BodyEntered += Damaging;
		area.BodyExited += UnDamaging;
		Position = Vector2.Zero;
		parent = GetParent<Node2D>();
		area.Position = parent.GlobalPosition;
		area.GlobalRotation = GlobalRotation;
		Timer timer = GetNode<Timer>("Timer");
		timer.Start();
		timer.Timeout += DieProcess;
		DamageTimer = GetNode<Timer>("DamageTimer");
		DamageTimer.Timeout += fireDamage;
		GetNode<CpuParticles2D>("Area2D/fireParticles").Emitting = true;
		Damage *= myTank.InitialDamage;
	}
	
	protected override void Damaging(Node2D damaging)
	{
		if (piercing < 0 || damaging == myTank) return;
		if (damaging is DamagingObject target )
		{
			enemyTanks.Add(target);
			if (enemyTanks.Count == 1)
				DamageTimer.Start();
		}
	}

	protected void UnDamaging(Node2D damaging)
	{
		if (damaging is DamagingObject target && damaging != myTank)
		{
			enemyTanks.Remove(target);
			if (enemyTanks.Count == 0)
				DamageTimer.Stop();
		}
	}

	void fireDamage()
	{
		foreach (var tanki in enemyTanks)
			tanki.TakeDamage(Damage);
	}
	
	protected virtual void DeferredDie()
	{
		QueueFree(); 
	}

	public override void _Process(double delta)
	{
		area.Position = parent.GlobalPosition;
		area.GlobalRotation = parent.GlobalRotation;
	}
}
