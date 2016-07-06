using UnityEngine;
using System.Collections;

public class GenerationStartTubeScript : MonoBehaviour {

    public GameObject Tube;
	// Use this for initialization
	void Start ()
    {
        for (int i = 1; i < 7; i++)
        {
            Instantiate(Tube, new Vector3(i*2000, 0, 0), Quaternion.Euler(0, 90, 0));
        }
    }
}
