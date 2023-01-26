using UnityEngine;
using System.Collections;

public class LatkInputVive : MonoBehaviour {

	public SteamVR_NewController steamControllerMain;
	public SteamVR_NewController steamControllerAlt;
	public LightningArtist latk;
    public Renderer collisionGuideRen;

    private float collisionDelay = 0.2f;
    private float repeatDelay = 0.5f;

	void Awake() {
		if (latk == null) latk = GetComponent<LightningArtist>();
	}

    void Update() {
        // draw
        if ((steamControllerMain.triggerPressed && !steamControllerMain.menuPressed)) {// || Input.GetKeyDown(KeyCode.Space)) {
            latk.clicked = true;
        } else {
            latk.clicked = false;
        }

        if (steamControllerMain.triggerPressed && steamControllerMain.menuPressed) {
            latk.inputErase();
        } else if (!steamControllerMain.triggerPressed && steamControllerMain.menuPressed) {
            latk.inputPush();
            latk.inputColorPick();
        }

        // new frame
        if (steamControllerAlt.triggerDown && steamControllerMain.menuPressed) {
            latk.inputNewFrameAndCopy();
            Debug.Log("Ctl: New Frame Copy");
        } else if (steamControllerAlt.triggerDown && !steamControllerMain.menuPressed) {
            latk.inputNewFrame();
            Debug.Log("Ctl: New Frame");
        }

        // show / hide all frames
        if ((!steamControllerMain.menuPressed && steamControllerAlt.menuDown)) {
            latk.showOnionSkin = !latk.showOnionSkin;
            if (latk.showOnionSkin) {
                latk.inputShowFrames();
            } else {
                latk.inputHideFrames();
            }
        }

        // ~ ~ ~ ~ ~ ~ ~ ~ ~

        if (steamControllerMain.menuPressed && steamControllerAlt.menuDown) {
            latk.inputDeleteFrame();
        }

        // *** write ***
        if (steamControllerMain.padDirDown && steamControllerAlt.padDirDown) {
            if ((steamControllerMain.padDown && steamControllerAlt.padPressed) || (steamControllerMain.padPressed && steamControllerAlt.padDown)) {
                if (!latk.isWritingFile) latk.armWriteFile = true;
            }
        }

        // dir pad main
        if (steamControllerMain.padDown) {
            if (steamControllerMain.padDirCenter) {
                if (latk.brushMode == LightningArtist.BrushMode.ADD) {
                    latk.brushMode = LightningArtist.BrushMode.SURFACE;
                } else {
                    latk.brushMode = LightningArtist.BrushMode.ADD;
                }
            } else if (steamControllerMain.padDirUp) {
                StartCoroutine(delayedUseCollisions());
            }
        }

        if (steamControllerMain.padPressed) {
            if (steamControllerMain.padDirLeft) {
                latk.brushSizeInc();
            } else if (steamControllerMain.padDirRight) {
                latk.brushSizeDec();
            }
        }

        // dir pad alt
        if (steamControllerAlt.padDown) {
            if (steamControllerMain.menuPressed) {
                if (steamControllerAlt.padDirCenter) {
                    // TODO capture
                } else if (steamControllerAlt.padDirUp) {
                    latk.inputNewLayer();
                } else  if (steamControllerAlt.padDirLeft) {
                    latk.inputNextLayer();
                } else if (steamControllerAlt.padDirRight) {
                    latk.inputPreviousLayer();
                }
            } else {
                if (steamControllerAlt.padDirCenter) {
                    latk.inputPlay();
                } else if (steamControllerAlt.padDirUp) {
                    latk.inputFirstFrame();
                } else if (steamControllerAlt.padDirRight) {
                    latk.inputFrameBack();
                    StartCoroutine(repeatFrameBack());
                } else if (steamControllerAlt.padDirLeft) {
                    latk.inputFrameForward();
                    StartCoroutine(repeatFrameForward());
                }
            }
        } 
    }

    IEnumerator repeatFrameForward() {
        yield return new WaitForSeconds(repeatDelay);
        while (steamControllerAlt.padPressed && steamControllerAlt.padDirLeft) {
            latk.inputFrameForward();
            yield return new WaitForSeconds(latk.frameInterval);
        }
    }

    IEnumerator repeatFrameBack() {
        yield return new WaitForSeconds(repeatDelay);
        while (steamControllerAlt.padPressed && steamControllerAlt.padDirRight) {
            latk.inputFrameBack();
            yield return new WaitForSeconds(latk.frameInterval);
        }
    }

    IEnumerator delayedUseCollisions() {
        yield return new WaitForSeconds(collisionDelay);
        if (steamControllerMain.padDirUp) {
            latk.useCollisions = !latk.useCollisions;
            if (collisionGuideRen != null) collisionGuideRen.enabled = latk.useCollisions;
        }
    }

}
