using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase : MonoBehaviour
{

    public CardData data;
    private SpriteRenderer bg;
    private SpriteRenderer power;
    private SpriteRenderer red_orb;
    private SpriteRenderer banner;
    private SpriteRenderer form;
    private TextMesh cardName;
    private TextMesh typeName;
    private TextMesh des;
    private TextMesh red_orbCount;
    private bool isInitComponent;
    public ECardAttackType attackType;

    public void Awake()
    {
        InitComponent();
    }
    
    private void InitComponent()
    {
        bg = transform.Find("card/bg").GetComponent<SpriteRenderer>();
        power = transform.Find("card/power").GetComponent<SpriteRenderer>();
        red_orb = transform.Find("card/red_orb").GetComponent<SpriteRenderer>();
        banner = transform.Find("card/banner").GetComponent<SpriteRenderer>();
        form = transform.Find("card/form").GetComponent<SpriteRenderer>();
        cardName = transform.Find("card/cardName").GetComponent<TextMesh>();
        typeName = transform.Find("card/typeName").GetComponent<TextMesh>();
        des = transform.Find("card/des").GetComponent<TextMesh>();
        red_orbCount = transform.Find("card/red_orbCount").GetComponent<TextMesh>();
        isInitComponent = true;

    }
    public  void RefreshShowInfo(CardData data)
    {
        if (isInitComponent==false)
        {
            InitComponent();
        }
        this.data = data;
        if (data==null)
        {
            return;
        }

        bg.sprite = this.data.bg;
        power.sprite = this.data.power;
        red_orb.sprite = this.data.red_orb;
        banner.sprite = this.data.banner;
        form.sprite = this.data.form;
        cardName.text = this.data.cardName;
        typeName.text = this.data.typeName;
        des.text = this.data.des;
        red_orbCount.text = this.data.red_orbCount;
        attackType =  this.data.eType;
        Vector3 pos = power.transform.localPosition;
        pos.y = this.data.eType == ECardAttackType.Power ? 0.75f : 0.5f;
        power.transform.localPosition = pos;
    }
    
    public void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }
}

public enum ECardAttackType
{
    Skill,
    Single,
    Power
}
