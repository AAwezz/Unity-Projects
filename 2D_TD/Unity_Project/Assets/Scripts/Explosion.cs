using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    GameObject target;
    public static float fireDmg = 1;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "creep" && !Creep.isBossLevel)
        {
            target = other.gameObject;
            other.SendMessage("getHealth", gameObject);
        }
    }

    void explode(int health)
    {
        target.SendMessage("ApplyDmg", fireDmg);
    }

    void explosion()
    {
        StartCoroutine(SpawnFireblast());
    }

    IEnumerator SpawnFireblast()
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        transform.position = new Vector3(vec.x, vec.y, 0);
        yield return new WaitForSeconds(0.0625f);
        transform.position = new Vector3(100, 100, 0);
        yield return new WaitForSeconds(0.0625f);
        transform.position = new Vector3(vec.x, vec.y, 0);
        yield return new WaitForSeconds(0.0625f);
        transform.position = new Vector3(100, 100, 0);
        yield return new WaitForSeconds(0.0625f);
        transform.position = new Vector3(vec.x, vec.y, 0);
        yield return new WaitForSeconds(0.0625f);
        transform.position = new Vector3(100, 100, 0);
        yield return new WaitForSeconds(0.0625f);
        transform.position = new Vector3(vec.x, vec.y, 0);
        yield return new WaitForSeconds(0.0625f);
        transform.position = new Vector3(100, 100, 0);
        yield return new WaitForSeconds(0.0625f);
        transform.position = new Vector3(vec.x, vec.y, 0);
        yield return new WaitForSeconds(0.0625f);
        transform.position = new Vector3(100, 100, 0);
        yield return new WaitForSeconds(0.0625f);
        transform.position = new Vector3(vec.x, vec.y, 0);
        yield return new WaitForSeconds(0.0625f);
        transform.position = new Vector3(100, 100, 0);
        yield return new WaitForSeconds(0.0625f);
        transform.position = new Vector3(vec.x, vec.y, 0);
        yield return new WaitForSeconds(0.0625f);
        transform.position = new Vector3(100, 100, 0);
        yield return new WaitForSeconds(0.0625f);
        transform.position = new Vector3(vec.x, vec.y, 0);
        yield return new WaitForSeconds(0.0625f);
        Destroy(gameObject);
    }
}
