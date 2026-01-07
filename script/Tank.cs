using Godot;
using System;
using System.Collections.Generic;
using vansur.script;

public partial class Tank : CharacterBody2D, DamagingObject
{
	[Export] public int Team = 0;
	[Export] public float Speed = 100;
	[Export] public float MaxSpeed = 160;
	[Export] public float Acceleration = 100;
	[Export] public float MaxHp = 100;
	[Export] public float RotationSpeed = 100;
	[Export] public Sprite2D Body;
	[Export] public float MaxArmor = 300;
	[Export] public float InitialDamage = 1;
	[Export] public PackedScene[] HeadScene = [];
	[Export]public bool isJoyStick = false;
	public float MinSpeed = 10;
	public float Hp { get; set; } = 100;
	public float Armor = 100;
	public Action<float, float> HpChanged;
	public Action<List<Head>> headAdded;
	public bool isReloaded = true;
                                
	private List<Head> _activeHeads = [];

	private readonly Dictionary<int, Vector2[]> _headPositions = new()
	{
		{ 1, [Vector2.Zero] },
		{ 2, [Vector2.Up*35, Vector2.Down*35] },
		{ 3, [Vector2.Right*40, new Vector2(-35, -20), new Vector2(-35, 20) ] },
		{ 4, [new Vector2(35, 35), new Vector2(-35, 35), new Vector2(35, -35), new Vector2(-35, -35)] }
	};
	
	public void UpdateHeads(int count)
	{
		foreach (var child in GetChildren()) if(child is Head) child.QueueFree();
		_activeHeads.Clear();
		if (count ==0) return;
		if (!_headPositions.TryGetValue(count, out var positions)) return;
		for (var i = 0; i < positions.Length; i++)
		{
			GD.Print(positions.Length, " ", count);
			var pos = positions[i];
			var head = HeadScene[i].Instantiate<Head>();
			head.ZIndex = 12;
			AddChild(head);
			head.Position = pos;//3;
			_activeHeads.Add(head);
		}
		headAdded?.Invoke(_activeHeads);
	}
	
	public override void _Ready()
	{
		MinSpeed = Speed;
		Hp = MaxHp;
		Armor = MaxArmor;
		UpdateHeads(HeadScene.Length);
	}

	public void TakeDamage(float damage)
	{
		if (Armor > 0 ) Armor -= damage;
		else Hp -= Mathf.Clamp(damage, 0, Hp);
		if (Hp <= 0) CallDeferred(MethodName.DefferedDeath);
		GD.Print("Hp: " + Hp, " Armor: "+ Armor);
		HpChanged?.Invoke(Hp/MaxHp, Armor/MaxArmor);
	}

	public virtual void DefferedDeath()
	{
		QueueFree();
	}
	
	
}
