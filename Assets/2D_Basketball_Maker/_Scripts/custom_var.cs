using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
//----------------------------------------------//----------------------------------------------
[System.Serializable]
public class _achievements{
	//----------------------------------------------
	[Header("Achievement Dats")]
	public string _name;
	public string _description;
	//----------------------------------------------
	public _achievement_type _type;
	public _conditions _condition;
	public int _quantity_or_ID;
	public _value_condition _value_is;
	[HideInInspector]
	public bool _finished;
	//----------------------------------------------
}
//----------------------------------------------
[System.Serializable]
public class _difficulty{
	public bool _player_always_left,_random_player_position;
	[Range(1,25)]
	public int _attempts = 3;
	//----------------------------------------------
	[Header("Basket Configuration")]
	public bool _random_basket_position;
	public _animations_config[] _basket_animation_list;
	//----------------------------------------------
	[Header("Obstacles")]
	public _obstacle_config[] _obstacles;
	//----------------------------------------------
}
//----------------------------------------------
[System.Serializable]
public class _level_config{
	[Header("Basic Level Info")]
	public Sprite _image_for_select_level;
	public Image _UI_locked_sprite;
	public string _stage_name;
	public int _points_to_unlock = 0;
	[Header("Level Config")]
	public GameObject _background;
	public bool _destroy_ball_on_dunk;
	public float _basket_position_x = 7f;
	public float _basket_position_y = 3f;

	[Range(0f,25f)]
	public float _gravity = 9.81f;
	public AudioClip _theme;
	//----------------------------------------------
	[Header("Prefabs")]
	public GameObject _particles_dunk;
	public GameObject _particles_miss;
	public GameObject _basket;
	//----------------------------------------------
	[Header("Difficulty Levels")]
	public _difficulty[] _Difficulty_levels;
	[Header("Unlock level")]
	public int _price_to_unlock;
	[HideInInspector]
	public bool _locked = false;
}
//----------------------------------------------
[System.Serializable]
public enum _conditions{In_one_play,In_Total_Game};
[System.Serializable]
public enum _achievement_type{Score,Unlock_Stage,Unlock_Ball,Money,Baskets,Matches};
[System.Serializable]
public enum _value_condition{Greater,GreaterOrEquals,Less,LessOrEquals,Equals,NotEqual};
//----------------------------------------------
[System.Serializable]
public enum _animations{Horizontal, Vertical, Circle};
[System.Serializable]
public enum _speed_anim{Very_Slow, Slow, Normal, Fast, Very_Fast, Super_Fast};
[System.Serializable]
public enum _distance_anim{Very_Short,Short,Middle,Long,Very_Long};
//----------------------------------------------
[System.Serializable]
public class _animations_config {
	public _animations _type_animation;
	[Range(0,200)]
	public int _dunks_to_active;
	public _speed_anim _speed;
	public _distance_anim _distance;
	public bool _stop_on_dunk = false;
}
//----------------------------------------------
[System.Serializable]
public class _ball_config {
	public Sprite _icon;
	public Material _material_ball;
	public int _price_to_unlock = 0;
	[HideInInspector]
	public bool _locked = false;
}
//----------------------------------------------
[System.Serializable]
public class _obstacle_config {
	public GameObject _prefab;
	[Range(-5.5f,5.5f)]
	public float _position_y;
	[Range(-11.5f,11.5f)]
	public float _position_x;
}
//----------------------------------------------