﻿using System.Collections;
using System.Collections.Generic;
using Assets.src.gen;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Constants;

public class PlanExecutor : MonoBehaviour {
    public List<Move> ThePlan;
    public List<Move> TheExecutedPlan;

    public List<EnemyMap> EnemyPrefabs;
    public List<Vector3> SpawnPoints;

    public PlayerControl Player;
    public Field Field;

    public int YardageGoal
    {
        get
        {
            return ThePlan.Concat(TheExecutedPlan).Max((Move move) => move.Yard) + Environment.SCORE_DISTANCE;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Player.transform.position.y >= YardageGoal * Field.YardLength)
        {
            Debug.Log("Win");
            //TODO: Actually Win
        }

		foreach (Move move in ThePlan)
        {
            if (move.Yard <= Player.transform.position.y / Field.YardLength)
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
