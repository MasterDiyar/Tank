using Godot;
using System;

namespace vansur.script;

public partial class OfHead : Head
{

	protected override void Spawn(Node child)
	{
		AddChild(child); 
	}
}
