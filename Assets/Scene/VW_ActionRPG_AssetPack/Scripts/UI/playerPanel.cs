using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerPanel : MonoBehaviour {

    public Image[] items;
    public Sprite[] attackType;

    public Image attackTypeImage;
    public Slider hpSlider;
    public Slider kpSlider;
    public Slider angrySlider;
    public Slider bossSlider;
    public Text hpText;
    public Text kpText;
    public Text angryText;

    public Button close;
    public Button opinion;
    public Button map;

    public Image moveControlSphere;
    public Image moveControlSmall;
    public GameObject SwordControl,ShotControl,changeControl;
    public int l = 4000;

    public Sprite UIMask;

#if UNITY_ANDROID//安卓
    void Start()
    {
        moveControlSphere.gameObject.SetActive(true);
        moveControlSmall.gameObject.SetActive(true);
        SwordControl.SetActive(true);
        ShotControl.SetActive(true);
        changeControl.SetActive(true);
}
#endif

#if UNITY_ANDROID//安卓
    void Update()
    {
        moveControl();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            backToMain();
        }
    }
#endif

    void FixedUpdate()
    {
        hpSlider.maxValue = PlayerManager.instance.data.maxHp;
        kpSlider.maxValue = PlayerManager.instance.data.maxKp;
        angrySlider.maxValue = 100;

        if (Mathf.Abs(hpSlider.value - PlayerManager.instance.data.hp) >= 30)
        {
            hpSlider.value += (hpSlider.value > PlayerManager.instance.data.hp ? -5 : 5);
        }
        else if(hpSlider.value != PlayerManager.instance.data.hp)
        {
            hpSlider.value += (hpSlider.value > PlayerManager.instance.data.hp ? -1 : 1);
        }
        hpText.text = PlayerManager.instance.data.hp + " / " + PlayerManager.instance.data.maxHp;

        if (Mathf.Abs(kpSlider.value - PlayerManager.instance.data.kp) >= 30)
        {
            kpSlider.value += (kpSlider.value > PlayerManager.instance.data.kp ? -5 : 5);
        }
        else if (kpSlider.value != PlayerManager.instance.data.kp)
        {
            kpSlider.value += (kpSlider.value > PlayerManager.instance.data.kp ? -1 : 1);
        }
        kpText.text = PlayerManager.instance.data.kp + " / " + PlayerManager.instance.data.maxKp;

        if (Mathf.Abs(angrySlider.value - PlayerManager.instance.angry) >= 30)
        {
            angrySlider.value += (angrySlider.value > PlayerManager.instance.angry ? -5 : 5);
        }
        else if (angrySlider.value != PlayerManager.instance.angry)
        {
            angrySlider.value += (angrySlider.value > PlayerManager.instance.angry ? -1 : 1);
        }

        if (PlayerManager.instance.angry>80)
            angryText.text = "S";
        else if (PlayerManager.instance.angry > 60)
            angryText.text = "A";
        else if (PlayerManager.instance.angry > 40)
            angryText.text = "B";
        else if (PlayerManager.instance.angry > 20)
            angryText.text = "C";
        else
            angryText.text = "D";

        attackTypeImage.sprite = attackType[PlayerManager.instance.attackState];
    }

    public void backToMain()
    {
        UIManager.instance.LoadLevel(0);
    }

    public void moveControl()
    {
        //有触摸点，且滑动
        if (Input.touchCount > 0)
        {
            Vector3 touchPos = Input.GetTouch(0).position;
            touchPos.z = moveControlSphere.rectTransform.position.z;

            //float X = Input.mousePosition.x;
            //float Y = Input.mousePosition.y;
            //Vector3 touchPos = new Vector3(X, Y, moveControlSmall.rectTransform.position.z);

            if (PlayerManager.instance.player == null)
            {
                Debug.LogError("角色还没有生成");
                return;
            }
            if ((touchPos - moveControlSphere.rectTransform.position).sqrMagnitude <= l*l)
            {
                touchPos.z = moveControlSmall.rectTransform.position.z;
                moveControlSmall.rectTransform.position = touchPos;
            }
            else if((touchPos - moveControlSphere.rectTransform.position).sqrMagnitude <= 4 * l * l)
            {
                Vector2 a = new Vector2(touchPos.x- moveControlSphere.rectTransform.position.x, touchPos.y - moveControlSphere.rectTransform.position.y).normalized;
                Vector3 p = new Vector3(moveControlSphere.rectTransform.position.x+a.x* l, moveControlSphere.rectTransform.position.y+a.y*l, moveControlSmall.rectTransform.position.z);
                moveControlSmall.rectTransform.position = p;
            }
            Vector2 dd=new Vector2(moveControlSmall.rectTransform.position.x -moveControlSphere.rectTransform.position.x,moveControlSmall.rectTransform.position.y - moveControlSphere.rectTransform.position.y);
            PlayerManager.instance.moveControl(dd.x,dd.y);
        }
        else
        {
            moveControlSmall.rectTransform.position = moveControlSphere.rectTransform.position;
            PlayerManager.instance.moveControl(0, 0);
        }
    }

    public void attackControl()
    {
        if (PlayerManager.instance.player == null)
        {
            Debug.LogError("角色还没有生成");
            return;
        }
        PlayerManager.instance.attackControl();
    }

    public void shotControl()
    {
        if (PlayerManager.instance.player == null)
        {
            Debug.LogError("角色还没有生成");
            return;
        }
        PlayerManager.instance.shotControl();
    }

    public void changeWeaponControl()
    {
        if (PlayerManager.instance.player == null)
        {
            Debug.LogError("角色还没有生成");
            return;
        }
        int t = PlayerManager.instance.attackState;
        PlayerManager.instance.changeWeaponControl(++t%3);
    }

    //null则用mask
    public void changeItem(int index, Sprite sprite)
    {
        if (sprite==null)
        {
            items[index].sprite = UIMask;
        }
        else
        {
            items[index].sprite = sprite;
        }
    }

    public void useItem(int index)
    {
        PlayerManager.instance.useItem(index);
    }
}
