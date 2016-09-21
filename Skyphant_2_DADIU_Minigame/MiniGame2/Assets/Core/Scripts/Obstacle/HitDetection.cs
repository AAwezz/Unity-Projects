using UnityEngine;
using System.Collections;

public class HitDetection : MonoBehaviour {

    private AdrenalineController adrenalineController;
    public float HitSafePeriod = 0.3f;
    public int AdrenalinePenalty = 30;
	private AudioManager audioMngr;
	private GameObject player;
    public bool ObjectIsBoostable;
    // Use this for initialization
    void Start () {

        adrenalineController = GameObject.FindGameObjectWithTag("UI").GetComponent<AdrenalineController>();
		audioMngr = GameObject.FindObjectOfType<AudioManager> ();
		player = GameObject.FindGameObjectWithTag ("Player");
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Player") return;
        
        if (collider.gameObject.GetComponent<PlayerBoost>().moveTowardsObject && ObjectIsBoostable) {
            collider.gameObject.GetComponent<PlayerBoost>().BoostHit();
            StartCoroutine(SplitMesh());
			audioMngr.HitTreePlay();
            Handheld.Vibrate();
        }
        else {
            Handheld.Vibrate();
            audioMngr.FailPlay();
            adrenalineController.DecreaseAdrenaline(AdrenalinePenalty);
            collider.gameObject.GetComponent<PlayerBoost>().FixCollider1(HitSafePeriod);

        }
    }

    IEnumerator SplitMesh ()
	{
		MeshFilter MF = transform.parent.GetComponent<MeshFilter>();
		MeshRenderer MR = transform.parent.GetComponent<MeshRenderer>();
		Mesh M = MF.mesh;
		Vector3[] verts = M.vertices;
		Vector3[] normals = M.normals;
		Vector2[] uvs = M.uv;
		Transform parent = transform.parent;

		var heading = player.transform.position - parent.position;
		var distance = heading.magnitude;
		var direction = heading / distance; // This is now the normalized direction.

		for (int submesh = 0; submesh < M.subMeshCount; submesh++)
		{
			int[] indices = M.GetTriangles(submesh);
			for (int i = 0; i < indices.Length; i += 30)
			{
				Vector3[] newVerts = new Vector3[3];
				Vector3[] newNormals = new Vector3[3];
				Vector2[] newUvs = new Vector2[3];
				for (int n = 0; n < 3; n++)
				{
					int index = indices[i + n];
					newVerts[n] = verts[index];
					newUvs[n] = uvs[index];
					newNormals[n] = normals[index];
				}
				Mesh mesh = new Mesh();
				mesh.vertices = newVerts;
				mesh.normals = newNormals;
				mesh.uv = newUvs;
				
				mesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };
				
				GameObject GO = new GameObject("Triangle " + (i / 3));
				GO.transform.position = parent.position;
				GO.transform.rotation = parent.rotation;
				GO.AddComponent<MeshRenderer>().material = MR.materials[submesh];
				GO.AddComponent<MeshFilter>().mesh = mesh;
				GO.AddComponent<BoxCollider>();
				GO.AddComponent<Rigidbody>().drag = 0.5f;
				GO.GetComponent<Rigidbody>().AddExplosionForce(4000f, parent.transform.position + (direction * 4f), 50f, 3.0F);
				
				Destroy(GO, 1 + Random.Range(0.0f, 1.0f));
			}
		}

		MR.enabled = false;
		
		//		Time.timeScale = 0.2f;
		yield return new WaitForSeconds(0.8f);
		//		Time.timeScale = 1.0f;
		Destroy(gameObject.transform.parent.gameObject);
	}
}
