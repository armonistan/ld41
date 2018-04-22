using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleEnemy : Enemy
{
    public int StartTackleDistance = 10;
    public float CHARGE_DURATION = 2f;
    private float _chargeTimer = 0f;

    private void Start()
    {
        State = States.Pursuing;
    }

    protected override void UpdateMovementSpeed()
    {
        if(State == States.Pursuing && _enemyToPlayerDeltaVector.magnitude > StartTackleDistance)
        {
            FollowPlayer();
        } else if(State == States.Pursuing)
        {
            State = States.Charging; 
            _currentSpeedX = 0;
            _currentSpeedY = 0;
        }
        else if(State == States.Charging && _chargeTimer <= CHARGE_DURATION) {
            _chargeTimer += Time.deltaTime;
        }
        else if(State == States.Charging && _chargeTimer > CHARGE_DURATION){
            _SPEED_INCREASE = 20f;
            FollowPlayer();
            State = States.Tackling;
        }
    }

    protected override void UpdateMovementVector()
    {
        if(State != States.Tackling)
        {
            base.UpdateMovementVector();
        }
    }
}
