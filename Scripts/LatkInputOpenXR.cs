using UnityEngine;
using System.Collections;

public class LatkInputOpenXR : MonoBehaviour {

	public OpenXR_NewController ctlMain;
	public OpenXR_NewController ctlAlt;
	public LightningArtist latk;
    public Renderer collisionGuideRen;
    public Collider collisionGuideCol;
    public Inference_informative onnx;

    private float collisionDelay = 0.2f;
    private float repeatDelay = 0.5f;

	void Awake() {
		if (latk == null) latk = GetComponent<LightningArtist>();
	}

    void Update() {
        // draw
        if ((ctlMain.triggerPressed && !ctlMain.menuPressed)) {// || Input.GetKeyDown(KeyCode.Space)) {
            latk.clicked = true;
        } else {
            latk.clicked = false;
        }

        if (ctlMain.triggerPressed && ctlMain.menuPressed) {
            latk.inputErase();
        } else if (!ctlMain.triggerPressed && ctlMain.menuPressed) {
            latk.inputPush();
            latk.inputColorPick();
        }

        // new frame
        if (ctlAlt.triggerDown && ctlMain.menuPressed) {
            latk.inputNewFrameAndCopy();
            Debug.Log("Ctl: New Frame Copy");
        } else if (ctlAlt.triggerDown && !ctlMain.menuPressed) {
            latk.inputNewFrame();
            Debug.Log("Ctl: New Frame");
        }

        // show / hide all frames
        if ((!ctlMain.menuPressed && ctlAlt.menuDown)) {
            latk.showOnionSkin = !latk.showOnionSkin;
            if (latk.showOnionSkin) {
                latk.inputShowFrames();
            } else {
                latk.inputHideFrames();
            }
        }

        // ~ ~ ~ ~ ~ ~ ~ ~ ~

        if (onnx != null && (ctlMain.menu2Pressed && ctlAlt.menu2Pressed)) {
            onnx.DoInference();
        }

        if (ctlMain.menuPressed && ctlAlt.menuDown) {
            latk.inputDeleteFrame();
        }

        // *** write ***
        if (ctlMain.padDirDown && ctlAlt.padDirDown) {
            if ((ctlMain.padDown && ctlAlt.padPressed) || (ctlMain.padPressed && ctlAlt.padDown)) {
                if (!latk.isWritingFile) latk.armWriteFile = true;
            }
        }

        // dir pad main
        if (ctlMain.padDown) {
            if (ctlMain.padDirCenter) {
                if (latk.brushMode == LightningArtist.BrushMode.ADD) {
                    latk.brushMode = LightningArtist.BrushMode.SURFACE;
                } else {
                    latk.brushMode = LightningArtist.BrushMode.ADD;
                }
            } else if (ctlMain.padDirUp) {
                StartCoroutine(delayedUseCollisions());
            }
        }

        if (ctlMain.padPressed) {
            if (ctlMain.padDirLeft) {
                latk.brushSizeInc();
            } else if (ctlMain.padDirRight) {
                latk.brushSizeDec();
            }
        }

        // dir pad alt
        if (ctlAlt.padDown) {
            if (ctlMain.menuPressed) {
                if (ctlAlt.padDirCenter) {
                    // TODO capture
                } else if (ctlAlt.padDirUp) {
                    latk.inputNewLayer();
                } else  if (ctlAlt.padDirLeft) {
                    latk.inputNextLayer();
                } else if (ctlAlt.padDirRight) {
                    latk.inputPreviousLayer();
                }
            } else {
                if (ctlAlt.padDirCenter) {
                    latk.inputPlay();
                } else if (ctlAlt.padDirUp) {
                    latk.inputFirstFrame();
                } else if (ctlAlt.padDirRight) {
                    latk.inputFrameBack();
                    StartCoroutine(repeatFrameBack());
                } else if (ctlAlt.padDirLeft) {
                    latk.inputFrameForward();
                    StartCoroutine(repeatFrameForward());
                }
            }
        } 
    }

    IEnumerator repeatFrameForward() {
        yield return new WaitForSeconds(repeatDelay);
        while (ctlAlt.padPressed && ctlAlt.padDirLeft) {
            latk.inputFrameForward();
            yield return new WaitForSeconds(latk.frameInterval);
        }
    }

    IEnumerator repeatFrameBack() {
        yield return new WaitForSeconds(repeatDelay);
        while (ctlAlt.padPressed && ctlAlt.padDirRight) {
            latk.inputFrameBack();
            yield return new WaitForSeconds(latk.frameInterval);
        }
    }

    IEnumerator delayedUseCollisions() {
        yield return new WaitForSeconds(collisionDelay);
        if (ctlMain.padDirUp) {
            latk.useCollisions = !latk.useCollisions;
            if (collisionGuideRen != null) collisionGuideRen.enabled = latk.useCollisions;
            if (collisionGuideCol != null) collisionGuideCol.enabled = latk.useCollisions;
        }
    }

}
