using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleEnemy : Enemy {


    public GameObject Player;
    public int BaseSpeed = 1;

    private float _radAngle = 0;
    private int _currentSpeed = 0;
    private Vector2 _enemyToPlayerDeltaVector = new Vector2();

    public Vector2 Velocity
    {
        get
        {
            return new Vector2(Mathf.Cos(_radAngle), Mathf.Sin(_radAngle)) * _currentSpeed * Time.deltaTime;
        }
        set
        {
            _radAngle = Mathf.Atan2(value.y, value.x);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _enemyToPlayerDeltaVector.x = Player.transform.position.x - this.transform.position.x;
        _enemyToPlayerDeltaVector.y = Player.transform.position.y - this.transform.position.y;
        Velocity = _enemyToPlayerDeltaVector;
        float currentEnemyVectorX = _enemyToPlayerDeltaVector.x;
        float currentEnemyVectorY = _enemyToPlayerDeltaVector.y;
        _currentSpeed = BaseSpeed;//(currentPlayerVectorX != 0 && currentEnemyVectorY != 0) ? PlayerBaseSpeed : PlayerIdleSpeed;
        gameObject.transform.Translate(Velocity);
    }
}
