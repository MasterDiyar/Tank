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
		switch (attackS)
		{
			case 3:
				reloadTimer.WaitTime = 0.3f;
				Randomness = 0.45f;
				break;
			case 7:
				reloadTimer.WaitTime = 0.2f;
				Randomness = 0.4f;
				break;
			case 15:
				reloadTimer.WaitTime = 0.1f;
				Randomness = 0.2f;
				break;
			case 23:
				reloadTimer.WaitTime = 0.05f;
				Randomness = 0.05f;
				break;
			case 30:
				PereGrev = true;
				attackS = 0;
				reloadTimer.WaitTime = 0.4f;
				Randomness = 0.5f;
				break;
		}
	}

	void OStil()
	{
		PereGrev = false;
		PereGrevTimer.Stop();
	}
	
	public override void TryAttack()
	{
		if (isReloaded && !PereGrev)
		{
			isReloaded = false;
			reloadTimer.Start();
			Attack();
		}
	}
}
