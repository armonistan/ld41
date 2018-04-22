using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleEnemy : Enemy {
    public GameObject Player;
    public int MAX_SPEED = 1;
    private float _SPEED_DECAY = .05f;
    private float _SPEED_INCREASE = .2f;

    private float _radAngle = 0;
    private float _currentSpeedX = 0;
    private float _currentSpeedY = 0;
    private Vector2 _enemyToPlayerDeltaVector = new Vector2();

    public Vector2 Velocity
    {
        get
        {
            return new Vector2(Mathf.Cos(_radAngle) * Math.Abs(_currentSpeedX), Mathf.Sin(_radAngle) * Math.Abs(_currentSpeedY)) * Time.deltaTime;
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
        UpdateMovementVector();
        UpdateMovementSpeed();
        gameObject.transform.Translate(Velocity);
    }

    void UpdateMovementVector()
    {
        _enemyToPlayerDeltaVector.x = Player.transform.position.x - this.transform.position.x;
        _enemyToPlayerDeltaVector.y = Player.transform.position.y - this.transform.position.y;
        Velocity = _enemyToPlayerDeltaVector;
    }

    void UpdateMovementSpeed()
    {
        //left right
        if (_enemyToPlayerDeltaVector.x > 0)
        {
            if (_currentSpeedX > 0)
            {
                _currentSpeedX -= _SPEED_DECAY + _SPEED_INCREASE;
            }
            else if (Math.Abs(_currentSpeedX) < MAX_SPEED)
            {
                _currentSpeedX -= _SPEED_INCREASE;
            }
        }
        else if (_enemyToPlayerDeltaVector.x < 0)
        {
            if (_currentSpeedX < 0)
            {
                _currentSpeedX += _SPEED_DECAY + _SPEED_INCREASE;
            }
            else if (Math.Abs(_currentSpeedX) < MAX_SPEED)
            {
                _currentSpeedX += _SPEED_INCREASE;
            }
        }
        else
        {
            if (_currentSpeedX < 0)
            {
                _currentSpeedX += _SPEED_DECAY;
            }
            else if (_currentSpeedX > 0)
            {
                _currentSpeedX -= _SPEED_DECAY;
            }
        }

        //forward backward
        if (_enemyToPlayerDeltaVector.y > 0)
        {
            if (_currentSpeedY > 0)
            {
                _currentSpeedY -= _SPEED_DECAY + _SPEED_INCREASE;
            }
            else if (Math.Abs(_currentSpeedY) < MAX_SPEED)
            {
                _currentSpeedY -= _SPEED_INCREASE;
            }
        }
        else if (_enemyToPlayerDeltaVector.y < 0)
        {
            if (_currentSpeedY < 0)
            {
                _currentSpeedY += _SPEED_DECAY + _SPEED_INCREASE;
            }
            else if (Math.Abs(_currentSpeedY) < MAX_SPEED)
            {
                _currentSpeedY += _SPEED_INCREASE;
            }
        }
        else
        {
            if (_currentSpeedY < 0)
            {
                _currentSpeedY += _SPEED_DECAY;
            }
            else if (_currentSpeedY > 0)
            {
                _currentSpeedY -= _SPEED_DECAY;
            }
        }
    }
}
