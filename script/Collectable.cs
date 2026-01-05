using Godot;
using System;
namespace vansur.script;

public partial class Collectable : Area2D
{
    [Export] public float
        Speed = 0,
        Damage = 0,
        Heal = 0,
        HealPercent = 0,
        AddHp = 0,
        ExtraArmor = 0,
        Repair = 0,
        RepairPercent = 0,
        ChangeHead = 0;

    [Export] private string[] upgrades= [];
             
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;

        
    }
    void OnBodyEntered(Node2D body)
    {
        if (body is Tank tank)
        {
            tank.MaxSpeed += Speed;
            tank.InitialDamage += Damage;
            tank.Hp += Heal;
            tank.Hp *= (HealPercent / 100f)+1;
            tank.MaxHp += AddHp;
            tank.MaxArmor += ExtraArmor;
            tank.Armor += Repair;
            tank.Armor *= RepairPercent / 100f + 1;

            CallDeferred("queue_free");
        }
    }
}