using Sandbox;

public sealed class Obstacle : Component
{
	[Property]
	public int speed = 0;

	[Property]
	public bool live = true;

	protected override void OnUpdate()
	{
		WorldPosition += new Vector3(-speed * Time.Delta, 0 , 0);

		//destroy if out of bounds
		if(WorldPosition.x < -500) live = false;
	}
}
