using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gpt_RestEnemyUI : MonoBehaviour
{
    public Text enemyNumText;

    public GameObject gravityObject;
    public GameObject phaseObject;
    private Gpt_EnemyGravityManeger gravityManager;
    private Gpt_EnemyPhaseControl phaseControl;

    private GameObject player;
    private Gpt_PlayerState playState;

    public Sprite[] Count;
    public GameObject GcountTen;
    public GameObject GcountOne;
    public GameObject GcountTenParent;
    public GameObject GcountOneParent;
    private Image countTen;
    private Image countOne;
    private Image countTenParent;
    private Image countOneParent;

    private int tenPoint;
    private int onePoint;
    private int tenPointParent;
    private int onePointParent;

    private bool count;
    private int Maxcount;

    private Gpt_InkColor playColor;

    void Start()
    {
        gravityManager = gravityObject.GetComponent<Gpt_EnemyGravityManeger>();
        phaseControl = phaseObject.GetComponent<Gpt_EnemyPhaseControl>();
        countTen = GcountTen.GetComponent<Image>();
        countOne = GcountOne.GetComponent<Image>();
        countTenParent = GcountTenParent.GetComponent<Image>();
        countOneParent = GcountOneParent.GetComponent<Image>();
        //gravityManager = FindObjectOfType<Gpt_EnemyGravityManeger>();
        //phaseControl = FindObjectOfType<Gpt_EnemyPhaseControl>();

        player = GameObject.Find("Player");
        playState = player.GetComponent<Gpt_PlayerState>();

        playColor = playState.PlayerColor;

        

        if (phaseControl == null)
        {
            Debug.Log("faffafasasfasfafa");
            //this.gameObject.SetActive(false);
        }
        
    }

    void Update()
    {
        if (phaseControl != null)
        {
            
            //enemyNumText.text = "" + Mathf.Max(0, phaseControl.GetAllClearEnemyNum() - gravityManager.GetEnemyNumCount());
            
        }
        Debug.Log(playColor);
        if (!count)
        {
            Maxcount = Mathf.Max(0, phaseControl.GetAllClearEnemyNum() - gravityManager.GetEnemyNumCount());
            count = true;
            tenPointParent = Mathf.Max(0, phaseControl.GetAllClearEnemyNum() - gravityManager.GetEnemyNumCount()) / 10;
            onePointParent = Mathf.Max(0, phaseControl.GetAllClearEnemyNum() - gravityManager.GetEnemyNumCount()) % 10;

            Debug.Log("tenP:" + tenPointParent);
            Debug.Log("oneP:" + onePointParent);
            switch (tenPointParent)
            {
                case 0:
                    countTenParent.sprite = Count[0];
                    break;
                case 1:
                    countTenParent.sprite = Count[1];
                    break;
                case 2:
                    countTenParent.sprite = Count[2];
                    break;
                case 3:
                    countTenParent.sprite = Count[3];
                    break;
                case 4:
                    countTenParent.sprite = Count[4];
                    break;
                case 5:
                    countTenParent.sprite = Count[5];
                    break;
                case 6:
                    countTenParent.sprite = Count[6];
                    break;
                case 7:
                    countTenParent.sprite = Count[7];
                    break;
                case 8:
                    countTenParent.sprite = Count[8];
                    break;
                case 9:
                    countTenParent.sprite = Count[9];
                    break;
            }

            switch (onePointParent)
            {
                case 0:
                    countOneParent.sprite = Count[0];
                    break;
                case 1:
                    countOneParent.sprite = Count[1];
                    break;
                case 2:
                    countOneParent.sprite = Count[2];
                    break;
                case 3:
                    countOneParent.sprite = Count[3];
                    break;
                case 4:
                    countOneParent.sprite = Count[4];
                    break;
                case 5:
                    countOneParent.sprite = Count[5];
                    break;
                case 6:
                    countOneParent.sprite = Count[6];
                    break;
                case 7:
                    countOneParent.sprite = Count[7];
                    break;
                case 8:
                    countOneParent.sprite = Count[8];
                    break;
                case 9:
                    countOneParent.sprite = Count[9];
                    break;
            }
        }
        
        tenPoint = Mathf.Max(0, phaseControl.GetAllClearEnemyNum() - gravityManager.GetEnemyNumCount()) / 10;
        onePoint = Mathf.Max(0, phaseControl.GetAllClearEnemyNum() - gravityManager.GetEnemyNumCount()) % 10;
        Debug.Log("fff]"+tenPoint);
        Debug.Log("ggg]" + onePoint);

        switch (tenPoint)
        {
            case 0:
                countTen.sprite = Count[0];
                break;
            case 1:
                countTen.sprite = Count[1];
                break;
            case 2:
                countTen.sprite = Count[2];
                break;
            case 3:
                countTen.sprite = Count[3];
                break;
            case 4:
                countTen.sprite = Count[4];
                break;
            case 5:
                countTen.sprite = Count[5];
                break;
            case 6:
                countTen.sprite = Count[6];
                break;
            case 7:
                countTen.sprite = Count[7];
                break;
            case 8:
                countTen.sprite = Count[8];
                break;
            case 9:
                countTen.sprite = Count[9];
                break;
        }

        switch (onePoint)
        {
            case 0:
                countOne.sprite = Count[0];
                break;
            case 1:
                countOne.sprite = Count[1];
                break;
            case 2:
                countOne.sprite = Count[2];
                break;
            case 3:
                countOne.sprite = Count[3];
                break;
            case 4:
                countOne.sprite = Count[4];
                break;
            case 5:
                countOne.sprite = Count[5];
                break;
            case 6:
                countOne.sprite = Count[6];
                break;
            case 7:
                countOne.sprite = Count[7];
                break;
            case 8:
                countOne.sprite = Count[8];
                break;
            case 9:
                countOne.sprite = Count[9];
                break;
        }



        //countOne.sprite = Count[1];
    }

}
