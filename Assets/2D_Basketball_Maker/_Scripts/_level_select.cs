using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class _level_select : MonoBehaviour {

	// VAR LIST
	//---------------------------------------
		[HideInInspector]
		public bool _is_menu_selection = true;
	//---------------------------------------
	// VAR UI
	//---------------------------------------
		[HideInInspector]
		public Image _background;
		[HideInInspector]
		public Text _name_stage;
		public Button[] _button_list;
		Sprite[] _difficulty_icons = new Sprite[2];
		Image[] _difficulty_list = new Image[2];
		Transform _parentHud;

	//---------------------------------------
	//---------------------------------------
	void Awake () {
		_create_buttons_dif (GetComponent<_design_control>()._levels[GetComponent<_game_options>()._level_p]._Difficulty_levels.Length);
		_update_level_info (GetComponent<_game_options>()._difficulty_l);
	}

	//---------------------------------------

	void _create_buttons_dif(int _l){
		Transform _p = GameObject.Find ("_Difficulty").transform;

		if (!_p) {
			Debug.Log ("File not found (_Difficulty)");

		} else if (_difficulty_list.Length != GetComponent<_design_control>()._levels[GetComponent<_game_options>()._level_p]._Difficulty_levels.Length){

			//---------------------------------------

			float _icon_size = 100;
			float _total_size = _icon_size*_l;
			float _x = -_total_size/2;
			_x = _x+_icon_size/2;
			_difficulty_icons[0] = Resources.Load<Sprite>("_level_difficulty");
			_difficulty_icons[1] = Resources.Load<Sprite>("_level_difficulty_selected");
			_difficulty_list = new Image[GetComponent<_design_control>()._levels[GetComponent<_game_options>()._level_p]._Difficulty_levels.Length];

			//---------------------------------------

			for(int i=0;i<_l;i++){

				//---------------------------------------

				GameObject _d = new GameObject();
				_d.AddComponent<RectTransform>();
				_d.AddComponent<CanvasRenderer>();
				_d.AddComponent<Image>();
				_d.AddComponent<Button>();
				_d.name = i.ToString();

				// ADD TO ARRAY
				//---------------------------------------
				_difficulty_list[i] = _d.GetComponent<Image>();
				//---------------------------------------

				Button _b = _d.GetComponent<Button>();
                _b.transform.SetParent(_p);
				_b.transform.localPosition = new Vector3(_x,-27,0);
				_b.transform.localScale = new Vector3(1,1,1);
				_b.onClick.AddListener( delegate{ this.gameObject.GetComponent<_level_select>()._update_difficulty(_b); } );

				// ADD NEW POSITION X
				//---------------------------------------
				_x = _x+_icon_size;
				//---------------------------------------

				// ADD SPRITES
				//---------------------------------------
				if(i == 0){
					_b.GetComponent<Image>().sprite = _difficulty_icons[1];
				}else{
					_b.GetComponent<Image>().sprite = _difficulty_icons[0];
				}
				//---------------------------------------
			}

		}

	}

	//---------------------------------------

	public void _update_level_info(int _s){
		_reset_all_buttons_outline ();
		GetComponent<_game_options> ()._level_p = _s;
		_background.sprite = GetComponent<_design_control>()._levels[_s]._image_for_select_level;
		_name_stage.text = GetComponent<_design_control>()._levels[_s]._stage_name;
		_button_list[_s].GetComponent<Outline>().enabled = true;
	}

	//---------------------------------------

	public void _update_difficulty(Button _level){
		int _d =  System.Convert.ToInt32(_level.name);
		GetComponent<_game_options>()._difficulty_l = _d;
		_reset_difficulty (_d);

	}

	//---------------------------------------
	
	void _reset_all_buttons_outline(){
		for (int i=0; i<_button_list.Length; i++) {
			_button_list[i].GetComponent<Outline>().enabled = false;
		}
	}

	//---------------------------------------
	
	void _reset_difficulty(int _l){

		for (int i=0; i<_difficulty_list.Length; i++) {
			//---------------------------------------
			if(i <= _l){
				_difficulty_list[i].sprite = _difficulty_icons[1];
			}else{
				_difficulty_list[i].sprite = _difficulty_icons[0];
			}
			//---------------------------------------
		}
	}

	//---------------------------------------
	
	public void _retry_game(){
		
		_Player.instance._destroyball ();
		GetComponent<_pausegame> ()._resume ();

		GetComponent<hud_control> ()._start_game ();
		int _at = GetComponent<_design_control> ()._levels [GetComponent<_game_options> ()._level_p]._Difficulty_levels [GetComponent<_game_options> ()._difficulty_l]._attempts;

		//---------------------------------------
		// CREATE ATTEMPS HUD
		//---------------------------------------
		if (GetComponent<_Game_Control> ()._time_attack_time == 0) {
			for(int i=0;i<GetComponent<_Game_Control>()._hud_attemps.Length;i++){
				GetComponent<_Game_Control>()._hud_attemps[i].enabled = true;
			}
		}
		//---------------------------------------

		//---------------------------------------
		// DESTROY OLD OBSTACLES
		//---------------------------------------
		GameObject[] _obslist = GameObject.FindGameObjectsWithTag ("Obstacle");

		if (_obslist.Length > 0) {
			
			for (int i = 0; i < _obslist.Length; i++) {
				Destroy(_obslist[i]);
			}

		}
		//---------------------------------------

		// OBSTACLES
		//---------------------------------------
		if(GetComponent<_design_control> ()._levels [GetComponent<_game_options> ()._level_p]._Difficulty_levels [GetComponent<_game_options> ()._difficulty_l]._obstacles.Length > 0){

			for (int i = 0; i < GetComponent<_design_control> ()._levels [GetComponent<_game_options> ()._level_p]._Difficulty_levels [GetComponent<_game_options> ()._difficulty_l]._obstacles.Length; i++) {

				GameObject _ob = Instantiate(GetComponent<_design_control> ()._levels [GetComponent<_game_options> ()._level_p]._Difficulty_levels [GetComponent<_game_options> ()._difficulty_l]._obstacles[i]._prefab);

				_ob.transform.position = new Vector3 (GetComponent<_design_control> ()._levels [GetComponent<_game_options> ()._level_p]._Difficulty_levels [GetComponent<_game_options> ()._difficulty_l]._obstacles[i]._position_x,GetComponent<_design_control> ()._levels [GetComponent<_game_options> ()._level_p]._Difficulty_levels [GetComponent<_game_options> ()._difficulty_l]._obstacles[i]._position_y,1.78f);

				if (_ob.GetComponent<_rotate_obstacles> ()) {

					if (_ob.GetComponent<_rotate_obstacles> ()._this_move_with_basket) {
						_ob.transform.parent = GameObject.Find ("_Basket").transform;
						_ob.transform.localPosition = new Vector3(0,0,0);
						_ob.name = "_Obstacle";
					}

				}

			}

		}
		//---------------------------------------
		
		GetComponent<_Game_Control>()._retry_game(_at);
		GetComponent<hud_control> ()._start_game ();
		_Player.instance.createBall (1f);
		_Player.instance._score = 0f;

		//---------------------------------------

		_Game_Control.instance._is_gameover = false;
	}


	//---------------------------------------

	public void _start_game(){
		// CREATE ATTEMPS HUD
		//---------------------------------------
		int _at = GetComponent<_design_control> ()._levels [GetComponent<_game_options> ()._level_p]._Difficulty_levels [GetComponent<_game_options> ()._difficulty_l]._attempts;
		if (!_parentHud) {
			_parentHud = GameObject.Find ("_backhud").transform;
		}
		Image[] _arr = new Image[_at];
		//---------------------------------------
		// DESTROY ATTEMP ICON
		//---------------------------------------
		for(int i=0;i<GetComponent<_Game_Control>()._hud_attemps.Length;i++){
			Destroy(GetComponent<_Game_Control>()._hud_attemps[i]);
		}
		//---------------------------------------
		// CREATE ATTEMP ICON
		//---------------------------------------
		if (GetComponent<_Game_Control> ()._time_attack_time == 0) {
			
			if (GetComponent<_Game_Control> ()._hud_attemps.Length != _at) {

				float _x = -900;
				for (int i=0; i<_at; i++) {

					GameObject _a = new GameObject ();
					_a.AddComponent<RectTransform> ();
					_a.AddComponent<CanvasRenderer> ();
					_a.AddComponent<Image> ();
					_a.name = "_Attemp";
					_a.transform.SetParent(_parentHud);
					_a.transform.localPosition = new Vector3 (_x, 0, 0);
					_a.transform.localScale = new Vector3 (1, 1, 1);

					_a.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("_attemp");
					_arr [i] = _a.GetComponent<Image> ();

					_x = _x + 100;
				}

			}
			//---------------------------------------
			GetComponent<_Game_Control>()._start_game(_at,_arr);
			//---------------------------------------
		}
		//---------------------------------------
		GameObject _backg = GameObject.Find ("BackgroundGame");

		if (_backg) {
			Destroy(_backg);
		}

		_backg = Instantiate(GetComponent<_design_control>()._levels[GetComponent<_game_options>()._level_p]._background);
		_backg.transform.localPosition = new Vector3 (0,0,5);
		_backg.name = "BackgroundGame";

		GameObject _player = Instantiate(Resources.Load("_Player", typeof(GameObject))) as GameObject;
		GameObject _basket = Instantiate(GetComponent<_design_control>()._levels[GetComponent<_game_options>()._level_p]._basket);
		_basket.transform.position = new Vector3 (GetComponent<_design_control>()._levels[GetComponent<_game_options>()._level_p]._basket_position_x,GetComponent<_design_control>()._levels[GetComponent<_game_options>()._level_p]._basket_position_y,1.78f);


		_player.name = "_Player";
		_basket.name = "_Basket";

		_player.GetComponent<_Player> ()._basketgo = _basket.transform;
		_player.GetComponent<_Player> ()._chek_basket_dats();

		GetComponent<hud_control> ()._start_game ();

		if (!GetComponent<_game_options>()._touch_mode) {
			GameObject.Find ("_CirclePlayer").GetComponent<SpriteRenderer> ().sprite = _Game_Control.instance._sprt_player;
		}
		//---------------------------------------
		//---------------------------------------
		// DESTROY OLD OBSTACLES
		//---------------------------------------
		GameObject[] _obslist = GameObject.FindGameObjectsWithTag ("Obstacle");
		if (_obslist.Length > 0) {
			for (int i = 0; i < _obslist.Length; i++) {
				Destroy(_obslist[i]);
			}
		}
		//---------------------------------------
		// OBSTACLES
		//---------------------------------------
		if(GetComponent<_design_control> ()._levels [GetComponent<_game_options> ()._level_p]._Difficulty_levels [GetComponent<_game_options> ()._difficulty_l]._obstacles.Length > 0){

			for (int i = 0; i < GetComponent<_design_control> ()._levels [GetComponent<_game_options> ()._level_p]._Difficulty_levels [GetComponent<_game_options> ()._difficulty_l]._obstacles.Length; i++) {

				GameObject _ob = Instantiate(GetComponent<_design_control> ()._levels [GetComponent<_game_options> ()._level_p]._Difficulty_levels [GetComponent<_game_options> ()._difficulty_l]._obstacles[i]._prefab);
				_ob.transform.position = new Vector3 (GetComponent<_design_control> ()._levels [GetComponent<_game_options> ()._level_p]._Difficulty_levels [GetComponent<_game_options> ()._difficulty_l]._obstacles[i]._position_x,GetComponent<_design_control> ()._levels [GetComponent<_game_options> ()._level_p]._Difficulty_levels [GetComponent<_game_options> ()._difficulty_l]._obstacles[i]._position_y,1.78f);
				if (_ob.GetComponent<_rotate_obstacles> ()) {
					
					if (_ob.GetComponent<_rotate_obstacles> ()._this_move_with_basket) {
						_ob.transform.parent = _basket.transform;
						_ob.transform.localPosition = new Vector3(0,0,0);
						_ob.name = "_Obstacle";
					}
				}
			}
		}
		//---------------------------------------

		// CHECK IF IS TIME ATTACK!
		//---------------------------------------
		if (GetComponent<_Game_Control> ()._time_attack_time > 0) {
			GetComponent<_counter> ()._stop ();
			GetComponent<hud_control> ()._time_game.enabled = true;
			GetComponent<_counter> ()._countdown ();
		} else {
			GetComponent<hud_control> ()._time_game.enabled = false;
		}
		//---------------------------------------
		//Music Play
		//---------------------------------------
		GetComponent<_audio_control>()._play_song(GetComponent<_design_control>()._levels[GetComponent<_game_options>()._level_p]._theme);
		//---------------------------------------
		_Player.instance.newplay();
		//---------------------------------------
	}
	//---------------------------------------
}