using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetLocal : MonoBehaviour
{
    public Transform target;

    private Vector3 diff;

    private void Awake()
    {
        diff = target.position - transform.position;
    }

    private void Update()
    {
        transform.position = target.position - diff;
    }
}
