using System.Collections;
using System.Collections.Generic;
using Assets.src.gen;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Gen;
using UnityEngine.SceneManagement;

public class PlanExecutor : MonoBehaviour {
    public List<Move> ThePlan;
    public List<Move> TheExecutedPlan;

    public List<EnemyMap> EnemyPrefabs;
    public List<Vector3> SpawnPoints;

    public int Money = 500;

    public PlayerControl Player;
    public Field Field;

    private Camera _camera;

    public int YardageGoal
    {
        get
        {
            return Mathf.CeilToInt(ThePlan.Concat(TheExecutedPlan).Max((Move move) => move.Yard) / (float)Field.YardsPerSection) * Field.YardsPerSection;
        }
    }

	// Use this for initialization
	void Start () {
        if (ThePlan.Count == 0)
        {
            ThePlan = PlanGenerator.CreatePlan();
        }

        _camera = FindObjectOfType<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Player != null)
        {
            if (Player.transform.position.y >= YardageGoal * Field.YardLength)
            {
                GameData.getCurrentPlayer().recordYardsCovered(YardageGoal);
                GameData.getCurrentPlayer().recordTouchdown();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
	}

    private void createEnemy(EnemyType enemy, Vector3 position)
    {
        var foundEnemy = EnemyPrefabs.Find((EnemyMap map) => map.EnemyType == enemy);
        if (foundEnemy != null) { Instantiate(foundEnemy.EnemyPrefab, position + new Vector3(_camera.transform.position.x, _camera.transform.position.y), Quaternion.identity); }
    }
}
