using UnityEngine;
using System.Collections;

public class aShoot : MonoBehaviour {

    public float speed, dmg;
    Transform destination;

    // Update is called once per frame
    void Update()
    {
        if (destination == null)
        {
            Destroy(gameObject);
        }
        else
        {
            float stepSize = Time.deltaTime * speed;
            transform.position = Vector3.MoveTowards(transform.position, destination.position, stepSize);

            if (transform.position.Equals(destination.position))
            {
                Creep t = destination.GetComponent<Creep>();
                //t.SendMessage("ApplyDmg", dmg);
                t.health -= dmg;
                Destroy(gameObject);
                if (t.health <= 0)
                {
                    t.onDeath();
                    money.amountOfGold += SpawnWave.levelCount / 10 + 1;
                }
            }
        }
    }

    public void setDestination(Transform v)
    {
        destination = v;
    }
}
