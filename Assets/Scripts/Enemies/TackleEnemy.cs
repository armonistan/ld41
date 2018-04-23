using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleEnemy : Enemy
{
    public int StartTackleDistance = 10;
    public float CHARGE_DURATION = 2f;
    public float TackleSpeed = 1f;

    private float _chargeTimer = 0f;
    private Animator _animationController;

    protected override void Start()
    {
        _animationController = GetComponent<Animator>();
        State = States.Pursuing;
    }

    protected override void UpdateMovementSpeed()
    {
        if(State == States.Pursuing && _enemyToPlayerDeltaVector.magnitude > StartTackleDistance)
        {
            SetMovementSpeedToFollowPlayer();
        }
        else if(State == States.Pursuing)
        {
            _currentSpeedX = 0;
            _currentSpeedY = 0;

            _animationController.SetTrigger("charging");

            State = States.Charging;
        }
        else if(State == States.Charging && _chargeTimer <= CHARGE_DURATION) {
            _chargeTimer += Time.deltaTime;
        }
        else if(State == States.Charging && _chargeTimer > CHARGE_DURATION)
        {
            _animationController.SetTrigger("running");
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
            _currentSpeedX -= TackleSpeed;
        }
        else if (_enemyToPlayerDeltaVector.x < 0)
        {
             _currentSpeedX += TackleSpeed;
        }

        //forward backward
        if (_enemyToPlayerDeltaVector.y > 0)
        {
            _currentSpeedY -= TackleSpeed;
        }
        else if (_enemyToPlayerDeltaVector.y < 0)
        {
             _currentSpeedY += TackleSpeed;
        }
    }

    protected override void UpdateMovementVector()
    {
        if(State != States.Tackling)
        {
            SetMovementVectorToPursuePlayer();
        }
    }
}
