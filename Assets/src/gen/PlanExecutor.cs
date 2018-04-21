using System.Collections;
using System.Collections.Generic;
using Assets.src.gen;
using UnityEngine;
using System.Linq;

public class PlanExecutor : MonoBehaviour {
    public List<Move> ThePlan;
    public List<Move> TheExecutedPlan;

    public List<EnemyMap> EnemyPrefabs;

    public int CurrentYard; //TODO: Get this from a player object

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Move move in ThePlan)
        {
            if (move.Yard <= CurrentYard)
            {
                foreach (EnemyType enemy in move.Enemies)
                {
                    createEnemy(enemy);//TODO: Place this logically
                }

                TheExecutedPlan.Add(move);
            }
        }

        var completedMoves = TheExecutedPlan.Intersect(ThePlan);
        ThePlan.RemoveAll((Move move) => completedMoves.Contains(move));
	}

    private void createEnemy(EnemyType enemy)
    {
        var foundEnemy = EnemyPrefabs.Find((EnemyMap map) => map.EnemyType == enemy);
        if (foundEnemy != null) { Instantiate(foundEnemy.EnemyPrefab); }
    }
}
