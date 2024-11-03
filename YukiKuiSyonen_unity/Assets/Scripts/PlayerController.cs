using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2.0f; // 動く速さの設定

    private float moveRange = 2.17f; // 動ける範囲の設定


    void Start()
    {

    }

    void Update()
    {
        // 水平方向の入力を-1,+1で取得
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // 現在の位置を取得
        Vector3 currentPos = transform.position;

        // x方向の移動量を計算
        // 1 or -1 × 速さ × 時間
        currentPos.x += horizontalInput * speed * Time.deltaTime;

        // xとの値をそれぞれ指定範囲内に収める
        // Mathf.Clamp(調整したい値,最小の値,最大の値);
        currentPos.x = Mathf.Clamp(currentPos.x, -moveRange, moveRange);

        // 位置を更新
        transform.position = currentPos;

        // 方向転換
        if (horizontalInput != 0)
        {
            // 入力方向に応じて向きを変える
            transform.localScale = new Vector3(horizontalInput, 1, 1);
        }
    }
}

/*
一旦PCの左右キーだけで移動できる機能の実装
    Transformで動かす
アニメーションに対応できるようなキャラクターの方向転換
*/
