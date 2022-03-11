using UnityEngine;
public class Ball : MonoBehaviour
{
  [Header("速度系数")]
  /// <summary>
  /// 速度系数
  /// </summary>
  public float SpeedFactor = 15f;
  [Tooltip("指示器的精灵")]
  public Sprite PointSprite;
  public float ballMaxVector = 3;
  public float ballminVector = 1;


  private Player player;
  private Rigidbody rb;
  private bool isTouchDown = false;
  //出发点
  private Vector3 StartPoint;
  // 触摸结束点
  private Vector3 EndPoint;
  private float Distance;
  private Vector3 VelocityVector;
  //推进速度
  private Vector3 PushSpeed;

  void Awake()
  {
    player = Player.instance;
  }
  void Start()
  {
    rb = GetComponent<Rigidbody>();
    rb.isKinematic = true;
    isTouchDown = true;
  }

  void OnMouseDown()
  {
    StartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
  }
  void OnMouseDrag()
  {
    // if (!isTouchDown) return;
    EndPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //计算拖动的距离
    Distance = Vector3.Distance(StartPoint, EndPoint);
    //计算速度向量
    VelocityVector = (StartPoint - EndPoint).normalized;

    //计算推进的速度

    PushSpeed = Distance * VelocityVector * SpeedFactor;

    player.UpdateTrajectoryPoints(transform.position, PushSpeed);
    player.ShowPoints();
  }

  void OnMouseUp()
  {
    /// 拖动大于0.5才能进行发射
    if (Distance > 0.5f)
    {
      rb.isKinematic = false;
      //添加脉冲
      rb.AddForce(PushSpeed.normalized, ForceMode.Impulse);
      rb.AddTorque(PushSpeed.normalized);
    }
    else
    {
      player.HidePoints();
    }

  }

  private void OnCollisionEnter(Collision other)
  {
    if (other.gameObject.tag != "Obstacle")
    {
      player.HidePoints();
    }


    if (other.gameObject.tag == "Floor")
    {
      transform.position = new Vector3(-2.1f, -5f, 0);
      transform.localEulerAngles = new Vector3(0, 0, 0);
      rb.isKinematic = true;
    }
  }
}
