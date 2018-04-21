using System;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : StatefulMonoBehavior<PlayerControl.States>
{
    public enum States
    {
		Idle,
        Running,
        Spinning
    }
		
	//directional controls
	public KeyCode MoveLeft = KeyCode.A;
	public KeyCode MoveRight = KeyCode.D;
	public KeyCode MoveForward = KeyCode.W;
	public KeyCode MoveBackward = KeyCode.S;

	public States State = States.Idle;

	//player speed
    public float PlayerBaseSpeed = 4;
	public float PlayerIdleSpeed = 0;

	private float _currentPlayerSpeed = 0;
	private Vector2 _currentPlayerVector = new Vector2();
	private double _radAngle = 0;

	//constants
	private float _LEFT_X = -1;
	private float _RIGHT_X = 1;
	private float _FORWARD_Y = 1;
	private float _BACKWARD_Y = -1;
	private float _IDLE = 0;



	public Vector2 Velocity
    {
        get
        {
			return new Vector2 (Mathf.Cos (_radAngle), Mathf.Sin (_radAngle)) * _currentPlayerSpeed * Time.deltaTime;
		}
        set
        {
            _radAngle = Mathf.Atan2(value.y, value.x);
        }
    }

    // Use this for initialization
	void Start () {
		
		//holding onto for reference later
		/*
        if (FindObjectOfType<GameControl>().GetGameState() == GameControl.States.EasyMode)
        {
            PauseFrames = 6;
        }
        else if (FindObjectOfType<GameControl>().GetGameState() == GameControl.States.HardMode)
        {
            PauseFrames = 3;
        }*/
	}
	
	// Update is called once per frame
	void Update () {
		//might be needed later
		/*if (Time.timeScale == GameControl.Paused)
	    {
	        return;
	    }*/

		//left right input
		if (Input.GetKey(MoveLeft))
		{
			_currentPlayerVector.x = _LEFT_X;
		}
		else if (Input.GetKey(MoveRight))
		{
			_currentPlayerVector.x = _RIGHT_X;
		}
		else
		{
			_currentPlayerVector.x = _IDLE;
		}

		//forward backward input
		if (Input.GetKey(MoveForward))
		{
			_currentPlayerVector.y = _FORWARD_Y;
		}
		else if (Input.GetKey(MoveBackward))
		{
			_currentPlayerVector.y = _BACKWARD_Y;
		}
		else
		{
			_currentPlayerVector.y = _IDLE;
		}


		Velocity = _currentPlayerVector;
		_currentPlayerSpeed = (_currentPlayerVector.x != 0 || _currentPlayerVector != 0) ? PlayerBaseSpeed : PlayerIdleSpeed;

        switch (State)
	    {
			case States.Idle:
	            break;
			case States.Running:
				gameObject.transform.Translate(Velocity);
	            break;
			case States.Spinning:
				gameObject.transform.Translate(Velocity);
                break;
	        default:
	            throw new ArgumentOutOfRangeException();
	    }
	}

    void OnTriggerStay2D(Collider2D other)
    {
        /*if (Time.timeScale == GameControl.Paused)
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
        }*/
    }

    void OnTriggerExit2D(Collider2D other)
    {
        /*WallControl wall;
        ObstacleControl obs;

        if ((wall = other.gameObject.GetComponent<WallControl>()) != null)
        {
            State = States.Idle;
        }
        else if ((obs = other.GetComponent<ObstacleControl>()) != null)
        {
            State = States.Idle;
        }*/
    }
}
