using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int positionsCapacity = 10;
    [SerializeField] private int maxPositionsCount = 50;

    private float _floatingSize;
    private bool isStarted;

    private void Awake()
    {
        _floatingSize = Settings.Instance.TrackSettings.elenemtElementSize;
    }

    private void Start()
    {
        Track.OnTrackUpdated += (distance) => MoveAll();
    }

    public void Reset()
    {
        positionsCapacity = 0;
    }

    public void StartTrajectory(Vector3 position)
    {
        isStarted = true;
        AddPoint(position);
    }

    public void AddPoint(Vector3 position)
    {
        if (!isStarted) return;
        lineRenderer.positionCount++;
        position = new Vector3(position.x, 0.5f, position.z);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);

        if (lineRenderer.positionCount > maxPositionsCount)
        {
            Vector3[] newPositions = new Vector3[positionsCapacity];
            var currentIndx = positionsCapacity;

            for (var i = lineRenderer.positionCount - 1; i > lineRenderer.positionCount - positionsCapacity - 1; i--)
                newPositions[--currentIndx] = lineRenderer.GetPosition(i);

            lineRenderer.positionCount = positionsCapacity;
            lineRenderer.SetPositions(newPositions);
        }
    }

    public void SetLastPoint(Vector3 position)
    {
        if (!isStarted) return;
        if (lineRenderer.positionCount <= 1)
            lineRenderer.positionCount++;
        position = new Vector3(position.x, 0.501f, position.z);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
    }

    public void MoveAll()
    {
        for (var i = 0; i < lineRenderer.positionCount; i++)
            lineRenderer.SetPosition(i, lineRenderer.GetPosition(i) - Vector3.right * _floatingSize);
    }

}
