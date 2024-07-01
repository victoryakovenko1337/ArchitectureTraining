using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _followingObject;
    [SerializeField] private float _rotationAngleX;
    [SerializeField] private float _distance;
    [SerializeField] private float _offsetY;

    private void LateUpdate()
    {
        if (_followingObject == null)
            return;

        var rotation = Quaternion.Euler(_rotationAngleX, 0, 0);

        var followingPosition = _followingObject.position;
        followingPosition.y += _offsetY;

        var position = rotation * new Vector3(0, 0, -_distance) + followingPosition;

        transform.rotation = rotation;
        transform.position = position;
    }

    public void Follow(GameObject following)
    {
        _followingObject = following.transform;
    }
}
