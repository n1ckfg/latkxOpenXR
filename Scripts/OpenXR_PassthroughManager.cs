using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenXR_PassthroughManager : MonoBehaviour {

    public OpenXR_TargetDevice targetDevice;

    private void Start() {
        switch (targetDevice.whichDevice) {
            case OpenXR_TargetDevice.WhichDevice.OCULUS:
                //var passthrough = gameObject.AddComponent<OVRPassthroughLayer>();
                //passthrough.overlayType = OVROverlay.OverlayType.Underlay;

                // https://github.com/DazzTDuck/Quest_Passthrough_MRTK3/blob/main/Assets/Oculus/SampleFramework/Usage/Passthrough/Scripts/EnableUnpremultipliedAlpha.cs
                // Since the alpha values for Selective Passthrough are written to the framebuffers after the color pass, we
                // need to ensure that the color values get multiplied by the alpha value during compositing. By default, this is
                // not the case, as framebuffers typically contain premultiplied color values. This step is only needed when
                // Selective Passthrough is non-binary (i.e. alpha values are neither 0 nor 1), and it doesn't work if the
                // framebuffer contains semi-transparent pixels even without Selective Passthrough, as those will have
                // premultiplied colors.
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_ANDROID
                OVRManager.eyeFovPremultipliedAlphaModeEnabled = false;
#endif
                break;
        }
    }

}

