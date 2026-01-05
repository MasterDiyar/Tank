using Godot;
using System;

namespace vansur.script;

public partial class LineBullet : Bullet
{
	Line2D line2D;
	public override void _Ready()
	{
		base._Ready();
		line2D = GetNode<Line2D>("Line2D");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		Vector2 localPos = line2D.ToLocal(area.GlobalPosition);
    
		line2D.AddPoint(localPos, 0);
		if (line2D.GetPointCount() > 30)
			line2D.RemovePoint(line2D.GetPointCount() - 1);
	}
	
	protected override void DeferredDie()
	{
		var boomScene = GD.Load<PackedScene>("res://scene/boom.tscn");
		var lapa = boomScene.Instantiate<Boom>();
    
		lapa.GlobalPosition = area.GlobalPosition; 
		lapa.Damage = Damage;
    
		GetTree().Root.AddChild(lapa);
    
		QueueFree(); 
	}
}
