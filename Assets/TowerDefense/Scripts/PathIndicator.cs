using UnityEngine;

public class PathIndicator : PathFollower
{
    public override void OnEndPath()
    {
        base.OnEndPath();
        Destroy(this.gameObject);
    }
}
