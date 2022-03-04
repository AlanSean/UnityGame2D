using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _Player : MonoBehaviour
{
  private static _Player _instance;
  public static _Player instance
  {
    get
    {
      if (!_instance)
      {
        _instance = GameObject.FindObjectOfType(typeof(_Player)) as _Player;
      }
      return _instance;
    }
  }

  //---------------------------------------
  bool _touch_mode = false;
  //---------------------------------------
  public GameObject TrajectoryPointPrefab;
  public GameObject BallPrefb;
  [Range(0, 30)]
  public int TrajectoryPoints = 20;
  public bool _full_trajectory = false;
  //---------------------------------------	
  GameObject ball = null;
  GameObject _tmpcircle = null;
  List<GameObject> trajectoryPoints;
  //---------------------------------------	
  Vector3 _force;
  //---------------------------------------
  [HideInInspector]
  public bool _moveball, _cparts = true;
  bool _stop_on_dunk = false;
  //---------------------------------------	
  Cloth _basket;
  //---------------------------------------	
  public Transform _basketgo;
  Transform _circleplayer;
  //---------------------------------------	
  int _dunks = 0;
  int _last_anim = -1;
  //---------------------------------------	
  public float _score = 0f;
  //---------------------------------------	
  hud_control _hud;
  _game_options _gameopt;
  _design_control _level_op;
  Rect _pause_zone = new Rect();
  //---------------------------------------	
  _basket_trigger[] _trigg_tmp = new _basket_trigger[2];
  //---------------------------------------
  //---------------------------------------	
  public void _addtriggers(int _i, _basket_trigger _bt)
  {
    _trigg_tmp[_i] = _bt;
  }

  //---------------------------------------	
  void _resettriggerbasket()
  {
    _trigg_tmp[0]._reset();
    _trigg_tmp[1]._reset();
  }
  //---------------------------------------	

  public void _destroyball()
  {
    if (_touch_mode)
    {
      Destroy(GameObject.Find("CTouch"));
    }
    _resettriggerbasket();

    if (ball)
    {
      Destroy(ball.GetComponent<_ball>()._shadow.gameObject);
      Destroy(ball);
    }

  }

  //---------------------------------------	
  public void _startupdate(Transform _b, Sprite _circle = null)
  {
    _basketgo = _b;
  }

  //---------------------------------------	

  void Awake()
  {
    //---------------------------------------
    _touch_mode = _Game_Control.instance.gameObject.GetComponent<_game_options>()._touch_mode;
    //---------------------------------------
    // NORMAL MODE
    //---------------------------------------
    if (!_touch_mode)
    {

      //---------------------------------------
      // TRAJECTORY POINTS
      //---------------------------------------
      trajectoryPoints = new List<GameObject>();
      GameObject _parent_tp = new GameObject();
      _parent_tp.name = "Trajectory_Points";
      _parent_tp.transform.position = new Vector3(0, 0, 0);

      for (int i = 0; i < TrajectoryPoints; i++)
      {
        GameObject D = (GameObject)Instantiate(TrajectoryPointPrefab);
        trajectoryPoints.Insert(i, D);
        D.transform.parent = _parent_tp.transform;
      }
      //---------------------------------------
    }
    _gameopt = GameObject.Find("_GameControl").GetComponent<_game_options>();
    _level_op = GameObject.Find("_GameControl").GetComponent<_design_control>();
    _hud = GameObject.Find("_GameControl").GetComponent<hud_control>();
    _circleplayer = GameObject.Find("_CirclePlayer").transform;
    //---------------------------------------
    if (_touch_mode)
    {
      _circleplayer.gameObject.SetActive(false);
      Debug.Log("FALSEE");
    }
    //---------------------------------------
    _change_gravity();
    _pause_zone = new Rect(Screen.width / 1.1f, 0, Screen.width, Screen.height / 6);
    //---------------------------------------
  }

  //---------------------------------------	

  public void _chek_basket_dats()
  {

    // ONLY FOR CLOTH PHYSICS
    //---------------------------------------
    if (GameObject.Find("_basket_add"))
    {
      _basket = GameObject.Find("_basket_add").GetComponent<Cloth>();
    }
    //---------------------------------------

    if (_level_op._levels[_gameopt._level_p]._Difficulty_levels[_gameopt._difficulty_l]._basket_animation_list.Length > 0)
    {
      _basket_animations();
    }

  }

  //---------------------------------------	

  public void _destroyballobstacles()
  {

    _resettriggerbasket();
    _particle_miss(ball.transform.position);

    Destroy(ball.GetComponent<_ball>()._shadow.gameObject);
    Destroy(ball);

    if (!_Game_Control.instance._is_gameover)
    {
      createBall();
    }
  }
  //---------------------------------------	
  void _change_gravity()
  {
    Physics.gravity = new Vector3(0f, -_level_op._levels[_gameopt._level_p]._gravity, 0f);
  }

  //---------------------------------------	
  void Update()
  {
    // CONTROLS
    //---------------------------------------
    if (Time.timeScale == 1 && !_Game_Control.instance._is_gameover)
    {
      //---------------------------------------
      if (!_touch_mode && _moveball)
      {
        //---------------------------------------
#if UNITY_IOS || UNITY_ANDROID
				//---------------------------------------
				if (Input.GetMouseButton (0) && !_pause_zone.Contains (Input.mousePosition)) { // Click or Touch in screen

				Vector3 vel = GetForce (this.transform.position, _camera_return ());
				this.transform.eulerAngles = new Vector3 (0, 0, Mathf.Atan2 (vel.y, vel.x) * Mathf.Rad2Deg);
				setTrajectory (this.transform.position, vel / ball.GetComponent<Rigidbody> ().mass, _camera_return ());
				_force = vel;

				} else if (Input.GetMouseButtonUp (0)) {
				_moveball = false;
				throwBall ();
				}
				//---------------------------------------
#else
        //---------------------------------------
        if (Input.GetMouseButtonDown(0) && !_pause_zone.Contains(Input.mousePosition))
        { // Click or Touch in screen
          _moveball = false;
          throwBall();
        }
        //---------------------------------------
        Vector3 vel = GetForce(this.transform.position, _camera_return());
        this.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg);
        setTrajectory(this.transform.position, vel / ball.GetComponent<Rigidbody>().mass, _camera_return());
        _force = vel;
#endif
        //---------------------------------------
      }
      else
      {
        //---------------------------------------
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        //---------------------------------------
        if (Input.GetMouseButton(0))
        {
          if (Physics.Raycast(ray, out hit))
          {
            if (hit.collider.name == "CTouch")
            {
              if (hit.point.x < hit.collider.transform.position.x)
              {
                hit.collider.GetComponent<_followball>()._ball.GetComponent<Rigidbody>().AddForce(_Game_Control.instance._power_click_on_touch_mode, ForceMode.Impulse);
              }
              else
              {
                hit.collider.GetComponent<_followball>()._ball.GetComponent<Rigidbody>().AddForce(new Vector3(-_Game_Control.instance._power_click_on_touch_mode.x, _Game_Control.instance._power_click_on_touch_mode.y, 0), ForceMode.Impulse);
              }
            }
          }
        }
        //---------------------------------------
      }
      //---------------------------------------
    }

    //---------------------------------------
  }

  //---------------------------------------

  Vector3 _camera_return()
  {
    return Camera.main.ScreenToWorldPoint(Input.mousePosition);
  }

  //---------------------------------------

  public void createBall(float _t = 0f)
  {
    ball = (GameObject)Instantiate(BallPrefb);
    ball.name = "Ball";
    Vector3 pos = transform.position;
    pos.z = 1;
    //---------------------------------------
    if (_touch_mode)
    {
      pos = new Vector3(Random.Range(-11, 11), 8, 1);
      ball.GetComponent<Rigidbody>().useGravity = true;
      ball.GetComponent<Rigidbody>().drag = 1.5f;
      ball.GetComponent<Rigidbody>().angularDrag = 1.5f;
      ball.GetComponent<CapsuleCollider>().enabled = true;
      Physics.queriesHitTriggers = true;
      GameObject _c = Instantiate(Resources.Load("_Circletouch")) as GameObject;
      _c.GetComponent<SpriteRenderer>().sprite = _Game_Control.instance._sprt_player;
      _c.name = "CTouch";
      _c.GetComponent<_followball>()._ball = ball.transform;
    }
    //---------------------------------------
    ball.transform.position = pos;
    ball.GetComponent<_ball>()._tball.GetComponent<Renderer>().material = _Game_Control.instance._ball_material;
    //---------------------------------------
    // ONLY FOR CLOTH PHYSICS
    //---------------------------------------
    if (_basket != null)
    {
      CapsuleCollider[] tmp = new CapsuleCollider[1];
      tmp[0] = ball.GetComponent<CapsuleCollider>();
      _basket.capsuleColliders = tmp;
    }
    //---------------------------------------

    _cparts = true;

    if (!_touch_mode)
    {
      if (_t == 0f)
      {
        _moveball = true;
      }
      else
      {
        _moveball = false;
        StartCoroutine(_reset_time_ball(_t));
      }

      resetrajectory(); // Reset alpha color
    }
    //---------------------------------------
  }
  //---------------------------------------

  IEnumerator _reset_time_ball(float _t)
  {
    yield return new WaitForSeconds(_t);
    _moveball = true;
  }

  //---------------------------------------

  void throwBall()
  {
    ball.GetComponent<Rigidbody>().useGravity = true;
    ball.GetComponent<Rigidbody>().AddForce(_force, ForceMode.Impulse);
    ball.GetComponent<CapsuleCollider>().enabled = true;
  }

  //---------------------------------------	
  Vector3 GetForce(Vector3 fromPos, Vector3 toPos)
  {
    Vector3 _t = toPos - fromPos;
    _t.y = _t.y + Physics.gravity.magnitude / 2;
    _t.y = _t.y * ball.GetComponent<Rigidbody>().mass;
    _t.x = _t.x * ball.GetComponent<Rigidbody>().mass;
    return _t;
  }

  //---------------------------------------	

  void resetrajectory()
  {
    for (int i = 0; i < TrajectoryPoints; i++)
    {
      trajectoryPoints[i].transform.position = new Vector3(0, 500, 0);
      trajectoryPoints[i].GetComponent<_trigger_dot>()._resetalpha();
    }
  }

  //---------------------------------------	

  public void _hidden_trajectory()
  {
    if (!_touch_mode)
    {
      for (int i = 0; i < TrajectoryPoints; i++)
      {
        trajectoryPoints[i].GetComponent<_trigger_dot>()._hiddenfast();
      }
    }
  }


  //---------------------------------------	
  void setTrajectory(Vector3 pStartPosition, Vector3 pVelocity, Vector3 _mouseposition)
  {
    float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
    float angle = Mathf.Rad2Deg * (Mathf.Atan2(pVelocity.y, pVelocity.x));
    float fTime = 0;

    fTime += 0.08f;
    for (int i = 0; i < TrajectoryPoints; i++)
    {
      float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
      float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics.gravity.magnitude * fTime * fTime / 2.0f);
      Vector3 pos = new Vector3(pStartPosition.x + dx, pStartPosition.y + dy, 1.75f);

      trajectoryPoints[i].transform.position = pos;
      trajectoryPoints[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude) * fTime, pVelocity.x) * Mathf.Rad2Deg);
      fTime += 0.08f;
    }
  }
  //---------------------------------------	

  public void newplay()
  {
    if (ball)
    {
      StartCoroutine(_createnewball(_gameopt._time_to_reset_ball));
    }

    if (!ball)
    {
      createBall();
    }
  }

  //---------------------------------------	
  IEnumerator _createnewball(float _t = 3f)
  {
    yield return new WaitForSeconds(_t);

    if (ball)
    {
      _particle_miss(ball.transform.position);
      _destroyball();
    }

    if (!_Game_Control.instance._is_gameover)
    {
      createBall();
    }

  }

  //---------------------------------------

  Vector3 _rand_player_p()
  {
    //---------------------------------------
    Vector3 _r = this.transform.position;
    //---------------------------------------
    if (_level_op._levels[_gameopt._level_p]._Difficulty_levels[_gameopt._difficulty_l]._player_always_left)
    {
      _r = new Vector3(Random.Range(-10, -2f), Random.Range(-4, 4f), 0f);
    }
    else
    {
      _r = new Vector3(Random.Range(-10, 10f), Random.Range(-4, 4f), 0f);
    }
    //---------------------------------------
    // CHECK POSITION OBSTACLES
    //---------------------------------------
    for (int i = 0; i < _level_op._levels[_gameopt._level_p]._Difficulty_levels[_gameopt._difficulty_l]._obstacles.Length; i++)
    {

      //---------------------------------------
      float _x = _level_op._levels[_gameopt._level_p]._Difficulty_levels[_gameopt._difficulty_l]._obstacles[i]._position_x;
      float _y = _level_op._levels[_gameopt._level_p]._Difficulty_levels[_gameopt._difficulty_l]._obstacles[i]._position_y;
      //---------------------------------------

      //---------------------------------------
      if (_r.x < _x + 1.5f && _r.x > _x - 1.5f)
      {

        _rand_player_p();

      }
      else if (_r.y < _y + 1.5f && _r.y > _y - 1.5f)
      {

        _rand_player_p();

      }
      //---------------------------------------

    }

    //---------------------------------------

    return _r;
  }

  //---------------------------------------
  public void _change_position(bool _isball)
  {

    if (_isball)
    {

      // Move Player Position
      //---------------------------------------
      this.transform.position = _rand_player_p();
      _circleplayer.position = this.transform.position;
      //---------------------------------------

    }
    else
    {

      // Move Basket Position
      //---------------------------------------

      _basketgo.gameObject.SetActive(false);

      if (_level_op._levels[_gameopt._level_p]._Difficulty_levels[_gameopt._difficulty_l]._player_always_left)
      {
        _basketgo.position = new Vector3(Random.Range(2, 9f), Random.Range(-2, 3f), 1.78f);
      }
      else
      {

        if (this.transform.position.x <= 0f)
        {
          _basketgo.position = new Vector3(Random.Range(2, 9f), Random.Range(-2, 3f), 1.78f);
        }
        else
        {
          _basketgo.position = new Vector3(Random.Range(-2, -9f), Random.Range(-2, 3f), 1.78f);
        }

      }

      _basketgo.gameObject.SetActive(true);
      //---------------------------------------

    }

  }
  //---------------------------------------	

  // ADD POINT
  //---------------------------------------	

  public void _add_point()
  {

    if (_level_op._levels[_gameopt._level_p]._destroy_ball_on_dunk)
    {
      _destroyball();
    }
    else
    {
      ball.GetComponent<_ball>()._set = true;
    }

    _Game_Control.instance._baskets++;
    _dunks++;
    _score = _score + _gameopt._score_on_dunk;

    //---------------------------------------	
    if (_level_op._levels[_gameopt._level_p]._particles_dunk != null)
    {
      if (_cparts)
      {
        _cparts = false;
        GameObject _part = Instantiate(_level_op._levels[_gameopt._level_p]._particles_dunk, ball.transform.position, Quaternion.identity) as GameObject;
        Destroy(_part, 3f);
      }
    }
    //---------------------------------------	

    _hud.update_score(_score);

    StartCoroutine(check_options());
  }

  //---------------------------------------	

  IEnumerator check_options()
  {
    yield return new WaitForSeconds(1f);

    // CHECK RANDOM POSITIONS
    //---------------------------------------	

    if (_level_op._levels[_gameopt._level_p]._Difficulty_levels[_gameopt._difficulty_l]._random_player_position)
    {
      _change_position(true);
    }

    //---------------------------------------	
    if (_level_op._levels[_gameopt._level_p]._Difficulty_levels[_gameopt._difficulty_l]._random_basket_position)
    {
      _change_position(false);
    }
    //---------------------------------------

    _destroyball();

    // STOP ANIMATIONS
    //---------------------------------------
    if (_stop_on_dunk)
    {
      StopAllCoroutines();

      if (_tmpcircle)
      {
        Destroy(_tmpcircle);
        _tmpcircle = null;
      }
    }
    //---------------------------------------

    // CHECK BASKET ANIMATIONS
    //---------------------------------------
    _basket_animations();

    // CREATE A NEW BALL
    //---------------------------------------
    createBall();
  }

  //---------------------------------------

  void _basket_animations()
  {

    for (int i = 0; i < _level_op._levels[_gameopt._level_p]._Difficulty_levels[_gameopt._difficulty_l]._basket_animation_list.Length; i++)
    {

      if (_level_op._levels[_gameopt._level_p]._Difficulty_levels[_gameopt._difficulty_l]._basket_animation_list[i]._dunks_to_active == _dunks)
      {
        _start_animation(i);
        break;
      }
    }

    //---------------------------------------

    if (!_stop_on_dunk)
    {

      // BASKET ANIMATIONS
      //------------------
      if (_level_op._levels[_gameopt._level_p]._Difficulty_levels[_gameopt._difficulty_l]._basket_animation_list.Length > 0)
      {
        _start_animation(_last_anim);
      }
      //------------------
    }
  }

  //---------------------------------------

  void _start_animation(int _selected)
  {
    StopAllCoroutines();

    if (_tmpcircle)
    {
      Destroy(_tmpcircle);
      _tmpcircle = null;
    }


    // BASKET ANIMATIONS
    //------------------
    if (_level_op._levels[_gameopt._level_p]._Difficulty_levels[_gameopt._difficulty_l]._basket_animation_list.Length > 0)
    {

      if (_selected == -1)
      {
        _selected = 0;
      }

      _stop_on_dunk = _level_op._levels[_gameopt._level_p]._Difficulty_levels[_gameopt._difficulty_l]._basket_animation_list[_selected]._stop_on_dunk;
      _last_anim = _selected;

      float _spd = _speed_a(_level_op._levels[_gameopt._level_p]._Difficulty_levels[_gameopt._difficulty_l]._basket_animation_list[_selected]._speed);
      float _dtc = _distance_a(_level_op._levels[_gameopt._level_p]._Difficulty_levels[_gameopt._difficulty_l]._basket_animation_list[_selected]._distance);


      switch (_level_op._levels[_gameopt._level_p]._Difficulty_levels[_gameopt._difficulty_l]._basket_animation_list[_selected]._type_animation)
      {

        case _animations.Horizontal:
          StartCoroutine(_horizontal_animation(_spd, _dtc));
          break;

        case _animations.Vertical:
          StartCoroutine(_vertical_animation(_spd, _dtc));
          break;

        case _animations.Circle:
          StartCoroutine(_circle_animation(_spd, _dtc));
          break;

      }
    }
    //------------------

  }

  //---------------------------------------

  // BASKET ANIMATIONS
  //---------------------------------------	

  float _speed_a(_speed_anim _s)
  {
    float _r = 0f;

    switch (_s)
    {
      case _speed_anim.Very_Slow:
        _r = 0.01f;
        break;

      case _speed_anim.Slow:
        _r = 0.025f;
        break;

      case _speed_anim.Normal:
        _r = 0.035f;
        break;

      case _speed_anim.Fast:
        _r = 0.055f;
        break;

      case _speed_anim.Very_Fast:
        _r = 0.08f;
        break;

      case _speed_anim.Super_Fast:
        _r = 0.1f;
        break;
    }

    return _r;
  }

  //---------------------------------------	

  float _distance_a(_distance_anim _s)
  {
    float _r = 0f;

    switch (_s)
    {
      case _distance_anim.Very_Short:
        _r = 0.3f;
        break;

      case _distance_anim.Short:
        _r = 0.5f;
        break;

      case _distance_anim.Middle:
        _r = 1f;
        break;

      case _distance_anim.Long:
        _r = 1.5f;
        break;

      case _distance_anim.Very_Long:
        _r = 2f;
        break;
    }

    return _r;
  }


  //---------------------------------------	

  IEnumerator _vertical_animation(float _speed, float _distance)
  {

    float _center = _basketgo.position.y;
    float _top = _center + _distance;
    float _down = _center - _distance;

    //---------------------------------------	

    while (_basketgo.position.y < _top)
    {
      _basketgo.position = new Vector3(_basketgo.position.x, _basketgo.position.y + _speed, _basketgo.position.z);
      yield return new WaitForSeconds(0.01f);

    }

    //---------------------------------------	

    while (_basketgo.position.y > _down)
    {
      _basketgo.position = new Vector3(_basketgo.position.x, _basketgo.position.y - _speed, _basketgo.position.z);
      yield return new WaitForSeconds(0.01f);
    }

    //---------------------------------------	

    while (_basketgo.position.y < _center)
    {
      _basketgo.position = new Vector3(_basketgo.position.x, _basketgo.position.y + _speed, _basketgo.position.z);
      yield return new WaitForSeconds(0.01f);
    }

    //---------------------------------------

    StartCoroutine(_vertical_animation(_speed, _distance));

  }

  //---------------------------------------	

  IEnumerator _horizontal_animation(float _speed, float _distance)
  {

    float _center = _basketgo.position.x;
    float _right = _center + _distance;
    float _left = _center - _distance;

    //---------------------------------------	

    while (_basketgo.position.x < _right)
    {
      _basketgo.position = new Vector3(_basketgo.position.x + _speed, _basketgo.position.y, _basketgo.position.z);
      yield return new WaitForSeconds(0.01f);

    }

    //---------------------------------------	

    while (_basketgo.position.x > _left)
    {
      _basketgo.position = new Vector3(_basketgo.position.x - _speed, _basketgo.position.y, _basketgo.position.z);
      yield return new WaitForSeconds(0.01f);
    }

    //---------------------------------------	

    while (_basketgo.position.x < _center)
    {
      _basketgo.position = new Vector3(_basketgo.position.x + _speed, _basketgo.position.y, _basketgo.position.z);
      yield return new WaitForSeconds(0.01f);
    }

    //---------------------------------------	


    StartCoroutine(_horizontal_animation(_speed, _distance));

  }

  //---------------------------------------	

  IEnumerator _circle_animation(float _speed, float _distance)
  {

    _speed = _speed * 25;
    _distance = _distance * 3;

    Transform _center = new GameObject().transform;
    _center.transform.position = _basketgo.position;

    Transform _pivot = new GameObject().transform;
    _pivot.parent = _center;
    _pivot.transform.localPosition = new Vector3(_distance, 0, 0);

    while (_center.eulerAngles.x < 360f)
    {
      _center.eulerAngles = new Vector3(_center.eulerAngles.x, _center.eulerAngles.y, _center.eulerAngles.z + _speed);
      _basketgo.position = _pivot.position;
      yield return new WaitForSeconds(0.01f);

    }

    //---------------------------------------	
  }

  //---------------------------------------	

  public void _particle_miss(Vector3 _p)
  {
    GameObject _part = Instantiate(_level_op._levels[_gameopt._level_p]._particles_miss, _p, Quaternion.identity) as GameObject;

    if (_audio_control.instance._play_sound())
    {
      _part.GetComponent<AudioSource>().Play();
    }
    Destroy(_part, 3f);
  }

  //---------------------------------------	


}