using System.Collections.Generic;
using UnityEngine;

public static class CurrentState {
	public static string game_id = null, current_date = null;
	public static string home_team_code, away_team_code;
	public static List<object>[] list_pitch;
	public static List<object>[] list_atbat;
	public static List<object>[] list_h;
	public static List<object>[] list_a;
	public static GameObject[] runner;
	public static float prev_time = 1.0f;
	public static bool ispause = false;
}
