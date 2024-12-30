using System;
using System.Numerics;
using Sandbox;

public sealed class Controller : Component
{
	//Movement Properties
	[Property]
	[Category("Movement")]
	[Range(0f,1000f,5f)]
	public int move_speed = 10;

	[Property]
	[Category("Movement")]
	[Range(0f,100f,5f)]
	public int jump_force = 10;

	//Sound Properties
	[Property]
	[Category("Sound")]
	public SoundEvent footstep;

	[Property]
	[Category("Sound")]
	public float footstep_volume = 0.5f;

	//Component Properties
	[Property]
	[Category("Components")]
	private Rigidbody rigidbody;

	[Property]
	[Category("Components")]
	SkinnedModelRenderer model_renderer;

	[Property]
	[Category("Components")]
	Prop ragdoll;

	//Private helpers
	private int left_right_boundaries = 300;
	private int grounded = 0;
	private int grounded_threshold = 10;
	private Vector3 facing_vector = Vector3.Zero;
	private bool game_over = false;

	protected override void OnStart()
	{
		model_renderer.OnFootstepEvent = x => { Sound.Play(footstep);};
		footstep.Volume = footstep_volume;
	}
	protected override void OnUpdate()
	{	
		if(game_over) return;
		//Collider Checks
		bool temp_grounded = false;
		foreach (Sandbox.Collider c in rigidbody.Touching)
		{
			if(c.GameObject.Name == "Floor_Buffer") temp_grounded = true;
			else if(c.GameObject.Tags.Has("obstacle")) { Die(); return; }
		}

		//Left-Right Movement
		int move_right = Input.Down("right") ? 1 : 0, move_left = Input.Down("left") ? 1 : 0;
		Vector3 movement_vector = move_right * Vector3.Right - move_left * Vector3.Right;

		rigidbody.Velocity = movement_vector * move_speed + new Vector3(0, 0 , rigidbody.Velocity.z);

		Vector3 restoring_force = new Vector3(0, - MathF.Sign(WorldPosition.y) * (MathF.Abs(WorldPosition.y) - left_right_boundaries) * 100, 0); // proportional to distance out-of-bounds
		if(MathF.Abs(WorldPosition.y) > left_right_boundaries) rigidbody.ApplyForce(restoring_force); // if out of bounds, restore

		//Jumping
		if(temp_grounded) grounded += 1;

		if(Input.Pressed("jump") && grounded > 10)
		{
			grounded = 0;
			rigidbody.Velocity = rigidbody.Velocity - new Vector3(0,0,rigidbody.Velocity.z);
			rigidbody.ApplyImpulse(new Vector3(0, 0 , jump_force * 5));
			PlayJumpAnimation();
		}

		//Update Animations
		AnimationHandler();
	}

	//Handles movement animations
	private void AnimationHandler()
	{
		if(grounded < grounded_threshold) model_renderer.Set("b_grounded", false);
		else model_renderer.Set("b_grounded", true);

		if(grounded > grounded_threshold) 
		{
			model_renderer.Set("move_x", 1000);
			footstep.Volume = footstep_volume;
		}
		else 
		{
			model_renderer.Set("move_x", 0);
			model_renderer.Set("move_y", 0);
			footstep.Volume = 0;
			return;
		}

		facing_vector = Vector3.Lerp(facing_vector, rigidbody.Velocity, 0.02f, true);
		facing_vector.z = 0;

		model_renderer.Set("move_y", -facing_vector.y * 5);
		model_renderer.Set("aim_head", facing_vector + new Vector3(100, 0, 0));
	}

	//Lol why not
	private void PlayJumpAnimation()
	{
		model_renderer.Set("b_jump", true);
	}

	private void Die()
	{
		game_over = true;
		ModelPhysics ragdoll = AddComponent<ModelPhysics>();
		ragdoll.Renderer = model_renderer;
		ragdoll.Model = model_renderer.Model;
		ragdoll.PhysicsGroup.ApplyImpulse(new Vector3(-5000, 0, 0));

		//Just in case
		model_renderer.Set("move_x", 0);
		model_renderer.Set("move_y", 0);
		footstep.Volume = 0;
	}
}
