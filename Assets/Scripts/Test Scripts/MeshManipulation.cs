using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MeshManipulation : MonoBehaviour
{
    public float lineWidth = 1f;
    public Material material;

    public struct Edge
    {
        public Vector3 v1;
        public Vector3 v2;

        public Edge(Vector3 v1, Vector3 v2)
        {
            if (v1.x < v2.x || (v1.x == v2.x && (v1.y < v2.y || (v1.y == v2.y && v1.z <= v2.z))))
            {
                this.v1 = v1;
                this.v2 = v2;
            }
            else
            {
                this.v1 = v2;
                this.v2 = v1;
            }
        }
    }

    void Start()
    {
        this.GetComponent<MeshRenderer>().enabled = false;

        Mesh objectMesh = this.GetComponent<MeshFilter>().mesh;
        //Edge[] edges = GetMeshEdges(objectMesh);
        List <Vector3> vertices = GetMeshVertices(objectMesh);

        //Printing in log
        // for (int i = 0; i < edges.Length; i++)
        // {
        //    print(i + ": " + edges[i].v1 + ", " + edges[i].v2);
        // }

        //DrawEdgeLines(edges);
        DrawVerticesLines(vertices);

    }

    private Edge[] GetMeshEdges(Mesh mesh)
    {
        HashSet<Edge> edges = new HashSet<Edge>();

        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 v1 = mesh.vertices[mesh.triangles[i]];
            Vector3 v2 = mesh.vertices[mesh.triangles[i + 1]];
            Vector3 v3 = mesh.vertices[mesh.triangles[i + 2]];
            edges.Add(new Edge(v1, v2));
            edges.Add(new Edge(v1, v3));
            edges.Add(new Edge(v2, v3));
        }

        return edges.ToArray();
    }

    void DrawEdgeLines(Edge[] edges){
        foreach(Edge edge in edges){
            GameObject lineObject = new GameObject();
            lineObject.transform.parent = this.transform;
            MeshRenderer meshRenderer = lineObject.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = lineObject.AddComponent<MeshFilter>();
            LineRenderer line =  lineObject.AddComponent<LineRenderer>();

            line.positionCount = 2;
            line.SetPosition(0, transform.TransformPoint(edge.v1));
            line.SetPosition(1, transform.TransformPoint(edge.v2));
            line.material = material;
            //line.startColor = Color.red;
            //line.endColor = Color.red;
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.numCapVertices = 1;
            line.alignment = LineAlignment.TransformZ;

            Mesh mesh = new Mesh();
            meshFilter.mesh = mesh;
            meshRenderer.material = material;
            line.BakeMesh(mesh, false);

            line.enabled = false;
        }
    }

    List<Vector3> GetMeshVertices(Mesh mesh){
        mesh = GetComponent<MeshFilter>().mesh;
        Vector3 [] vertices = mesh.vertices;
  
        List<Vector3> VertexList = new List<Vector3>();
        foreach(Vector3 vertex in vertices)
        {
            VertexList.Add(vertex);
 
        }
        VertexList = VertexList.Distinct().ToList();
        return VertexList;
    }

    void DrawVerticesLines(List<Vector3> vertices){
        foreach(Vector3 vertice0 in vertices){
            GameObject lineObject = new GameObject();
            lineObject.transform.parent = this.transform;
            MeshRenderer meshRenderer = lineObject.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = lineObject.AddComponent<MeshFilter>();
            LineRenderer line =  lineObject.AddComponent<LineRenderer>();

            line.positionCount = vertices.Count;
            for(int i = 0; i < line.positionCount; i++){
                line.SetPosition(i, transform.TransformPoint(vertices[i]));
            }
            
            line.material = material;
            //line.startColor = Color.red;
            //line.endColor = Color.red;
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.numCapVertices = 1;
            line.alignment = LineAlignment.TransformZ;

            Mesh mesh = new Mesh();
            meshFilter.mesh = mesh;
            meshRenderer.material = material;
            line.BakeMesh(mesh, false);

            line.enabled = false;
        }
    }
}
