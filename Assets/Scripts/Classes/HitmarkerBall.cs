using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitmarkerBall : Ball {

    public GameObject Hitmarker { get; set; }
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

    }

    public void SetHitmarker(GameObject hitmarker)
    {
        Hitmarker = hitmarker;
    }

    public override bool CheckPlayerCollison(IPlayer player, ref Vector2 hit)
    {
        bool playerCollision = base.CheckPlayerCollison(player, ref hit);
        if (playerCollision)
        {
            CreateHitmarker(hit);
        }
        return playerCollision;
    }

    public override EdgeCollision CheckMapEdgeCollision(IMap map)
    {
        EdgeCollision edgeCollision = base.CheckMapEdgeCollision(map); //Call base method, which keeps ball in bounds and reverses direction
        if (edgeCollision == EdgeCollision.Top)
        {
            CreateHitmarker(new Vector2(Bounds.center.x, Bounds.max.y));
        }
        else if (edgeCollision == EdgeCollision.Bottom)
        {
            CreateHitmarker(new Vector2(Bounds.center.x, Bounds.min.y));
        }
        return edgeCollision;
    }


    public void CreateHitmarker(Vector2 position)
    {
        GameObject newHitmarker = GameObject.Instantiate(Hitmarker, position, Quaternion.identity);
        newHitmarker.transform.LookAt(newHitmarker.transform.position + Camera.main.transform.rotation * Vector3.down,
                         Camera.main.transform.rotation * Vector3.back);
        Object.Destroy(newHitmarker, 0.3f);
    }
}
