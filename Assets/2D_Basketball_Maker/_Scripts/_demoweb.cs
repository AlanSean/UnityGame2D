using UnityEngine;
using System.Collections;

public class _demoweb : MonoBehaviour {
	public void _add_coins_demo(){
		_Game_Control.instance._money = _Game_Control.instance._money + 5000;
		_Game_Control.instance.GetComponent<hud_control> ()._update_money ();
	}
}
