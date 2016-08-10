using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gpt_FukaTestMesh : MonoBehaviour {

    public MeshFilter meshFilter;

    public int W = 10;
    public int H = 10;


    int meshCount = 0;

    void Start()
    {
        MakeMesh(W, H);
    }


    public void MakeMesh(int W,int H)
    {
        meshCount = W * H;
        Mesh nextMesh = new Mesh();

        float scaleX = 10.0f / W;
        float scaleY = 10.0f / H;

        Vector3[] vertices = new Vector3[W * H];
        for (int i = 0; i < W; i++)
        {
            for (int j = 0; j < H; j++)
            {
                vertices[i * H + j] = new Vector3(i * scaleX, j * scaleY, 0);
            }
        }
        int[] triAngles = new int[W * H * 3];
        for (int i = 0; i < W - 1; i++)
        {
            for (int j = 0; j < H - 1; j++)
            {
                int now = (i * H + j) * 3;
                int nowP = i * H + j;
                triAngles[now] = nowP;
                triAngles[now + 1] = nowP + 1;
                triAngles[now + 2] = nowP + H;
            }
        }

        nextMesh.vertices = vertices;
        nextMesh.triangles = triAngles;

        meshFilter.sharedMesh = nextMesh;
    }

    public int GetMeshCount()
    {
        return meshCount;
    }
}
