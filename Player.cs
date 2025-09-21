using Godot;
using System;

public partial class Player : Node2D
{
	private bool spawnposition = false;
	public int animal = Globals.animal;
	public Vector2 posit = new Vector2(0, 300);
	[Export] public PackedScene Deer;
	[Export] public PackedScene Squirrel;
	[Export] public PackedScene Bird;
	[Export] public PackedScene Frog;
	[Export] public Camera2D Camera;
	[Export] private AudioStreamPlayer totemsfx;

	private CharacterBody2D currentAnimal;

	public override void _Ready()
	{
		Sswitch();
		Globals.released = false;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Globals.sound && totemsfx != null)
		{
			if (Globals.totem && !totemsfx.Playing)
				totemsfx.Play();
			else if (!Globals.totem && totemsfx.Playing)
				totemsfx.Stop();
		}

		animal = Globals.animal;

		if (currentAnimal != null)
		{
			GlobalPosition = new Vector2(currentAnimal.GlobalPosition.X, 260);
			posit = currentAnimal.GlobalPosition;
		}

		if (Globals.transform)
		{
			Globals.transform = false;
			if (currentAnimal.IsOnFloor())
				spawnposition = true;
			else
				spawnposition = false;
			Sswitch();
		}
	}

	private void SpawnAnimal(PackedScene animalScene)
	{
		if (currentAnimal != null)
			Globals.exit = true;
		Globals.released = true;

		currentAnimal = animalScene.Instantiate<CharacterBody2D>();
		Vector2 startPos = new Vector2(posit.X - 200, posit.Y - 120);
		currentAnimal.GlobalPosition = startPos;
		GetParent().CallDeferred("add_child", currentAnimal);

		if (spawnposition)
		{
			var tween = currentAnimal.CreateTween();
			tween.TweenProperty(currentAnimal, "global_position", posit + new Vector2(0, -40), 1)
				.SetEase(Tween.EaseType.In)
				.SetTrans(Tween.TransitionType.Quad);
		}
		else
		{
			var tween = currentAnimal.CreateTween();
			tween.TweenProperty(currentAnimal, "global_position", posit, 1)
				.SetEase(Tween.EaseType.In)
				.SetTrans(Tween.TransitionType.Quad);
		}
	}

	private void Sswitch()
	{
		switch (animal)
		{
			case 1:
				SpawnAnimal(Deer);
				break;
			case 2:
				SpawnAnimal(Squirrel);
				break;
			case 3:
				SpawnAnimal(Bird);
				break;
			case 4:
				SpawnAnimal(Frog);
				break;
		}
	}
}
