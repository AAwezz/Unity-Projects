using UnityEngine;
using System.Collections;

public class NearMissDetection : MonoBehaviour {

    private AdrenalineController adrenalineController;
    private NearMissControl nearMissControl;
    public int AdrenalineAwarded = 10;
	private AudioManager audioMngr;
    // Use this for initialization
    void Start () {

        adrenalineController = GameObject.FindGameObjectWithTag("UI").GetComponent<AdrenalineController>();
        nearMissControl = GameObject.FindGameObjectWithTag("NearMissText").GetComponent<NearMissControl>();
		audioMngr = GameObject.FindObjectOfType<AudioManager> ();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" || other.gameObject.GetComponent<PlayerBoost>().moveTowardsObject) return;
        StartCoroutine(NearMiss(other.gameObject));
    }

    IEnumerator NearMiss(GameObject go)
    {
		audioMngr.NearMissPlay();
        yield return new WaitForSeconds(.5f);
        if (go.GetComponent<Collider>().enabled)
      {
            adrenalineController.IncreaseAdrenaline(AdrenalineAwarded);
            nearMissControl.NearMiss();
       }
        yield return null;
       
    }
}
