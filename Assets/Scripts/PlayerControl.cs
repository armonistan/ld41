using System;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : StatefulMonoBehavior<PlayerControl.States>
{
    public enum States
    {
		Default,
        Spinning,
        StiffArming,
        Tackling
    }

    public GameObject DeadPlayer;
    public Field FieldRenderer;
    private Animator _animationController;
		
	//directional controls
	public KeyCode MoveLeft = KeyCode.LeftArrow;
	public KeyCode MoveRight = KeyCode.RightArrow;
	public KeyCode MoveForward = KeyCode.UpArrow;
	public KeyCode MoveBackward = KeyCode.DownArrow;

    //special moves
    public KeyCode SpinKey = KeyCode.Space;
    public KeyCode TackleKey = KeyCode.R;
    public KeyCode StiffArmKey = KeyCode.E;

	//player speed
    public float PlayerMaxSpeed = 8;

    //private variables
	private float _currentPlayerSpeedX = 0;
    private float _currentPlayerSpeedY = 0;
    private Vector2 _currentPlayerVector = new Vector2();
    private float _radAngle = 0;
    private float spinTimer = 0f;
    private float tackleTimer = 0f;
    private float stiffArmTimer = 0f;
    private float _styleTimer = 0f;

    //constants
    private float _LEFT_X = -1;
	private float _RIGHT_X = 1;
	private float _FORWARD_Y = 1;
	private float _BACKWARD_Y = -1;

    public float SPEED_DECAY = .05f;
    public float SPEED_INCREASE = .2f;
    public float MaxSpeedModifier = 0.9f;
    public float MinMaxSpeed = 2f;
    public float SPIN_DURATION = 1f;
    public float TACKLE_DURATION = 1f;
    public float STIFF_ARM_DURATION = 1f;

    public float StyleChargeTime = 4f;
    public int MaxStyle = 5;
    public int TackleStyleCost = 2;
    public int SpinStyleCost = 1;
    public int StiffArmStyleCost = 3;

    public float BulkChargeTime = 8f;
    private int MaxBulk = 5;
    private float _bulkTimer = 0f;

    public int STYLE = 5;
    public int BULK = 5;

    float JukeButtonCooldown = 0.5f ; // Half a second before reset
    int JukeButtonCount = 0;

    private PlayerAbilities.Abilities ability;

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
        State = States.Default;
        _animationController = GetComponent<Animator>();

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
        HandleBulk();
        HandleStyle();
        UpdatePlayerMovementInput();
        UpdatePlayerSpecialInput();
        UpdatePlayerPosition();
	}

    public void InitializeStats()
    {
        var stats = GameData.getCurrentPlayer();

        STYLE = MaxStyle = stats.getStyle();
        BULK = MaxBulk = stats.getBulk();
        PlayerMaxSpeed = stats.getSpeed() * MaxSpeedModifier + MinMaxSpeed;
        ability = stats.getAbility();

        switch(ability)
        {
            case PlayerAbilities.Abilities.Stylin:
                StyleChargeTime /= 2;
                break;
            case PlayerAbilities.Abilities.Stiffie:
                STIFF_ARM_DURATION *= 2;
                break;
            case PlayerAbilities.Abilities.Shephard:
                TACKLE_DURATION *= 2;
                SPEED_INCREASE /= 2;
                break;
            case PlayerAbilities.Abilities.GreasedLightning:
                PlayerMaxSpeed *= 2;
                _LEFT_X /= 2;
                _RIGHT_X /= 2;
                _BACKWARD_Y /= 2;
                _FORWARD_Y /= 2;
                break;
            case PlayerAbilities.Abilities.Annihilator:
                BULK = 1;
                break;
            default:
                break;
        }
    }

    private void HandleBulk()
    {
        if (ability == PlayerAbilities.Abilities.Unit)
        {
            if (BULK < MaxBulk)
            {
                if (_bulkTimer < BulkChargeTime)
                {
                    _bulkTimer += Time.deltaTime;
                }
                else
                {
                    BULK++;
                    _bulkTimer = 0f;
                }
            }
            else
            {
                _bulkTimer = 0f;
            }
        }

        var planExecutor = FindObjectOfType<PlanExecutor>();

        if (BULK <= 0 && !planExecutor.HasScoredTouchDown())
        {
            GameData.getCurrentPlayer().recordYardsCovered(transform.position.y / FieldRenderer.YardLength);

            Instantiate(DeadPlayer, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void HandleStyle()
    {
        if (ability == PlayerAbilities.Abilities.Annihilator)
        {
            STYLE = MaxStyle;
        }
        if (STYLE < MaxStyle)
        {
            if (_styleTimer < StyleChargeTime)
            {
                _styleTimer += Time.deltaTime;
            }
            else
            {
                STYLE++;
                _styleTimer = 0f;
            }
        }
        else
        {
            _styleTimer = 0f;
        }
    }

    private void UpdatePlayerPosition()
    {
        Velocity = _currentPlayerVector;
        float currentPlayerVectorX = _currentPlayerVector.x;
        float currentPlayerVectorY = _currentPlayerVector.y;
        gameObject.transform.Translate(Velocity);
    }

    void UpdatePlayerMovementInput() {
        if (ability == PlayerAbilities.Abilities.Blenderman)
        {
            if (_currentPlayerSpeedY < 0)
            {
                _currentPlayerSpeedY += SPEED_DECAY + SPEED_INCREASE;
            }
            else if (Math.Abs(_currentPlayerSpeedY) < PlayerMaxSpeed)
            {
                _currentPlayerVector.y = _FORWARD_Y;
                _currentPlayerSpeedY += SPEED_INCREASE;
            }

            return;
        }

        //left right input
        if (Input.GetKey(MoveLeft))
        {
            if (_currentPlayerSpeedX > 0) {
                _currentPlayerSpeedX -= SPEED_DECAY + SPEED_INCREASE;
            }
            else if (Math.Abs(_currentPlayerSpeedX) < PlayerMaxSpeed)
            {
                _currentPlayerVector.x = _LEFT_X;
                _currentPlayerSpeedX -= SPEED_INCREASE;
            }
        }
        else if (Input.GetKey(MoveRight))
        {
            if (_currentPlayerSpeedX < 0)
            {
                _currentPlayerSpeedX += SPEED_DECAY + SPEED_INCREASE;
            }
            else if (Math.Abs(_currentPlayerSpeedX) < PlayerMaxSpeed)
            {
                _currentPlayerVector.x = _RIGHT_X;
                _currentPlayerSpeedX += SPEED_INCREASE;
            }
        }
        else
        {
            if (_currentPlayerSpeedX < 0)
            {
                _currentPlayerSpeedX += SPEED_DECAY;
            }
            else if (_currentPlayerSpeedX > 0)
            {
                _currentPlayerSpeedX -= SPEED_DECAY;
            }
        }

        //forward backward input
        if (Input.GetKey(MoveBackward) && ability != PlayerAbilities.Abilities.Stylin)
        {
            if (_currentPlayerSpeedY > 0)
            {
                _currentPlayerSpeedY -= SPEED_DECAY + SPEED_INCREASE;
            }
            else if (Math.Abs(_currentPlayerSpeedY) < PlayerMaxSpeed)
            {
                _currentPlayerVector.y = _BACKWARD_Y;
                _currentPlayerSpeedY -= SPEED_INCREASE;
            }
        }
        else if (Input.GetKey(MoveForward))
        {
            if (_currentPlayerSpeedY < 0)
            {
                _currentPlayerSpeedY += SPEED_DECAY + SPEED_INCREASE;
            }
            else if (Math.Abs(_currentPlayerSpeedY) < PlayerMaxSpeed)
            {
                _currentPlayerVector.y = _FORWARD_Y;
                _currentPlayerSpeedY += SPEED_INCREASE;
            }
        }
        else
        {
            if (_currentPlayerSpeedY < 0)
            {
                _currentPlayerSpeedY += SPEED_DECAY;
            }
            else if (_currentPlayerSpeedY > 0)
            {
                _currentPlayerSpeedY -= SPEED_DECAY;
            }
        }
    }

    void UpdatePlayerSpecialInput()
    {
        CheckJuke();
        CheckSpin();
        CheckTackle();
        CheckStiffArm();
    }

    void CheckSpin()
    {
        if (STYLE >= SpinStyleCost && Input.GetKeyDown(SpinKey) && State == States.Default && ability != PlayerAbilities.Abilities.Unit)
        {
            gameObject.GetComponents<AudioSource>()[0].Play();
            if (ability == PlayerAbilities.Abilities.DidNotHit && UnityEngine.Random.value > .5)
            {
                // no cost for this
            } else
            {
                STYLE -= SpinStyleCost;
            }
            State = States.Spinning;
            _animationController.SetTrigger("spinning");
        }
        else if (State == States.Spinning)
        {
            if (spinTimer <= SPIN_DURATION)
            {
                spinTimer += Time.deltaTime;
            } else
            {
                spinTimer = 0f;
                State = States.Default;
                _animationController.SetTrigger("running");
            }
        }
    }

    void CheckTackle()
    {
        if (STYLE >= TackleStyleCost && Input.GetKeyDown(TackleKey) && State == States.Default)
        {
            STYLE -= TackleStyleCost;
            State = States.Tackling;
            _animationController.SetTrigger("tackling");
        }
        else if (State == States.Tackling)
        {
            if (tackleTimer <= TACKLE_DURATION)
            {
                tackleTimer += Time.deltaTime;
            }
            else
            {
                tackleTimer = 0f;
                State = States.Default;
                _animationController.SetTrigger("running");
            }
        }
    }

    void CheckStiffArm()
    {
        if (STYLE >= StiffArmStyleCost && Input.GetKeyDown(StiffArmKey) && State == States.Default)
        {
            STYLE -= StiffArmStyleCost;
            State = States.StiffArming;
            _animationController.SetTrigger("stiffing");
        }
        else if (State == States.StiffArming)
        {
            if (Input.GetKeyDown(StiffArmKey))
            {
                stiffArmTimer = 0f;
                State = States.Default;
                _animationController.SetTrigger("running");
            }
            else if (stiffArmTimer <= STIFF_ARM_DURATION)
            {
                stiffArmTimer += Time.deltaTime;
            }
            else
            {
                stiffArmTimer = 0f;
                State = States.Default;
                _animationController.SetTrigger("running");
            }
        }
    }

    void CheckJuke()
    {
        KeyCode juke = (_currentPlayerVector.x > 0) ? KeyCode.LeftArrow : KeyCode.RightArrow;

        if (Input.GetKeyDown(juke))
        {
            if (JukeButtonCooldown > 0 && JukeButtonCount == 1/*Number of Taps you want Minus One*/)
            {
                _currentPlayerVector.x *= -1;
                _currentPlayerSpeedX *= -1;
            }
            else
            {
                JukeButtonCooldown = 0.5f;
                JukeButtonCount += 1;
            }
        }

        if (JukeButtonCooldown > 0)
        {

            JukeButtonCooldown -= 1 * Time.deltaTime;

        }
        else
        {
            JukeButtonCount = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Edge")
        {
            BULK = 0;
        }
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
