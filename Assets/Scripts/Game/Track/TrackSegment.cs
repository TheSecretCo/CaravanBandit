using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSegment : MonoBehaviour
{
    [SerializeField]
    public Transform startPoint;
    [SerializeField]
    public Transform endPoint;

    public ObjectPool objectPool { get; private set; }
    public int segmentIndex { get; private set; }

    // Use this for initialization
    void Start ()
    {
        //Debug.LogError(startPoint.position);
        //Debug.LogError(endPoint.position);
        //Debug.LogError(endPoint.position - startPoint.position);
    }

    //// Update is called once per frame
    //void Update ()
    //{

    //}

    public void SetSegment (ObjectPool _objectPool, int _index)
    {
        objectPool = _objectPool;
        segmentIndex = _index;
    }
}
