using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public Sprite PointSprite;
  [Range(0, 30)]
  public int TrajectoryPoints = 20;
  public GameObject PointsPrefab;
  public GameObject BallPrefab;
  List<GameObject> Trajectorys = new List<GameObject>();
  private GameObject Ball;
  private GameObject Points;
  void Awake()
  {
    GameObject TraParent = new GameObject();
    TraParent.name = "Trajectorys";
    TraParent.transform.position = new Vector3(0, 0, 0);

    for (int i = 0; i < TrajectoryPoints; i++)
    {
      Points = (GameObject)Instantiate(PointsPrefab);
      Points.transform.parent = TraParent.transform;
      Trajectorys.Insert(i, Points);
    }
  }
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}
