using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
//---------------------------------------
public class _achievements_config : MonoBehaviour {
	//---------------------------------------
	private static _achievements_config _instance;
	public static _achievements_config instance{get{if (!_instance){_instance = GameObject.FindObjectOfType(typeof(_achievements_config)) as _achievements_config;}return _instance;}}
	//---------------------------------------
	[Header("Achievements")]
	public _achievements[] _achievements;
	//---------------------------------------
	[Header("HUD Objects")]
	public Text[] _title;
	public Text[] _desc;
	public Image[] _finished;
	//---------------------------------------
	int _page = 0;
	int _total_page = 0;
	int _items_page = 4;
	int _start_on = 0;
	//---------------------------------------
	void Awake(){
		_save_achievements ();
		_read_achievements ();
		_total_page = _achievements.Length / _items_page;
	}
	//---------------------------------------
	public void _load_page(){
		//---------------------------------------
		for (int i = 0; i < _items_page; i++) {
			//---------------------------------------
			int _a = _start_on+i;
			//---------------------------------------
			if (_a > _achievements.Length-1) {
				_title [i].gameObject.transform.parent.gameObject.SetActive (false);
			} else {
				_title [i].gameObject.transform.parent.gameObject.SetActive (true);
				//---------------------------------------
				_title [i].text = _achievements [_a]._name;
				_desc [i].text = _achievements [_a]._description;
				//---------------------------------------
				if (_achievements [_a]._finished) {
					_finished [i].color = Color.white;
				} else {
					_finished [i].color = Color.black;
				}
				//---------------------------------------
			}
		}
		//---------------------------------------
	}
	//---------------------------------------
	public void _next_page(){
		//---------------------------------------
		_page++;
		//---------------------------------------
		if (_page > _total_page) {
			_page = 0;
		}
		//---------------------------------------
		_start_on = _items_page * _page;
		//---------------------------------------
		_load_page();
		//---------------------------------------
	}
	//---------------------------------------
	public void _prev_page(){
		//---------------------------------------
		_page--;
		//---------------------------------------
		if (_page < 0) {
			_page = _total_page;
		}
		//---------------------------------------
		_start_on = _items_page * _page;
		//---------------------------------------
		_load_page();
		//---------------------------------------
	}
	//---------------------------------------
	public void _check (_conditions _cond,_achievement_type _type, int _value) {
		//---------------------------------------
		for (int i = 0; i < _achievements.Length; i++) {
			//---------------------------------------
			if (_achievements [i]._condition == _cond && _achievements [i]._finished == false) {
				if (_achievements [i]._type == _type) {
					if (_check_result(_value,_achievements [i]._quantity_or_ID, _achievements [i]._value_is)) {
						_achievements [i]._finished = true;
						_save_achievements ();
						_read_achievements ();
					}
				}
			}
			//---------------------------------------
		}
		//---------------------------------------
	}
	//---------------------------------------
	void _save_achievements(){
		string _achiev_list = "";
		//---------------------------------------
		for (int i = 0; i < _achievements.Length; i++) {
			//---------------------------------------
			_achiev_list+=i.ToString()+_bool_to_string(_achievements [i]._finished)+"-/-";
			//---------------------------------------
		}
		//---------------------------------------
		PlayerPrefs.SetString("achievements",_achiev_list);
		//---------------------------------------
	}
	//---------------------------------------
	void _read_achievements(){
		string[] _arr_d;
		string _achiev_list = PlayerPrefs.GetString("achievements");
		_arr_d = _achiev_list.Split ("-/-"[0]);
		for (int i = 0; i < _arr_d.Length; i++) {
			if(_arr_d[i].Length > 0){
				//---------------------------------------
				char _chr = _arr_d[i][0];
				//---------------------------------------
				if (System.Char.IsDigit (_chr))
				{
					string _n = ""+_arr_d [i] [0];
					int _ID = int.Parse(_n);
					string _bool = _arr_d [i].Substring (1,_arr_d[i].Length-1);
					_achievements [_ID]._finished = _string_to_bool (_bool);
				}
				//---------------------------------------
			}
		}
		//---------------------------------------
	}
	//---------------------------------------
	bool _check_result(int _user, int _achievement,_value_condition _value){
		bool _r = false;

		switch(_value)
		{
		case _value_condition.Equals:
			if(_user == _achievement){
				_r = true;
			}
			break;
		case _value_condition.Greater: 
			if(_user > _achievement){
				_r = true;
			}
			break;
		case _value_condition.GreaterOrEquals: 
			if(_user >= _achievement){
				_r = true;
			}
			break;
		case _value_condition.Less: 
			if(_user < _achievement){
				_r = true;
			}
			break;
		case _value_condition.LessOrEquals: 
			if(_user <= _achievement){
				_r = true;
			}
			break;
		case _value_condition.NotEqual: 
			if(_user != _achievement){
				_r = true;
			}
			break;
		}
		return _r;
	}
	//---------------------------------------
	string _bool_to_string(bool _c){string _r = "true";if (!_c) {_r = "false";}return _r;}
	//---------------------------------------
	//---------------------------------------
	bool _string_to_bool(string _c){bool _r = true;if (_c == "false") {_r = false;}return _r;}
	//---------------------------------------
}