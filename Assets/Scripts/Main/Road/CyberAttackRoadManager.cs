using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サイバー攻撃が出てくる道の管理プロジェクト
/// </summary>
public class CyberAttackRoadManager : MonoBehaviour
{
    [SerializeField, Header("サイバー攻撃エフェクト")]
    private GameObject[] cyberAttackEffect = new GameObject[4];

    [Header("サイバー攻撃のポイントエフェクト")]
    [SerializeField] private GameObject[] cyberAttackPointEffect_1 = new GameObject[4];
    [SerializeField] private GameObject[] cyberAttackPointEffect_2 = new GameObject[4];
    [SerializeField] private GameObject[] cyberAttackPointEffect_3 = new GameObject[4];

    private int minOccurrenceVal = -4;          // 最小発生座標の値
    private int maxOccurrenceVal = 4;           // 最大発生座標の値

    private int[] occurrenceVal = new int[4];   // 発生座標の値

    [Header("サイバー攻撃表示関係")]
    [SerializeField, Tooltip("サイバー攻撃の最小発生時間の間隔")]
    private float minCyberGenerationTime = 4.0f;

    [SerializeField, Tooltip("サイバー攻撃の最大発生時間の間隔")]
    private float maxCyberGenerationTime = 6.0f;

    private float[] cyberGenerationTime = new float[4];    // 発生時間

    [SerializeField, Tooltip("サイバー攻撃表示の時間")]
    private float cyberDisplayTime = 1.5f;

    private float[] cyberTime = new float[4];  // サイバー攻撃の表示時間

    // Start is called before the first frame update
    void Start()
    {        
        for (int i = 0; i < cyberAttackEffect.Length; i++)
        {
            // サイバー攻撃の発生座標の値
            occurrenceVal[i] = Random.Range(minOccurrenceVal, maxOccurrenceVal + 1);
            // 次の発生時間を入れる
            cyberGenerationTime[i] = Random.Range(minCyberGenerationTime, maxCyberGenerationTime + 1.0f);
            //サイバー攻撃表示の時間を入れる
            cyberTime[i] = cyberDisplayTime;
        }


    }

    // Update is called once per frame
    void Update()
    {
        // サイバー攻撃の発生
        for (int i = 0; i < cyberAttackEffect.Length; i++) 
        {
            // サイバー攻撃表示
            if (cyberAttackEffect[i].activeSelf)
            {
                // サイバー攻撃の表示時間
                if (cyberTime[i] < 0.0f)
                {
                    // サイバー攻撃表示の時間を入れる
                    cyberTime[i] = cyberDisplayTime;

                    // サイバー攻撃エフェクトの非表示
                    cyberAttackEffect[i].SetActive(false);
                }
                else
                {
                    cyberTime[i] -= Time.deltaTime;
                }
            }
            // サイバー攻撃非表示
            else
            {
                // サイバー攻撃の発生時間
                if (cyberGenerationTime[i] < 0.0f)
                {
                    // 次の発生時間を入れる
                    cyberGenerationTime[i] = Random.Range(minCyberGenerationTime, maxCyberGenerationTime + 1.0f);

                    // サイバー攻撃のポイントの非表示
                    cyberAttackPointEffect_1[i].SetActive(false);
                    cyberAttackPointEffect_2[i].SetActive(false);
                    cyberAttackPointEffect_3[i].SetActive(false);

                    // サイバー攻撃エフェクトの表示
                    // 発生座標を入れる
                    cyberAttackEffect[i].transform.localPosition = new Vector3(occurrenceVal[i], 1f, 0f);
                    cyberAttackEffect[i].SetActive(true);

                    // サイバー攻撃の発生座標の値
                    occurrenceVal[i] = Random.Range(minOccurrenceVal, maxOccurrenceVal + 1);
                }
                else
                {
                    cyberGenerationTime[i] -= Time.deltaTime;

                    // サイバー攻撃のポイントの表示、非表示
                    if (cyberGenerationTime[i] < 1.0f)
                    {
                        cyberAttackPointEffect_1[i].SetActive(false);
                        cyberAttackPointEffect_2[i].SetActive(false);
                        cyberAttackPointEffect_3[i].SetActive(true);
                    }
                    else if (cyberGenerationTime[i] < 2f)
                    {
                        cyberAttackPointEffect_1[i].SetActive(false);
                        cyberAttackPointEffect_2[i].SetActive(true);
                        cyberAttackPointEffect_3[i].SetActive(false);
                    }
                    else if (cyberGenerationTime[i] < 3f)
                    {
                        // 発生座標を入れる
                        cyberAttackPointEffect_1[i].transform.localPosition = new Vector3(occurrenceVal[i], 0f, 0f);
                        cyberAttackPointEffect_2[i].transform.localPosition = new Vector3(occurrenceVal[i], 0f, 0f);
                        cyberAttackPointEffect_3[i].transform.localPosition = new Vector3(occurrenceVal[i], 0f, 0f);

                        cyberAttackPointEffect_1[i].SetActive(true);
                        cyberAttackPointEffect_2[i].SetActive(false);
                        cyberAttackPointEffect_3[i].SetActive(false);
                    }
                }
            }
        }


    }
}
