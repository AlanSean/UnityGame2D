using UnityEngine;
using TMPro;
using WeChatWASM;
public class PlayerController : MonoBehaviour
{
  //跳跃状态
  private bool IsGrounp;
  private int jumpcount = 2;
  // 跳跃按下
  private bool JumpPressed = false;

  // 人物
  private Rigidbody2D rb;
  //动画
  private Animator anim;
  //是否受伤
  private bool IsHurt; // 默认是false


  [Tooltip("人物碰撞体")]
  public Collider2D coll;
  [Tooltip("移动速度")]
  public float speed;
  [Tooltip("跳跃高度")]
  public float jumpforce;
  public Transform groundCheck;
  [Tooltip("地面")]
  public LayerMask ground;
  [Tooltip("樱桃字体")]
  public TMP_Text CherryNum;
  [Tooltip("樱桃计数")]
  public int CherryCount = 0;
  //受伤反弹速度
  [Tooltip("受伤反弹速度")]
  public int HurtSpeed;
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    // coll = GetComponent<Collider2D>();
    // jumpCount = 2;
    // WX.InitSDK((int code) =>
    // {
    //   Debug.Log(code);
    // });
  }

  void FixedUpdate()
  {
    RoleControl();
  }
  void Update()
  {
    if (Input.GetButtonDown("Jump") && jumpcount > 0)
    {
      JumpPressed = true;
    }
  }
  //控制人物移动
  void RoleControl()
  {
    rolemove();
    roleJump();
    SwitchAnim();
  }
  void rolemove()
  {
    if (IsHurt) return;

    float horizontalmove = Input.GetAxisRaw("Horizontal");

    rb.velocity = new Vector2(horizontalmove * speed, rb.velocity.y);

    //角色移动
    if (horizontalmove != 0)
    {
      transform.localScale = new Vector3(horizontalmove, 1, 1);
    }

  }
  void roleJump()
  {
    IsGrounp = coll.IsTouchingLayers(ground);

    if (IsGrounp)
    {
      jumpcount = 2;
    }

    // JumpPressed = jumpcount > 0;
    if (JumpPressed && (IsGrounp || !IsGrounp && jumpcount > 0))
    {
      rb.velocity = new Vector2(rb.velocity.x, jumpforce);
      jumpcount--;
      JumpPressed = false;
    }
  }
  void SwitchAnim()
  {
    if (rb.velocity.y < 0.1f && !IsGrounp)
    {
      // anim.SetBool("falling", true);
    }


    anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

    if (IsGrounp)
    {
      anim.SetBool("falling", false);
      anim.SetBool("idle", true);
    }
    else
    {
      if (rb.velocity.y > 0)
      {
        anim.SetBool("jumping", true);
      }
      else if (rb.velocity.y < 0)
      {
        anim.SetBool("jumping", false);
        anim.SetBool("falling", true);
      }
    }


    //受伤状态重置
    if (!anim.GetBool("jumping") && IsHurt)
    {
      anim.SetBool("hurt", true);
      // anim.SetFloat("running", 0);
      if (Mathf.Abs(rb.velocity.x) < 0.1f)
      {
        anim.SetBool("hurt", false);
        // anim.SetBool("idle", true);
        IsHurt = false;
      }
    }

  }

  //收集物品
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag == "Collection")
    {
      Destroy(collision.gameObject);
      CherryCount += 1;
      CherryNum.text = CherryCount.ToString();
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    //与青蛙碰撞
    EnemyCollision(collision, "Enemy");
  }

  //触碰敌人- 消灭或者受伤
  void TouchEnemy(Collision2D collision)
  {
    if (anim.GetBool("falling"))
    {
      Destroy(collision.gameObject);
      rb.velocity = new Vector2(rb.velocity.x, jumpforce);
      anim.SetBool("jumping", true);
    }
    else if (transform.position.x < collision.gameObject.transform.position.x)
    {
      rb.velocity = new Vector2(-1 * HurtSpeed, rb.velocity.y);
      IsHurt = true;
    }
    else if (transform.position.x > collision.gameObject.transform.position.x)
    {
      rb.velocity = new Vector2(HurtSpeed, rb.velocity.y);
      IsHurt = true;
    }
  }
  //与敌人碰撞
  void EnemyCollision(Collision2D collision, string tag)
  {
    if (collision.gameObject.tag != tag) return;

    TouchEnemy(collision);
  }
}
