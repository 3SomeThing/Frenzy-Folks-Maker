using Unity.XR.CoreUtils;
using UnityEngine;

public class PlayerPositionSpawner : MonoBehaviour
{
	// public static void SpawnAndRotatePlayerPosition(Vector3 position, Quaternion rotation) {
	// 	XROrigin origin = VrGameState.Instance.xrOrigin;
	//
	// 	Vector3 targetForward = rotation * Vector3.forward;
	// 	targetForward.y = 0f;
	// 	// origin.MatchOriginUpCameraForward(Vector3.up, targetForward.normalized);
	//
	// 	origin.transform.position = position;
	// 	origin.MatchOriginUpCameraForward(rotation * Vector3.up,  position);
	// 	
	// 	// origin.MoveCameraToWorldLocation(position);
	// }
	
	public static void SpawnAndRotatePlayerPosition(Vector3 position, Quaternion rotation)
	{
		XROrigin origin = VrGameState.Instance.xrOrigin;

		// Évite qu'un CharacterController écrase la téléportation
		var cc = origin.GetComponent<CharacterController>();
		if (cc != null) cc.enabled = false;

		// 1. Rotation : la caméra regarde dans la direction voulue (yaw seulement)
		Vector3 targetForward = rotation * Vector3.forward;
		targetForward.y = 0f;
		origin.MatchOriginUpCameraForward(Vector3.up, targetForward.normalized);

		// 2. Position : l'origin au sol sur `position`, tête centrée au-dessus
		Vector3 camOffsetXZ = origin.Camera.transform.position - origin.transform.position;
		camOffsetXZ.y = 0f;
		origin.transform.position = position - camOffsetXZ;

		Physics.SyncTransforms();
		if (cc != null) cc.enabled = true;

		Debug.Log($"Spawn demandé: {position} | origin: {origin.transform.position} | cam: {origin.Camera.transform.position}");
	}
}