using Godot;
using System;

public partial class UserUI : CanvasLayer
{
	[Export] private Line2D HpLine;
	[Export] private Line2D ArmorLine;
	[Export]public Tank Tank;
	

	public override void _Ready()
	{
		Tank.HpChanged += (ro, ar) => Chage(HpLine, ArmorLine,ro, ar);
	}

	void Chage(Line2D line, Line2D arLine, float ratio, float armorRatio)
	{
		line.SetPointPosition(1, new Vector2(10+200*ratio,1060));
		arLine.SetPointPosition(1, new Vector2(10+400*armorRatio,1020));
	}
}
