using UnityEngine;
using System.Collections;

public class _obstacle_options : MonoBehaviour {
	public bool _destroy_ball_on_touch_me,_destroy_me_on_touch,_play_sound_on_touch;
	public GameObject _particles_destroy_touch = null;

	public void chek_action () {
		//---------------------------------------	
		if (_play_sound_on_touch) {
			
			if (_audio_control.instance._play_sound ()) {
				GetComponent<AudioSource> ().Play ();
			}

		}

		//---------------------------------------	
		if (_destroy_ball_on_touch_me) { // Destroy ball on touch obstacle
			_Player.instance._destroyballobstacles();
		}
		//---------------------------------------	
		if (_destroy_me_on_touch) { // Destroy obstacle on touch
			//---------------------------------------	
			if (_particles_destroy_touch != null) {
				Instantiate(_particles_destroy_touch,this.transform.position,Quaternion.identity);
			}
			//---------------------------------------	

			if (this.transform.parent) {
				
				if(this.transform.parent.GetComponent<_rotate_obstacles> ()){
					this.transform.parent.GetComponent<_rotate_obstacles> ()._objets.Remove (this.transform);
				}

			}

			Destroy(this.gameObject);
		}

		//---------------------------------------	
	}
}
