using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CompositeCollider2D;

/// <summary>
/// ビームが出てくる道の管理プロジェクト
/// </summary>
public class BeamRoadManager : MonoBehaviour
{

    [SerializeField, Header("ビームエフェクト")]
    private GameObject beamEffect;

    [SerializeField, Header("床")]
    private MeshRenderer floor;

    [SerializeField, Header("床のマテリアル")] 
    private Material[] floorMaterialArray = new Material[2];

    [Header("ビーム表示関係")]
    [SerializeField, Tooltip("ビームの最小発生時間の間隔")]
    private float minBeamGenerationTime = 4.0f;

    [SerializeField, Tooltip("ビームの最大発生時間の間隔")]
    private float maxBeamGenerationTime = 6.0f;

    private float beamGenerationTime = 0.0f;    // 発生時間

    [SerializeField, Tooltip("ビーム表示の時間")]
    private float beamDisplayTime = 1.5f;

    private float beamTime = 0f;  // ビームの時間


    private void Awake()
    {
        // 発生時間を入れる
        beamGenerationTime = Random.Range(minBeamGenerationTime, maxBeamGenerationTime + 1.0f);

        //ビーム表示の時間を入れる
        beamTime = beamDisplayTime;
    }

    // Update is called once per frame
    void Update()
    {
        // ビームエフェクト表示
        if (beamEffect.activeSelf)
        {
            // ビームの表示時間
            if (beamTime < 0.0f)
            {
                //ビーム表示の時間を入れる
                beamTime = beamDisplayTime;

                // 床のmaterialを戻す
                floor.material = floorMaterialArray[0];

                // ビームエフェクトの非表示
                beamEffect.SetActive(false);
            }
            else
            {
                beamTime -= Time.deltaTime;
            }
        }
        // ビームエフェクト非表示
        else
        {
            // ビームの発生時間
            if (beamGenerationTime <= 0.0f)
            {
                // 次の発生時間を入れる
                beamGenerationTime = Random.Range(minBeamGenerationTime, maxBeamGenerationTime + 1.0f);

                // 床のmaterialを切り替える
                floor.material = floorMaterialArray[3];

                // ビームエフェクトの表示
                beamEffect.SetActive(true);
            }
            else
            {
                beamGenerationTime -= Time.deltaTime;

                // 床のmaterialを切り替える
                if (beamGenerationTime < 0.5f)
                {
                    floor.material = floorMaterialArray[0];
                }
                else if (beamGenerationTime < 1f)
                {
                    floor.material = floorMaterialArray[3];
                }
                else if (beamGenerationTime < 1.5f)
                {
                    floor.material = floorMaterialArray[0];
                }
                else if(beamGenerationTime < 2f)
                {
                    floor.material = floorMaterialArray[2];
                }
                else if (beamGenerationTime < 2.5f)
                {
                    floor.material = floorMaterialArray[0];
                }
                else if (beamGenerationTime < 3f)
                {
                    floor.material = floorMaterialArray[1];
                }

            }
        }
    }
}
