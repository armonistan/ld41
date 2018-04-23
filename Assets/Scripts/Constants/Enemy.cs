using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

[Serializable]
public enum EnemyType
{
    CraftyBoi
}

[Serializable]
public class EnemyMap
{
    public EnemyType EnemyType;
    public Enemy EnemyPrefab;
}

public class Enemy : StatefulMonoBehavior<Enemy.States>
{
    public enum States {
        Pursuing,
        Charging,
        Tackling,
        StiffArmed,
        StiffArmedTossed
    }

    public float MAX_SPEED = 1f;
    public float STIFF_ARM_TOSS_DURATION = 10f;
    private float _radAngle = 0;
    protected float _SPEED_DECAY = .05f;
    protected float _SPEED_INCREASE = .2f;
    protected float _currentSpeedX = 0;
    protected float _currentSpeedY = 0;
    protected Vector2 _enemyToPlayerDeltaVector = new Vector2();

    public GameObject DeadPrefab;

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
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (State == States.StiffArmed)
        {
            HandlePlayerStiffArm();
        } else if (State == States.StiffArmedTossed)
        {
            HandlePlayerStiffArmTossed();
        }
        else
        {
            UpdateMovementVector();
            UpdateMovementSpeed();
        }
        gameObject.transform.Translate(Velocity);
    }

    protected virtual void UpdateMovementVector()
    {
        SetMovementVectorToPursuePlayer();
    }

    protected void SetMovementVectorToPursuePlayer()
    {
        var player = FindObjectOfType<PlayerControl>();

        _enemyToPlayerDeltaVector.x = player.transform.position.x - this.transform.position.x;
        _enemyToPlayerDeltaVector.y = player.transform.position.y - this.transform.position.y;
        Velocity = _enemyToPlayerDeltaVector;
    }

    protected virtual void UpdateMovementSpeed()
    {
        SetMovementSpeedToFollowPlayer();
    }

    protected void SetMovementSpeedToFollowPlayer()
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

    void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerControl player; 

        if ((player = other.gameObject.GetComponentInChildren<PlayerControl>()) == null)
        {
            return;
        }

        State = (player.State == PlayerControl.States.StiffArming) ? States.StiffArmed : State;

        if (player.State == PlayerControl.States.Spinning)
        {
            HandlePlayerSpinning();
        }
        else if (player.State == PlayerControl.States.Tackling)
        {
            HandlePlayerTackle();
        }
        else
        {
            HurtPlayer(player);
            HandlePlayerTackle();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerControl player;

        if ((player = other.gameObject.GetComponentInChildren<PlayerControl>()) == null)
        {
            return;
        }

        State = (State == States.StiffArmed) ? States.StiffArmedTossed : State;
    }

    protected virtual void HurtPlayer(PlayerControl player)
    {
        player.BULK--;
    }

    protected virtual void HandlePlayerStiffArm()
    {
        Debug.Log("Being Stiff Armed");
    }

    protected virtual void HandlePlayerSpinning()
    {
        Debug.Log("He is Spinning Two FAST");
    }

    protected virtual void HandlePlayerTackle()
    {
        Instantiate(DeadPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected virtual void HandlePlayerStiffArmTossed()
    {
        Debug.Log("He tossed me?");
    }
}
