using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;

    [SerializeField] Transform camChild;
    [SerializeField] Transform floorBuild;

    RaycastHit hit;

    public float buildReach = 10;
    public int gridSize = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(camChild.position, camChild.forward, out hit, buildReach, layerMask))
        {
            floorBuild.position = new Vector3(Mathf.RoundToInt(hit.point.x) != 0 ? Mathf.RoundToInt(hit.point.x/gridSize) * gridSize : gridSize,
            (Mathf.RoundToInt(hit.point.y) != 0 ? Mathf.RoundToInt(hit.point.y/gridSize) * gridSize : 0) + floorBuild.localScale.y,
            Mathf.RoundToInt(hit.point.z) != 0 ? Mathf.RoundToInt(hit.point.z / gridSize) * gridSize : gridSize);
            print(hit.transform.gameObject.name);
        }
    }
}
