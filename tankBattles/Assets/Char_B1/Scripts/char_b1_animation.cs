using UnityEngine;
using System.Collections;

public class char_b1_animation : MonoBehaviour {
	public float BodyCannonAngle;
	float lastFrameBodyCannonAngle;
	Vector3 offsetBodyCannon;
	Transform boneBodyCannon;
	float maxBoneCannon = 25;
	float minBoneCannon = -25;





	public float TurretAngle;
	float lastFrameTurretAngle;
	Vector3 offsetTurret;
	Transform boneTurretRotation;
	float maxTurretAngle = 360;
	float minTurretAngle = -360;

	public float TurretCannonAngle;
	float lastFrameTurretCannonAngle;
	Vector3 offsetTurretCannon;
	Transform boneTurretCannon;
	float maxTurretCannon = 25;
	float minTurretCannon = -8;

	public float TreadRotationRate;
	Renderer tredRenderer;
	Vector2 uvOffset = new Vector2 (0.0f, 0.0f);

	// Use this for initialization
	void Start () {
		boneBodyCannon = transform.Find ("Root/BodyCannon");
		boneTurretRotation = transform.Find ("Root/connectBone001/TurretRotate");
		boneTurretCannon = transform.Find ("Root/connectBone001/TurretRotate/TurretToCannon/TurretCannon");
		offsetBodyCannon = boneBodyCannon.localEulerAngles;
		offsetTurret = boneTurretRotation.localEulerAngles;
		offsetTurretCannon = boneTurretCannon.localEulerAngles;
		tredRenderer = GetComponentInChildren<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (BodyCannonAngle != lastFrameBodyCannonAngle) {
			if (BodyCannonAngle > maxBoneCannon) {
				BodyCannonAngle = maxBoneCannon;
			}
			else if (BodyCannonAngle < minBoneCannon) {
				BodyCannonAngle = minBoneCannon;
			}
			boneBodyCannon.localEulerAngles = new Vector3 (offsetBodyCannon.x + BodyCannonAngle, offsetBodyCannon.y, offsetBodyCannon.z);
			lastFrameBodyCannonAngle = BodyCannonAngle;
		}

		if (TurretAngle != lastFrameTurretAngle) {
			if (TurretAngle > maxTurretAngle) {
				TurretAngle = maxTurretAngle;
			}
			else if (TurretAngle < minTurretAngle) {
				TurretAngle = minTurretAngle;
			}
			boneTurretRotation.localEulerAngles = new Vector3 (offsetTurret.x, offsetTurret.y + TurretAngle, offsetTurret.z);
			lastFrameTurretAngle = TurretAngle;
		}

		if (TurretCannonAngle != lastFrameTurretCannonAngle) {
			if (TurretCannonAngle > maxTurretCannon) {
				TurretCannonAngle = maxTurretCannon;
			}
			else if (TurretCannonAngle < minTurretCannon) {
				TurretCannonAngle = minTurretCannon;
			}
			boneTurretCannon.localEulerAngles = new Vector3 (offsetTurretCannon.x, offsetTurretCannon.y, offsetTurretCannon.z + TurretCannonAngle);
			lastFrameTurretCannonAngle = TurretCannonAngle;
		}
		if (TreadRotationRate != 0) {
			if (tredRenderer.enabled) {
				Vector2 uvAnimationRate = new Vector2 (TreadRotationRate, 0.0f);
				uvOffset += (uvAnimationRate * Time.deltaTime);
				tredRenderer.material.mainTextureOffset = uvOffset;
			}
		}
	}
}
