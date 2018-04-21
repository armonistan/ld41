using System.Collections;
using System.Collections.Generic;
using Assets.src.gen;
using UnityEngine;

public class PlanExecutor : MonoBehaviour {
    public Move[] ThePlan;
    public Move[] TheExecutedPlan;
    public int CurrentYard; //TODO: Get this from a player object

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Move move in ThePlan)
        {
            if (move.Yard >= CurrentYard)
            {
                foreach (EnemyType enemy in move.Enemies)
                {
                    ///TODO: Create the enemies
                }
            }
        }
	}

    private void createEnemy(EnemyType enemy)
    {

    }
}
