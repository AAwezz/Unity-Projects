using UnityEngine;
using System.Collections;
//using System;

public class EnemyController : MonoBehaviour {

    public Camera GhoulEyeCam;
    public GameObject[] Rooms;
    public GameObject GhoulEyes;
    public float GazeOnPlayerTime = 1.2f;
    public float fieldOfViewAngle = 110f;

    public bool Hunting = false;

    //public variables that are not to be changed in inspector
    public Vector3 lastPlayerSighting;
    public bool _hasTagetLocation = false, _survey = false, _surveyingRunning = false, _playerSpottet = false;

    public bool PlayerInSight = false;

    Vector3 _targetLocation;
    float _gazeOnPlayerTimer = 0;
    NavMeshAgent _enemy;
    Animator _aniController;
    Rigidbody _rigidBody;
    Renderer _playerHeadRenderer;  

    Vector3 _positionLastFrame;
	// Use this for initialization
	void Start () {
        _enemy = this.gameObject.GetComponent<NavMeshAgent>();
        _aniController = this.gameObject.GetComponent<Animator>();
        _rigidBody = this.gameObject.GetComponent<Rigidbody>();
        _positionLastFrame = this.gameObject.transform.position;
    }

	// Update is called once per frame
	void Update () {
        if (!Hunting)
        {
            return;
        }
      
        //makes the animation of walking activate and deactivate
        /*if(!_aniController.GetBool("Walking") && this.gameObject.transform.position != _positionLastFrame)
        {
            _aniController.SetBool("Walking", true);
        }
        if (this.gameObject.transform.position == _positionLastFrame)
        {
            _aniController.SetBool("Walking", false);
        }

        _positionLastFrame = this.gameObject.transform.position;
        */
        if (PlayerInSight)
        {
            ChasePlayer();
        }

        if (_surveyingRunning)
        {
            return;
        }

        if (_survey && _hasTagetLocation)
        {
            StartCoroutine(Survey());
            return;
        }

        if (_enemy.remainingDistance == 0 && _hasTagetLocation)
        {
            _survey = true;
            return;
        }
        if (!_hasTagetLocation && !_surveyingRunning)
        {
            PickDestination();
        }

	}

    private void PickDestination()
    {
        int pos = Random.Range(0, Rooms.Length);
        _targetLocation = Rooms[pos].transform.position;
        _enemy.SetDestination(_targetLocation);
        _hasTagetLocation = true;
    }

    IEnumerator Survey()
    {
        _hasTagetLocation = false;
        _survey = false;

        _surveyingRunning = true;
        _enemy.SetDestination(_targetLocation + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f)));

        float _startTime = Time.time;

        while (_startTime + 1.5f > Time.time && !PlayerInSight) // wait for 1.5 sec
        {
            if (PlayerInSight)
            {
                break;
            }
            yield return null;
        }

        _enemy.SetDestination(_targetLocation + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f)));

        _startTime = Time.time;

        while (_startTime + 1.5f > Time.time && !PlayerInSight) // wait for 1.5 sec
        {
            if (PlayerInSight)
            {
                break;
            }
            yield return null;
        }

        _enemy.SetDestination(_targetLocation + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f)));

        _startTime = Time.time;

        while (_startTime + 2f > Time.time && !PlayerInSight) // wait for 2 sec
        {
            if (PlayerInSight)
            {
                break;
            }
            yield return null;
        }
        _surveyingRunning = false;
        yield return null;
    }

    public void ChasePlayer()
    {
        _targetLocation = lastPlayerSighting;
        _enemy.SetDestination(_targetLocation);
        _hasTagetLocation = true;
        _survey = false;
        _surveyingRunning = false;
    }

    public void LookingForPlayer()
    {

    }


}
