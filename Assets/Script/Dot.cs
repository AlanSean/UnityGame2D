using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
  [SerializeField] public int pointAmount = 100;
  private LineRenderer lineRenderer;
  private float timeStamp;
  private float dotSpacing;
  private Vector3 dotPos;
  void Start()
  {
    lineRenderer = GetComponent<LineRenderer>();
    lineRenderer.positionCount = pointAmount;
    dotSpacing = 1 / (float)pointAmount;
  }
  public void UpdateLine(Vector2 BallPos, Vector2 pushSpeed)
  {
    for (int i = 0; i < pointAmount; ++i)
    {
      timeStamp = i / (float)pointAmount;
      dotPos.x = BallPos.x + pushSpeed.x * timeStamp;
      dotPos.y = (BallPos.y + pushSpeed.y * timeStamp) - 0.5f * Physics2D.gravity.magnitude * timeStamp * timeStamp;
      lineRenderer.SetPosition(i, dotPos);
    }
  }
}