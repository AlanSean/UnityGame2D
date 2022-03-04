using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
  // Start is called before the first frame update
  public SpriteRenderer _sprite;
  void Awake()
  {
    _sprite.sprite = GetComponent<Control>().PointSprite;
    Debug.Log(GetComponent<Control>().PointSprite);
  }

  // Update is called once per frame
  void Update()
  {

  }

  void OnTriggerEnter(Collider other)
  {
    if (other.name == "Ball")
    {
      StartCoroutine("_Fade");
    }
  }

  private void _SetSpriteColor(float a)
  {
    _sprite.color = new Color(_sprite.color.r, _sprite.color.b, _sprite.color.g, a);
  }
  private IEnumerator _Fade()
  {
    if (_sprite.color.a > 0f)
    {
      yield return null;
      _SetSpriteColor(_sprite.color.a - 0.1f);
    }
  }

  public void HidePoints()
  {
    StartCoroutine("_Fade");
  }
  public void ShowPoints()
  {
    StopCoroutine("_Fade");
    _SetSpriteColor(1f);
  }
}
