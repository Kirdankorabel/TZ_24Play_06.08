using UnityEngine;

[CreateAssetMenu(fileName = "TrackSettings", menuName = "Settings", order = 1)]
public class TrackSettings : ScriptableObject
{
    public int platformCount = 4;
    public int trackElementCount = 4;
    public readonly int elenemtElementSize = 40;
    public readonly int min = -2;
    public readonly int max = 2;
}