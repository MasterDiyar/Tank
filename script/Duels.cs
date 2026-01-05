using Godot;
using System;

public partial class Duels : Node2D
{
	[Export] private Line2D HpLine1, HpLine2;
	[Export] private Tank Tank1, Tank2;
	public override void _Ready()
	{
		Tank1.HpChanged += (ro) => Chage(HpLine1, ro);
		Tank2.HpChanged += (ro) => Chage(HpLine2, ro);
	}

	void Chage(Line2D line, float ratio)
	{
		line.SetPointPosition(1, new Vector2(10+100*ratio,30));
	}
	
}
