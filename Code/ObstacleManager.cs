using Sandbox;

public sealed class ObstacleManager : Component
{
	//Obstacles
	[Property]
	[Category("Obstacles")]
	public List<GameObject> obstacle_list;

	// Spawning Properties
	[Property]
	[Category("Spawning")]
	public float spawn_delay_min;

	[Property]
	[Category("Spawning")]
	public float spawn_delay_max;

	[Property]
	[Category("Spawning")]
	public float spawn_range;

	[Property]
	[Category("Settings")]
	[Range(0, 1000, 5)]
	public int obstacle_speed;

	private float time_since_spawn = 0f;
	private RangedFloat spawn_time_cutoff = new RangedFloat();
	private RangedFloat spawn_location = new RangedFloat();

	protected override void OnUpdate()
	{
		time_since_spawn += Time.Delta;
		if(time_since_spawn > spawn_time_cutoff.GetValue())
		{
			SpawnObject(obstacle_list[0]);
			time_since_spawn = 0;
		}
	}

	protected override void OnStart()
	{
		spawn_time_cutoff.Min = spawn_delay_min;
		spawn_time_cutoff.Max = spawn_delay_max;

		spawn_location.Min = -spawn_range;
		spawn_location.Max = spawn_range;
	}

	private void SpawnObject(GameObject g)
	{
		GameObject new_gameObject = g.Clone();
		new_gameObject.WorldPosition = new Vector3(8000, spawn_location.GetValue(), 0);
	}
}
