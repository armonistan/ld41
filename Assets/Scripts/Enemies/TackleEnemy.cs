﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleEnemy : Enemy
{
    public int StartTackleDistance = 10;
    public float CHARGE_DURATION = 2f;
    private float _chargeTimer = 0f;
    private float _TACKLE_SPEED = 1f;

    protected override void Start()
    {
        State = States.Pursuing;
    }

    protected override void UpdateMovementSpeed()
    {
        if(State == States.Pursuing && _enemyToPlayerDeltaVector.magnitude > StartTackleDistance)
        {
            SetMovementSpeedToFollowPlayer();
        } else if(State == States.Pursuing)
        {
            _currentSpeedX = 0;
            _currentSpeedY = 0;
            State = States.Charging;
        }
        else if(State == States.Charging && _chargeTimer <= CHARGE_DURATION) {
            _chargeTimer += Time.deltaTime;
        }
        else if(State == States.Charging && _chargeTimer > CHARGE_DURATION){
            State = States.Tackling;
        } else if(State == States.Tackling)
        {
            SetMovementSpeedToTacklePlayer();
        }
    }

    private void SetMovementSpeedToTacklePlayer()
    {
        //left right
        if (_enemyToPlayerDeltaVector.x > 0)
        {
            _currentSpeedX -= _TACKLE_SPEED;
        }
        else if (_enemyToPlayerDeltaVector.x < 0)
        {
             _currentSpeedX += _TACKLE_SPEED;
        }

        //forward backward
        if (_enemyToPlayerDeltaVector.y > 0)
        {
            _currentSpeedY -= _TACKLE_SPEED;
        }
        else if (_enemyToPlayerDeltaVector.y < 0)
        {
             _currentSpeedY += _TACKLE_SPEED;
        }

        Debug.Log(_currentSpeedY);
    }

    protected override void UpdateMovementVector()
    {
        if(State != States.Tackling)
        {
            SetMovementVectorToPursuePlayer();
        }
    }
}
