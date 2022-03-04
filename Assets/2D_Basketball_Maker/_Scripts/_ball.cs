using UnityEngine;
using System.Collections;

public class _ball : MonoBehaviour {
	public bool _set = false;
	public Transform _shadow;
	public Transform _tball;
	public float _shadow_distance = 5f;

	public AudioClip[] _ball_sounds;
	// Touch
	// Fail

	bool _hidden_t = false;

	//---------------------------------------	
	void Awake(){
		_shadow.parent = null;
		_shadow.transform.position = new Vector3 (this.transform.position.x,-4.5f,this.transform.position.z);
	}
	//---------------------------------------	
	void OnCollisionEnter(Collision collision) {
		
		if (!_hidden_t) {
			if (collision.gameObject.tag != "Obstacle") {
				_Player.instance._hidden_trajectory();
				_hidden_t = true;
			}
		}
		//---------------------------------------	
		if (!_set) {
			
			if (collision.gameObject.tag == "Floor") {
				this.name = "Ballnull";
				
				if (_audio_control.instance._play_sound ()) {
					GetComponent<AudioSource> ().clip = _ball_sounds [0];
					GetComponent<AudioSource> ().Play ();
				}

				_set = true;
				_Player.instance.newplay ();
				_Game_Control.instance._fail ();

			} else if (collision.gameObject.tag == "Obstacle") {

				if (_audio_control.instance._play_sound ()) {
					GetComponent<AudioSource> ().clip = _ball_sounds [1];
					GetComponent<AudioSource> ().Play ();
				}

				//_set = true;
				collision.gameObject.GetComponent<_obstacle_options> ().chek_action ();

			} else if (collision.gameObject.tag == "Basket") {

				if (_audio_control.instance._play_sound ()) {
					GetComponent<AudioSource> ().clip = _ball_sounds [2];
					GetComponent<AudioSource> ().Play ();
				}

			}
			//---------------------------------------	
		} else {
			if (collision.gameObject.tag == "Floor") {

				if (_audio_control.instance._play_sound ()) {
					GetComponent<AudioSource> ().clip = _ball_sounds [0];
					GetComponent<AudioSource> ().Play ();
				}
			}
		}

		//---------------------------------------	
	}

	//---------------------------------------	

	void FixedUpdate(){
		float _val = this.transform.position.y - _shadow_distance;
		_val = _val / 15f;
		_shadow.position = new Vector3 (this.transform.position.x,-_shadow_distance,this.transform.position.z);
		_shadow.localScale = new Vector3 (_val,_val/2,_val);

		_shadow.GetComponent<SpriteRenderer> ().color = new Color (_shadow.GetComponent<SpriteRenderer> ().color.r,_shadow.GetComponent<SpriteRenderer> ().color.g,_shadow.GetComponent<SpriteRenderer> ().color.b,Mathf.Abs(_val));

		if (!_set) {
			_tball.transform.Rotate(-Vector3.up * 300f * Time.deltaTime);
		}
	}

	//---------------------------------------	

}
