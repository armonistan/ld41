﻿using System;
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
        }
        else if (State == States.StiffArmedTossed)
        {
            HandlePlayerStiffArmTossed();
        }
        else
        {
            UpdateMovementVector();
            UpdateMovementSpeed();
            gameObject.transform.Translate(Velocity);
        }
    }

    protected virtual void UpdateMovementVector()
    {
        SetMovementVectorToPursuePlayer();
    }

    protected void SetMovementVectorToPursuePlayer()
    {
        var player = FindObjectOfType<PlayerControl>();

        if (player != null)
        {
            _enemyToPlayerDeltaVector.x = player.transform.position.x - this.transform.position.x;
            _enemyToPlayerDeltaVector.y = player.transform.position.y - this.transform.position.y;
            Velocity = _enemyToPlayerDeltaVector;
        }
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
        if (other.tag == "Edge" || other.tag == "Enemy")
        {
            Die();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerControl player; 

        if ((player = other.gameObject.GetComponentInChildren<PlayerControl>()) == null)
        {
            return;
        }

        if (player.State == PlayerControl.States.StiffArming)
        {
            State = States.StiffArmed;
        }
        else if (player.State == PlayerControl.States.Spinning)
        {
            HandlePlayerSpinning();
        }
        else if (player.State == PlayerControl.States.Tackling)
        {
            GameData.getCurrentPlayer().recordBrokenTackle();
            Die();
        }
        else if (State == States.StiffArmed)
        {
            State = States.StiffArmedTossed;
            GameData.getCurrentPlayer().recordStiffArm();
        }
        else if (State != States.StiffArmedTossed)
        {
            HurtPlayer(player);
            Die();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerControl player;

        if ((player = other.gameObject.GetComponentInChildren<PlayerControl>()) == null)
        {
            return;
        }
    }

    protected virtual void HurtPlayer(PlayerControl player)
    {
        if (GameData.getCurrentPlayer().getAbility() == PlayerAbilities.Abilities.GoldenBoy)
        {
            player.BULK -= 2;
        } else
        {
            player.BULK--;
        }
        System.Random r = new System.Random();
        int soundIndex = (r.Next(1, 3));
        Debug.Log(soundIndex);
        player.gameObject.GetComponents<AudioSource>()[soundIndex].Play();
    }

    protected virtual void HandlePlayerStiffArm()
    {
        var player = FindObjectOfType<PlayerControl>();
        gameObject.transform.Translate(player.Velocity);
    }

    protected virtual void HandlePlayerSpinning()
    {
        if (GameData.getCurrentPlayer().getAbility() == PlayerAbilities.Abilities.Blenderman)
        {
            GameData.getCurrentPlayer().recordBrokenTackle();
            Die();
        }
    }

    protected virtual void Die()
    {
        GameData.getCurrentPlayer().recordEnemyDefeated();
        Instantiate(DeadPrefab, new Vector3(transform.position.x, transform.position.y, 1), Quaternion.identity);
        Destroy(gameObject);
    }

    protected virtual void HandlePlayerStiffArmTossed()
    {
        var player = FindObjectOfType<PlayerControl>();
        Debug.Log("tossed");

        if (player != null)
        {
            _enemyToPlayerDeltaVector.x = this.transform.position.x - player.transform.position.x;
            _enemyToPlayerDeltaVector.y = this.transform.position.y - player.transform.position.y;
            Velocity = _enemyToPlayerDeltaVector;
            MAX_SPEED = _SPEED_INCREASE = player.BULK * 3;
            SetMovementSpeedToFollowPlayer();
            gameObject.transform.Translate(Velocity);
        }
    }
}
