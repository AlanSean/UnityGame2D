using UnityEngine;
using System.Collections;

public class _trigger_dot : MonoBehaviour {

	public SpriteRenderer _sprite;
	//---------------------------------------	

	void Awake(){
		_sprite.sprite = _Game_Control.instance._sprt_trajectory;
	}

	//---------------------------------------	

	void OnTriggerEnter(Collider other) {
		if (other.name == "Ball") {
			//StartCoroutine(_fademe());
			StartCoroutine("_fademe");
		}
	}

	//---------------------------------------	

	IEnumerator _fademe(){
			// SPRITE FADE
			//---------------------------------------
			while (_sprite.color.a > 0f) {
				yield return null;
				_sprite.color = new Color(_sprite.color.r,_sprite.color.g,_sprite.color.b,_sprite.color.a-0.1f);
			}
	}

	public void _hiddenfast(){
		StartCoroutine("_fademe");
	}

	public void _resetalpha(){
		StopCoroutine ("_fademe");
			//SPRITE FADE
			//---------------------------------------
			_sprite.color = new Color(_sprite.color.r,_sprite.color.g,_sprite.color.b,1f);
	}
}
