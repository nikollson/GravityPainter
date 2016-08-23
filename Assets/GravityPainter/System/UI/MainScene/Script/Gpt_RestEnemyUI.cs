using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gpt_RestEnemyUI : MonoBehaviour
{
    public Text enemyNumText;

    private Gpt_EnemyGravityManeger gravityManager;
    private Gpt_EnemyPhaseControl phaseControl;

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

    void Start()
    {
        countTen = GcountTen.GetComponent<Image>();
        countOne = GcountOne.GetComponent<Image>();
        countTenParent = GcountTenParent.GetComponent<Image>();
        countOneParent = GcountOneParent.GetComponent<Image>();
        gravityManager = FindObjectOfType<Gpt_EnemyGravityManeger>();
        phaseControl = FindObjectOfType<Gpt_EnemyPhaseControl>();

        if(phaseControl == null)
        {
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (phaseControl != null)
        {
            Debug.Log("fff");
            enemyNumText.text = "" + Mathf.Max(0, phaseControl.GetAllClearEnemyNum() - gravityManager.GetEnemyNumCount());
            
        }
        tenPoint = Mathf.Max(0, phaseControl.GetAllClearEnemyNum() - gravityManager.GetEnemyNumCount()) / 10;
        onePoint = Mathf.Max(0, phaseControl.GetAllClearEnemyNum() - gravityManager.GetEnemyNumCount()) % 10;

        
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

        //switch (tenPointParent)
        //{
        //    case 0:
        //        countTen.sprite = Count[0];
        //        break;
        //    case 1:
        //        break;
        //    case 2:
        //        break;
        //    case 3:
        //        break;
        //    case 4:
        //        break;
        //    case 5:
        //        break;
        //    case 6:
        //        break;
        //    case 7:
        //        break;
        //    case 8:
        //        break;
        //    case 9:
        //        break;
        //}

        //switch (tenPointParent)
        //{
        //    case 0:
        //        countTen.sprite = Count[0];
        //        break;
        //    case 1:
        //        break;
        //    case 2:
        //        break;
        //    case 3:
        //        break;
        //    case 4:
        //        break;
        //    case 5:
        //        break;
        //    case 6:
        //        break;
        //    case 7:
        //        break;
        //    case 8:
        //        break;
        //    case 9:
        //        break;
        //}

        countOne.sprite = Count[1];
    }

}
