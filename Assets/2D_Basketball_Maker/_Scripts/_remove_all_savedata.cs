using UnityEngine;
using System.Collections;

public class _remove_all_savedata : MonoBehaviour {

	void Awake(){
		Debug.Log ("REMOVE ALL DATA");
		PlayerPrefs.DeleteKey ("achievements");
		PlayerPrefs.DeleteKey ("_stage_locked");
		PlayerPrefs.DeleteKey ("_ball_locked");
		PlayerPrefs.DeleteKey ("_total_matches");
		PlayerPrefs.DeleteKey ("_total_baskets");
		PlayerPrefs.DeleteKey ("_total_money");
		PlayerPrefs.DeleteKey ("_money");
	}

}
