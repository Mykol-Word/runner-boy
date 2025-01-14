using System;
using Sandbox;

public sealed class GameManager : Component
{
	[Property, Category("Components")] public Controller player;
	[Property, Category("Components")] public ObstacleManager obstacle_manager;
	[Property, Category("Components")] public Score score_panel;

	[Property, Category("Parameters")] public int game_speed = 0;
	[Property, Category("Parameters")] public int max_game_speed = 2000;
	[Property, Category("Parameters")] public int starting_game_speed = 500;
	[Property, Category("Parameters"), Range(0, 100, 1)] public int speed_ramp_up = 10;
	
	private float time_since_start = 0f;
	private double internal_game_speed = 0f;

	protected override void OnStart()
	{
		game_speed = starting_game_speed;
	}
	protected override void OnUpdate()
	{
		obstacle_manager.game_speed = game_speed;

		time_since_start += Time.Delta;
		if(!player.game_over) 
		{
			score_panel.score = (int)time_since_start;
			internal_game_speed = MathX.Clamp(game_speed + speed_ramp_up * Time.Delta , starting_game_speed, max_game_speed);
			game_speed = (int)internal_game_speed;
		}
		else 
		{
			obstacle_manager.make_obstacles = false;
			game_speed = (int)MathX.Lerp(game_speed, 0, 0.5f, true);
		}
		
	}
}
