using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーが出てくる道の管理プロジェクト
/// </summary>
public class EnemyRoadManager : MonoBehaviour
{
    [SerializeField, Header("生成するエネミーオブジェクト")]
    private GameObject enemyObject;

    private Transform enemyListObj; // エネミーのリストオブジェクト

    [Header("左右の目標のブロック座標")]
    [SerializeField, Tooltip("一番右のブロック座標")]
    private Transform rightMostBlockPos;

    [SerializeField, Tooltip("一番左のブロック座標")]
    private Transform leftMostBlockPos;

    [Header("敵の生成情報")]
    [SerializeField, Tooltip("敵のスタートする側面方向")]
    private StartGenerationSide startSide = StartGenerationSide.Right;

    [SerializeField, Tooltip("敵の最小生成時間の間隔")]
    private float minGenerationTime = 1.0f;

    [SerializeField, Tooltip("敵の最大生成時間の間隔")]
    private float maxGenerationTime = 3.0f;

    private float generationTime = 0.0f;    // 生成時間

    private Vector3 rightQuaternion = new Vector3(0f, -90f, 0f);

    private Vector3 leftQuaternion = new Vector3(0f, 90f, 0f);

    /// <summary>
    /// 生成をスタートする側面
    /// </summary>
    private enum StartGenerationSide
    { 
        None,
        Right,
        Left
    }

    private void Awake()
    {
        // 生成時間を入れる
        generationTime = 0f;

        // ゲームオブジェクト参照
        enemyListObj = GameObject.FindWithTag("EnemyList").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(generationTime < 0.0f)
        {
            // 生成時間を入れる
            generationTime = Random.Range(minGenerationTime, maxGenerationTime + 1.0f);

            // 敵の生成
            if (startSide == StartGenerationSide.Right)
            {
                GameObject gameObject =  Instantiate(enemyObject, new Vector3(rightMostBlockPos.position.x, 1f, rightMostBlockPos.position.z), Quaternion.Euler(rightQuaternion), enemyListObj);
                ViralEnemyManager enemy = gameObject.GetComponent<ViralEnemyManager>();
                enemy.HorizontalAxisMove(leftMostBlockPos.position.x - 1.0f);
            }
            else if(startSide == StartGenerationSide.Left)
            {
                GameObject gameObject = Instantiate(enemyObject, new Vector3(leftMostBlockPos.position.x, 1f, leftMostBlockPos.position.z), Quaternion.Euler(leftQuaternion), enemyListObj);
                ViralEnemyManager enemy = gameObject.GetComponent<ViralEnemyManager>();
                enemy.HorizontalAxisMove(rightMostBlockPos.position.x + 1.0f);
            }
        }
        else
        {
            generationTime -= Time.deltaTime;   
        }

    }
}
