using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class _design_control : MonoBehaviour {
	[Header("List of GameObjects")]
	// GAME STYLE
	//---------------------------------------
	public _ball_config[] _ball_materials;
	public Sprite[] _trajectory_points_textures;
	public Sprite[] _player_textures;
	//---------------------------------------
	[Header("Level Style")]
	// STAGE STYLE
	//---------------------------------------
	public _level_config[] _levels;
	//---------------------------------------

	//---------------------------------------
	[HideInInspector]
	public Image[] _i_ball = new Image[6];
	[HideInInspector]
	public Image[] _i_ball_locked = new Image[6];
	[HideInInspector]
	public int _selectmode = 0;
	// Balls
	// Player
	// Trajectory
	int _page = -1;
	//---------------------------------------

	//---------------------------------------

	public void _next(int _r = 1){
		int _total_pages = _return_pages();

		if (_lnght () > 6) {
			_page++;
		}

		if (_page > _total_pages || _r == 0) {
			_page = 0;
		}

		_update_img ();
	}
	//---------------------------------------
	
	public void _prev(){
		int _total_pages = _return_pages();

		if (_lnght () > 6) {
			_page--;
		}
		
		if (_page < 0) {
			_page = _total_pages;
		}
		
		_update_img ();
	}

	//---------------------------------------

	public void _update_img(bool _n = true){
		int _start_on = _page*6;

			for (int i=0; i<6; i++) {
				if(_lnght() < _start_on+1){
					_i_ball[i].enabled = false;
				    _i_ball_locked [i].gameObject.SetActive (false);
				}else{
					_i_ball[i].enabled = true;
					_i_ball[i].sprite = _return_sprite(_start_on);
					_i_ball[i].name = _start_on.ToString();
				//---------------------------------------
				// Only balls
				if (_selectmode == 0) {
					//---------------------------------------
					if (_ball_materials [_start_on]._locked) {
						_i_ball_locked [i].gameObject.SetActive (true);
					} else {
						_i_ball_locked [i].gameObject.SetActive (false);
					}
					//---------------------------------------
				} else {
					_i_ball_locked [i].gameObject.SetActive (false);
				}
				//---------------------------------------
				}
				
				_start_on++;
			}
	}



	//---------------------------------------
	
	int _return_pages(){
		int _r = 0;
		
		if (_selectmode == 0) {
			_r = _ball_materials.Length / 6;
			
		} else if (_selectmode == 1) {
			_r = _player_textures.Length / 6;
		} else{
			_r = _trajectory_points_textures.Length / 6;
		}
		return _r;
	}


	//---------------------------------------
	
	Sprite _return_sprite(int _i){
		Sprite _r = null;
		
		if (_selectmode == 0) {
			_r = _ball_materials[_i]._icon;
		} else if (_selectmode == 1) {
			_r = _player_textures[_i];
		} else{
			_r = _trajectory_points_textures[_i];
		}
		
		return _r;
	}
	
	//---------------------------------------

	int _lnght(){
		int _r = 0;
		
		if (_selectmode == 0) {
			_r = _ball_materials.Length;
			
		} else if (_selectmode == 1) {
			_r = _player_textures.Length;
		} else{
			_r = _trajectory_points_textures.Length;
		}
		return _r;
	}
	
	//---------------------------------------
	
	public void _update_selection(Image _img){
		int _t = int.Parse (_img.name);

		if (_selectmode == 0) {
			_Game_Control.instance._ball_material = _ball_materials[_t]._material_ball;
			GetComponent<hud_control> ()._ballicon_default.sprite = _ball_materials[_t]._icon;
			
		} else if (_selectmode == 1) {
			_Game_Control.instance._sprt_player = _player_textures[_t];
			GetComponent<hud_control> ()._player_default.sprite = _player_textures[_t];
		}else{
		_Game_Control.instance._sprt_trajectory = _trajectory_points_textures[_t];
		GetComponent<hud_control> ()._trajectory_default.sprite = _trajectory_points_textures[_t];
		}

		GetComponent<hud_control> ()._objects_hud_control [5].SetActive (false);
		
	}


}
