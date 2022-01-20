using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraplingHook : MonoBehaviour
{
    public GameObject Silah, kamera, Namlu;

    public float Mesafe, AimAssist;

    SpringJoint joint;

    public LayerMask Tutunulabilir;

    LineRenderer lr;

    bool tutundu;

    Vector3 TutNokta;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.SphereCast(kamera.transform.position, AimAssist, kamera.transform.forward, out hit, Mesafe, Tutunulabilir))
            {
                TutNokta = hit.point;
                joint = gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = TutNokta;
                float TutMesafe = Vector3.Distance(transform.position, TutNokta);
                joint.maxDistance = TutMesafe * 0.8f;
                joint.minDistance = TutMesafe / 0.2f;
                joint.spring = 6f;
                joint.damper = 8f;
                joint.massScale = 4f;
                tutundu = true;
                lr.positionCount = 2;
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            Destroy(joint);
            tutundu = false;
            Silah.transform.rotation = new Quaternion(0,0,0,0);
            lr.positionCount = 0;
        }
        if(tutundu)
        {
            lr.SetPosition(0, Namlu.transform.position);
            lr.SetPosition(1, TutNokta);
            Silah.transform.LookAt(TutNokta);

            if(Input.GetMouseButton(1))
            {
                transform.position = Vector3.Slerp(transform.position, TutNokta, 8 * Time.deltaTime);

                float TutMesafe = Vector3.Distance(transform.position, TutNokta);

                if (TutMesafe <= 1)
                {
                    Destroy(joint);
                    tutundu = false;
                    Silah.transform.rotation = new Quaternion(0, 0, 0, 0);
                    lr.positionCount = 0;
                }
            }
        }
    }
}
