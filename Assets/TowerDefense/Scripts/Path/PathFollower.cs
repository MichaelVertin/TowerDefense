using UnityEngine;

public class PathFollower : MonoBehaviour
{
    UnitPath path;
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
        path.UpdateTransformAtDistance(this, _distance);
    }

    public virtual void FixedUpdate()
    {
        _distance += _speed * Time.deltaTime * .01f;
        path.UpdateTransformAtDistance(this, _distance);
    }

    public virtual void OnEndPath()
    {

    }
}
