using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
  // Start is called before the first frame update
  public SpriteRenderer _sprite;
  private bool hidden = true;

  void Awake()
  {
    _sprite.sprite = Player.instance.PointSprite;
    _sprite.sortingLayerName = "player";
    _SetSpriteOpacity(0);
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.name == "Ball") HidePoints();
  }
  ///
  /// <summary>
  /// 设置Sprite的透明度
  /// </summary>
  /// <param name="a">透明度 </param>
  private void _SetSpriteOpacity(float a)
  {
    _sprite.color = new Color(_sprite.color.r, _sprite.color.b, _sprite.color.g, a);
  }
  private IEnumerator _Fade()
  {
    while (_sprite.color.a > 0f)
    {
      yield return null;
      _SetSpriteOpacity(_sprite.color.a - 0.1f);
    }
  }

  public void HidePoints()
  {
    if (hidden) return;
    hidden = true;
    StartCoroutine("_Fade");
  }
  public void ShowPoints()
  {
    if (!hidden) return;
    hidden = false;
    StopCoroutine("_Fade");
    _SetSpriteOpacity(1f);
  }
}
