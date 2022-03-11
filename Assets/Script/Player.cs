using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  private static Player _instance;
  public static Player instance
  {
    get
    {
      if (_instance) return _instance;
      _instance = GameObject.FindObjectOfType<Player>();
      return _instance;
    }
  }

  public Sprite PointSprite;
  [Range(0, 100)]
  public int TrajectoryPoints = 20;
  public GameObject PointsPrefab;
  public GameObject BallPrefab;
  public GameObject TraParent;
  List<GameObject> Trajectorys = new List<GameObject>();
  private GameObject Ball;
  private GameObject Points;
  private float timeStamp;
  private float dotSpacing;
  private Vector3 dotPos;
  void Awake()
  {
    TrajectoryPointsInit();
  }



  //指示器初始化
  private void TrajectoryPointsInit()
  {
    float scaleFactor = 1f / TrajectoryPoints;
    float scale = 1f;
    for (int i = 0; i < TrajectoryPoints; i++)
    {
      Points = (GameObject)Instantiate(PointsPrefab);
      Points.transform.parent = TraParent.transform;
      Points.transform.localScale = Vector3.one * scale;
      scale -= scaleFactor;
      Trajectorys.Insert(i, Points);
    }
  }

  //更新指示器
  public void UpdateTrajectoryPoints(Vector3 BallPos, Vector3 pushSpeed)
  {
    timeStamp = 0.08f;
    for (int i = 0; i < TrajectoryPoints; ++i)
    {
      dotPos.x = BallPos.x + pushSpeed.x * timeStamp;
      dotPos.y = (BallPos.y + pushSpeed.y * timeStamp) - 0.5f * Physics2D.gravity.magnitude * timeStamp * timeStamp;
      Trajectorys[i].transform.position = new Vector3(dotPos.x, dotPos.y, 0f);
      timeStamp += 0.05f;
    }
  }
  public void ShowPoints()
  {
    for (int i = 0; i < TrajectoryPoints; i++)
    {
      // Trajectorys[i].transform.position = new Vector3(0, 0, 0);
      Trajectorys[i].GetComponent<Points>().ShowPoints();
    }
  }
  public void HidePoints()
  {
    for (int i = 0; i < TrajectoryPoints; i++)
    {
      Trajectorys[i].GetComponent<Points>().HidePoints();
    }
  }
}
