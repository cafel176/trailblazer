using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadingScene : MonoBehaviour {

    public Slider loadingSlider;
    public Text loadingNum;

    void Start()
    {
        int y = UIManager.instance.nowLevel > 0 ? UIManager.instance.nowLevel+1 : 0;
        LoadGame(y);
    }

    public void LoadGame(int scene)
    {
        StartCoroutine(StartLoading(scene));
    }

    //异步加载
    //当获得AsyncOperation.progress的值后，不立即更新进度条的数值，而是每一帧在原有的数值上加1
    //这样就会产生数字不停滚动的动画效果了，迅雷中显示下载进度就用了这个方法。
    private IEnumerator StartLoading(int scene)
    {
        UIManager.instance.beWhite(0.02f);
        yield return new WaitForSeconds(1);
        int displayProgress = 0;
        int toProgress = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            toProgress = (int)op.progress * 100;
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                SetLoadingPercentage(displayProgress);
                yield return new WaitForEndOfFrame();
            }
        }

        toProgress = 100;
        while (displayProgress < toProgress)
        {
            ++displayProgress;
            SetLoadingPercentage(displayProgress);

            yield return new WaitForEndOfFrame();
        }
        UIManager.instance.beBlack(0.02f);
        yield return new WaitForSeconds(1);
        op.allowSceneActivation = true;
    }

    //UI显示进度
    private void SetLoadingPercentage(int Progress)
    {
        loadingSlider.value = Progress;
        loadingNum.text = Progress + "%";
    }
}
