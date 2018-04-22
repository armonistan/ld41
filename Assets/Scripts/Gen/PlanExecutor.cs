using System.Collections;
using System.Collections.Generic;
using Assets.src.gen;
using UnityEngine;
using System.Linq;

public class PlanExecutor : MonoBehaviour {
    public List<Move> ThePlan;
    public List<Move> TheExecutedPlan;

    public List<EnemyMap> EnemyPrefabs;
    public List<Vector3> SpawnPoints;

    public PlayerControl Player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Move move in ThePlan)
        {
            if (move.Yard <= Player.transform.position.y)
            {
                for (var i = 0; i < move.Enemies.Length; i++)
                {
                    createEnemy(move.Enemies[i], SpawnPoints[i % SpawnPoints.Count]);
                }

                TheExecutedPlan.Add(move);
            }
        }

        var completedMoves = TheExecutedPlan.Intersect(ThePlan);
        ThePlan.RemoveAll((Move move) => completedMoves.Contains(move));
	}

    private void createEnemy(EnemyType enemy, Vector3 position)
    {
        var foundEnemy = EnemyPrefabs.Find((EnemyMap map) => map.EnemyType == enemy);
        if (foundEnemy != null) { Instantiate(foundEnemy.EnemyPrefab, position + Player.transform.position, Quaternion.identity); }
    }
}
