using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _rotate_obstacles : MonoBehaviour {
	public float _speed = 10f;
	public List<Transform> _objets;
	public bool _this_move_with_basket = true;


	void Update () {
		transform.Rotate(-Vector3.forward * _speed * Time.deltaTime, Space.World);

		if (_objets.Count > 0) {

			for(int i=0;i<_objets.Count;i++){
				_objets[i].Rotate(Vector3.forward * _speed *2* Time.deltaTime, Space.World);
			}
		}
	
	}
}
