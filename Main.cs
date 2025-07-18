using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }
	private int _score;

	private void OnTimerScoreTimeout()
	{
		_score++;
		GetNode<Hud>("HUD").UpdateScore(_score);
	}

	private void OnTimerStartTimeout()
	{
		GetNode<Timer>("TimerMob").Start();
		GetNode<Timer>("TimerScore").Start();
	}

	private void OnTimerMobTimeout()
	{
		Mob mob = MobScene.Instantiate<Mob>();

		var spawnMobPoint = GetNode<PathFollow2D>("PathMob/SpawnMobPoint");
		spawnMobPoint.ProgressRatio = GD.Randf();

		float direction = spawnMobPoint.Rotation - Mathf.Pi / 2;

		mob.Position = spawnMobPoint.Position;
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;

		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);

		AddChild(mob);
	}
	public void GameOver()
	{
		GetNode<Timer>("TimerMob").Stop();
		GetNode<Timer>("TimerScore").Stop();
		GetNode<Hud>("HUD").ShowGameOver();

		GetNode<AudioStreamPlayer>("ThemeMusic").Stop();
		GetNode<AudioStreamPlayer>("GameOverSound").Play();

	}

	public void GameStart()
	{
		GetNode<AudioStreamPlayer>("ThemeMusic").Play();
		
		_score = 0;
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");

		player.Start(startPosition.Position);
		GetNode<Timer>("TimerStart").Start();

		var hud = GetNode<Hud>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
	}
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}
