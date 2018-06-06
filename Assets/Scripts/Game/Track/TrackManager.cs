using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PrefabAttribute("Singleton/TrackManager")]
public class TrackManager : Singleton<TrackManager>
{
    [SerializeField]
    public bool withVehicles;
    [HideInInspector]
    public bool isTrackReady = false;
    [SerializeField]
    int totalTrackLength;

    public LevelConfig levelConfig { get; private set; }

    List<ObjectPool> trackPoolList = new List<ObjectPool>();
    public List<TrackSegment> presentedTrackSegment = new List<TrackSegment>();

    List<ObjectPool> vehiclePoolList = new List<ObjectPool>();
    public List<VehicleControlScript> vehicleControlScripts = new List<VehicleControlScript>();

    int currentTrackIndex = 0;
    int lastTrackIndex = 0;
    [SerializeField]
    int trackCountPerFrame = 5;

    [SerializeField]
    public float laneOffset = 2.0f;

    public float k_FloatingOriginThreshold = 10000f;

    bool firstVehicleCreated = false;

    public CharacterControlScript characterControlScript;

    private void OnEnable()
    {
        TimeManager.Instance.onUpdate += Instance_OnUpdate;
    }

    private void OnDisable()
    {
        isTrackReady = false;
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.onUpdate -= Instance_OnUpdate;
        }
    }

    public void Init()
    {
    }

    // Use this for initialization
    void Start()
    {
        //trackPool = new ObjectPool();
        InitTracksForLevel(0);
        LoadTrack(currentTrackIndex, currentTrackIndex + trackCountPerFrame);
        GenerateVehicle();
    }

    public void RegisterCharacter (CharacterControlScript _characterControlScript)
    {
        characterControlScript = _characterControlScript;
    }

    // Update is called once per frame
    void Instance_OnUpdate(float obj)
    {


    }

    public void InitTracksForLevel(int _level)
    {
        levelConfig = LevelManager.Instance.GetLevel(_level);

        lastTrackIndex = levelConfig.tracks.Count;

        for (int i = 0; i < levelConfig.tracks.Count; i++)
        {
            string trackName = levelConfig.trackPath + levelConfig.tracks[i];
            ObjectPool trackPool = trackPoolList.Find(item => levelConfig.tracks[i].Equals(item.name));
            if (trackPool == null)
            {
                GameObject aGameObject = Resources.Load<GameObject>(trackName);
                trackPool = new ObjectPool(aGameObject, 5);
                trackPoolList.Add(trackPool);
            }
        }
    }

    public void LoadTrack(int _currentIndex, int _endIndex)
    {
        currentTrackIndex = _endIndex;

        TrackSegment lastTrackSegment = null;
        if (presentedTrackSegment.Count > 0)
        {
            lastTrackSegment = presentedTrackSegment[presentedTrackSegment.Count - 1];
        }

        for (int i = _currentIndex; i < _endIndex; i++)
        {
            int index = i;
            if (levelConfig.randomTrack)
            {
                index = Random.Range(0, levelConfig.tracks.Count);
            }

            if (index >= levelConfig.tracks.Count)
            {
                Debug.LogError("Track not available!");
                index = levelConfig.tracks.Count - 1;
            }

            string trackName = levelConfig.tracks[index];
            ObjectPool trackPool = trackPoolList.Find(item => trackName.Equals(item.name));
            GameObject trackGameObject = trackPool.Get();
            TrackSegment segment = trackGameObject.GetComponent<TrackSegment>();
            segment.SetSegment(trackPool, i);

            Vector3 lastTrackExitPosition = new Vector3(0.0f, 0.0f, -1 * ((segment.endPoint.position.z + segment.startPoint.position.z) / 2.0f));
            if (lastTrackSegment != null)
            {
                lastTrackExitPosition = lastTrackSegment.endPoint.position;
            }
            trackGameObject.transform.position = new Vector3(trackGameObject.transform.position.x, trackGameObject.transform.position.y, lastTrackExitPosition.z + ((segment.endPoint.position.z + segment.startPoint.position.z) / 2.0f));
            presentedTrackSegment.Add(segment);
            lastTrackSegment = segment;
        }

        isTrackReady = true;
    }

    public TrackSegment FindCurrentSegmentAt(Vector3 _posision)
    {
        for (int i = 0; i < presentedTrackSegment.Count; i++)
        {
            TrackSegment trackSegment = presentedTrackSegment[i];
            if (trackSegment.startPoint.position.z <= _posision.z && trackSegment.endPoint.position.z >= _posision.z)
            {
                return trackSegment;
            }
        }

        return null;
    }

    public TrackSegment GetSegmentWithSegmentIndex (int _index)
    {
        TrackSegment segment = presentedTrackSegment.Find(item => item.segmentIndex.Equals(_index));
        if (segment == null)
        {
            segment = presentedTrackSegment[0];
        }

        return segment;
    }

    public void RemoveTrackSegement(int _index)
    {
        TrackSegment trackSegementToRemove = null;
        for (int i = 0; i < presentedTrackSegment.Count; i++)
        {
            trackSegementToRemove = presentedTrackSegment[i];
            if (trackSegementToRemove.segmentIndex.Equals(_index))
            {
                break;
            }
            trackSegementToRemove = null;
        }

        if (trackSegementToRemove == null)
        {
            return;
        }

        trackSegementToRemove.objectPool.Free(trackSegementToRemove.gameObject);
        presentedTrackSegment.Remove(trackSegementToRemove);

        LoadTrack(currentTrackIndex, currentTrackIndex + 1);
    }

    public void ReCenterTracks(Vector3 _offset)
    {
        int count = presentedTrackSegment.Count;
        for (int i = 0; i < count; i++)
        {
            presentedTrackSegment[i].transform.position -= _offset;
        }

        for (int i = 0; i < vehicleControlScripts.Count; i++)
        {
            VehicleControlScript vehicleControlScript = vehicleControlScripts[i];
            vehicleControlScript.transform.position -= _offset;
        }

        //count = m_PastSegments.Count;
        //for (int i = 0; i < count; i++)
        //{
        //    m_PastSegments[i].transform.position -= currentPos;
        //}

        // Recalculate current world position based on the moved world
        //m_Segments[0].GetPointAtInWorldUnit(m_CurrentSegmentDistance, out currentPos, out currentRot);
    }

    public void GenerateVehicle()
    {
        if (levelConfig == null)
        {
            return;
        }

        StartCoroutine(GenerateVehicleRoutine());
    }

    IEnumerator GenerateVehicleRoutine()
    {
        float nextVehicleAfter = 1.0f;
        if (withVehicles)
        {
            int randomVehicleIndex = Random.Range(0, levelConfig.vehicles.Count);
            string vehicleName = levelConfig.vehicles[randomVehicleIndex];

            ObjectPool vehiclePool = vehiclePoolList.Find(item => vehicleName.Equals(item.name));
            if (vehiclePool == null)
            {
                GameObject aGameObject = Resources.Load<GameObject>("Vehicles/" + vehicleName);
                vehiclePool = new ObjectPool(aGameObject, 5);
                vehiclePoolList.Add(vehiclePool);
            }

            GameObject vehicleGO = vehiclePool.Get();
            //vehicleGO.transform.localScale = Vector3.one;
            VehicleControlScript vehicleControlScript = vehicleGO.GetComponent<VehicleControlScript>();
            vehicleControlScript.SetupVehicle(vehiclePool, !firstVehicleCreated);
            vehicleControlScripts.Add(vehicleControlScript);
            firstVehicleCreated = true;

            nextVehicleAfter = Random.Range(levelConfig.timeBetweenVehicleMin, levelConfig.timeBetweenVehicleMax);
            yield return new WaitForSeconds(nextVehicleAfter);
            GenerateVehicle();
        }
    }

    public void RemoveVehicleFromTrack (VehicleControlScript _vehicleControlScript)
    {
        vehicleControlScripts.Remove(_vehicleControlScript);
    }

    public void RemoveAllVehicles ()
    {
        for (int i = 0; i < vehicleControlScripts.Count; i++)
        {
            vehicleControlScripts[i].Free();
        }

        characterControlScript.RegisterVehicle(null);
        vehicleControlScripts.Clear();
        firstVehicleCreated = false;
    }
}
