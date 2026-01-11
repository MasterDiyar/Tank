using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using vansur.script;

public partial class AiBehavior : Node2D
{
	/// <summary>
	/// radar
	///
	/// if unit find someone
	/// na kogo napodet vazhnost
	///	Player
	/// Player core
	/// Player other building
	/// Different Fraction
	/// Different Fraction buildings
	///
	/// </summary>
    [Export] public int Difficulty = 0;
    private float[] SearchDistance = [300, 600, 1000, 2000];

    public enum BehaviorType { Attack, Move, RandomMove, Idle }
    private BehaviorType _currentBehavior = BehaviorType.Idle;

    private Vector2 _targetDirection = Vector2.Zero;
    private Tank _myTank, _closestTank;
    private List<Head> _heads = [];

    public override void _Ready()
    {
        _myTank = GetParent<Tank>(); 
        
        var timer = new Timer() {
            WaitTime = 1.2,
            Autostart = true
        };
        timer.Timeout += AiChoosing;
        AddChild(timer);

        _myTank.headAdded += list => _heads = [.. list];
    }

    private void AiChoosing()
    {
        var sqrd = SearchDistance[Difficulty] * SearchDistance[Difficulty];
        var allTanks = GetTree().GetNodesInGroup("tanks")
            .OfType<Tank>()
            .Where(t => IsInstanceValid(t) && t != _myTank && t.Team != _myTank.Team)
            .ToList();
        
        _closestTank = allTanks
            .OrderBy(t => t.GlobalPosition.DistanceSquaredTo(_myTank.GlobalPosition))
            .FirstOrDefault();
        
        if (_closestTank != null && _myTank.GlobalPosition.DistanceSquaredTo(_closestTank.GlobalPosition) > sqrd)
            _closestTank = null;

        int rng = GD.RandRange(0, 3);
        _currentBehavior = (BehaviorType)rng;

        if (_currentBehavior == BehaviorType.RandomMove)
            _targetDirection = new Vector2(GD.Randf() * 2 - 1, GD.Randf() * 2 - 1).Normalized();
        
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsInstanceValid(_myTank)) return;

        UpdateBehaviorLogic((float)delta);
        ApplyMovement((float)delta);
    }

    private void UpdateBehaviorLogic(float delta)
    {
        bool hasTarget = IsInstanceValid(_closestTank);

        switch (_currentBehavior)
        {
            case BehaviorType.Attack:
                if (hasTarget)
                {
                    _targetDirection = (_closestTank.GlobalPosition - _myTank.GlobalPosition).Normalized();
                    HandleShooting(delta);
                }
                else _currentBehavior = BehaviorType.RandomMove;
                break;

            case BehaviorType.Move:
                if (hasTarget)
                    _targetDirection = (_closestTank.GlobalPosition - _myTank.GlobalPosition).Normalized();
                else 
                    _currentBehavior = BehaviorType.Idle;
                break;

            case BehaviorType.Idle:
                _targetDirection = Vector2.Zero;
                break;
                
        }
    }

    private void HandleShooting(float delta)
    {
        if (!IsInstanceValid(_closestTank)) return;
        float distSq = _myTank.GlobalPosition.DistanceSquaredTo(_closestTank.GlobalPosition);

        foreach (var head in _heads) {
            var targetAngle = (_closestTank.GlobalPosition - head.GlobalPosition).Angle();
            head.GlobalRotation = Mathf.LerpAngle(head.GlobalRotation, targetAngle, head.RotationSpeed * delta);
            if (head.MaxDist*head.MaxDist >= distSq) 
                head.TryAttack();
        }
    }

    private void ApplyMovement(float delta)
    {
        if (_targetDirection.Length() > 0) {
            float targetAngle = _targetDirection.Angle();
            _myTank.GlobalRotation = Mathf.LerpAngle(_myTank.GlobalRotation, targetAngle, _myTank.RotationSpeed * delta);
            
            _myTank.Speed = Mathf.MoveToward(_myTank.Speed, _myTank.MaxSpeed, _myTank.Acceleration * delta);
        }else
            _myTank.Speed = Mathf.MoveToward(_myTank.Speed, 0, _myTank.Acceleration * delta);

        Vector2 currentForward = Vector2.FromAngle(_myTank.GlobalRotation);
        if (_targetDirection.Length() > 0)
            _myTank.Velocity = currentForward * _myTank.Speed;
        else
            _myTank.Velocity = _myTank.Velocity.MoveToward(Vector2.Zero, _myTank.Acceleration * delta);

        _myTank.MoveAndSlide();
    }
}
