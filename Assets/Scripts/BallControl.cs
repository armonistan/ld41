﻿using System;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallControl : StatefulMonoBehavior<BallControl.States>
{
    public enum States
    {
        Idle,
        Pause,
        Bounce
    }

	public float DegAngle;
	public float BallSteeringMagnitude = 1;
	public KeyCode LeftBallSteering = KeyCode.A;
	public KeyCode RightBallSteering = KeyCode.D;

    public float Speed = 4;
    public int StrongReflectBonus = 1;
    public int BaseSpeedClass = 1;
    public float[] SpeedClasses;

    public int PauseFrames = 5;

    public PowerupControl.PowerupType PersonalPowerupType;
    public int BounceBonus = 0;

    public int ReflectPoints = 100;
    public int SweetReflectPoints = 300;
    public int ObstaclePoints = 200;
    public int DestroyObstaclePoints = 400;

    public Sprite baseContrail;
    public Sprite leftSteeringContrail;
    public Sprite rightSteeringContrail;

	private SpawnControl _spawnControl;
    private PowerupControl _powerupControl;
    private UiControl _uiControl;

    private Transform _contrail;

    public float RadAngle
    {
        get { return Mathf.Deg2Rad * DegAngle; }
        set { DegAngle = value * Mathf.Rad2Deg; }
    }

	public Vector2 Velocity
    {
        get
        {
            Speed = Mathf.Clamp(SpeedClasses[CurrentSpeedClass], SpeedClasses.First(), SpeedClasses.Last());
            return new Vector2 (Mathf.Cos (RadAngle), Mathf.Sin (RadAngle)) * Speed * Time.deltaTime;
		}
        set
        {
            RadAngle = Mathf.Atan2(value.y, value.x);
        }
    }

    public int CurrentSpeedClass
    {
        get
        {
            var powerupBonus = 0;

            switch (PersonalPowerupType)
            {
                case PowerupControl.PowerupType.Faster:
                    powerupBonus = 1;
                    break;
                case PowerupControl.PowerupType.Slower:
                    powerupBonus = -1;
                    break;
            }

            return Mathf.Clamp(BaseSpeedClass + powerupBonus + BounceBonus, 0, SpeedClasses.Length - 1);
        }
    }

    // Use this for initialization
	void Start () {
		_spawnControl = FindObjectOfType<SpawnControl>();
	    _powerupControl = FindObjectOfType<PowerupControl>();
        _uiControl = FindObjectOfType<UiControl> ();
        _contrail = transform.Find("Contrail");
		DegAngle = UnityEngine.Random.Range (0, 360);

        if (FindObjectOfType<GameControl>().GetGameState() == GameControl.States.EasyMode)
        {
            PauseFrames = 6;
        }
        else if (FindObjectOfType<GameControl>().GetGameState() == GameControl.States.HardMode)
        {
            PauseFrames = 3;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeScale == GameControl.Paused)
	    {
	        return;
	    }

	    if (_contrail)
	    {
	        _contrail.transform.rotation = Quaternion.AngleAxis(DegAngle + 180, Vector3.forward);

	        if (Input.GetKey(LeftBallSteering))
	        {
	            RadAngle += (BallSteeringMagnitude * Time.deltaTime);
	            _contrail.GetComponent<SpriteRenderer>().sprite = leftSteeringContrail;
	        }
	        else if (Input.GetKey(RightBallSteering))
	        {
	            RadAngle -= (BallSteeringMagnitude * Time.deltaTime);
	            _contrail.GetComponent<SpriteRenderer>().sprite = rightSteeringContrail;
	        }
	        else
	        {
	            _contrail.GetComponent<SpriteRenderer>().sprite = baseContrail;
	        }

            _contrail.GetComponent<SpriteRenderer>().color = _powerupControl.GetPowerupColor(PersonalPowerupType);
	    }

        switch (State)
	    {
		case States.Idle:
				gameObject.transform.Translate(Velocity);
	            break;
	        case States.Pause:
                if (PersonalPowerupType != PowerupControl.PowerupType.Shield && Counter > PauseFrames)
	            {
	                Destroy(gameObject);
	            }
	            else
	            {
	                IncrementCounter();
	            }
	            break;
			case States.Bounce:
				gameObject.transform.Translate(Velocity);
                break;
	        default:
	            throw new ArgumentOutOfRangeException();
	    }
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (Time.timeScale == GameControl.Paused)
        {
            return;
        }

        WallControl wall;
        ObstacleControl obs;
        PointManiaControl point;

        if ((wall = other.gameObject.GetComponent<WallControl>()) != null)
        {
            HandleWall(wall);
        }
        else if ((obs = other.GetComponent<ObstacleControl>()) != null)
        {
            HandleObstacle(obs);
        }
        else if ((point = other.GetComponent<PointManiaControl>()) != null)
        {
            HandlePointMania(point);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        WallControl wall;
        ObstacleControl obs;

        if ((wall = other.gameObject.GetComponent<WallControl>()) != null)
        {
            State = States.Idle;
        }
        else if ((obs = other.GetComponent<ObstacleControl>()) != null)
        {
            State = States.Idle;
        }
    }

    private void HandleObstacle(ObstacleControl obs)
    {
        if (State == States.Idle && obs.State != ObstacleControl.States.Spawning)
        {
            var obstacleNormal = transform.position - obs.transform.position;

            if (obs.State < ObstacleControl.States.OneThird)
            {
                //add score for hitting obstacle
                _uiControl.AddScore(ObstaclePoints);
                obs.State = obs.State + 1;

                HandleBounce(obstacleNormal, false);
            }
            else
            {
                //add score and boost multiplier for breaking obstacle
                _uiControl.BoostMultiplier();
                _uiControl.AddScore(SweetReflectPoints);

                HandleBounce(obstacleNormal, obs.CurrentPowerupType != PowerupControl.PowerupType.Slower);

                if (obs.CurrentPowerupType == PowerupControl.PowerupType.Multiball)
                {
                    FindObjectOfType<SpawnControl>().SpawnBalls();
                    PersonalPowerupType = PowerupControl.PowerupType.None;
                }
                else if(obs.CurrentPowerupType != PowerupControl.PowerupType.None)
                {
                    _powerupControl.SetAllPowerups(obs.CurrentPowerupType);
                }

                State = States.Idle;
                Destroy(obs.gameObject);
            }
        }
    }

    private void HandleWall(WallControl wall)
    {
        switch (wall.State)
        {
            case WallControl.States.Idle:
            case WallControl.States.Primed:
            case WallControl.States.Charging:
                if (State == States.Idle || State == States.Pause)
                {
                    if (Counter > PauseFrames && PersonalPowerupType == PowerupControl.PowerupType.Shield)
                    {
                        HandleWallBounce(wall.Normal, false);
                        PersonalPowerupType = PowerupControl.PowerupType.None;
                    }
                    else
                    {
                        State = States.Pause;
                    }
                }
                break;
            case WallControl.States.Reflect:
                // set multiplier back to 1, broke a chain of strong hits
                _uiControl.ResetMultipler();
                if (State == States.Pause)
                {
                    //sweet spot scoring
                    _uiControl.AddScore(SweetReflectPoints);
                    HandleWallBounce(wall.Normal, false);
                    if (wall.NeedsEnabled) 
                    {
						wall.State = WallControl.States.Idle;
                    }
                    else
                    {
                    	wall.State = WallControl.States.Primed;
                    }
                }
                else if (State == States.Idle)
                {
                    //normal scoring
                    _uiControl.AddScore(ReflectPoints);
                    HandleWallBounce(wall.Normal, false);
                    wall.State = WallControl.States.ShortCooldown;
                }
                break;
            case WallControl.States.ShortCooldown:
            case WallControl.States.LongCooldown:
                if (State != States.Bounce)
                {
                    if (PersonalPowerupType == PowerupControl.PowerupType.Shield)
                    {
                        HandleWallBounce(wall.Normal, false);
                        PersonalPowerupType = PowerupControl.PowerupType.None;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                break;
            case WallControl.States.StrongReflect:
                //add one to the current multiplier
                _uiControl.BoostMultiplier();
                if (State == States.Pause)
                {
                    //sweet spot scoring
                    _uiControl.AddScore(SweetReflectPoints);
                    HandleWallBounce(wall.Normal, true);
					if (wall.NeedsEnabled) 
                    {
						wall.State = WallControl.States.Idle;
                    }
                    else
                    {
                    	wall.State = WallControl.States.Primed;
                    }
                }
                else if (State == States.Idle)
                {
                    //normal scoring
                    _uiControl.AddScore(ReflectPoints);
                    HandleWallBounce(wall.Normal, true);
                    wall.State = WallControl.States.ShortCooldown;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void HandleWallBounce(Vector2 normal, bool strongBounce)
    {
        HandleBounce(normal, strongBounce);
        _spawnControl.RegisterBounce();
    }

    private void HandleBounce(Vector2 normal, bool strongBounce)
    {
        // Source: http://stackoverflow.com/questions/573084/how-to-calculate-bounce-angle
        var u = (Vector2.Dot(Velocity, normal) / Vector2.Dot(normal, normal)) * normal;
        var w = Velocity - u;

        BounceBonus = strongBounce ? StrongReflectBonus : 0;

        //TODO: Add friction?
        Velocity = w - u;

        State = States.Bounce;
    }

    private void HandlePointMania(PointManiaControl point)
    {
        _uiControl.AddScore(point.PointManiaPoints);
        Destroy(point.gameObject);
    }
}
