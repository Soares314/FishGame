using Godot;
using System;

public partial class Sprite2d : Sprite2D
{
	private int _velocidade = 400;
	private float _angulo = Mathf.Pi;
	public Sprite2d()
	{
		GD.Print("Hello, world!");
	}

	public override void _Ready()
	{
		var timer = GetNode<Timer>("Timer");
		GD.Print($"Timer: {timer}");
	}
	public override void _Process(double delta)
	{
		Rotation += _angulo * (float)delta;
		var cord_x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		var cord_y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		if (cord_x != 0 && cord_y != 0)
		{
			cord_x = cord_x * 0.7071f;
			cord_y = cord_y * 0.7071f;
		}
		var vel_x = cord_x * (float)delta * _velocidade;
		var vel_y = cord_y * (float)delta * _velocidade;

		Position += new Vector2(vel_x, vel_y);
	}
}
