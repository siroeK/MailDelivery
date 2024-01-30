using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージの管理プロジェクト
/// </summary>
public class StageManager : MonoBehaviour
{
    [Header("ゲームステージの情報")]
    [SerializeField, Tooltip("最背面の位置")]
    private float rearMostPos = -0.5f;
    [SerializeField, Tooltip("最大側面の位置")]
    private float maxSidePos = 4.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 最背面の位置情報を渡す
    /// </summary>
    public float GetRearMostPos()
    {
        return rearMostPos;
    }

    /// <summary>
    /// 最大側面の位置情報を渡す
    /// </summary>
    public float GetMaxSidePos()
    {
        return maxSidePos;
    }


}
