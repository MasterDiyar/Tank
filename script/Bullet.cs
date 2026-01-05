using Godot;
using System;
namespace vansur.script;
public partial class Bullet : Node2D
{
	[Export]public float Speed = 280;
	[Export]public float Acceleration = 400;
	[Export]public float Damage = 60;
	[Export]public bool spawnBoom = true;
	private AudioStreamPlayer2D AudioStreamPlayer2D;
	public Tank myTank;
	protected Area2D area;
	[Export]protected int piercing = 0;
	public override void _Ready()
	{
		area = GetNode<Area2D>("Area2D");
		area.TopLevel = true;
		area.BodyEntered += Damaging;
		area.GlobalPosition = GlobalPosition;
		GlobalPosition = Vector2.Zero;
		area.GlobalRotation = GlobalRotation;
		Timer timer = GetNode<Timer>("Timer");
		timer.Start();
		timer.Timeout += DieProcess;
	}

	protected virtual void Damaging(Node2D body)
	{
		if (piercing < 0 || body == myTank) return;
		if (body is DamagingObject target)
		{
			piercing--;
			target.TakeDamage(Damage);
			DieProcess();
		}
	}

	protected void DieProcess()
	{
		area.SetDeferred("monitoring", false);
		area.SetDeferred("monitorable", false);
		CallDeferred(MethodName.DeferredDie);
	}

	protected virtual void DeferredDie()
	{
		if (spawnBoom){
			var boomScene = GD.Load<PackedScene>("res://scene/boom.tscn");
			var lapa = boomScene.Instantiate<Boom>();

			lapa.GlobalPosition = area.GlobalPosition;
			lapa.Damage = Damage;

			GetTree().Root.AddChild(lapa);
		}
    
		QueueFree(); 
	}

	public override void _Process(double delta)
	{
		area.Position += (float)delta*Speed*Vector2.FromAngle(area.GlobalRotation);
		Speed += Acceleration*(float)delta;
	}
}
