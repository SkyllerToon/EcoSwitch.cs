using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

public partial class PlayerDeer : CharacterBody2D
{
    // === Constants ===
    private const float DEER_GRAVITY = 12f;
    private const float DEER_SPEED = 100f;
    private const float DEER_JUMP = -160f;
    private const float DEER_SWIM = 50f;
    private const float DEER_SWIM_JUMP = -100f;

    // === Variables ===
    private float speed = DEER_SPEED;
    private float jump = DEER_JUMP;
    private bool jumping = false;
    private bool fall = false;
    private bool sink = false;
    private bool pressed = false;

    // === States ===
    public bool deerattack = false;
    private bool hurted = false;
    private Vector2 knockback = Vector2.Zero;
    private float knockPower = 20f;

    private string state = "Deer";
    private bool wind = false;
    private bool windout = false;
    private bool water = false;
    private bool swim = false;

    // === Colors ===
    private bool color = true;
    private bool colorblue = false;
    private bool colorgreen = false;

    // === Configurations ===
    private ConfigFile config = new ConfigFile();
    private bool lockplayer = false;
    private bool elock = false;
    private bool exit = false;
    private bool enter = false;
    private bool imunity = false;

    // === Nodes ===
    [Export] private Sprite2D sprite;
    [Export] private Area2D hurtbox;
    [Export] private Area2D attackbox;
    [Export] private Timer jumptimer;
    [Export] private Timer exittimer;
    [Export] private AudioStreamPlayer jumpsfx;
    [Export] private AudioStreamPlayer runsfx;
    [Export] private AudioStreamPlayer attacksfx;
    [Export] private AudioStreamPlayer hurtsfx;
    [Export] public AnimationPlayer animation;
    [Export] private AnimationPlayer animation2;

    public override void _Ready()
    {
        Globals.cloud = false;
        EnterScene();
        Globals.animal = 1;
        Globals.player = this;
        Globals.exit = false;
        Visible = true;
        elock = false;
        lockplayer = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Globals.totem)
            imunity = true;
        else if (enter)
            imunity = true;
        else if (exit)
            imunity = true;
        else
            imunity = false;
        
        if (imunity)
            ImunityOn();
        else
            ImunityOff();

        if (exit == true)
        {
            speed = -100;
        }

        if (Globals.exit)
        {
            exit = true;
            exittimer.Start();
            Globals.exit = false;
        }

        if (Globals.lockFlag)
            lockplayer = true;
        else if (!elock)
            lockplayer = false;       

        ApplyGravity();
        if (!deerattack)
            SetState();
        RunState();
        Winstate();
        SwimState();
        jumpState();
        if (Input.IsActionJustPressed("ui_down") && Globals.animal == 1 && !deerattack && !lockplayer && IsOnFloor() && !Globals.Sswitch)
		{
			elock = true;
			_ = StartDeerAttack();
		}
        HandleKnockback();
        UpdateColor();
        MoveAndSlide();
    }

    private void ApplyGravity()
    {
        if (!IsOnFloor())
        {
            Velocity += new Vector2(0, DEER_GRAVITY);
        }
        else
        {
            fall = false;
            jumping = false;
        }

        if (!IsOnFloor() && !jumping)
            fall = true;
    }

    private async void EnterScene()
    {
        if (!Globals.totem)
        {
            enter = true;
            await ToSignal(GetTree().CreateTimer(1.5f), "timeout");
            enter = false;
        }
    }

    private void ImunityOn()
    {
        hurtbox.SetCollisionMaskValue(3, false);
        if (!fall)
            SetCollisionMaskValue(3, false);
        else
            SetCollisionMaskValue(3, true);
        if (!animation2.IsPlaying())
            animation2.Play("Shine");  
    }

    private void ImunityOff()
    {
        hurtbox.SetCollisionMaskValue(3, true);
        SetCollisionMaskValue(3, true);
        if (animation2.IsPlaying())
            animation2.Stop();
    }

    private void RunState()
    {
        if (!lockplayer)
            Velocity = new Vector2(speed, Velocity.Y);
        else
            Velocity = new Vector2(0, Velocity.Y);
    }

    private void HandleKnockback()
    {
        if (knockback.Length() > 0 && !lockplayer)
        {
            Velocity = knockback;
        }
    }

    private void UpdateColor()
    {
        if (hurted)
            sprite.Modulate = new Color(1, 0, 0);
        else
            SetColor();
    }

    private void SetColor()
    {
        if (colorblue) sprite.Modulate = new Color(0, 1, 1);
        else if (colorgreen) sprite.Modulate = new Color(0, 1, 0);
        else if (color) sprite.Modulate = new Color(1, 1, 1);
        else sprite.Modulate = new Color(1, 0, 1);
    }

    private void SetSound()
    {
        if (!Globals.sound)
        {
            SoundOff();
            return;
        }
    
        if (!runsfx.Playing)
            runsfx.Play();
    }

    private void SoundOff()
    {
        if (runsfx.Playing)
            runsfx.Stop();
        if (jumpsfx.Playing)
            jumpsfx.Stop();
    }

    private async Task StartDeerAttack()
    {
        if (Globals.sound)
            attacksfx.Play();
        attackbox.SetCollisionLayerValue(1, true);
        deerattack = true;
        state = "Deer_attack";

        float dashSpeed = 200f;
        Velocity = new Vector2(dashSpeed * MathF.Sign(sprite.Scale.X), Velocity.Y);

        float duration = 0.25f;
        Tween t = GetTree().CreateTween();
        t.TweenProperty(this, "knockback", Vector2.Zero, duration)
        .SetDelay(0.1f)
        .SetTrans(Tween.TransitionType.Linear)
        .SetEase(Tween.EaseType.InOut);

        animation.Play(state);
        await ToSignal(animation, "animation_finished");

        deerattack = false;
        elock = false;
        attackbox.SetCollisionLayerValue(1, false);
    }

    private void SetState()
    {
        if (IsOnFloor() && Velocity.X == 0)
        {
            SoundOff();
            state = "Deer";
        }
        else if (IsOnFloor() && Velocity.X != 0 && !lockplayer)
        {
            SetSound();
            state = "Deer_run";
        }
        else if (!IsOnFloor() && swim)
        {
            SoundOff();
            state = "Deer_run";
        }
        else if (fall)
            state = "Deer_jump2";
        else if (!IsOnFloor())
        {
            SoundOff();
            state = "Deer_jump";
        }

        if (animation.CurrentAnimation != state)
            animation.Play(state);
    }

    private void jumpState()
    {
        if (Input.IsActionPressed("ui_up") && !lockplayer && !Globals.Sswitch && !exit)
		{
            if (!pressed && !fall)
            {
                if (Globals.sound)
                    jumpsfx.Play();
                Jump();								
			}
		}
		else
		{
			pressed = false;
			jumping = false;

			if (!jumptimer.IsStopped())
				jumptimer.Stop();
		}
	}

    private void Jump()
    {
        if (!fall)
			jumping = true;
		if (jumping)
			Velocity = new Vector2(Velocity.X, jump);
			if (IsOnFloor())
				jumptimer.Start();
    }

    private void On_Jumptimer_Timeout()
    {
        jumping = false;
        fall = true;
        pressed = true;
    }

    private void On_ExitTimer_Timeout()
    {
        Globals.released = false;
        QueueFree();
    }

    private void OnHurtboxBodyEntered(Node body)
    {
        if (body is Node2D node2D && !exit && !imunity)
        {
            Vector2 kb = new Vector2((GlobalPosition.X - node2D.GlobalPosition.X) * knockPower, -200);
            HandleDeathOrDamage(kb);
        }
    }

    private void OnHurtboxAreaEntered(Area2D area)
    {
        if (area is Area2D area2D && !exit && !imunity)
        {
            Vector2 kb = new Vector2(0, jump * 2);
            HandleDeathOrDamage(kb);
        }
    }

    private async void HandleDeathOrDamage(Vector2 kb)
    {
        if (Globals.life < 1)
        {
            Globals.lockFlag = true;
            elock = true;
            lockplayer = true;

            TakeDamage(kb);
            await ToSignal(GetTree().CreateTimer(0.3f), "timeout");

            Visible = false;
            Globals.control = false;
        }
        else if (!lockplayer)
        {
            TakeDamage(kb);
        }
    }

    private void TakeDamage(Vector2 knockbackForce, float duration = 0.25f)
    {
        Globals.life -= 1;
        if (knockbackForce != Vector2.Zero)
        {
            if (Globals.sound) hurtsfx.Play();
            knockback = knockbackForce;

            Tween t = GetTree().CreateTween();
            t.SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
            t.Parallel().TweenProperty(this, "knockback", Vector2.Zero, duration).SetDelay(0.1f);
        }

        hurted = true;
        _ = ResetHurtState();
    }

    private async Task ResetHurtState()
    {
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        hurted = false;
    }

    private async void Winstate()
    {
        if (wind && !lockplayer)
            Velocity = new Vector2(Velocity.X, jump * (Input.IsActionPressed("ui_up") ? 2f : 1f));
        else if (windout)
        {
            Velocity = new Vector2(Velocity.X, jump);
            await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
            windout = false;
        }
    }

    private void SwimState()
    {
        if (water)
        {
            swim = true;
            color = false;
            colorgreen = false;
            colorblue = true;
        }
        else
        {
            sink = false;
            swim = false;
            colorblue = false;
            if (!colorgreen)
                color = true;
        }

        if (swim)
        {
            jump = DEER_SWIM_JUMP;
            speed = DEER_SWIM;
        }
        else
        {
            jump = DEER_JUMP;
            speed = DEER_SPEED;
        }
    }
    
    public Dictionary<string, Variant> Save()
	{
		return new Dictionary<string, Variant>
		{
			{"position", Position},
			{"rotation", Rotation},
			{"active", Visible}
		};
	}

	public void Carregar(Dictionary<string, Variant> dados)
	{
		Position = dados["position"].AsVector2();
		Rotation = dados["rotation"].AsSingle();
		Visible = dados["active"].AsBool();

		if (!Visible) QueueFree();
	}
}