using Godot;
using System;

namespace vansur.script;

public partial class APlusbullet : LineBullet
{
	[Export] private int PebbleCount = 5;
	[Export] private float Spread = 0.3f;

	protected override void DeferredDie()
	{
		var af = -1;
		for (int i = 0; i < PebbleCount; i++){
			var boomScene = GD.Load<PackedScene>("res://scene/pebble.tscn");
			var lapa = boomScene.Instantiate<Bullet>();

			lapa.GlobalPosition = area.GlobalPosition;
			lapa.Damage = Damage;
			lapa.myTank = myTank;
			
			lapa.Rotation = Rotation - af * GD.Randf() * Spread;
			af *= -1;
			GetTree().Root.CallDeferred("add_child", lapa);
		}
    
		QueueFree(); 
	}
}
