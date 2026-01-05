using Godot;
using System;
using vansur.script;

public partial class Tank : CharacterBody2D, DamagingObject
{
	[Export] public float Speed = 100;
	[Export] public float MaxSpeed = 160;
	[Export] public float Acceleration = 100;
	[Export] public float MaxHp = 100;
	[Export] public float RotationSpeed = 100;
	[Export] public PackedScene BulletPrefab;
	[Export] public Sprite2D Body;
	[Export] public float MaxArmor = 300;
	public float MinSpeed = 10;
	public float Hp { get; set; } = 100;
	public float Armor = 100;
	public Action<float> HpChanged;
	

	public bool isReloaded = true;
	[Export]public bool isJoyStick = false;
	
	public override void _Ready()
	{
		MinSpeed = Speed;
		Hp = MaxHp;
		Armor = MaxArmor;
	}

	public void TakeDamage(float damage)
	{
		if (Armor > 0 ) Armor -= damage;
		else Hp -= Mathf.Clamp(damage, 0, Hp);
		if (Hp <= 0) CallDeferred(MethodName.DefferedDeath);
		GD.Print("Hp: " + Hp, " Armor: "+ Armor);
		HpChanged?.Invoke(Hp/MaxHp);
	}

	public virtual void DefferedDeath()
	{
		QueueFree();
	}
	
	
}
