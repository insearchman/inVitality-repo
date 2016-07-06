using UnityEngine;
using System.Collections;

public class GeneratorNewTube : MonoBehaviour {
    public GameObject Tube;
	// Use this for initialization
	void Start () {
        transform.parent.gameObject.name = transform.parent.gameObject.name.Replace("(Clone)", "");
        
    }
	
	// Update is called once per frame
	void OnTriggerEnter()
    {
        Check(new Vector3(transform.position.x+10000, 0, 0), 1.0f);
    }
    void Check(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        if (hitColliders.Length == 0)
        {
            Instantiate(Tube, center, Quaternion.Euler(0, 90, 0));
        }
    }
}
