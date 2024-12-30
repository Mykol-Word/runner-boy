using Sandbox;

public sealed class FloorMover : Component
{
	[Property]
	public int speed = 0;

	[Property]
	public Rigidbody rigidbody;

	private Vector3 start_pos;

	protected override void OnStart()
	{
		start_pos = new Vector3(10000, 0, 0);
	}
	protected override void OnUpdate()
	{
		rigidbody.Velocity = new Vector3(-speed, 0, 0);

		if(start_pos.x - LocalPosition.x >= 10000) LocalPosition = start_pos;
	}
}
