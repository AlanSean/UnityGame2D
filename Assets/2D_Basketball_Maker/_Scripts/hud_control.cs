using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class hud_control : MonoBehaviour {
	//---------------------------------------
	[HideInInspector]
	public GameObject[] _objects_hud_control;
	//---------------------------------------
	//NEW
	//---------------------------------------
	[HideInInspector]
	public GameObject _achievement;
	[HideInInspector]
	public GameObject _unlock;
	[HideInInspector]
	public Text[] _money;
	[HideInInspector]
	public Text _price;
	[HideInInspector]
	public Image _icon;
	public GameObject _button_buy;
	public GameObject _low_money;
	//---------------------------------------
	// Time Attack
	[HideInInspector]
	public Text _time_selected;
	[HideInInspector]
	public GameObject _timepack;
	[HideInInspector]
	public Text _time_game;
	//---------------------------------------
	[HideInInspector]
	public Text _score;
	[HideInInspector]
	public Text _best_score;
	//---------------------------------------
	// GAME OVER
	[HideInInspector]
	public Text _gov_score;
	[HideInInspector]
	public Text _gov_best_score;
	//---------------------------------------
	// Menu
	[HideInInspector]
	public Image _ballicon_default;
	[HideInInspector]
	public Image _player_default;
	[HideInInspector]
	public Image _trajectory_default;
	//---------------------------------------
	public Image _touchm;
	public Sprite[] _touchb;
	public Text _T_touch;
	//---------------------------------------
	void Awake(){
		if (PlayerPrefs.HasKey ("bestscore")) {
			_gov_best_score.text = PlayerPrefs.GetInt ("bestscore").ToString ("0000000");
			_best_score.text = PlayerPrefs.GetInt ("bestscore").ToString ("0000000");
		} else {
			PlayerPrefs.GetInt("bestscore",0);
		}
		//---------------------------------------
	}
	//---------------------------------------
	public void _start_game(){
		_score.text = "0000000";
		_objects_hud_control [0].SetActive (false);
		_objects_hud_control [2].SetActive (false);
		_objects_hud_control [1].SetActive (true);
	}
	//---------------------------------------
	public void update_score(float _s){
		_score.text = _s.ToString("0000000");
	}
	//---------------------------------------
	public void _gameover(){
		_gov_score.text = _Player.instance._score.ToString("0000000");
		_objects_hud_control[0].SetActive(false);
		_objects_hud_control[1].SetActive(false);
		_objects_hud_control[4].SetActive(false);
		_objects_hud_control[2].SetActive(true);
		_objects_hud_control[3].SetActive(true);
		_objects_hud_control[3].GetComponent<Animator> ().enabled = true;

		if (PlayerPrefs.HasKey ("bestscore")) {
			if (_Player.instance._score > PlayerPrefs.GetInt ("bestscore")) {
				_objects_hud_control [4].SetActive (true);
				PlayerPrefs.SetInt ("bestscore", (int)_Player.instance._score);
				_gov_best_score.text = _Player.instance._score.ToString ("0000000");
				_best_score.text = _Player.instance._score.ToString ("0000000");
			}
		} else {
			_gov_best_score.text = _Player.instance._score.ToString ("0000000");
			_best_score.text = _Player.instance._score.ToString ("0000000");
		}
	}
	//---------------------------------------
	public void _changemenu(int _s) {
		GetComponent<_design_control> ()._selectmode = _s;
		_objects_hud_control [5].SetActive (true);
		GetComponent<_design_control> ()._next (0);
	}
	//---------------------------------------
	public void _view_time() {
		_timepack.SetActive (true);
	}
	//---------------------------------------
	public void _achievement_view() {
		_achievements_config.instance._load_page ();
		_achievement.SetActive (true);
	}
	//---------------------------------------
	public void _achievement_close() {
		_achievement.SetActive (false);
	}
	//---------------------------------------
	public void _unlock_open() {
		_unlock.SetActive (true);
	}
	//---------------------------------------
	public void _unlock_close() {
		_unlock.SetActive (false);
	}
	//---------------------------------------
	public void _update_money(){
		_money[0].text = _Game_Control.instance._money+" X";
		_money[1].text = _Game_Control.instance._money+" X";
		_money[2].text = _Game_Control.instance._money+" X";
	}
	//---------------------------------------
	public void _add_money(int _t){
		StartCoroutine (_add_m (_t));
	}
	//---------------------------------------
	IEnumerator _add_m(int _total){
		int _i_new_total = _Game_Control.instance._money + _total;
		int _money_t = _Game_Control.instance._money;
		//---------------------------------------
		for (int i = 0; i < 1000; i++) {

			if (_money_t+5< _i_new_total) {
				_money_t = _money_t + 5;
				_money [2].text = _money_t + " X";
			} else {
				_Game_Control.instance._money = _i_new_total;
				PlayerPrefs.SetInt("_money",_i_new_total);
				_update_money ();
				break;
			}
			yield return new WaitForSeconds (0.05f);
		}
		//---------------------------------------
	}
	//---------------------------------------
	public void _touch_mode(){
		//---------------------------------------
		if(GetComponent<_game_options>()._touch_mode){
			GetComponent<_game_options>()._touch_mode = false;
			_touchm.sprite = _touchb[0];
			_T_touch.text = "Normal Mode";
		}else{
			GetComponent<_game_options>()._touch_mode = true;
			_touchm.sprite = _touchb[1];
			_T_touch.text = "Touch Mode";
		}
		//---------------------------------------
	}
	//---------------------------------------
}
