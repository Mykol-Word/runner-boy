using Sandbox;

public sealed class Obstacle : Component
{
	[Property]
	public Rigidbody rigidbody;

	[Property]
	[Range(0, 1000, 5)]
	public int speed;

	protected override void OnUpdate()
	{
		rigidbody.Velocity = new Vector3(-speed, rigidbody.Velocity.y , rigidbody.Velocity.z);

		//destroy if out of bounds
		if(WorldPosition.x < -500) GameObject.Destroy();
	}
}
