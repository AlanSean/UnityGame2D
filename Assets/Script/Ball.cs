using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class Ball : MonoBehaviour
{
  [Header("速度系数")]
  /// <summary>
  /// 速度系数
  /// </summary>
  public float SpeedFactor = 5f;
  [Tooltip("指示器的精灵")]
  public Sprite PointSprite;
  public float ballMaxVector = 3;
  public float ballminVector = 1;
  private Rigidbody2D rb;
  private bool isTouchDown = false;
  //出发点
  private Vector3 StartPoint;
  // 触摸结束点
  private Vector3 EndPoint;
  private float Distance;
  private Vector3 VelocityVector;
  //推进速度
  private Vector3 PushSpeed;

  private List<GameObject> trajectoryPoints;

  void Awake()
  {
    trajectoryPoints = new List<GameObject>();
    GameObject _parent_tp = new GameObject();
  }
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    rb.isKinematic = true;
    isTouchDown = true;
  }

  void OnMouseDown()
  {
    Debug.Log("down");
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

    Debug.Log("move" + PushSpeed);
    // dot.UpdateLine(transform.position, PushSpeed);
  }

  void OnMouseUp()
  {
    rb.isKinematic = false;
    //添加脉冲
    rb.AddForce(PushSpeed, ForceMode2D.Impulse);
    Debug.Log("up" + PushSpeed);
  }
}
