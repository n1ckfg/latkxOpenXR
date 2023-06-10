using UnityEngine;
using System.Collections;

public class OpenXRPaletteToggle : MonoBehaviour {

	public OpenXR_NewController ctl;
	public GameObject laser;

	public enum CtlModeL { JET, COLOR };
	public CtlModeL ctlModeL = CtlModeL.JET;

	// jet
	//public Rigidbody rb;

	// color
	public GameObject colorModeObj;

	//private bool fixLaserRot = false;

	void Awake() {
		colorModeObj.SetActive(false);
	}

	void Start() {
		switchCtlMode(CtlModeL.JET);
	}

	void Update() {
		if (ctl.menuDown) {
			switchCtlMode(CtlModeL.COLOR);
		} else if (ctl.menuUp) {
			switchCtlMode(CtlModeL.JET);
		}
	}

	public void switchCtlMode(CtlModeL mode) {
		ctlModeL = mode;
		if (ctlModeL == CtlModeL.JET) {
			colorModeObj.SetActive(false);
			//rb.isKinematic = false;
			laser.SetActive(false);
		}

		if (ctlModeL == CtlModeL.COLOR) {
			//rb.isKinematic = true;
			colorModeObj.SetActive(true);
			laser.SetActive(true);
		}
	}

}
