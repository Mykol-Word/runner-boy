using System;
using Sandbox;

public sealed class ObstacleManager : Component
{
	//Obstacles
	[Property]
	[Category("Objects")]
	public List<GameObject> obstacle_list;
	[Property]

	[Category("Objects")]
	public GameObject sign;

	[Property]
	[Category("Objects")]
	public FloorMover floor_mover;

	// Spawning Properties
	[Property]
	[Category("Spawning")]
	[Description("Minimum obstacle spawn delay (seconds)")]
	public float spawn_delay_min;

	[Property]
	[Category("Spawning")]
	[Description("Maximum obstacle spawn delay (seconds)")]
	public float spawn_delay_max;

	[Property]
	[Category("Spawning")]
	[Description("Distance to the left and right obstacles can spawn")]
	public int spawn_range = 400;

	[Property]
	[Category("Spawning")]
	[Description("How far away obstacles spawn")]
	public int spawn_distance = 8000;

	[Property]
	[Category("Spawning")]
	[Description("How long before a train can spawn again")]
	public float train_spawn_time_cutoff = 5f;

	[Property]
	[Category("Spawning")]
	[Description("How long before a overhead sign can spawn again")]
	public float sign_spawn_time_cutoff = 5f;

	[Property]
	[Category("Settings")]
	[Range(0, 1000, 5)]
	[Description("How fast danger comes at you")]
	public int game_speed;

	private float time_since_spawn = 0f;
	private float time_since_train = 0f;
	private float time_since_sign = 0f;
	private RangedFloat spawn_time_cutoff = new RangedFloat();
	private RangedFloat spawn_location = new RangedFloat();

	protected override void OnUpdate()
	{
		//Set floor and movement speed
		floor_mover.speed = game_speed;

		time_since_spawn += Time.Delta; time_since_train += Time.Delta; time_since_sign += Time.Delta;
		
		if(time_since_spawn > spawn_time_cutoff.GetValue())
		{
			int spawn_index = new Random().Next(0, obstacle_list.Count); // random int [0, # of obstacles]
			SpawnObject(obstacle_list[spawn_index]);
			time_since_spawn = 0;
		}
		if(time_since_sign > sign_spawn_time_cutoff && new Random().Next(1,500) == 1 ) // lazy variation 
		{
			time_since_sign = 0f;
			SpawnObject(sign, false);
		}
	}

	protected override void OnStart()
	{
		//Initialized random float ranges
		spawn_time_cutoff.Min = spawn_delay_min;
		spawn_time_cutoff.Max = spawn_delay_max;

		spawn_location.Min = -spawn_range;
		spawn_location.Max = spawn_range;
	}

	//Spawns new GameObject g randomly on the road
	private void SpawnObject(GameObject g, bool random=true)
	{
		if(g.Name == "Tanker")
		{
			if(time_since_train < train_spawn_time_cutoff) return;
			time_since_train = 0f;
		}

		GameObject new_gameObject = g.Clone();

		//Set pos and random rotation
		if(random)
		{
			Angles new_angle = new Angles();
			new_angle.yaw = new Random().Next(-15, 15);
			new_gameObject.LocalRotation = new_angle.ToRotation();
			new_gameObject.WorldPosition = new Vector3(spawn_distance, spawn_location.GetValue(), 0);
		}
		else new_gameObject.WorldPosition = new Vector3(spawn_distance, 0, 70);

		//Set new obstacle's speed
		Obstacle new_obstacle = new_gameObject.GetComponent<Obstacle>();
		new_obstacle.speed = game_speed;
	}
}
