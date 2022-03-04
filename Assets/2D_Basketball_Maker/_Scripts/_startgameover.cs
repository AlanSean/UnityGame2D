using UnityEngine;
using System.Collections;

public class _startgameover : MonoBehaviour {

	public void go_game_over () {
		this.GetComponent<Animator> ().enabled = false;
	}
}
