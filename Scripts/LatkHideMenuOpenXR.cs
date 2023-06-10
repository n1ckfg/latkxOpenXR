using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatkHideMenuOpenXR : MonoBehaviour {

	public OpenXR_NewController[] ctl;
	public Renderer[] ren;

	private bool firstRun = true;
	private bool show = true; 

	void Start() {
		showHide(true);
	}

	void Update () {
		if (firstRun) {
			for (int i = 0; i < ctl.Length; i++) {
				if (Input.GetMouseButtonDown(0) || Input.anyKeyDown || ctl[i].triggerDown || ctl[i].menuDown || ctl[i].gripDown || ctl[i].padDown) {
					showHide(false);
					firstRun = false;
				}
			}
		} else if (Input.GetKeyDown (KeyCode.I)) {
			showHide(!show);
		}
	}

	void showHide(bool b) {
		show = b;
		for (int j = 0; j < ren.Length; j++) {
			ren [j].enabled = b;
		}
	}

}
