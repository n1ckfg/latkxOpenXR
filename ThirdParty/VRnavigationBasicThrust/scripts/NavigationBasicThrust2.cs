using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class NavigationBasicThrust2 : MonoBehaviour
{
    public Rigidbody NaviBase;
    public Vector3 ThrustDirection;
    public float ThrustForce;
    public bool ShowThrustMockup = true;
    public GameObject ThrustMockup;

    public SteamVR_NewController ctl;
    FixedJoint joint;
    GameObject attachedObject;
    Vector3 tempVector;

    void FixedUpdate() {
        // add force
        if (ctl.triggerPressed) {
            tempVector = Quaternion.Euler(ThrustDirection) * Vector3.forward;
            NaviBase.AddForce(transform.rotation * tempVector * ThrustForce);
            NaviBase.maxAngularVelocity = 2f;
        }

        // show trust mockup
        if (ShowThrustMockup && ThrustMockup != null) {
            if (attachedObject == null && ctl.triggerDown) {
                attachedObject = (GameObject)GameObject.Instantiate(ThrustMockup, Vector3.zero, Quaternion.identity);
                attachedObject.transform.SetParent(this.transform, false);
                attachedObject.transform.Rotate(ThrustDirection);
            } else if (attachedObject != null && ctl.triggerUp) {
                Destroy(attachedObject);
            }
        }
    }
}
