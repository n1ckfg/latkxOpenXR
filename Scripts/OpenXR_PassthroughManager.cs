using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenXR_PassthroughManager : MonoBehaviour {

    public OpenXR_TargetDevice targetDevice;

    private void Start() {
        switch (targetDevice.whichDevice) {
            case OpenXR_TargetDevice.WhichDevice.OCULUS:
                var passthrough = gameObject.AddComponent<OVRPassthroughLayer>();
                passthrough.overlayType = OVROverlay.OverlayType.Underlay;
                break;
        }
    }

}

