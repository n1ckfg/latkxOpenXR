using UnityEngine;
using System.Collections;

public class LatkPaletteToggleOpenXR : MonoBehaviour {

	public OpenXR_NewController ctl;
	public GameObject laser;
	public enum CtlModel { OFF, COLOR };
	public CtlModel ctlModeL = CtlModel.OFF;
	public GameObject colorModeObj;

	void Awake() {
		colorModeObj.SetActive(false);
	}

	void Start() {
		switchCtlMode(CtlModel.OFF);
	}

	void Update() {
		if (ctl.menuDown) {
			switchCtlMode(CtlModel.COLOR);
		} else if (ctl.menuUp) {
			switchCtlMode(CtlModel.OFF);
		}
	}

	public void switchCtlMode(CtlModel mode) {
		ctlModeL = mode;
		if (ctlModeL == CtlModel.OFF) {
			colorModeObj.SetActive(false);
			laser.SetActive(false);
		}

		if (ctlModeL == CtlModel.COLOR) {
			colorModeObj.SetActive(true);
			laser.SetActive(true);
		}
	}

}
