using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColliderScript : MonoBehaviour {

    [SerializeField]
    Collider m_collider;

    Collider pendingOtherCollider;
	// Use this for initialization
	void Start () 
    {
        //m_collider = GetComponent<Collider>();
	}

	//// Update is called once per frame
	//void Update () {

	//}

	private void OnTriggerEnter(Collider _other)
	{
        if (TrackManager.Instance.characterControlScript.isJumping)
        {
            pendingOtherCollider = _other;
            return;
        }
        RegisterVehicleToCharacter(_other);
	}

	private void OnTriggerStay(Collider _other)
	{
        if (!TrackManager.Instance.characterControlScript.isJumping && pendingOtherCollider != null && pendingOtherCollider.Equals(_other))
        {
            RegisterVehicleToCharacter(_other);    
        }
	}

    void RegisterVehicleToCharacter (Collider _other)
    {
        SetTrigger(false);
        TrackManager.Instance.characterControlScript.RegisterVehicle(_other.GetComponent<VehicleControlScript>());
        pendingOtherCollider = null;
    }

	public void SetTrigger (bool _isTrigger)
    {
        m_collider.isTrigger = _isTrigger;
    }
}
