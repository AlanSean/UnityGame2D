using UnityEngine;
using System.Collections;

public class _game_options : MonoBehaviour {
	[Header("Cofiguration")]
	public float _time_to_reset_ball = 2f;
	public float _score_on_dunk = 250f;
	[HideInInspector]
	public int _level_p = 0;
	[HideInInspector]
	public int _difficulty_l = 0;
	public bool _touch_mode = false;
	//----------------------------------------------


}
