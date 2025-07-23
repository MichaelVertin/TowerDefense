using System.Collections;
using UnityEngine;

public class PathIndicatorGenerator : MonoBehaviour
{
    [SerializeField] PathIndicator pathIndicatorPrefab;
    [SerializeField] UnitPath path;
    [SerializeField] int indicatorGroupSize = 3;
    [SerializeField] float timeBetweenIndicators = .25f;
    [SerializeField] float timeBetweenIndicatorGroups = 3f;
    [SerializeField] float indicatorGroupCount = 3;

    private void Start()
    {
        StartCoroutine(SpawnIndicators());
    }

    private IEnumerator SpawnIndicators()
    {
        yield return null;
        for (int j = 0; j < indicatorGroupCount; j++)
        {
            for(int i = 0; i < indicatorGroupSize; i++)
            {
                PathIndicator indicator = Instantiate<PathIndicator>(pathIndicatorPrefab);
                indicator.Init(path);
                yield return new WaitForSeconds(timeBetweenIndicators);
            }
            yield return new WaitForSeconds(timeBetweenIndicatorGroups - indicatorGroupSize * timeBetweenIndicators);
        }
    }
}
