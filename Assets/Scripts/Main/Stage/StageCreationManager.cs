using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

/// <summary>
/// ステージ生成管理のプロジェクト
/// </summary>
public class StageCreationManager : MonoBehaviour
{
    [SerializeField, Header("プレイヤーのオブジェクト参照")]
    private Transform player;

    [SerializeField, Header("道の管理オブジェクト")]
    private Transform roads = null;

    [SerializeField, Header("無効領域の道(エリア)")]
    private GameObject[] invalidArea = new GameObject[4];

    [SerializeField, Header("普通の道")]
    private GameObject[] normalRoad = new GameObject[31];

    [SerializeField, Header("敵の道")]
    private GameObject[] enemyRoad = new GameObject[12];

    [SerializeField, Header("ビームの道")]
    private GameObject[] beamRoad = new GameObject[4]; 

    [SerializeField, Header("サイバー攻撃の道")]
    private GameObject cyberAttackRoad = null;

    [Header("生成・削除する関係の値")]
    [SerializeField, Tooltip("初めに生成する道の値")]
    private int firstCreationRoadVal = 30;

    [SerializeField, Tooltip("普通の道区分の最大生成値")]
    private int maxNormalClassification = 2;

    [SerializeField, Tooltip("妨げるの道区分の最大生成値")]
    private int maxHinderClassification = 5;

    [SerializeField, Tooltip("無効領域の道(エリア)生成座標")]
    private Vector3 invalidAreaPos = new Vector3(0f, 0f, -3f);

    private int invalidAreaCreation = 0;        // 生成する無効領域の道(エリア)

    private int normalRoadCreation = 0;         // 生成する普通の道

    private int firstNormalRoadCreation = 0;    // 初めの生成する普通の道

    private int enemyRoadCreation = 0;          // 生成する敵の道

    private int beamRoadCreation = 0;           // 生成するビームの道

    private int roadCreationVal = 2;            // 道の生成の値

    private int hinderRoadTypesNum = 3;         // 妨げる道の種類の数

    private bool normalRoadCreationFlg = false; // 通常の道を作成するか 

    [SerializeField, Tooltip("生成・削除する所の値")]
    private int creatAndDeletPointVal = 15;

    [SerializeField, Tooltip("生成・削除する所の増分値")]
    private int creatAndDeletIncrementalVal = 15;

    /// <summary>
    /// 妨げる道の種類(数を増やして確率操作)
    /// </summary>
    public  enum HinderRoadTypes
    {
        enemy_1 = 0,        // 敵
        enemy_2 = 1,        // 敵
        beam = 2,           // ビーム
        cyberAttack = 3,    // サイバー攻撃
    }

    void Awake()
    {
        // 妨げる道の種類の数を入れる
        hinderRoadTypesNum = System.Enum.GetNames(typeof(HinderRoadTypes)).Length;

        // 初めのステージを生成
        StartStageCreation();
    }

    // Update is called once per frame
    void Update()
    {
        // 道の生成・削除する
        if (creatAndDeletPointVal == player.localPosition.z)
        {
            // 道の生成
            StartRoadCreation(creatAndDeletPointVal + firstCreationRoadVal);    // 生成・削除する所の値 + 初めに生成する道の値

            // 道の削除
            // 子オブジェクトを取得する
            var roadChildren = GetChildren(roads);
            // 取得した子オブジェクト名をログ出力
            for (var i = 0; i < roadChildren.Length; i++)
            {
                // 値を比べて小さかったら削除する
                if (roadChildren[i].position.z < (creatAndDeletPointVal - creatAndDeletIncrementalVal))
                {
                    Destroy(roadChildren[i].gameObject); // 道のオブジェクトを削除
                }
            }

            // 生成・削除する所の値を増やす
            creatAndDeletPointVal += creatAndDeletIncrementalVal;
        }
    }

    /// <summary>
    /// 初めのステージを生成
    /// </summary>
    private void StartStageCreation()
    {
        FirstInvalidAreaCreation();
        FirstNormalCreation();
        StartRoadCreation(firstCreationRoadVal);
    }


    /// <summary>
    /// 初めの通常の道の生成
    /// </summary>
    private void FirstNormalCreation()
    {
        for (int i = 1; i >= -2; i--)
        {
            firstNormalRoadCreation = Random.Range(0, normalRoad.Length);
            Instantiate(normalRoad[firstNormalRoadCreation], new Vector3(0f, 0.5f, i), Quaternion.identity, roads);
        }
    }

    /// <summary>
    /// 初めの無効領域の生成
    /// </summary>
    private void FirstInvalidAreaCreation()
    {
        invalidAreaCreation = Random.Range(0, invalidArea.Length);  
        Instantiate(invalidArea[invalidAreaCreation], new Vector3(0f, 0.5f, -3f), Quaternion.identity, roads);
    }

    /// <summary>
    ///　道生成を始める
    /// </summary>
    private void StartRoadCreation(int creationVal)
    {
        // 道生成
        while (roadCreationVal < creationVal)
        {
            // 道生成
            RoadCreation();
        }
    }

    /// <summary>
    /// 道生成
    /// </summary>
    private void RoadCreation()
    {
        // 通常の道を生成する
        if (normalRoadCreationFlg)
        {
            // 道の生成する量
            int creationVal = Random.Range(1, maxNormalClassification + 1); // 1 ～ maxNormalClassification

            for (int i = 0; i < creationVal; i++)
            {
                NormalRoadCreation(roadCreationVal);
                roadCreationVal++;  // 道の生成の値を増やす
            }
        }
        // 妨げる道を生成する 
        else
        {
            HinderRoadCreation();
        }

        // 通常の道を作成するか
        normalRoadCreationFlg = !normalRoadCreationFlg;
    }

    /// <summary>
    /// 妨げる道を生成する
    /// </summary>
    private void HinderRoadCreation()
    {
        // 道の生成する量
        int creationVal = Random.Range(1, maxHinderClassification + 1); // 1 ～ maxHinderClassification

        for (int i = 0; i < creationVal; i++)
        {
            // 妨げる道の生成
            int roadType = Random.Range(0, hinderRoadTypesNum);

            // 妨げる道の種類
            switch (roadType)
            {
                // 敵
                case (int)HinderRoadTypes.enemy_1:
                    EnemyRoadCreation(roadCreationVal);
                    break;

                // 敵
                case (int)HinderRoadTypes.enemy_2:
                    EnemyRoadCreation(roadCreationVal);
                    break;

                // ビーム
                case (int)HinderRoadTypes.beam:
                    BeamRoadCreation(roadCreationVal);
                    break;

                // サイバー攻撃
                case (int)HinderRoadTypes.cyberAttack:
                    CyberAttackRoadCreation(roadCreationVal);
                    break;

                default:
                    Debug.LogError("妨げる道の生成の値が間違っています。" + roadType);
                    break;
            }

            roadCreationVal++;  // 道の生成の値を増やす
        }
    }

    /// <summary>
    /// 通常の道生成
    /// </summary>
    private void NormalRoadCreation(int posZ)
    {
        normalRoadCreation = Random.Range(0, normalRoad.Length);
        Instantiate(normalRoad[normalRoadCreation], new Vector3(0f, 0.5f, posZ), Quaternion.identity, roads);
    }

    /// <summary>
    /// 敵の道生成
    /// </summary>
    private void EnemyRoadCreation(int posZ)
    {
        enemyRoadCreation = Random.Range(0, enemyRoad.Length);
        Instantiate(enemyRoad[enemyRoadCreation], new Vector3(0f, 0.5f, posZ), Quaternion.identity, roads);
    }

    /// <summary>
    /// ビームの道生成
    /// </summary>
    private void BeamRoadCreation(int posZ)
    {
        beamRoadCreation = Random.Range(0, beamRoad.Length);
        Instantiate(beamRoad[beamRoadCreation], new Vector3(0f, 0.5f, posZ), Quaternion.identity, roads);
    }

    /// <summary>
    /// サイバー攻撃の道生成
    /// </summary>
    private void CyberAttackRoadCreation(int posZ)
    {
        Instantiate(cyberAttackRoad, new Vector3(0f, 0.5f, posZ), Quaternion.identity, roads);
    }

    // parent直下の子オブジェクトをforループで取得する
    private static Transform[] GetChildren(Transform parent)
    {
        // 子オブジェクトを格納する配列作成
        var children = new Transform[parent.childCount];

        // 0～個数-1までの子を順番に配列に格納
        for (var i = 0; i < children.Length; ++i)
        {
            children[i] = parent.GetChild(i);
        }

        // 子オブジェクトが格納された配列
        return children;
    }

}
