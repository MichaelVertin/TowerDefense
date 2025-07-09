using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] UnitPath path;
    [SerializeField] float _speed = 300f;

    protected float _distance = 0.0f;

    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    public void Init(UnitPath path)
    {
        this.path = path;
    }

    void FixedUpdate()
    {
        path.UpdateTransformAtDistance(this.transform, _distance);
        _distance += .0001f * _speed * Time.deltaTime;
    }
}
