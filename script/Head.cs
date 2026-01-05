using Godot;

namespace vansur.script;

public partial class Head : AnimatedSprite2D
{
    [Export] private PackedScene[] BulletScenes;
    [Export] protected float OffSet = 0;
    [Export] protected float BetweenAngle = 0;
    [Export] protected float SpawnOffset = 0;
    [Export] public float RotationSpeed = 4;
    [Export]public Timer reloadTimer;
    [Export] protected AudioStreamPlayer2D AttackSound;
    [Export] protected float Randomness = 0;
    private Tank parent;
    bool isReloaded = true;

    public override void _Ready()
    {
        parent = GetParent<Tank>();
        reloadTimer.Timeout += Reloading;
    }
    
    void Reloading()
    {
        reloadTimer.Stop();
        isReloaded = true;
    }

    public virtual void Attack()
    {
        for (var i = 0; i < BulletScenes.Length; i++) {
            AttackSound.Play();
            var bullet = BulletScenes[i];
            var bL = bullet.Instantiate<Bullet>();
            bL.GlobalRotation = BetweenAngle * i + OffSet + GlobalRotation + GD.Randf()*Randomness;
            bL.GlobalPosition = GlobalPosition + SpawnOffset*Vector2.FromAngle(GlobalRotation);
            bL.myTank = GetParent<Tank>();
            Spawn(bL);
        }
    }

    protected virtual void Spawn(Node child)
    {
        GetTree().Root.AddChild(child);
    }

    public void TryAttack()
    {
        if (isReloaded)
        {
            isReloaded = false;
            reloadTimer.Start();
            Attack();
        }
    }
}