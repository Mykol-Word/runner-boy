using Sandbox;

public sealed class Obstacle : Component
{
	[Property]
	public Rigidbody rigidbody;

	[Property]
	public int speed = 0;

	protected override void OnUpdate()
	{
		
		rigidbody.Velocity = new Vector3(-speed, 0, 0);

		//destroy if out of bounds
		if(WorldPosition.x < -500) GameObject.Destroy();
	}
}
