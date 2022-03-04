using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class _Game_Control : MonoBehaviour {
	private static _Game_Control _instance;
	public static _Game_Control instance{get{if (!_instance){_instance = GameObject.FindObjectOfType(typeof(_Game_Control)) as _Game_Control;}return _instance;}}
	//---------------------------------------
	[HideInInspector]
	public int _time_attack_time = 0;
	[HideInInspector]
	public Image[] _hud_attemps = new Image[2];
	int _attemps = 0;
	[HideInInspector]
	public bool _is_gameover = false;
	[HideInInspector]
	public int _money = 0;
	public int _basket_money = 25;
	//---------------------------------------
	public Vector3 _power_click_on_touch_mode = new Vector3(1,2,0);
	//---------------------------------------
	// Achievements
	int _total_matches = 0;
	int _total_baskets = 0;
	int _total_money = 0;
	int _total_score = 0;
	[HideInInspector]
	int _matches = 0;
	[HideInInspector]
	public int _baskets = 0;
	//---------------------------------------
	// Sprites to gameplay
	//---------------------------------------
	[Header("Default Design")]
	public Material _ball_material; // Default Ball
	public Sprite _sprt_trajectory; // Default Trajectory Poing
	public Sprite _sprt_player; // Default Player
	//---------------------------------------
	void Awake(){
		//---------------------------------------
		_total_matches = PlayerPrefs.GetInt("_total_matches");
		_total_baskets = PlayerPrefs.GetInt("_total_baskets");
		_total_money = PlayerPrefs.GetInt("_total_money");
		_total_score = PlayerPrefs.GetInt("_total_score");
		//---------------------------------------
		_money = PlayerPrefs.GetInt("_money");
		GetComponent<hud_control> ()._update_money ();
		//---------------------------------------
		GetComponent<_unlock_items>()._check_stages_ui ();
		//---------------------------------------
	}
	//---------------------------------------
	public void _main_menu(){
		//---------------------------------------
		GetComponent<_unlock_items>()._check_stages_ui ();
		//---------------------------------------
		// Destroy all objects from scene gameplay
		_Player.instance._destroyball ();
		Destroy (GameObject.Find ("BackgroundGame"));
		Destroy (GameObject.Find ("_Player"));
		Destroy (GameObject.Find ("_Basket"));
		Destroy (GameObject.Find ("_shadowball"));
		Destroy (GameObject.Find ("_CirclePlayer"));

		for (int i = 0; i < _hud_attemps.Length; i++) {
				if(_hud_attemps [i]){
					Destroy (_hud_attemps [i].gameObject);
				}
		}
		//---------------------------------------
		// Destroy trayectori
		Destroy (GameObject.Find ("Trajectory_Points"));
		//---------------------------------------
		// Reset
		//---------------------------------------
		Time.timeScale = 1f;
		GetComponent<hud_control> ()._objects_hud_control [6].SetActive (false);
		GetComponent<hud_control> ()._objects_hud_control [2].SetActive (false);
		_is_gameover = false;
		_Player.instance._moveball = false;
		_hud_attemps = new Image[0];
		//---------------------------------------
		GetComponent<hud_control> ()._objects_hud_control [0].SetActive (true);
		//---------------------------------------
		// Reset Music
		//---------------------------------------
		GetComponent<_audio_control>()._play_song();
		//---------------------------------------
	}
	//---------------------------------------
	public void _quit_game(){
		Application.Quit ();
	}
	//---------------------------------------
	public void _retry_game(int _at){
		if (_time_attack_time == 0) {
			_attemps = _at;
			GetComponent<hud_control> ()._time_game.enabled = false;

		} else {
			GetComponent<_counter> ()._stop ();
			GetComponent<hud_control> ()._time_game.enabled = true;
			GetComponent<_counter> ()._countdown ();
		}
		_Player.instance._moveball = false;
	}
	//---------------------------------------
	public void _start_game(int _at,Image[] _arr){
		_hud_attemps = _arr;
		_attemps = _at;
		_matches++;
	}
	//---------------------------------------
	public void _fail(){
		if (_time_attack_time == 0) {
			//---------------------------------------

			_attemps = _attemps - 1;

			if (_attemps == 0) {
				Debug.Log ("Game Over");
				_Game_Over ();
			} else {
				_hud_attemps[_attemps].enabled = false;
			}
			//---------------------------------------
		}
	}
	//---------------------------------------

	public void _Game_Over(){
		//---------------------------------------
		// UPDATE DATS
		//---------------------------------------
		int _m = _baskets*_basket_money;

		_total_matches = _total_matches+_matches;
		_total_baskets = _total_baskets + _baskets;
		_total_money = _total_money + _m;
		//---------------------------------------
		PlayerPrefs.GetInt("_total_matches",_total_matches);
		PlayerPrefs.GetInt("_total_baskets",_total_baskets);
		PlayerPrefs.GetInt("_total_money",_total_money);
		PlayerPrefs.GetInt("_total_score",_total_score);
		//---------------------------------------
		// UPDATE MONEY
		//---------------------------------------
		GetComponent<hud_control> ()._add_money (_m);
		//---------------------------------------
		// GAMEOVER VAR
		//---------------------------------------
		_Player.instance._moveball = false;
		_is_gameover = true;
		_audio_control.instance._gameover_sound ();
		GetComponent<hud_control> ()._gameover();
		Destroy (GameObject.Find ("_shadowball"));
		_Player.instance._destroyball ();
		GetComponent<_counter> ()._stop ();
		//---------------------------------------
		//ACHIEVEMENTS
		//---------------------------------------
		_achievements_config.instance._check(_conditions.In_Total_Game,_achievement_type.Baskets,_total_baskets);
		_achievements_config.instance._check(_conditions.In_Total_Game,_achievement_type.Matches,_total_matches);
		_achievements_config.instance._check(_conditions.In_Total_Game,_achievement_type.Money,_total_money);
		_achievements_config.instance._check(_conditions.In_Total_Game,_achievement_type.Score,_total_score);

		_achievements_config.instance._check(_conditions.In_one_play,_achievement_type.Baskets,_baskets);
		_achievements_config.instance._check(_conditions.In_one_play,_achievement_type.Matches,_matches);
		_achievements_config.instance._check(_conditions.In_one_play,_achievement_type.Money,_money);
		int _t = (int)_Player.instance._score;
		_achievements_config.instance._check(_conditions.In_one_play,_achievement_type.Score,_t);
		//---------------------------------------
		_baskets = 0;
		//---------------------------------------
	}
	//---------------------------------------
	public void _update_time_attack(int _time){
		GetComponent<hud_control> ()._timepack.SetActive (false);
		if (_time == 0) {
			GetComponent<hud_control> ()._time_selected.text = "No Time";
		} else {
			GetComponent<hud_control> ()._time_selected.text = _time.ToString()+" Seconds";
		}
		_time_attack_time = _time;
	}
	//---------------------------------------
}