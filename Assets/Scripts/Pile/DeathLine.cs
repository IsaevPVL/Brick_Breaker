using UnityEngine;

//[ExecuteInEditMode]
public class DeathLine : MonoBehaviour
{
    private void Start()
    {
        Boundaries boundaries = GameObject.FindObjectOfType<Boundaries>();
        Vector3 deathLineLevel = GameObject.Find("Death Line Level").transform.position;

        transform.position = deathLineLevel;

        GetComponent<BoxCollider>().size = new Vector3(boundaries.deeperCorners[1].x * 2f, 0, 1);

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(new Vector3[] {
            new Vector3(boundaries.deeperCorners[0].x, transform.position.y, transform.position.z),
            new Vector3(boundaries.deeperCorners[1].x, transform.position.y, transform.position.z)
            });
    }
}