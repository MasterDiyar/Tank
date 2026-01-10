using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace vansur.script;

public partial class UserMove : Node2D
{
	[Export] public Tank myTank;
	[Export] public bool IsJoySitck = false;
	private string[] _negativeAction = ["w", "a", "lsu", "lsl"];
	private string[] _positiveAction = ["s", "d", "lsd", "lsr"];
	private int _i = 0;
	private List<Head> heads = [];

	public override void _Ready()
	{
		_i = (IsJoySitck) ? 2 : 0;
		heads = myTank.GetChildren().OfType<Head>().ToList();
		myTank.headAdded += (newList) => heads = [..newList];
	}
	
	void Movement(float fDelta)
	{
		float forwardInput = Input.GetAxis(_negativeAction[_i], _positiveAction[_i]); 
		float rotationInput = Input.GetAxis(_negativeAction[_i+1], _positiveAction[_i+1]);
		Vector2 inputDirection = new Vector2(rotationInput, forwardInput).Normalized();
		if (inputDirection.Length() > 0) {
			float targetAngle = inputDirection.Angle();
			myTank.GlobalRotation = Mathf.LerpAngle(myTank.GlobalRotation, targetAngle, myTank.RotationSpeed * fDelta);
		}
		Vector2 currentForward = Vector2.FromAngle(myTank.GlobalRotation);

		if (inputDirection.Length() > 0) {
			myTank.Speed = Mathf.MoveToward(myTank.Speed, myTank.MaxSpeed, myTank.Acceleration * fDelta);
			myTank.Velocity = currentForward * myTank.Speed;
		} else {
			myTank.Speed = Mathf.MoveToward(myTank.Speed, 0, myTank.Acceleration * fDelta);
			myTank.Velocity = myTank.Velocity.MoveToward(Vector2.Zero, myTank.Acceleration * fDelta);
		}

		myTank.MoveAndSlide();
	}

	public override void _PhysicsProcess(double delta)
	{
		Movement((float)delta);
		if (IsJoySitck) JoyAttacking((float)delta);
		else Attacking((float)delta);
	}

	void Attacking(float delta)
	{
		var mousePos = GetGlobalMousePosition();
		foreach (var head in heads)
		{
			var targetAngle = (mousePos - head.GlobalPosition).Angle();
			head.GlobalRotation = Mathf.LerpAngle(
				head.GlobalRotation, targetAngle,
				head.RotationSpeed * delta);
			if (Input.IsActionPressed("lm"))
				head.TryAttack();
		}
	}

	void JoyAttacking(float delta)
	{
		Vector2 stickDir = new Vector2(
			Input.GetActionStrength("rsr") - Input.GetActionStrength("rsl"),
			Input.GetActionStrength("rsd") - Input.GetActionStrength("rsu")
		);
		if (stickDir.Length() > 0.2f)
			foreach (var head in heads)
			{
				float targetAngle = stickDir.Angle();
				head.GlobalRotation = Mathf.LerpAngle(
					head.GlobalRotation, 
					targetAngle, 
					head.RotationSpeed * delta
				);
				if (Input.IsActionPressed("r2") )
					head.TryAttack();
			}
	}

	
}
