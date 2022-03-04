using UnityEngine;
using System.Collections;

public class _pausegame : MonoBehaviour {
	//---------------------------------------
	public void _pause () {
		Time.timeScale = 0f;
		GetComponent<hud_control> ()._objects_hud_control [6].SetActive (true);

	}
	//---------------------------------------
	public void _resume () {
		Time.timeScale = 1f;
		GetComponent<hud_control> ()._objects_hud_control [6].SetActive (false);

	}
	//---------------------------------------
}
