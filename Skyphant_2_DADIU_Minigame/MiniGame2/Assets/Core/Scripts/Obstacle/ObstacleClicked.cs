using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.EventSystems;

public class ObstacleClicked : MonoBehaviour {


    private PlayerBoost playerBoost;


	// Use this for initialization
	void Start () {
        playerBoost = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBoost>();
        if(playerBoost == null)
            Debug.LogError("Cannot find boost controller");
	}
	
	// Update is called once per frame
	void Update () {
        // Makes sure you cannot boost through the canvas
        if (EventSystem.current.IsPointerOverGameObject())
           return;

        // Did we do a mouse press
	    if (!Input.GetMouseButtonDown(0)) return;

        // Find all elemets hit
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    var hits = Physics.RaycastAll(ray, playerBoost.MaxDistanceToObstacle);

        // Look if I am hit
	    foreach (var hit in hits.Where(hit => hit.collider.gameObject.Equals(gameObject)))
	        playerBoost.MoveTowardsObstacle(transform.position);
	    
	}


    /*void OnMouseDown()
    {
  

        Debug.Log("CLicked!");
        var transPos = this.transform.position;
        playerBoost.MoveTowardsObstacle(transform.position);

    }*/
}
