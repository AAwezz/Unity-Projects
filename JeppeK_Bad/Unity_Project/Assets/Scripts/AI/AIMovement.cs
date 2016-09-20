using UnityEngine;
using System.Collections;

public class AIMovement : MonoBehaviour {

    //Needed for double speed powerup
    public bool boolDoubleSpeed = false;
    public float timeForHalfSpeed;
    private float timeElapsed = 0;

    //public Transform destination;
    GameObject player;
    private NavMeshAgent agent;

    void Start () 
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        GameMasterPublicVariables.AISpeed = agent.speed;
    }

    void Update()
    {
        if (GameMasterPublicVariables.EnemyHalfSpeed == true)
        {
            agent.speed = halfMovementSpeed(GameMasterPublicVariables.AISpeed);
        } else
        {
            agent.speed = GameMasterPublicVariables.AISpeed;
        }

        if (!Input.anyKey)
        {
            agent.Stop();
            return;
        }
        else
        {
            agent.Resume();
        }
        agent.SetDestination(player.transform.position);
    }

    private float halfMovementSpeed(float speed)
    {
        if (timeElapsed < timeForHalfSpeed)
        {
            var halfSpeed = speed / 4;
            timeElapsed += Time.deltaTime;
            return halfSpeed;
        }
        else
        {
            GameMasterPublicVariables.EnemyHalfSpeed = false;
            timeElapsed = 0;
            return speed;
        }
    }
}

