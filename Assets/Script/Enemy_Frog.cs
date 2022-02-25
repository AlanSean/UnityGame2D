using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : MonoBehaviour
{
  private bool IsGrounp;
  private Rigidbody2D rb;
  private bool FaceLeft = true;
  [Tooltip("青蛙左右边界")]
  private float leftx, rightx;
  [Tooltip("青蛙动画")]
  private Animator anim;


  [Tooltip("青蛙的移动速度")]
  public float Speed;
  [Tooltip("青蛙的跳跃速度")]
  public float JumpForce;
  [Tooltip("青蛙左右边界体")]
  public Transform leftpoint, rightpoint;
  [Tooltip("青蛙的碰撞体")]
  public Collider2D coll;
  [Tooltip("地面")]
  public LayerMask ground;
  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    coll = GetComponent<Collider2D>();
    leftx = leftpoint.position.x;
    rightx = rightpoint.position.x;

    Destroy(leftpoint.gameObject);
    Destroy(rightpoint.gameObject);
  }

  // Update is called once per frame
  void Update()
  {
    switchAnim();
  }

  void Movement()
  {

    if (IsGrounp)
    {
      anim.SetBool("jumping", true);
      rb.velocity = new Vector2(FaceLeft ? -Speed : Speed, JumpForce);
    }


    if (transform.position.x <= leftx)
    {
      rb.velocity = new Vector2(Speed, JumpForce);
      transform.localScale = new Vector3(-1, 1, 1);
      FaceLeft = false;
    }
    else if (transform.position.x >= rightx)
    {
      rb.velocity = new Vector2(-Speed, JumpForce);
      transform.localScale = new Vector3(1, 1, 1);
      FaceLeft = true;
    }
  }

  void switchAnim()
  {
    IsGrounp = coll.IsTouchingLayers(ground);
    if (anim.GetBool("jumping"))
    {
      if (rb.velocity.y < 0.1f)
      {
        anim.SetBool("jumping", false);
        anim.SetBool("falling", true);
      }
    }
    else if (anim.GetBool("falling") && IsGrounp)
    {
      anim.SetBool("falling", false);
    }
  }
}
