using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleControlScript : MonoBehaviour, IGameObject
{
    LevelConfig levelConfig;
    public float speed { get; set; }

    public GameObjectType gameObjectType { get; set; }

    ObjectPool vehiclePool;

    private void OnEnable()
    {
        TimeManager.Instance.onUpdate += Instance_OnUpdate;
    }

    private void OnDisable()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.onUpdate -= Instance_OnUpdate;
        }
    }

    // Use this for initialization
	void Start () 
    {
        
	}
	
    public void SetupVehicle (ObjectPool _vehiclePool, bool _isFirstVehicle)
    {
        gameObjectType = GameObjectType.Vehicle;
        vehiclePool = _vehiclePool;
        levelConfig = TrackManager.Instance.levelConfig;

        // set track
        int randomTrack = UnityEngine.Random.Range(0, 3);
        Vector3 randomTrackPosition = Vector3.zero;
        if (randomTrack == 0)
        {
            randomTrackPosition.x = -TrackManager.Instance.laneOffset;
        }
        if (randomTrack == 2)
        {
            randomTrackPosition.x = TrackManager.Instance.laneOffset;
        }

        speed = UnityEngine.Random.Range(levelConfig.vehicleSpeedMin, levelConfig.vehicleSpeedMax);
        if (TrackManager.Instance.characterControlScript != null)
        {
            TrackSegment trackSegment = TrackManager.Instance.FindCurrentSegmentAt(TrackManager.Instance.characterControlScript.transform.position);

            if (trackSegment != null)
            {
                int random = UnityEngine.Random.Range(0, 2);
                bool fromFront = Convert.ToBoolean(random);

                int segmentIndex = trackSegment.segmentIndex - 1;
                speed = UnityEngine.Random.Range(levelConfig.vehicleSpeedMax * 0.5f, levelConfig.vehicleSpeedMax);
                if (fromFront)
                {
                    segmentIndex = trackSegment.segmentIndex + 5;
                    speed = UnityEngine.Random.Range(levelConfig.vehicleSpeedMin, levelConfig.vehicleSpeedMax * 0.5f);
                }

                trackSegment = TrackManager.Instance.GetSegmentWithSegmentIndex(segmentIndex);
                randomTrackPosition.z = trackSegment.startPoint.position.z;
            }

            if (_isFirstVehicle)
            {
                randomTrackPosition = new Vector3(TrackManager.Instance.characterControlScript.characterColliderScript.transform.position.x, 
                                                  TrackManager.Instance.characterControlScript.transform.position.y, 
                                                  TrackManager.Instance.characterControlScript.transform.position.z);
            }
        }
        else
        {
            // very first time. character will be created after vehicle.
            randomTrackPosition = Vector3.zero;
        }
        transform.position = randomTrackPosition;
    }

    private void Instance_OnUpdate(float _deltaTime)
    {
        float scaledSpeed = speed * _deltaTime;
        Vector3 movingDirection = transform.forward;
        movingDirection *= scaledSpeed;
        transform.position = transform.position + movingDirection;
        //forwardTargetPosition = transform.position;

        TrackSegment trackSegment = TrackManager.Instance.FindCurrentSegmentAt(transform.position);
        if (trackSegment == null)
        {
            Free();
            TrackManager.Instance.RemoveVehicleFromTrack(this);
        }
    }

	private void OnTriggerEnter(Collider _other)
	{
        IGameObject iGameObject = _other.GetComponent<IGameObject>();
        if (iGameObject != null && iGameObject is VehicleControlScript)
        {
            VehicleControlScript otherVehicle = iGameObject as VehicleControlScript;
            if (otherVehicle.transform.position.z < transform.position.z)
            {
                iGameObject.speed *= 0.8f;
                speed *= 1.2f;
            }
        }
	}

    public void Free ()
    {
        vehiclePool.Free(gameObject);
    }
}
