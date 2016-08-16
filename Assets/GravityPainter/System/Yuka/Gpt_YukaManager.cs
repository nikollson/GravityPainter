using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CanEditMultipleObjects]
//[CustomEditor(typeof(CustomSprite))]
public class Gpt_YukaManager : MonoBehaviour {

    public Transform allTileParent;

    public Sprite stageTile;
    public float tileSize = 3;

    public ReadTileSetting readTileSetting;

    void Start() { if (Application.isPlaying) Start_Normal(); }
    void Update()
    {
        if (!Application.isPlaying) Update_Editor();
        if (Application.isPlaying) Update_Normal();
    }

    void Start_Normal()
    {

    }



    void Update_Normal()
    {

    }





    void Update_Editor()
    {
        if (allTileParent.localScale.x != tileSize) allTileParent.localScale = new Vector3(tileSize, tileSize, tileSize);
    }

    [System.Serializable]
    public class ReadTileSetting
    {
        public Color white;
        public Color hole;
        public Color red;
        public Color blue;
        public Color yellow;
        public Color redRandom;
        public Color blueRandom;
        public Color yellowRandom;
    }
}
