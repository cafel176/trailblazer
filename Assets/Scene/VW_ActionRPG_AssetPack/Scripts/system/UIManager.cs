using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance=null;

    public GameObject BlackPanel;
    public GameObject gameControler;

    //用于跳转场景的记录
    [HideInInspector]
    public int nowLevel = 0;
    [HideInInspector]
    public int[] seeds = new int[3];
    [HideInInspector]
    public bool haveSeed = false;

    private GameObject blackPanel = null;
    private GameObject gamemanager = null;

    private void Awake()
    {
        //创造管理器实例
        if (UIManager.instance == null)
        {
            UIManager.instance = this;
            DontDestroyOnLoad(UIManager.instance.gameObject);
        }
    }

    //0代表首页，1-3分别代表1-3层
    public void LoadLevel(int level)
    {
        nowLevel = level;
        beBlack(0.02f);
        StartCoroutine(wait(1, level == 0));
    }

    IEnumerator wait(float time,bool back)
    {
        yield return new WaitForSeconds(time);
        if (back)
        {
            Destroy(UIManager.instance.gameObject);
            if (gamemanager != null)
                Destroy(gamemanager);
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void spawnGameControler()
    {
        if (gamemanager == null)
        {
            gamemanager = Instantiate(gameControler) as GameObject;
        }
    }

    //退出游戏
    public void Exit()
    {
        Application.Quit();
    }

    //淡入，使用协程执行，可能需要手动写等待时间
    public void beBlack(float smoothing)
    {
        if (blackPanel == null)
        {
            GameObject go = GameObject.Instantiate(BlackPanel) as GameObject;
            blackPanel = go;
            go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
            go.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            go.GetComponent<BlackPanel>().beBlack(smoothing);
        }
        else
        {
            blackPanel.GetComponent<BlackPanel>().beBlack(smoothing);
        }
    }

    //淡出，使用协程执行，可能需要手动写等待时间
    public void beWhite(float smoothing)
    {
        if (blackPanel == null)
        {
            GameObject go = GameObject.Instantiate(BlackPanel) as GameObject;
            blackPanel = go;
            go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
            go.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            Color color = go.GetComponent<RawImage>().color;
            color.a = 1;
            go.GetComponent<RawImage>().color = color;
            go.GetComponent<BlackPanel>().beWhite(smoothing);
        }
        else
        {
            blackPanel.GetComponent<BlackPanel>().beWhite(smoothing);
        }
    }

    public void inputSeed(InputField g)
    {
        string txt = g.text;
        if (txt.Length == 6)
        {
            int a = int.Parse(txt);
            seeds[2] = a % 100;
            seeds[1] = (a / 100)%100;
            seeds[0] = a / 10000;
            LoadLevel(1);
        }
    }
}
