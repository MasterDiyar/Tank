using Godot;
using Godot.Collections;

namespace vansur.script.Singles;

public partial class StatInfo : Node
{
    public static readonly Dictionary<int, PackedScene> HeadScenes = new()
    {
        { 0, GD.Load<PackedScene>("res://scene/heads/0h_head.tscn") }, //heavy
        { 1, GD.Load<PackedScene>("res://scene/heads/0f_head.tscn") }, //fire
        { 2, GD.Load<PackedScene>("res://scene/heads/0l_head.tscn") }, //little
        { 3, GD.Load<PackedScene>("res://scene/heads/0t_head.tscn") }, //triple
        { 4, GD.Load<PackedScene>("res://scene/heads/0n_head.tscn") }, //negev
    };
}