using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _followball : MonoBehaviour {
	public Transform _ball;

	void Update () {
		this.transform.position = _ball.position;
	}
}
