using Godot;
using System;
namespace vansur.script;

public partial class N0Head : Head
{
	private int attackS = 0;
	private bool PereGrev = false;
	[Export] Timer PereGrevTimer;
	public override void _Ready()
	{
		base._Ready();
		PereGrevTimer.Timeout += OStil;
	}

	public override void Attack()
	{
		base.Attack();
		attackS ++;
		PereGrevTimer.Start();
		if (attackS == 30)
		{
			PereGrev = true;
			attackS = 0;
		}
	}

	void OStil()
	{
		PereGrev = false;
		PereGrevTimer.Stop();
	}
	
	public virtual void TryAttack()
	{
		if (!PereGrev)
			base.TryAttack();
	}
}
