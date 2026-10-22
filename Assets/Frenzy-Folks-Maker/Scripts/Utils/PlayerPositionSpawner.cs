using Unity.XR.CoreUtils;
using UnityEngine;

public class PlayerPositionSpawner : MonoBehaviour
{
	public static void SpawnAndRotatePlayerPosition(Vector3 position, Quaternion rotation)
	{
		XROrigin origin = VrGameState.Instance.xrOrigin;

		var cc = origin.GetComponent<CharacterController>();
		if (cc != null) cc.enabled = false;

		Vector3 targetForward = rotation * Vector3.forward;
		targetForward.y = 0f;
		origin.MatchOriginUpCameraForward(Vector3.up, targetForward.normalized);

		Vector3 camOffsetXZ = origin.Camera.transform.position - origin.transform.position;
		camOffsetXZ.y = 0f;
		origin.transform.position = position - camOffsetXZ;

		Physics.SyncTransforms();
		if (cc != null) cc.enabled = true;
	}
}