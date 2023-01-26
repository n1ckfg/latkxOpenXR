// https://stackoverflow.com/questions/39437044/how-to-pick-color-from-raycast-hit-point-in-unity
// https://www.youtube.com/watch?v=wysIsMEQ3_Y

using UnityEngine;

public class DraggableVive : MonoBehaviour {

	public SteamVR_NewController steamCtl;
	public Transform minBound;
	public bool fixX = false;
	public bool fixY = false;
	public bool fixZ = false;
	public Transform thumb;	

	bool dragging;

	void FixedUpdate() {
		Vector3 rayPos = steamCtl.transform.position;
		Vector3 rayDir = steamCtl.transform.forward;

		if (steamCtl.menuPressed) {
			dragging = false;
			Ray ray = new Ray(rayPos, rayDir);
			RaycastHit hit;
			if (GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity)) {
				dragging = true;
			}
		}

		if (steamCtl.menuUp) dragging = false;

		if (dragging && steamCtl.menuPressed) {
			Ray ray = new Ray(rayPos, rayDir);
			RaycastHit hit;
			if (GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity)) {
				var point = hit.point;
				SetThumbPosition(point); 
				Vector3 message = Vector3.one - (thumb.localPosition - minBound.localPosition) / GetComponent<BoxCollider>().size.x;

				SendMessage("OnDrag", message);
			}
		}
	}

	void SetThumbPosition(Vector3 point) {
		Vector3 temp = thumb.localPosition;
		thumb.position = point;
		thumb.localPosition = new Vector3(fixX ? temp.x : thumb.localPosition.x, fixY ? temp.y : thumb.localPosition.y, thumb.localPosition.z-1);
	}

}
