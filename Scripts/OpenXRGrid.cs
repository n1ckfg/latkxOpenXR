using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using PressureLib;

public class OpenXRGrid : MonoBehaviour {

    public LightningArtist latk;
    public Transform mainCtl;
    public Transform altCtl;
    public Renderer grid;
    public Vector2 cursor = Vector2.zero;
    public bool isActive = false;
    public float triggerDistance = 0.001f;
    public float timeout = 10f;

    [HideInInspector] public bool triggerDown = false;
    [HideInInspector] public bool triggerPressed = false;
    [HideInInspector] public bool triggerUp = false;
    [HideInInspector] public bool menuDown = false;
    [HideInInspector] public bool menuPressed = false;
    [HideInInspector] public bool menuUp = false;

    private Vector3 origPos;
    private Quaternion origRot;
    private Vector2 lastCursor = Vector2.zero;
    private float lastCursorDist = 0f;
    //private float sensitivityX = 2f;
    //private float sensitivityY = 2f;
    private bool blockRetrigger = false;
    private float retriggerTimeout = 5f;
    private float timeoutCounter = 0f;
    private Vector2 posOffset = new Vector2(-0.5f, -0.5f);
    private float gridOffset = 0.36f;

    private void Awake() {
        if (!latk.useCollisions) grid.enabled = false;

        // for some reason sensitivity is greater in build than in editor
//#if !UNITY_EDITOR
		//float sensReduce = 8f;
		//sensitivityX /= sensReduce;
		//sensitivityY /= sensReduce;
//#endif
    }

    private void Start() {
        posOffset += new Vector2(0.025f, 0.025f);
        origPos = transform.localPosition;
        origRot = transform.localRotation;

        posOffset.y += gridOffset/2f;
        grid.transform.localPosition = new Vector3(grid.transform.localPosition.x, grid.transform.localPosition.y, grid.transform.localPosition.z + gridOffset);

        cursorUpdate();
    }

    private void Update() {
        cursorUpdate();

        if (Input.GetMouseButton(0)) timeoutCounter = 0;

        if (lastCursorDist >= triggerDistance) {
            timeoutCounter = 0f;
            if (!isActive && !blockRetrigger) {
                wacomModeStart();
            }
        } else {
            timeoutCounter += Time.deltaTime;
            if (timeoutCounter > timeout) wacomModeEnd();
        }

        if (isActive) {
            wacomModeUpdate();
        }
    }

    void cursorUpdate() {
        lastCursor = new Vector2(cursor.x, cursor.y);
        //cursor = new Vector2((PressureManager.cursorPosition.x / Screen.width) + posOffset.x, (PressureManager.cursorPosition.y / Screen.width) + posOffset.y + gridOffset);
        cursor = new Vector2((1f / Screen.width) + posOffset.x, (1f / Screen.width) + posOffset.y + gridOffset);
        lastCursorDist = Vector2.Distance(cursor, lastCursor);
    }

    void wacomModeStart() {
        latk.useCollisions = false;
        timeoutCounter = 0f;
        isActive = true;
        grid.enabled = true;
        transform.parent = altCtl.transform;
        transform.localPosition = origPos;
        transform.localRotation = origRot;
    }

    void wacomModeEnd() {
        timeoutCounter = 0f;
        isActive = false;
        if (!latk.useCollisions) grid.enabled = false;
        transform.parent = mainCtl.transform;
        transform.localPosition = origPos;
        transform.localRotation = origRot;
        StartCoroutine(startTimeout());
    }

    void wacomModeUpdate() {
        transform.localPosition = new Vector3(cursor.x, transform.localPosition.y, cursor.y);

        triggerDown = Input.GetMouseButtonDown(0);
        triggerPressed = Input.GetMouseButton(0);
        triggerUp = Input.GetMouseButtonUp(0);

        menuDown = Input.GetMouseButtonDown(1);
        menuPressed = Input.GetMouseButton(1);
        menuUp = Input.GetMouseButtonUp(1);
    }

    private IEnumerator startTimeout() {
        blockRetrigger = true;
        yield return new WaitForSeconds(retriggerTimeout);
        blockRetrigger = false;
    }

}
 