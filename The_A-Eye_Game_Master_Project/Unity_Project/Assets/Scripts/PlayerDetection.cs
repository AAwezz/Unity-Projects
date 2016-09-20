using UnityEngine;
using System.Collections;

public class PlayerDetection : MonoBehaviour {

    GameObject _player;
    EnemyController _enemy;
    SphereCollider _eyeSightRange;

	// Use this for initialization
	void Start () {
        _player = GameObject.FindGameObjectWithTag("Player");
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
        _eyeSightRange = this.gameObject.GetComponent<SphereCollider>();
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Im not crashed");
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("PlayerInRange");
            _enemy.PlayerInSight = false;

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, _enemy.gameObject.transform.forward);

            // If the angle between forward and where the player is, is less than half the angle of view...
            if (angle < _enemy.fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;

                // ... and if a raycast towards the player hits something...
                if (Physics.Raycast(this.transform.position, direction.normalized, out hit, _eyeSightRange.radius))
                {
                    //Debug.Log(hit.collider.name);
                    // ... and if the raycast hits the player...
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        // ... the player is in sight.
                        //Debug.DrawRay(transform.position, _player.transform.position, Color.green);
                        _enemy.PlayerInSight = true;
                        //Debug.Log(_enemy.PlayerInSight);

                        // Set the last global sighting is the players current position.
                        _enemy.lastPlayerSighting = _player.transform.position;

                    }
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        // If the player leaves the trigger zone...
        if (other.gameObject.tag == "Player")
            // ... the player is not in sight.
            _enemy.PlayerInSight = false;
    }
}
