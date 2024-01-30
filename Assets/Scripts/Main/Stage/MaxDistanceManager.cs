using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static MainManager;

/// <summary>
/// 最大の進行距離のプロジェクト
/// </summary>
public class MaxDistanceManager : MonoBehaviour
{

    [SerializeField, Header("プレイヤーのオブジェクト参照")]
    private Transform player;

    [SerializeField, Header("最大の進んだ距離参照")]
    private Transform maxDistanceCanvas;

    [SerializeField, Header("最大の進んだ距離テキスト参照")]
    private TextMeshProUGUI maxDistanceText;

    private bool distanceUpdateFlg = false;     // 距離の更新

    // SEを参照するやつ
    private SESound SESound;

    // Start is called before the first frame update
    void Start()
    {
        // 座標を更新
        maxDistanceCanvas.localPosition = new Vector3(0f, maxDistanceCanvas.localPosition.y, TitleManager.gameMaxAdvanced);

        maxDistanceText.text = "最高ライン：" + TitleManager.gameMaxAdvanced.ToString();  // 最大の進んだ距離テキスト表示

        // SEのスクリプト参照
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();
    }

    // Update is called once per frame
    void Update()
    {
        if (distanceUpdateFlg) return;

        if(maxDistanceCanvas.localPosition.z < player.localPosition.z)
        {
            // 成功音のSE
            SESound.SuccessSoundSE();

            distanceUpdateFlg = true;
        }

    }
}
