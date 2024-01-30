using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラの追従
/// </summary>
public class CameraManager : MonoBehaviour
{
    [SerializeField, Header("プレイヤー情報格納")]
    private GameObject player;
    
    private Vector3 offset;      //相対距離取得用

    // Start is called before the first frame update
    void Start()
    {
        // MainCamera(自分自身)とplayerとの相対距離を求める
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //新しいトランスフォームの値を代入する
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z + offset.z);
    }
}
