using System.Diagnostics;
using Sandbox;

public sealed class FloorMover : Component
{
	[Property]
	public int speed = 0;


	private Vector3 start_pos;

	protected override void OnUpdate()
	{
		WorldPosition += new Vector3(-speed * Time.Delta, 0 , 0);

		if(LocalPosition.x <= 5000) LocalPosition += new Vector3(5000, 0, 0);
	}
}
