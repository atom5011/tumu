﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    [Header("ゲームに登場する干支の最大種類数")]
    public int etoTypeCount = 5;

    [Header("ゲーム開始時に生成する干支の数")]
    public int createEtoCount = 50;

    [Header("現在のスコア")]
    public int score = 0;

    [Header("干支を消した際に加算されるスコア")]
    public int etoPoint = 100;

    [Header("消した干支の数")]
    public int eraseEtoCount = 0;

    [SerializeField, Header("1回辺りのゲーム時間")]
    private int initTime = 60;

    [Header("現在のゲームの残り時間")]
    public float gameTime;

    /// <summary>
    /// 干支の基本情報
    /// </summary>
    [System.Serializable]
    public class EtoData
    {
        public EtoType etoType;
        public Sprite sprite;

        //コンストラクタ（インスタンス（new）時に用意している因数への値の代入を強制するメソッド）
        public EtoData(EtoType etoType, Sprite sprite)
        {
            this.etoType = etoType;
            this.sprite = sprite;
        }
    }

    [Header("干支１２種類のリスト")]
    public List<EtoData> etoDataList = new List<EtoData>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // ゲームの初期化
        InitGame();
    }

    /// <summary>
    /// ゲーム初期化
    /// </summary>
    private void InitGame()
    {
        score = 0;
        eraseEtoCount = 0;

        // ゲーム時間を設定
        gameTime = initTime;

        Debug.Log("Init Game");
    }

    /// <summary>
    /// 現在のゲームシーンを再読み込み
    /// </summary>
    /// <returns></returns>
    public IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1.0f);

        // 現在のゲームシーンを取得し、シーンの名前を使ってLoadScene処理を行う(再度、同じゲームシーンを呼び出す)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // 初期化 GameDataゲームオブジェクトはシーン遷移しても破棄されない設定になっていますので、ここで再度、初期化の処理を行う必要
        InitGame();
    }

    /// <summary>
    /// 干支データのリストを作成
    /// </summary>
    /// <returns></returns>
    public IEnumerator IniEtoDataList()
    {
        //干支の画像を読み込むための変数を配列で用意（GameManagerの宣言フィールドで用意していたものを、このメソッド内のみで使用するように変更）
        Sprite[] etoSprites = new Sprite[(int)EtoType.Count];

        //Resources.LoadAllを行い、分割されている干支の画像を順番にすべて読み込んで配列に代入
        etoSprites = Resources.LoadAll<Sprite>("Sprites/eto");

        //ゲームに登場する１２種類の干支データを作成
        for(int i = 0; i < (int)EtoType.Count; i++)
        {
            //干支の情報を扱うクラスEtoDataをインスタンス（new EtoData()9し、コンストラクタを使って値を代入
            EtoData etoData = new EtoData((EtoType)i, etoSprites[i]);

            //干支データをListへ追加
            etoDataList.Add(etoData);
        }
        yield break;
    }
}