using Godot;
using System;

namespace vansur.script;

public partial class Fpv : Tank
{
    public override void _Ready()
    {
        base._Ready();
        AnimatedSprite2D a = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        a.Play();
    }
}