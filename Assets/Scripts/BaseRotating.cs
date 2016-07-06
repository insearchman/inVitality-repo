using UnityEngine;
using System.Collections;

public class BaseRotating : MonoBehaviour
{
    float mhorizontal;
    float rspeed;
    float maxrspeed;
    float wm;

    void Start ()
    {
        rspeed = 1f;
        maxrspeed = 1f;
    }
	
	void Update () {

        wm = GetComponentInChildren<WingsTailAnim>().wm;
        mhorizontal = Input.GetAxis("Mouse X");
        if (mhorizontal > 0 && mhorizontal > maxrspeed) mhorizontal = maxrspeed;
        if (mhorizontal < 0 && mhorizontal < -(maxrspeed)) mhorizontal = -(maxrspeed);

        Quaternion rotationx = Quaternion.AngleAxis(mhorizontal * (rspeed+wm*50f) * Time.deltaTime, Vector3.left);
        transform.rotation *= rotationx;

        Debug.Log("wm = " + wm);
    }
}
