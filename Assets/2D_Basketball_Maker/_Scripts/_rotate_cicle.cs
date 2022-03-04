using UnityEngine;
using System.Collections;

public class _rotate_cicle : MonoBehaviour {

	void Awake(){
		this.transform.parent = null;
	}

	void Update() 
	{
		this.transform.Rotate(-Vector3.forward * 100f * Time.deltaTime);
	}
}
