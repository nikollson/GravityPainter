using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class Gpt_YukaManager : MonoBehaviour {

    [Button("MakeTile", "MakeTiles")]
    [SerializeField]
    private int _makeTileint = 0;

    public Texture2D stageTile;
    public float tileSize = 3;

    public ReadTileSetting setting;

    public Gpt_YukaParts[] yukaParts;

    void Start()
    {
        yukaParts = this.transform.GetComponentsInChildren<Gpt_YukaParts>();
    }
    
    public void DoExplode(int color, Vector3 point, float radius)
    {
        Gpt_InkColor inkColor = Gpt_InkColor.NONE;
        if (color == 1) inkColor = Gpt_InkColor.RED;
        if (color == 2) inkColor = Gpt_InkColor.BLUE;
        if (color == 3) inkColor = Gpt_InkColor.YELLOW;

        DoExplode(inkColor, point, radius);
    }
    public void DoExplode(Gpt_InkColor color ,Vector3 point, float radius)
    {
        foreach(var a in yukaParts)
        {
            a.DoExplode(color, point, radius);
        }
    }



#if UNITY_EDITOR
    void MakeTile()
    {
        Debug.Log("Make Tile Start..");
        MakeTile_ClearChilds();
        MakeTile_SetTiles();
        Debug.Log("Make Tile End!!");
    }

    void MakeTile_ClearChilds()
    {
        Transform parent = setting.allTileParent.transform;

        for (int i = 0; i < 10000; i++)
        {
            if (parent.childCount == 0) break;
            Editor.DestroyImmediate(parent.GetChild(0).gameObject);
        }
    }

    class Q
    {
        public int i, j;
        public Q(int i,int j)
        {
            this.i = i;
            this.j = j;
        }
    }
    void MakeTile_SetTiles()
    {
        Transform parent = setting.allTileParent;
        ReadTileSetting.Mode[,] colors = GetTilesMode();

        for(int i = 0; i < colors.GetLength(0); i++)
        {
            for(int j = 0; j < colors.GetLength(1); j++)
            {
                if (colors[i, j] != ReadTileSetting.Mode.Hole)
                {
                    ReadTileSetting.Mode startMode = colors[i, j];
                    
                    GameObject yukaParts = (GameObject)Instantiate(setting.partsPrefab, parent);

                    Queue<Q> que = new Queue<Q>();
                    que.Enqueue(new Q(i, j));

                    for (int k = 0; k < 10000; k++)
                    {
                        if (que.Count == 0) break;
                        Q u = que.Peek();
                        que.Dequeue();

                        if (u.i < 0 || u.j < 0 || u.i >= colors.GetLength(0) || u.j >= colors.GetLength(1)) continue;
                        if (colors[u.i, u.j] != startMode) continue;
                        colors[u.i, u.j] = ReadTileSetting.Mode.Hole;

                        var obj = (GameObject)Instantiate(setting.basePrefab, yukaParts.transform);
                        obj.transform.localPosition = new Vector3(-u.i * tileSize, 0, u.j * tileSize);
                        obj.transform.localScale = new Vector3(tileSize, obj.transform.localScale.y, tileSize);

                        int[] dx = { 1, 0, -1, 0 };
                        int[] dy = { 0, 1, 0, -1 };

                        for(int p = 0; p < 4; p++)
                        {
                            que.Enqueue(new Q(u.i + dx[p], u.j + dy[p]));
                        }
                    }


                    SetYukaParts(yukaParts, startMode);
                }
            }
        }
    }

    void SetYukaParts(GameObject yukaPartsObject, ReadTileSetting.Mode mode)
    {
        yukaPartsObject.transform.localPosition = new Vector3(0, 0, 0);
        yukaPartsObject.name = setting.partsPrefab.name + "_" + mode;

        var yukaParts = yukaPartsObject.GetComponent<Gpt_YukaParts>();

        Gpt_YukaParts.YukaColor nextColor = Gpt_YukaParts.YukaColor.WHITE;
        if (mode == ReadTileSetting.Mode.Red || mode == ReadTileSetting.Mode.RedPattern) nextColor = Gpt_YukaParts.YukaColor.RED;
        if (mode == ReadTileSetting.Mode.Blue || mode == ReadTileSetting.Mode.BluePattern) nextColor = Gpt_YukaParts.YukaColor.BLUE;
        if (mode == ReadTileSetting.Mode.Yellow || mode == ReadTileSetting.Mode.YellowPattern) nextColor = Gpt_YukaParts.YukaColor.YELLOW;
        yukaParts.SetColor(nextColor);
        yukaParts.firstColor = nextColor;

        bool isPattern = false;
        isPattern |= mode == ReadTileSetting.Mode.RedPattern;
        isPattern |= mode == ReadTileSetting.Mode.BluePattern;
        isPattern |= mode == ReadTileSetting.Mode.YellowPattern;

        yukaParts.changeMode = isPattern ? Gpt_YukaParts.Mode.RANDOM : Gpt_YukaParts.Mode.CONST;
    }

    ReadTileSetting.Mode[,] GetTilesMode()
    {
        ReadTileSetting.Mode[,] colors = new ReadTileSetting.Mode[stageTile.height, stageTile.width];

        for (int i = 0; i < stageTile.height; i++)
        {
            for (int j = 0; j < stageTile.width; j++)
            {
                Color c = stageTile.GetPixel(j, i);
                colors[i, j] = setting.GetMode(c);
            }
        }
        return colors;
    }
#endif

    [System.Serializable]
    public class ReadTileSetting
    {
        public Transform allTileParent;
        public GameObject partsPrefab;
        public GameObject basePrefab;
        public Color white;
        public Color hole;
        public Color red;
        public Color blue;
        public Color yellow;
        public Color redPattern;
        public Color bluePattern;
        public Color yellowPattern;

        public enum Mode { White, Hole, Red, Blue, Yellow, RedPattern, BluePattern, YellowPattern };
        public Mode GetMode(Color color)
        {
            Mode ret = Mode.White;
            float mini = GetDist(color, white);

            for(int i = 0; i < 7; i++)
            {
                Mode next = Mode.White;
                Color opcol = white;

                if (i == 0) { next = Mode.Red; opcol = red; }
                if (i == 1) { next = Mode.Blue; opcol = blue; }
                if (i == 2) { next = Mode.Yellow; opcol = yellow; }
                if (i == 3) { next = Mode.RedPattern; opcol = redPattern; }
                if (i == 4) { next = Mode.BluePattern; opcol = bluePattern; }
                if (i == 5) { next = Mode.YellowPattern; opcol = yellowPattern; }
                if (i == 6) { next = Mode.Hole; opcol = hole; }

                float dist = GetDist(opcol, color);
                if(dist< mini)
                {
                    mini = dist;
                    ret = next;
                }
            }

            return ret;
        }
        private float GetDist(Color a, Color b)
        {
            return Mathf.Abs(a.r - b.r) + Mathf.Abs(a.g - b.g) + Mathf.Abs(a.b - b.b);
        }
    }
}
