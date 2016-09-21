using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class PlayerBoost : MonoBehaviour
{

    public float speed = 10;
    public bool moveTowardsObject;
    private bool moveBack;
    private Vector3 targetPosition;
    public float MaxDistanceToObstacle = 100;
    public float MinDistanceToObstacle = 50;
    public float BoostCost;
    public float MinAdrenalin;
    public float MotionBlurAmount = 10f;
    private AdrenalineController adrenalineController;
    private float _oldSpeed = 0;
    private Transform coinAttractor;
    public float CoinAttractorExpandPeriod = 2;
    public float CoinAttractorReducePeriod = 2;
    public float CoinAttractorSizeIncrease = 3;
    public float CoinAttractorSizeDecrease = .1f;
    public bool playerIsDead = false;
    private float maxBootTime = 3f;

    private Vector3 tPos;
    private AudioManager audioMngr;

    // Use this for initialization
    void Start()
    {
        coinAttractor = transform.FindChild("CoinAttractor");
        adrenalineController = GameObject.FindGameObjectWithTag("UI").GetComponent<AdrenalineController>();
        if(adrenalineController == null)
            Debug.LogError("Cannot find adrenalineController");

        audioMngr = GameObject.FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveTowardsObject) { 
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed*Time.deltaTime);
            Camera.main.GetComponent<CameraMotionBlur>().velocityScale = MotionBlurAmount;
            StartCoroutine(StopBoost());
            // transform.Rotate(new Vector3(1f, 0, 0), 5f*Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, targetPosition) <= 1) { 
            moveBack = true;
            Camera.main.GetComponent<CameraMotionBlur>().velocityScale = 0f;
        }
        if (transform.localPosition.Equals(Vector3.zero))
        {
            moveBack = false;
            Camera.main.GetComponent<CameraMotionBlur>().velocityScale = 0f;
        }
        if (moveBack)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed*Time.deltaTime);
            Camera.main.GetComponent<CameraMotionBlur>().velocityScale = 0f;
        }
    }

    public void BoostHit()
    {
        moveBack = true;
        moveTowardsObject = false;
        adrenalineController.DecreaseAdrenaline(BoostCost);
        GetComponentInChildren<ParticleSystem>().Stop();
        StartCoroutine(updateCoinAttractor(CoinAttractorExpandPeriod));
        GetComponent<Animator>().SetBool("IsBoosting", false);
    }


    private IEnumerator updateCoinAttractor(float expandPeriod)
    {
        coinAttractor.GetComponent<Collider>().enabled = true;
        coinAttractor.GetComponent<SphereCollider>().radius = coinAttractor.GetComponent<SphereCollider>().radius * CoinAttractorSizeIncrease;
        yield return new WaitForSeconds(expandPeriod);
        coinAttractor.GetComponent<SphereCollider>().radius = coinAttractor.GetComponent<SphereCollider>().radius / CoinAttractorSizeIncrease;

    }

    private IEnumerator reduceCoinAttractor(float reducePeriod)
    {
        coinAttractor.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(reducePeriod);
        coinAttractor.GetComponent<Collider>().enabled = true;

    }

    private IEnumerator StopBoost()
    {
        yield return new WaitForSeconds(maxBootTime);
        if(moveTowardsObject)
            BoostHit();
    }

    public void MoveTowardsObstacle(Vector3 position)
    {
        // We need some amount of adrenaline to do this
        if (adrenalineController.AdrenalineBar.value >= MinAdrenalin)
        {
            //The cosine of the angle between the two vectors, it will be positive if the target position is in front of the player
            Vector3 heading = position - transform.position;
            float dot = Vector3.Dot(heading, transform.forward);

            if (dot < MaxDistanceToObstacle && dot > MinDistanceToObstacle)
            {
                moveTowardsObject = true;
                targetPosition = new Vector3(position.x, position.y, position.z);
                GetComponentInChildren<ParticleSystem>().Play();
                GetComponent<Animator>().SetBool("IsBoosting", true);
            }
        }
        else
        {
            audioMngr.FailPlay();
        }
    }


    public void FixCollider1(float i)
    {
        StartCoroutine(FixCollider2(i));

    }

    public IEnumerator FixCollider2(float HitSafePeriod)
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(reduceCoinAttractor(CoinAttractorReducePeriod));
        yield return new WaitForSeconds(HitSafePeriod);
        if (!playerIsDead)
        {
            this.gameObject.GetComponent<Collider>().enabled = true;
        }
    }
}



