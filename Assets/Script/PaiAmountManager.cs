using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牌の量操作
public class PaiAmountManager : MonoBehaviour
{
    // 牌の量入力UI
    [SerializeField]
    private GameObject paiAmountUI = null;
    // 山ゲームオブジェクト
    [SerializeField]
    private GameObject yamaGameObject = null;
    // 牌ゲームオブジェクトリスト
    [SerializeField]
    private List<GameObject> paiGameObjectList;

    // 牌の量オブジェクト
    private PaiAmountStatus paiAmountStatus;

    // 牌の数を初期化して取得する
    public void getPaiAmount()
    {
        // 牌の量入力UIの牌の量スクリプトを初期値にする
        //this.paiAmountUI.GetComponent<PaiAmountStatus>() = new PaiAmountStatus();
        this.paiAmountStatus = new PaiAmountStatus();
    }

    // 牌の数をロードして取得する
    public void loadPaiAmount() { }

    // 初期化した牌の量オブジェクトを取得
    public PaiAmountStatus getInitPaiAmountStatus()
    {
        return new PaiAmountStatus();
    }

    // 牌の数の入力を取得して山ゲームオブジェクトに牌ゲームオブジェクトをその個数入れる
    public void setPaiAmount()
    {
        // 山ゲームオブジェクトの子オブジェクトを削除
        foreach (Transform t in this.yamaGameObject.transform)
        {
            GameObject.Destroy(t.gameObject);
        }

        // 牌の量オブジェクトがnullだったら新規生成
        if (this.paiAmountStatus == null)
        {
            this.paiAmountStatus = new PaiAmountStatus();
        }

        /* 入力された分だけ牌を山ゲームオブジェクトの子にセット */
        // 牌を特定
        foreach (GameObject paiGameObject in this.paiGameObjectList)
        {
            // 牌スクリプト取得
            Pai paiScript = paiGameObject.GetComponent<Pai>();

            // 牌の種類を判別
            if (paiScript.PaiKind == PaiStatus.PAIKIND.字牌)
            {
                switch (paiScript.PaiCharacters)
                {
                    case PaiStatus.PAICHARACTERS.東:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.東);
                        break;
                    case PaiStatus.PAICHARACTERS.南:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.南);
                        break;
                    case PaiStatus.PAICHARACTERS.西:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.西);
                        break;
                    case PaiStatus.PAICHARACTERS.北:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.北);
                        break;
                    case PaiStatus.PAICHARACTERS.白:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.白);
                        break;
                    case PaiStatus.PAICHARACTERS.發:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.發);
                        break;
                    case PaiStatus.PAICHARACTERS.中:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.中);
                        break;
                    default:
                        break;
                }
            }
            else if (paiScript.PaiKind == PaiStatus.PAIKIND.萬子)
            {
                switch (paiScript.PaiNumber)
                {
                    case 1:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.一萬);
                        break;
                    case 2:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.二萬);
                        break;
                    case 3:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.三萬);
                        break;
                    case 4:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.四萬);
                        break;
                    case 5:
                        // 赤チェック
                        if (paiScript.IsRed)
                        {
                            setPaiOnYama(paiGameObject, this.paiAmountStatus.赤五萬);
                        }
                        else
                        {
                            setPaiOnYama(paiGameObject, this.paiAmountStatus.五萬);
                        }
                        break;
                    case 6:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.六萬);
                        break;
                    case 7:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.七萬);
                        break;
                    case 8:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.八萬);
                        break;
                    case 9:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.九萬);
                        break;
                    default:
                        break;
                }
            }
            else if (paiScript.PaiKind == PaiStatus.PAIKIND.索子)
            {
                switch (paiScript.PaiNumber)
                {
                    case 1:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.一索);
                        break;
                    case 2:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.二索);
                        break;
                    case 3:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.三索);
                        break;
                    case 4:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.四索);
                        break;
                    case 5:
                        // 赤チェック
                        if (paiScript.IsRed)
                        {
                            setPaiOnYama(paiGameObject, this.paiAmountStatus.赤五索);
                        }
                        else
                        {
                            setPaiOnYama(paiGameObject, this.paiAmountStatus.五索);
                        }
                        break;
                    case 6:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.六索);
                        break;
                    case 7:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.七索);
                        break;
                    case 8:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.八索);
                        break;
                    case 9:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.九索);
                        break;
                    default:
                        break;
                }
            }
            else if (paiScript.PaiKind == PaiStatus.PAIKIND.筒子)
            {
                switch (paiScript.PaiNumber)
                {
                    case 1:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.一筒);
                        break;
                    case 2:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.二筒);
                        break;
                    case 3:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.三筒);
                        break;
                    case 4:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.四筒);
                        break;
                    case 5:
                        // 赤チェック
                        if (paiScript.IsRed)
                        {
                            setPaiOnYama(paiGameObject, this.paiAmountStatus.赤五筒);
                        }
                        else
                        {
                            setPaiOnYama(paiGameObject, this.paiAmountStatus.五筒);
                        }
                        break;
                    case 6:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.六筒);
                        break;
                    case 7:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.七筒);
                        break;
                    case 8:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.八筒);
                        break;
                    case 9:
                        setPaiOnYama(paiGameObject, this.paiAmountStatus.九筒);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    // 山の子に引数の牌ゲームオブジェクトを引数の個数生成
    public void setPaiOnYama(GameObject paiGameObject, int amount)
    {
        // 牌ゲームオブジェクトを複製
        GameObject obj = Instantiate(paiGameObject, Vector3.zero, Quaternion.identity);
        // 複製した牌ゲームオブジェクトを山の子にセット
        obj.transform.SetParent(this.yamaGameObject.transform);
    }
}
