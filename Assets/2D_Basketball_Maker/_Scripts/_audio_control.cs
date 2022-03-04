using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class _audio_control : MonoBehaviour {
	private static _audio_control _instance;
	public static _audio_control instance{get{if (!_instance){
				_instance = GameObject.FindObjectOfType(typeof(_audio_control)) as _audio_control;}
			return _instance;}}
	
	public AudioClip _song_main_theme;
	bool _music,_fx = true;
	public AudioSource _gameover;
	public AudioSource _click;
	public AudioSource _buy;
	[HideInInspector]
	public Sprite[] _textures_sound_buttons = new Sprite[4];
	// Music True
	// Music False
	// Sound True
	// Sound False
	[HideInInspector]
	public Button[] _buttons = new Button[2];
	//Music
	//Sound

	void Awake(){
		_loadprefs ();
		_play_song ();
	}

	//---------------------------------------

	void _loadprefs(){

		if (PlayerPrefs.GetInt ("_music") == 0) {
			_music = true;
		} else {
			_music = false;
		}
		//---------------------------------------
		if (PlayerPrefs.GetInt ("_fx") == 0) {
			_fx = true;
		} else {
			_fx = false;
		}

		// Update Buttons
		_change_sprite (0, _music);
		_change_sprite (1, _fx);

	}


	//---------------------------------------
	public void _b_music() {

		if (_music) {
			_music = false;
			PlayerPrefs.SetInt ("_music", 1); // Save option
			GetComponent<AudioSource> ().mute = true; // Mute True
		} else {
			_music = true;
			PlayerPrefs.SetInt ("_music", 0); // Save option

			if(!GetComponent<AudioSource> ().isPlaying){
				GetComponent<AudioSource> ().Play ();
			}
			GetComponent<AudioSource> ().mute = false; // Mute False
		}

		// Update Buttons
		_change_sprite (0, _music);

	}

	//---------------------------------------

	public void _b_fx() {
		
		if (_fx) {
			_fx = false;
			PlayerPrefs.SetInt ("_fx", 1); // Save option
		} else {
			_fx = true;
			PlayerPrefs.SetInt ("_fx", 0); // Save option
		}

		// Update Buttons
		_change_sprite (1, _fx);

	}

	//---------------------------------------

	public void _play_song(AudioClip _s = null){

		if (_s != null) { // Load Song
			GetComponent<AudioSource> ().clip = _s;
		} else {
			GetComponent<AudioSource> ().clip = _song_main_theme;
		}

		if (_music) {
			GetComponent<AudioSource> ().Play (); // Play Song
		}

	}

	//---------------------------------------

	public bool _play_sound(){
		return _fx;
	}

	//---------------------------------------

	public void _gameover_sound(){
		if (_fx) {
			_gameover.Play ();
		}
	}

	//---------------------------------------

	public void _hud_click(){
		if (_fx) {
			_click.Play ();
		}
	}
	//---------------------------------------

	public void _buy_sound(){
		if (_fx) {
			_buy.Play ();
		}
	}
	//---------------------------------------

	void _change_sprite(int _b,bool _action){

		if (_b == 0) {

			if (_action) {
				_buttons [0].image.sprite = _textures_sound_buttons [0];
			} else {
				_buttons [0].image.sprite = _textures_sound_buttons [1];
			}

		} else {
			
			if (_action) {
				_buttons [1].image.sprite = _textures_sound_buttons [2];
			} else {
				_buttons [1].image.sprite = _textures_sound_buttons [3];
			}

		}

	}

	//---------------------------------------
}