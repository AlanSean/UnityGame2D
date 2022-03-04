using UnityEngine;
using System.Collections;

public class _counter : MonoBehaviour {

	//---------------------------------------
	public void _countdown () {
		
		StartCoroutine ("_countdowngo",GetComponent<_Game_Control> ()._time_attack_time);

	}

	public void _stop(){
		StopCoroutine ("_countdowngo");
	}
	//---------------------------------------
	IEnumerator _countdowngo(int _time_limit){
		for (int i = _time_limit; i>=0; i--) {
			_time_limit = i;

			GetComponent<hud_control> ()._time_game.text = "Time: "+i.ToString ();
			yield return new WaitForSeconds (1f);
		}

		GetComponent<_Game_Control> ()._Game_Over ();
	}
	//---------------------------------------
}