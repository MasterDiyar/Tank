using Godot;
using System;

public partial class Duels : Node2D
{
	[Export] private Line2D HpLine1, HpLine2;
	[Export] private Line2D ArmorLine1, ArmorLine2;
	public Tank Tank1, Tank2;
	

	public void Init()
	{
		Tank1.HpChanged += (ro, ar) => Chage(HpLine1, ArmorLine1,ro, ar);
        		Tank2.HpChanged += (ro, ar) => Chage(HpLine2, ArmorLine2 ,ro, ar);
	}

	void Chage(Line2D line, Line2D arLine, float ratio, float armorRatio)
	{
		line.SetPointPosition(1, new Vector2(10+100*ratio,30));
		arLine.SetPointPosition(1, new Vector2(10+150*armorRatio,50));
	}
	
}
