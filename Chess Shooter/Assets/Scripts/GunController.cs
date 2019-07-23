using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

	public Transform weaponHold;
	public Gun[] allGuns;
    public Transform gunPreviewPosition;
    public GunPreview[] gunsPreviews;
    private bool[] unlocked = { true,false,false,false,false};
    private int equiped = 0;

    Gun equippedGun;
    GunPreview preview;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && equiped != 0)
        {
            EquipGun(0);
        }else if (Input.GetKeyDown(KeyCode.Alpha2) && equiped != 1 && unlocked[1] == true)
        {
            EquipGun(1);
        } else if (Input.GetKeyDown(KeyCode.Alpha3) && equiped != 2 && unlocked[2] == true)
        {
            EquipGun(2);
        } else if (Input.GetKeyDown(KeyCode.Alpha4) && equiped != 3 && unlocked[3] == true)
        {
            EquipGun(3);
        } else if (Input.GetKeyDown(KeyCode.Alpha5) && equiped != 4 && unlocked[4] == true)
        {
            EquipGun(4);
        }
    }

    public void ResetGunPreview()
    {
        if (preview != null)
        {
            Destroy(preview.gameObject);
        }
    }

    public void EquipGun(Gun gunToEquip) {
		if (equippedGun != null) {
			Destroy(equippedGun.gameObject);
		}
		equippedGun = Instantiate (gunToEquip, weaponHold.position,weaponHold.rotation) as Gun;
		equippedGun.transform.parent = weaponHold;
	}

	public void EquipGun(int weaponIndex) {
        equiped = weaponIndex;
        EquipGun (allGuns [weaponIndex]);
        if (preview != null)
        {
            Destroy(preview.gameObject);
        }
        preview = Instantiate(gunsPreviews[weaponIndex], gunPreviewPosition.position, gunPreviewPosition.rotation) as GunPreview;
        preview.transform.parent = gunPreviewPosition;
    }

    public void UnlockGun(int gun)
    {
        unlocked[gun] = true;
    }
	public void OnTriggerHold() {
		if (equippedGun != null) {
			equippedGun.OnTriggerHold();
		}
	}

	public void OnTriggerRelease() {
		if (equippedGun != null) {
			equippedGun.OnTriggerRelease();
		}
	}

	public float GunHeight {
		get {
			return weaponHold.position.y;
		}
	}

	public void Aim(Vector3 aimPoint) {
		if (equippedGun != null) {
			equippedGun.Aim(aimPoint);
		}
	}

	public void Reload() {
		if (equippedGun != null) {
			equippedGun.Reload();
		}
	}

}