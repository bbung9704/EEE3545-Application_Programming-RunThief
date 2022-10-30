using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float moveDump = 3f;
    [SerializeField] private float rotateDump = 15f;
    private Vector3 offset;
    private Vector3 playerDirection;
    private Quaternion playerRotation;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        Vector3 cameraPosition = player.transform.position + (player.transform.rotation * offset);
        transform.position = Vector3.Lerp(transform.position, cameraPosition, moveDump * Time.deltaTime);

        Quaternion playerRotation = Quaternion.LookRotation(player.transform.position - transform.position, player.transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, rotateDump * Time.deltaTime);
    }
}
