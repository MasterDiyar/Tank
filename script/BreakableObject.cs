using Godot;
using System;
using vansur.script;

public partial class BreakableObject : StaticBody2D, DamagingObject
{
	public float Hp { get; set; } = 1000;
	[Export] float MaxHp { get; set; } = 1000;
	[Export] PackedScene smokeParticles;
	[Export] Texture2D ruin_texture;
	[Export] Vector2 SpawnOffset = new Vector2(0, 0); 
	[Export] Vector2 SpawnSize = new Vector2(1, 1);
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
		var smk = smokeParticles.Instantiate<SmokeParticles>();
		smk.GlobalPosition = GlobalPosition + SpawnOffset;
		smk.Emitting = true;
		smk.sprite.Texture = ruin_texture;
		smk.sprite.Position = -SpawnOffset;
		smk.sprite.Scale = SpawnSize;
		GetParent().CallDeferred("add_child", smk);
		QueueFree();
	}
}
