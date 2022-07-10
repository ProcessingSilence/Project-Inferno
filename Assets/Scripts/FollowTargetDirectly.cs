using UnityEngine;
using UnityEngine.Serialization;

public class FollowTargetDirectly : MonoBehaviour
{
    [FormerlySerializedAs("player")] [SerializeField] private Transform target;
    [SerializeField] private bool lockX, lockY, lockZ;
    [SerializeField] private Vector3 offset;

    private Vector3 lockedPos;

    private void Awake()
    {
        lockedPos = transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 currentPos = target.position + offset;
        Vector3 setPos = new Vector3
        (
            lockX ? lockedPos.x : currentPos.x,
            lockY ? lockedPos.y : currentPos.y,
            lockZ ? lockedPos.z : currentPos.z
        );
        transform.position = setPos;
    }
}
