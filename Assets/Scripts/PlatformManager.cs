using UnityEngine;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour
{
    public Transform player;
    public List<Transform> platformParents = new List<Transform>();

    private List<PlatformUnit> platforms = new List<PlatformUnit>();

    void Start()
    {

        if (platformParents.Count < 3)
        {
            Debug.LogError("En az 3 platform parent eklemelisin!");
            return;
        }


        foreach (Transform parent in platformParents)
        {
            platforms.Add(new PlatformUnit(parent));
        }


        for (int i = 1; i < platforms.Count; i++)
        {
            Align(platforms[i], platforms[i - 1]);
        }
    }

    void Update()
    {
        if (platforms.Count < 2) return;


        if (player.position.z > platforms[1].MidZ)
        {
            MoveFirstToEnd();
        }
    }

    void MoveFirstToEnd()
    {
        PlatformUnit first = platforms[0];
        PlatformUnit last = platforms[platforms.Count - 1];

        Align(first, last);

        platforms.RemoveAt(0);
        platforms.Add(first);
    }

    void Align(PlatformUnit toMove, PlatformUnit reference)
    {
        Vector3 offset =
            reference.EndPos.position - toMove.StartPos.position;

        toMove.Parent.position += offset;
        toMove.UpdateMid();
    }
}

class PlatformUnit
{
    public Transform Parent;
    public Transform StartPos;
    public Transform EndPos;
    public float MidZ;

    public PlatformUnit(Transform parent)
    {
        Parent = parent;

        StartPos = parent.Find("Platform_1/StartPos") ??
                   parent.Find("StartPos");

        EndPos = parent.Find("Platform_1/EndPos") ??
                   parent.Find("EndPos");

        if (StartPos == null || EndPos == null)
        {
            Debug.LogError(parent.name + " içinde StartPos veya EndPos bulunamadı!");
        }

        UpdateMid();
    }

    public void UpdateMid()
    {
        MidZ = (StartPos.position.z + EndPos.position.z) * 0.5f;
    }
}
