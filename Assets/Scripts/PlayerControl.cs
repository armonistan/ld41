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
	public KeyCode MoveLeft = KeyCode.LeftArrow;
	public KeyCode MoveRight = KeyCode.RightArrow;
	public KeyCode MoveForward = KeyCode.UpArrow;
	public KeyCode MoveBackward = KeyCode.DownArrow;

    //special moves
    public KeyCode Spin = KeyCode.Q;
    public KeyCode Sprint = KeyCode.W;
    public KeyCode Tackle = KeyCode.R;

	//public States State = States.Running;

	//player speed
    public float PlayerMaxSpeed = 8;
	public float PlayerIdleSpeed = 0;

    //private variables
	private float _currentPlayerSpeedX = 0;
    private float _currentPlayerSpeedY = 0;
    private Vector2 _currentPlayerVector = new Vector2();
	private float _radAngle = 0;
    private bool _notSpinning = true;

	//constants
	private float _LEFT_X = -1;
	private float _RIGHT_X = 1;
	private float _FORWARD_Y = 1;
	private float _BACKWARD_Y = -1;
	private float _SPEED_DECAY = .05f;
    private float _SPEED_INCREASE = .2f;

    float ButtonCooler = 0.5f ; // Half a second before reset
    int ButtonCount = 0;

    public Vector2 Velocity
    {
        get
        {
			return new Vector2 (Mathf.Cos (_radAngle) * Math.Abs(_currentPlayerSpeedX), Mathf.Sin (_radAngle) * Math.Abs(_currentPlayerSpeedY)) * Time.deltaTime;
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

        UpdatePlayerMovementInput();
        UpdatePlayerSpecialInput();



        Velocity = _currentPlayerVector;
		float currentPlayerVectorX = _currentPlayerVector.x;
		float currentPlayerVectorY = _currentPlayerVector.y;
		gameObject.transform.Translate(Velocity);
        /*switch (State)
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
	    }*/
	}

    void UpdatePlayerMovementInput() {
        //left right input
        if (Input.GetKey(MoveLeft))
        {
            if (_currentPlayerSpeedX > 0) {
                _currentPlayerSpeedX -= _SPEED_DECAY + _SPEED_INCREASE;
            }
            else if (Math.Abs(_currentPlayerSpeedX) < PlayerMaxSpeed)
            {
                _currentPlayerVector.x = _LEFT_X;
                _currentPlayerSpeedX -= _SPEED_INCREASE;
            }
        }
        else if (Input.GetKey(MoveRight))
        {
            if (_currentPlayerSpeedX < 0)
            {
                _currentPlayerSpeedX += _SPEED_DECAY + _SPEED_INCREASE;
            }
            else if (Math.Abs(_currentPlayerSpeedX) < PlayerMaxSpeed)
            {
                _currentPlayerVector.x = _RIGHT_X;
                _currentPlayerSpeedX += _SPEED_INCREASE;
            }
        }
        else
        {
            if (_currentPlayerSpeedX < 0)
            {
                _currentPlayerSpeedX += _SPEED_DECAY;
            }
            else if (_currentPlayerSpeedX > 0)
            {
                _currentPlayerSpeedX -= _SPEED_DECAY;
            }
        }

        //forward backward input
        if (Input.GetKey(MoveBackward))
        {
            if (_currentPlayerSpeedY > 0)
            {
                _currentPlayerSpeedY -= _SPEED_DECAY + _SPEED_INCREASE;
            }
            else if (Math.Abs(_currentPlayerSpeedY) < PlayerMaxSpeed)
            {
                _currentPlayerVector.y = _BACKWARD_Y;
                _currentPlayerSpeedY -= _SPEED_INCREASE;
            }
        }
        else if (Input.GetKey(MoveForward))
        {
            if (_currentPlayerSpeedY < 0)
            {
                _currentPlayerSpeedY += _SPEED_DECAY + _SPEED_INCREASE;
            }
            else if (Math.Abs(_currentPlayerSpeedY) < PlayerMaxSpeed)
            {
                _currentPlayerVector.y = _FORWARD_Y;
                _currentPlayerSpeedY += _SPEED_INCREASE;
            }
        }
        else
        {
            if (_currentPlayerSpeedY < 0)
            {
                _currentPlayerSpeedY += _SPEED_DECAY;
            }
            else if (_currentPlayerSpeedY > 0)
            {
                _currentPlayerSpeedY -= _SPEED_DECAY;
            }
        }
    }

    void UpdatePlayerSpecialInput()
    {
        if (Input.GetKey(Sprint))
        {
            //PlayerBaseSpeed = 8;
        }
        else
        {
            //PlayerBaseSpeed = 4;
        }
        CheckSpin();
        CheckJuke();
    }

    void CheckSpin()
    {
        if (Input.GetKeyDown(Spin) && _notSpinning)
        {
            _notSpinning = false;
        }
        else if (!_notSpinning)
        {
            Debug.Log("neat trick");
            if (Mathf.Approximately(transform.rotation.z, 0f))
            {
                _notSpinning = true;
            }
        }
    }

    void CheckJuke()
    {
        KeyCode juke = (_currentPlayerVector.x > 0) ? KeyCode.LeftArrow : KeyCode.RightArrow;

        if (Input.GetKeyDown(juke))
        {
            if (ButtonCooler > 0 && ButtonCount == 1/*Number of Taps you want Minus One*/)
            {
                _currentPlayerVector.x *= -1;
                _currentPlayerSpeedX *= -1;
            }
            else
            {
                ButtonCooler = 0.5f;
                ButtonCount += 1;
            }
        }

        if (ButtonCooler > 0)
        {

            ButtonCooler -= 1 * Time.deltaTime;

        }
        else
        {
            ButtonCount = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enter collide");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("colliding");
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
        Debug.Log("exiting");
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
