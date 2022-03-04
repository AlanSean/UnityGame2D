using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class _unlock_items : MonoBehaviour {
	//---------------------------------------
	int _price = 0;
	int _ID = 0;
	bool _is_ball = false;
	//---------------------------------------
	void Awake(){
		_check_balls_locked ();
		_check_stages_locked ();
		_check_stages_ui ();
		//_load_unlockeables ();
	}
	//---------------------------------------
	void _check_balls_locked (){
		for (int i = 0; i < GetComponent<_design_control> ()._ball_materials.Length; i++) {
			if(GetComponent<_design_control> ()._ball_materials[i]._price_to_unlock > 0){
				//---------------------------------------
				GetComponent<_design_control> ()._ball_materials [i]._locked = true;
				//---------------------------------------
			}
		}
	}
	//---------------------------------------
	void _check_stages_locked (){
		//---------------------------------------
		// CHECK LOCKED STAGES
		//---------------------------------------
		for (int i = 0; i < GetComponent<_design_control> ()._levels.Length; i++) {
			//---------------------------------------
			if (GetComponent<_design_control> ()._levels [i]._price_to_unlock > 0) {
				//---------------------------------------
				GetComponent<_design_control> ()._levels [i]._locked = true;
				//---------------------------------------
			} else {
				GetComponent<_design_control> ()._levels [i]._locked = false;
			}
			//---------------------------------------
		}
	}
	//---------------------------------------
	public void _check_stages_ui (){
		//---------------------------------------
		// CHECK LOCKED STAGES
		//---------------------------------------
		for (int i = 0; i < GetComponent<_design_control> ()._levels.Length; i++) {
			//---------------------------------------
			if (GetComponent<_design_control> ()._levels [i]._locked) {
				GetComponent<_design_control> ()._levels [i]._UI_locked_sprite.gameObject.SetActive (true);
			} else {
				GetComponent<_design_control> ()._levels [i]._UI_locked_sprite.gameObject.SetActive (false);
			}
			//---------------------------------------
		}
	}
	//---------------------------------------
	public void _buy_item(){
		//---------------------------------------
		Debug.Log ("BUY ITEM");
		//---------------------------------------
		_audio_control.instance._buy_sound(); // Play Sound
		GetComponent<hud_control>()._unlock_close();
		//---------------------------------------
		int _t = _Game_Control.instance._money;
		_t = _t - _price;
		_Game_Control.instance._money = _t;
		//---------------------------------------
		if (_is_ball) {
			//---------------------------------------
			_achievements_config.instance._check (_conditions.In_Total_Game, _achievement_type.Unlock_Ball, _ID);
			GetComponent<_design_control> ()._ball_materials [_ID]._locked = false;
			GetComponent<_design_control> ()._update_img ();
			//---------------------------------------
		} else {
			//---------------------------------------
			_achievements_config.instance._check (_conditions.In_Total_Game, _achievement_type.Unlock_Stage, _ID);
			GetComponent<_design_control> ()._levels [_ID]._locked = false;
			_check_stages_ui ();
			//---------------------------------------
		}
		GetComponent<hud_control> ()._update_money ();
		//---------------------------------------
		_save_unlock(_is_ball);
		//---------------------------------------
	}
	//---------------------------------------
	public void _unlock_ball (Image ID) {
		//---------------------------------------
		_is_ball = true;
		_ID = int.Parse (ID.name);
		//---------------------------------------
		GetComponent<hud_control> ()._icon.rectTransform.sizeDelta = new Vector2(300,300);
		GetComponent<hud_control> ()._icon.sprite = GetComponent<_design_control> ()._ball_materials [_ID]._icon;
		GetComponent<hud_control> ()._price.text = GetComponent<_design_control> ()._ball_materials [_ID]._price_to_unlock.ToString();
		GetComponent<hud_control> ()._unlock.SetActive (true);
		//---------------------------------------
		_price = GetComponent<_design_control> ()._ball_materials [_ID]._price_to_unlock;
		//---------------------------------------
		if (_Game_Control.instance._money < GetComponent<_design_control> ()._ball_materials [_ID]._price_to_unlock) {
			GetComponent<hud_control> ()._button_buy.SetActive (false);
			GetComponent<hud_control> ()._low_money.SetActive (true);
		} else {
			GetComponent<hud_control> ()._button_buy.SetActive (true);
			GetComponent<hud_control> ()._low_money.SetActive (false);
		}
		//---------------------------------------
	}
	//---------------------------------------
	public void _unlock_stage (int _IDT) {
		_is_ball = false;
		_ID = _IDT;
		//---------------------------------------
		GetComponent<hud_control> ()._icon.rectTransform.sizeDelta = new Vector2(300,200);
		GetComponent<hud_control> ()._icon.sprite = GetComponent<_design_control> ()._levels[_ID]._image_for_select_level;
		GetComponent<hud_control> ()._price.text = GetComponent<_design_control> ()._levels[_ID]._price_to_unlock.ToString();
		GetComponent<hud_control> ()._unlock.SetActive (true);
		//---------------------------------------
		_price = GetComponent<_design_control> ()._levels[_ID]._price_to_unlock;
		//---------------------------------------
		if (_Game_Control.instance._money < GetComponent<_design_control> ()._levels[_ID]._price_to_unlock) {
			GetComponent<hud_control> ()._button_buy.SetActive (false);
			GetComponent<hud_control> ()._low_money.SetActive (true);
		} else {
			GetComponent<hud_control> ()._button_buy.SetActive (true);
			GetComponent<hud_control> ()._low_money.SetActive (false);
		}
		//---------------------------------------
	}
	//---------------------------------------
	void _save_unlock(bool _isball = true){
		string _t = "";

		if (_isball) { // Is Ball
			for (int i = 0; i < GetComponent<_design_control> ()._ball_materials.Length; i++) {
				if (GetComponent<_design_control> ()._ball_materials [i]._locked) {
					_t += "true";
				} else {
					_t += "false";
				}
				_t += "/";
			}
			//---------------------------------------
			PlayerPrefs.SetString ("_ball_locked", _t);
			//---------------------------------------
		} else {
			for (int i = 0; i < GetComponent<_design_control> ()._levels.Length; i++) {
				if (GetComponent<_design_control> ()._levels[i]._locked) {
					_t += "true";
				} else {
					_t += "false";
				}
				_t += "/";
			}
			//---------------------------------------
			PlayerPrefs.SetString ("_stage_locked", _t);
			//---------------------------------------
		}
	}
	//---------------------------------------
	void _load_unlockeables(bool _isball = true){
		//---------------------------------------
		if(PlayerPrefs.HasKey("_ball_locked")){
			//---------------------------------------
			string _t = PlayerPrefs.GetString ("_ball_locked");
			string[] _arr = _t.Split(new string[] {"/"}, System.StringSplitOptions.None);

			//---------------------------------------
			for (int i = 0; i < GetComponent<_design_control> ()._ball_materials.Length; i++) {
				GetComponent<_design_control> ()._ball_materials [i]._locked = _string_to_bool(_arr [i]);
			}
			//---------------------------------------
		}
		//---------------------------------------
		if(PlayerPrefs.HasKey("_stage_locked")){
			//---------------------------------------
			string _t = PlayerPrefs.GetString ("_stage_locked");
			string[] _arr = _t.Split(new string[] {"/"}, System.StringSplitOptions.None);

			//---------------------------------------
			for (int i = 0; i < GetComponent<_design_control> ()._levels.Length; i++) {
				Debug.Log (_arr [i]);
				GetComponent<_design_control> ()._levels [i]._locked = _string_to_bool(_arr [i]);
			}
			//---------------------------------------
		}
		//---------------------------------------
	}
	//---------------------------------------
	bool _string_to_bool(string _s){
		bool _r = true;

		if (_s == "false") {
			_r = false;
		}
		return _r;
	}
	//---------------------------------------
}
