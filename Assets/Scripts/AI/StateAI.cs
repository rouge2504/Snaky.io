using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAI
{
    public enum STATE { WANDER, ATTACK, SLEEP, ESCAPE };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE name;
    protected EVENT stage;
    protected StateAI nextState;

    protected GameObject snakeObject;
    protected GameObject player;
    protected SnakeVision snakeVision;
    protected float timeToDelayRotate = 1.5f;
    protected float timingToDelayRotate = 0;
    protected float timeToDelayEscape = 0.2f;
    protected float timingToDelayEscape = 0f;
    protected float axisRot;
    protected float speedRot;

    public StateAI(GameObject _snakeObject, GameObject _player, SnakeVision _snakeVision)
    {
        snakeObject = _snakeObject;
        player = _player;
        snakeVision = _snakeVision;
        stage = EVENT.ENTER;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public StateAI Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState; 
        }

        return this;

    }

    public bool SeeAnotherSnake ()
    {

        snakeVision = snakeObject.GetComponent<SnakeHeadMove>().snakeVision;
        if (snakeVision.seeAnotherSnake)
        {
            float rand = Random.Range(0, 100);
            //Debug.Log("Rand: " + rand);
            if (rand < 85)
            {
                //Debug.Log("See another snake");
                return true;
            }
        }
        return false;

    }

    public void Move()
    {
        snakeObject.transform.Translate(0, 0, 2.5f * Time.deltaTime);
    }

    public void SetRotate()
    {
        axisRot = Random.Range(-GameConstants.ROTATION_IA, GameConstants.ROTATION_IA);
        speedRot = GameConstants.ROTATION_SPEED_IA;


    }
}

public class Wander : StateAI
{
    public Wander(GameObject _snakeObject, GameObject _player, SnakeVision _snakeVision) : base(_snakeObject, _player, _snakeVision)
    {
        name = STATE.WANDER;
    }

    public override void Enter()
    {
        SetRotate();
        base.Enter();
    }

    public override void Update()
    {
        Move();
        snakeObject.transform.Rotate(0, axisRot * speedRot * Time.deltaTime, 0);
        timingToDelayRotate += Time.deltaTime;
        if (timingToDelayRotate > timeToDelayRotate)
        {
            if (Random.Range(0, 100) < 20)
            {
                nextState = new Sleep(snakeObject, player, snakeVision);
                stage = EVENT.EXIT;
            }
            timingToDelayRotate = 0;
        }
        //base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class Sleep : StateAI
{
    public Sleep(GameObject _snakeObject, GameObject _player, SnakeVision _snakeVision) : base(_snakeObject, _player, _snakeVision)
    {
        name = STATE.SLEEP;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        //snakeVision.Log();
        timingToDelayEscape += Time.deltaTime;
        if (SeeAnotherSnake() && timingToDelayEscape > timeToDelayEscape)
        {
            snakeVision.seeAnotherSnake = false;
            nextState = new Escape(snakeObject, player, snakeVision);
            stage = EVENT.EXIT;
            timingToDelayEscape = 0;
            return;
        }
        Move();
        timingToDelayRotate += Time.deltaTime;
        if (timingToDelayRotate > timeToDelayRotate)
        {
            if (Random.Range(0, 100) < 10)
            {
                nextState = new Wander(snakeObject, player, snakeVision);
                stage = EVENT.EXIT;
            }
            timingToDelayRotate = 0;
        }
       // base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class Escape : StateAI
{
    private float timeToDriff = 3;
    private float timingToDriff = 0;
    public enum STATE_ESCAPE { DRIFF, FLIP_180}
    private STATE_ESCAPE state_escape;
    public Escape(GameObject _snakeObject, GameObject _player, SnakeVision _snakeVision) : base(_snakeObject, _player, _snakeVision)
    {
        name = STATE.ESCAPE;
    }
    public override void Enter()
    {
        float rnd = Random.Range(0, 100);

        state_escape = (rnd < 60) ? STATE_ESCAPE.FLIP_180 : STATE_ESCAPE.DRIFF;
        SetRotate();
        base.Enter();
    }

    private void Drift()
    {
        timingToDriff += Time.deltaTime;
        Move();
        snakeObject.transform.Rotate(0, axisRot * speedRot * Time.deltaTime, 0);
        if (timingToDriff > timeToDriff)
        {
            timingToDriff = 0;
            nextState = new Sleep(snakeObject, player, snakeVision);
            stage = EVENT.EXIT;
        }
    }

    private void Flip()
    {
        snakeObject.transform.Rotate(0, 180, 0);
        snakeVision.seeAnotherSnake = false;
        nextState = new Sleep(snakeObject, player, snakeVision);
        stage = EVENT.EXIT;
    }

    public override void Update()
    {
        switch (state_escape)
        {
            case STATE_ESCAPE.DRIFF:
                Drift();
                break;
            case STATE_ESCAPE.FLIP_180:
                Flip();
                break;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
