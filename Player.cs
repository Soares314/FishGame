using Godot;
using System;

public partial class Player : Area2D
{
	[Signal]
	public delegate void HitEventHandler();
	[Export]
	public int Speed { get; set; } = 400;
	public Vector2 ScreenSize;

	private void OnBodyEntered(Node2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit);

		GetNode<CollisionShape2D>("CollisionPlayer").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionPlayer").Disabled = false;
	}
	public override void _Ready()
	{
		Hide();
		ScreenSize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimationPlayer");

		velocity.X = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		velocity.Y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");

		if (velocity.X != 0)
		{
			animatedSprite2D.Animation = "walk";
			animatedSprite2D.FlipV = false;

			animatedSprite2D.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0)
		{
			animatedSprite2D.Animation = "up";
			animatedSprite2D.FlipV = velocity.Y > 0;
		}

		if (velocity.Length() > 0)
		{
			animatedSprite2D.Play();
			velocity = velocity.Normalized() * Speed * (float)delta;
		}
		else
		{
			animatedSprite2D.Stop();
		}

		Position = new Vector2(
			Math.Clamp(Position.X + velocity.X, 0, ScreenSize.X),
			Math.Clamp(Position.Y + velocity.Y, 0, ScreenSize.Y)
		);
	}
}
