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
    public KeyCode Juke = KeyCode.E;
    public KeyCode Tackle = KeyCode.R;

	//public States State = States.Running;

	//player speed
    public float PlayerBaseSpeed = 4;
	public float PlayerIdleSpeed = 0;

    //private variables
	private float _currentPlayerSpeed = 0;
	private Vector2 _currentPlayerVector = new Vector2();
	private float _radAngle = 0;
    private bool _notSpinning = true;

	//constants
	private float _LEFT_X = -1;
	private float _RIGHT_X = 1;
	private float _FORWARD_Y = 1;
	private float _BACKWARD_Y = -1;
	private float _IDLE = 0;
    private float _DEGREES_SPUN_PER_UPDATE = 90;


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

    public Vector2 JukeVelocity
    {
        get
        {
            return new Vector2(Mathf.Cos(_radAngle), Mathf.Sin(_radAngle)) * 60 * Time.deltaTime;
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
		_currentPlayerSpeed = (currentPlayerVectorX != 0 || currentPlayerVectorY != 0) ? PlayerBaseSpeed : PlayerIdleSpeed;
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
    }

    void UpdatePlayerSpecialInput()
    {
        if (Input.GetKeyDown(Spin) && _notSpinning)
        {
            _notSpinning = false;
        }
        else if (!_notSpinning)
        {
            transform.Rotate(0, 0, _DEGREES_SPUN_PER_UPDATE);
            if (Mathf.Approximately(transform.rotation.z, 0f))
            {
                _notSpinning = true;
            }
        }

        if (Input.GetKey(Sprint))
        {
            PlayerBaseSpeed = 8;
        }
        else
        {
            PlayerBaseSpeed = 4;
        }

        if (Input.GetKeyDown(Juke))
        {
            JukeVelocity = _currentPlayerVector;
            gameObject.transform.Translate(JukeVelocity);
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
