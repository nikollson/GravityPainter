using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class Gpt_YukaManager : MonoBehaviour {


    public enum ExplosionMode { WAY4, WAY8, BOSS_GOLEM };

    [Button("MakeTile", "MakeTiles")]
    [SerializeField]
    private int _makeTileint = 0;

    public Transform tileParent;
    public Texture2D stageTile;
    public float tileSize = 3;
    public ExplosionMode explosionMode;

    public VisualSetting visualSetting;
    public ReadTileSetting readSetting;
    public TileSetting tileSetting;
    public GolemSetting golemSetting;

    private Gpt_YukaParts[] yukaParts;
    private Gpt_YukaBox[,] tiles;

    

    void Start()
    {
        yukaParts = tileParent.GetComponentsInChildren<Gpt_YukaParts>();

        List<Gpt_YukaBox> boxes = new List<Gpt_YukaBox>();
        foreach (var a in yukaParts)
        {
            Gpt_YukaBox[] tmp = a.GetYukaBoxes();
            for (int i = 0; i < tmp.Length; i++) tmp[i].Setting(tileSetting, a, a.GetFirstColor());
            boxes.AddRange(tmp);
        }

        int w = 0;
        int h = 0;
        foreach (var a in boxes)
        {
            Q coordinate = GetCoordinate(a.transform.localPosition);
            w = Mathf.Max(w, Mathf.Abs(coordinate.x));
            h = Mathf.Max(h, Mathf.Abs(coordinate.y));
        }


        if (explosionMode != ExplosionMode.BOSS_GOLEM)
        {
            tiles = new Gpt_YukaBox[h + 1, w + 1];

            foreach (var a in boxes)
            {
                Q coordinate = GetCoordinate(a.transform.localPosition);
                tiles[Mathf.Abs(coordinate.y), Mathf.Abs(coordinate.x)] = a;
            }
        }

        if(explosionMode == ExplosionMode.BOSS_GOLEM)
        {
            tiles = new Gpt_YukaBox[1, boxes.Count + 1];
            for(int i = 0; i < boxes.Count; i++)
            {
                tiles[0, i] = boxes[i];
            }
        }

        /*
        string s = "";
        for(int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                s += tiles[i, j] == null ? "   " : "" + tiles[i, j].Color;
                s += " ";
            }
            s += "\n";
        }
        Debug.Log(s);
        */
    }
    
    public void DoExplode(int color, Vector3 point, float radius)
    {
        Gpt_InkColor inkColor = Gpt_InkColor.NONE;
        if (color == 1) inkColor = Gpt_InkColor.RED;
        if (color == 2) inkColor = Gpt_InkColor.BLUE;
        if (color == 3) inkColor = Gpt_InkColor.YELLOW;

        DoExplode(inkColor, point, radius);
    }


    class Q
    {
        public int x, y;
        public int count;
        public Q(int x, int y)
        {
            this.x = x;
            this.y = y;
            count = 0;
        }
        public Q(int x,int y,int count)
        {
            this.x = x;
            this.y = y;
            this.count = count;
        }
    }

    public void DoExplode(Gpt_InkColor color, Vector3 point, float radius)
    {
        if (explosionMode != ExplosionMode.BOSS_GOLEM) DoExplode_Way(color, point, radius);
        if (explosionMode == ExplosionMode.BOSS_GOLEM) DoExplode_Golem(color, point, radius);
    }

    void DoExplode_Golem(Gpt_InkColor color, Vector3 point, float radius)
    {
        var list = new List<Gpt_YukaBox>();
        int h = tiles.GetLength(0);
        int w = tiles.GetLength(1);

        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                if (tiles[i, j] == null) continue;
                if (tiles[i, j].Color != color) continue;
                if (!tiles[i, j].CanSetExplode()) continue;
                list.Add(tiles[i, j]);
            }
        }
        float angle = Mathf.Atan2(point.z, point.x) / Mathf.PI * 180;


        foreach(var a in list)
        {
            float tangle = Mathf.Atan2(a.transform.parent.position.z, a.transform.parent.position.x) / Mathf.PI * 180;
            float angleDist = AngleDistDigree(tangle, angle);

            if (a.Color == color && angleDist  < golemSetting.angleDist)
            {
                int timing = 10;
                SetExplode(a, timing);
            }
        }
    }

    float AngleDistDigree(float a, float b)
    {
        return Mathf.Min(Mathf.Abs(a - b), Mathf.Abs(a + 360 - b), Mathf.Abs(b + 360 - a));
    }

    void DoExplode_Way(Gpt_InkColor color, Vector3 point, float radius)
    {
        var list = new List<Gpt_YukaBox>();
        Queue<Q> que = new Queue<Q>();

        int h = tiles.GetLength(0);
        int w = tiles.GetLength(1);

        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                if (tiles[i, j] == null) continue;
                if (tiles[i, j].Color != color) continue;
                if (!tiles[i, j].CanSetExplode()) continue;
                Vector3 dist = (tiles[i, j].transform.position - point);
                if ((dist - new Vector3(0, dist.y, 0)).magnitude < radius) que.Enqueue(new Q(j, i, 0));
            }
        }


        int[,] visit = new int[h, w];
        for (int i = 0; i < h; i++) for (int j = 0; j < w; j++) visit[i, j] = 0;


        for (int i = 0; i < 10000; i++)
        {
            if (que.Count == 0) break;

            Q u = que.Peek();
            que.Dequeue();

            if (visit[u.y, u.x] == 1) continue;
            visit[u.y, u.x] = 1;
            SetExplode(tiles[u.y, u.x], u.count);

            int[] dx = { 1, 0, -1, 0 };
            int[] dy = { 0, 1, 0, -1 };

            if (explosionMode == ExplosionMode.WAY8)
            {
                dx = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
                dy = new int[] { -1, 0, 1, 1, -1, 1, 0, -1 };
            }

            for (int p = 0; p < dx.Length; p++)
            {
                int nx = u.x + dx[p];
                int ny = u.y + dy[p];

                if (nx < 0 || ny < 0 || nx >= w || ny >= h) continue;
                if (visit[ny, nx] == 1) continue;
                if (tiles[ny, nx] == null) continue;
                if (tiles[u.y, u.x].Color != tiles[ny, nx].Color) continue;

                que.Enqueue(new Q(nx, ny, u.count + 1));
            }

        }
    }
    

    void SetExplode(Gpt_YukaBox yukaBox, int timing)
    {

        float flushTiming = timing * visualSetting.flushRunScale;
        float explodeTiming = visualSetting.explodeWait + timing * visualSetting.explodeRunScale;
        float reverseTiming = visualSetting.reverseTime + timing * visualSetting.reverseRunScale;
        float reverseEndTiming = visualSetting.reverseEndTime + timing * visualSetting.reverseRunScale;
        yukaBox.SetExplode(flushTiming, explodeTiming, reverseTiming, reverseEndTiming);
    }
    
    Q GetCoordinate(Vector3 localPosition)
    {
        return new Q((int)(localPosition.z / tileSize + 0.4999), (int)(-localPosition.x / tileSize + 0.4999));
    }

    public Vector2 GetTileCordinate(Vector3 position)
    {
        if (tiles == null) return Vector2.zero;
        int h = tiles.GetLength(0);
        int w = tiles.GetLength(1);

        int neari = 0;
        int nearj = 0;
        float mini = 1000000000;


        for(int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                if (tiles[i, j] == null) continue;
                float dist = (tiles[i, j].transform.position - position).magnitude;
                if (mini > dist)
                {
                    mini = dist;
                    neari = i;
                    nearj = j;
                }
            }
        }
        return new Vector2(nearj, neari);
    }

    public bool HasTile(Vector3 position)
    {
        Vector2 cd = GetTileCordinate(position);
        Gpt_YukaBox tile = tiles[(int)cd.y, (int)cd.x];
        if (tile == null) return false;
        if (!tile.CanSetExplode()) return false;

        Vector3 dist = tile.transform.position - position;
        dist.y = 0;

        return dist.magnitude < tileSize * 1.1;
    }
    

#if UNITY_EDITOR
    void MakeTile()
    {
        Debug.Log("Make Tile Start..");
        MakeTile_ClearChilds();
        MakeTile_SetTiles();
        Debug.Log("Make Tile End!!");
    }


    int nameCount = 0;
    void MakeTile_ClearChilds()
    {
        nameCount = 0;
        Transform parent = tileParent.transform;

        for (int i = 0; i < 10000; i++)
        {
            if (parent.childCount == 0) break;
            Editor.DestroyImmediate(parent.GetChild(0).gameObject);
        }
    }

    void MakeTile_SetTiles()
    {
        Transform parent = tileParent;
        ReadTileSetting.Mode[,] colors = GetTilesMode();

        for (int i = 0; i < colors.GetLength(0); i++)
        {
            for (int j = 0; j < colors.GetLength(1); j++)
            {
                if (colors[i, j] != ReadTileSetting.Mode.Hole)
                {
                    ReadTileSetting.Mode startMode = colors[i, j];

                    GameObject yukaParts = (GameObject)Instantiate(readSetting.partsPrefab, parent);

                    Queue<Q> que = new Queue<Q>();
                    que.Enqueue(new Q(j, i));

                    for (int k = 0; k < 10000; k++)
                    {
                        if (que.Count == 0) break;
                        Q u = que.Peek();
                        que.Dequeue();

                        if (u.x < 0 || u.y < 0 || u.y >= colors.GetLength(0) || u.x >= colors.GetLength(1)) continue;
                        if (colors[u.y, u.x] != startMode) continue;
                        colors[u.y, u.x] = ReadTileSetting.Mode.Hole;

                        var obj = (GameObject)PrefabUtility.InstantiatePrefab(readSetting.basePrefab);
                        obj.transform.parent = yukaParts.transform;
                        obj.transform.localPosition = new Vector3(-u.y * tileSize, 0, u.x * tileSize);
                        Vector3 moto = obj.transform.localScale;
                        obj.transform.localScale = new Vector3(moto.x * tileSize, moto.y, moto.z * tileSize);

                        int[] dx = { 1, 0, -1, 0 };
                        int[] dy = { 0, 1, 0, -1 };

                        for (int p = 0; p < 4; p++)
                        {
                            que.Enqueue(new Q(u.x + dx[p], u.y + dy[p]));
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
        yukaPartsObject.name = "" + nameCount + "_" + mode;
        nameCount++;

        var yukaParts = yukaPartsObject.GetComponent<Gpt_YukaParts>();

        Gpt_YukaParts.YukaColor nextColor = Gpt_YukaParts.YukaColor.WHITE;
        if (mode == ReadTileSetting.Mode.Red || mode == ReadTileSetting.Mode.RedPattern) nextColor = Gpt_YukaParts.YukaColor.RED;
        if (mode == ReadTileSetting.Mode.Blue || mode == ReadTileSetting.Mode.BluePattern) nextColor = Gpt_YukaParts.YukaColor.BLUE;
        if (mode == ReadTileSetting.Mode.Yellow || mode == ReadTileSetting.Mode.YellowPattern) nextColor = Gpt_YukaParts.YukaColor.YELLOW;
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
                colors[i, j] = readSetting.GetMode(c);
            }
        }
        return colors;
    }
#endif

    [System.Serializable]
    public class VisualSetting
    {
        public float flushRunScale = 0.04f;
        public float explodeWait = 0.9f;
        public float explodeRunScale = 0.4f;
        public float reverseTime = 15;
        public float reverseEndTime = 18;
        public float reverseRunScale = 0.1f;
    }

    [System.Serializable]
    public class ReadTileSetting
    {
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

    [System.Serializable]
    public class TileSetting
    {
        public int StartHP = 2;
        public float fallDistance = 10;

        public Material white;
        public Material red;
        public Material redF;
        public Material redCrack;
        public Material redCrackF;
        public Material blue;
        public Material blueF;
        public Material blueCrack;
        public Material blueCrackF;
        public Material yellow;
        public Material yellowF;
        public Material yellowCrack;
        public Material yellowCrackF;

        public Material GetMaterial(Gpt_InkColor color, int HP, bool flushing)
        {
            Material[] materials = new Material[]
            {
               white,white,white,white,
                red,redF,redCrack,redCrackF,
                blue,blueF,blueCrack,blueCrackF,
                yellow,yellowF,yellowCrack,yellowCrackF,
            };

            int colorNum = 0;
            if (color == Gpt_InkColor.RED) colorNum = 4;
            if (color == Gpt_InkColor.BLUE) colorNum = 8;
            if (color == Gpt_InkColor.YELLOW) colorNum = 12;
            int crackedNum = HP <= 1 ? 2 : 0;
            int flushNum = flushing ? 1 : 0;

            int materialNum = colorNum + crackedNum + flushNum;

            return materials[materialNum];
        }
    }

    [System.Serializable]
    public class GolemSetting
    {
        public float angleDist = 40;
    }
}
