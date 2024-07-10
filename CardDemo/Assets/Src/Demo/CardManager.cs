using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Vector3 rootPos=new Vector3(0,-33.5f,20);
    public GameObject cardItem;
    public GameObject selectPlayerEffect;
    public float size =15f;
    public CardBase temporaryCard;
    public CardEffect CardEffect;
    public ArrowEffectManager lineEffect;
    public CardData[] cardDataList;
    
    private float minPos = 1.415f;
    private float maxPos = 1.73f;
    private List<CardItem> cardList;
    private Vector3 temporaryCardStartPos;
    /// <summary>
    /// 偶数
    /// </summary>
    private List<float> rotPos_EvenNumber;
    /// <summary>
    /// 奇数
    /// </summary>
    private List<float> rotPos_OddNumber;

    private float interval_EvenNumber;
    private float interval_OddNumber;
    private int CardMaxCount=8;
    /// <summary>
    /// 当前点击选中的卡牌
    /// </summary>
    private CardItem nowTaskItem;
    
    private CardItem nowSelectItem;

    /// <summary>
    /// 当前选中敌人
    /// </summary>
    private Role nowSelectPlayer;

    /// <summary>
    /// 当前鼠标指向的卡牌
    /// </summary>
    public CardItem NowSelectItem
    {
        get => nowSelectItem;
        set
        {
            if (nowSelectItem!=value)
            {
                nowSelectItem = value;
                RefreshSelectItem(nowSelectItem);
            }
           
        }
    }

    #region 数据初始化
    void Start()
    {
        InitCard();
       
    }
    
    /// <summary>
    /// 数据初始化
    /// </summary>
    public  void InitCard()
    {
        int EvenNumber = CardMaxCount % 2 == 0 ? CardMaxCount : CardMaxCount - 1;
        int OddNumber = CardMaxCount % 2 == 0 ? CardMaxCount-1 : CardMaxCount;
        rotPos_EvenNumber=InitRotPos(EvenNumber,out interval_EvenNumber);
        rotPos_OddNumber=InitRotPos(OddNumber,out interval_OddNumber);
    }
    public List<float> InitRotPos(int count,out float interval)
    {
        List<float> rotPos=new List<float>();
        interval = (maxPos - minPos)/count;
        for (int i = 0; i < count; i++)
        {
            float nowPos = maxPos - interval * i;
            rotPos.Add(nowPos);
        }
        return rotPos;
    }
    

  #endregion

   
   
   
    
    // Update is called once per frame
    void Update()
    {
        TaskItemDetection();
        RefereshCard();
        SelectItemDetection();
        CardUseEffect();

    }
   
    /// <summary>
    /// 添加卡牌
    /// </summary>
    public  void AddCard()
    {
        if (cardList==null)
        {
            cardList=new List<CardItem>();
        }

        if (cardList.Count>=CardMaxCount)
        {
            Debug.Log("手牌数量上限");
            return;
        }
        GameObject item = Instantiate(cardItem,this.transform);
        CardItem text=item.GetComponent<CardItem>();
        float a=Random.Range(0f, cardDataList.Length);
      
        int index=(int)Random.Range(0f, cardDataList.Length);
        text.RefreshShowInfo(cardDataList[index]);
        text.RefreshData(rootPos,0,0,0);
        cardList.Add(text);
    }
    /// <summary>
    /// 手牌状态刷新
    /// </summary>
    public void RefereshCard()
    {
        if (cardList==null)
        {
            return;
        }

        List<float> rotPos;
        int strtCount = 0;
        float interval;
        if (cardList.Count%2==0)
        {
            rotPos = rotPos_EvenNumber;
            interval = interval_EvenNumber;
            strtCount= rotPos_EvenNumber.Count / 2 - cardList.Count / 2;
        }
        else
        {
            rotPos = rotPos_OddNumber;
            interval = interval_OddNumber;
            strtCount= (rotPos_OddNumber.Count+1) / 2 - (cardList.Count+1) / 2;
        }

        int TaskIndex = 0;
        bool isTaskIndex=GetTaskIndex(out TaskIndex);
        
        for (int i = 0; i < cardList.Count; i++)
        {
            float shifting = 0;
            float indexNowNumber = 0.005f;
            float Difference = TaskIndex - i;
            float absDifference = Difference > 0 ? 3-Difference : 3+Difference;
            if (absDifference<0)
            {
                absDifference = 0;
            }
            if (isTaskIndex&&TaskIndex!=i)
            {
                shifting = (TaskIndex > i) ? indexNowNumber*absDifference : -indexNowNumber*absDifference;
            }
         
            cardList[i].RefreshData(rootPos,rotPos[strtCount+i]+shifting,size,i); 
        }
    }
    /// <summary>
    /// 销毁卡牌
    /// </summary>
    public  void RemoveCard()
    {
        if (cardList==null)
        {
            return;
        }

        CardItem item = cardList[cardList.Count - 1];
        cardList.Remove(item);
        Destroy(item.gameObject);
    }
    /// <summary>
    /// 销毁卡牌
    /// </summary>
    /// <param name="item"></param>
    public  void RemoveCard(CardItem item)
    {
        if (cardList==null)
        {
            return;
        }
        cardList.Remove(item);
        Destroy(item.gameObject);
    }

    private Vector3 oldmousePosition;
    
    /// <summary>
    /// 选中卡牌检测
    /// </summary>
    public void SelectItemDetection()
    {
        if (oldmousePosition==Input.mousePosition)
        {
            return;
        }

        if (nowTaskItem!=null)
        {
            return;
        }

        oldmousePosition = Input.mousePosition;
        // 从鼠标位置创建一条射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Card");
        // 检测射线是否与物体相交
        if (Physics.Raycast(ray, out hit,1000, layerMask))
        {
            if (hit.collider.gameObject!=null)
            {
                NowSelectItem = hit.collider.gameObject.GetComponent<CardItem>();
             
                return;
            }
        }
       
        NowSelectItem = null;
    }
    
    /// <summary>
    /// 玩家操作检测
    /// </summary>
    public void TaskItemDetection()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddCard();
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (nowTaskItem!=null)
            {
                nowTaskItem.gameObject.SetActive(true);
                nowTaskItem.isTake = false;
                nowTaskItem = null;
            }
            
            temporaryCardStartPos = temporaryCard.transform.position;
            SelectCard();
         
        }

        if (Input.GetMouseButton(0))
        {
            SelectEnemy();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (nowTaskItem!=null)
            {
                nowTaskItem.isWaitAttack = false;
                if (IsDestoryCard())
                {
                    PlayEndAnim(nowTaskItem);
                    RemoveCard(nowTaskItem);
                }
                else
                {
                    nowTaskItem.gameObject.SetActive(true);
                    nowTaskItem.isTake = false;
                    nowTaskItem = null;
                }
                nowSelectPlayer = null;
            }
        }
    }
    /// <summary>
    /// 刷新当前选中的卡牌
    /// </summary>
    /// <param name="selectItem"></param>
    public  void RefreshSelectItem(CardItem selectItem)
    {
        if (cardList==null)
        {
            return;
        }
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].isSelect = cardList[i]==selectItem;
            cardList[i].isTake = cardList[i]==selectItem;
            if (cardList[i]==selectItem)
            {
                temporaryCard.gameObject.transform.position = cardList[i].gameObject.transform.position;
            }
        }
    }
    /// <summary>
    /// 得到当前选中的卡牌下标
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool GetTaskIndex(out int index)
    {
        index = 0;
        for (int i = 0; i < cardList.Count; i++)
        {
            if ( cardList[i].isTake)
            {
                index = i;
                return true;
            }
           
        }

        return false;
    }
    /// <summary>
    /// 设置卡牌使用特效
    /// </summary>
    public  void CardUseEffect()
    {
        if (nowTaskItem==null)
        {
            temporaryCard.gameObject.SetActive(false);
            lineEffect.gameObject.SetActive(false);
            return;
        }
        temporaryCard.RefreshShowInfo(nowTaskItem.data);
        temporaryCard.gameObject.SetActive(true);
        
        Vector3 mousePosition = Input.mousePosition; // 获取鼠标位置
        mousePosition.z = Camera.main.nearClipPlane; // 设置z坐标为摄像机近裁剪平面的位置

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 5f;
        Vector3 centPos=new  Vector3(0,-2.9f,4);
        bool isWaitAttack = false;
       
        if (nowTaskItem.attackType==ECardAttackType.Single)
        {
            if (worldPosition.y>-2.4f)
            {
                isWaitAttack = true;
            }
        }
       // print(worldPosition+":"+isWaitAttack);
        temporaryCard.gameObject.transform.position = Vector3.Lerp( temporaryCard.gameObject.transform.position,isWaitAttack?centPos:worldPosition,Time.deltaTime*15); 
        lineEffect.SetStartPos(isWaitAttack?centPos:worldPosition);
        lineEffect.SetColor(nowSelectPlayer!=null);
        lineEffect.gameObject.SetActive(isWaitAttack);
        if (nowSelectPlayer!=null)
        {
            selectPlayerEffect.transform.position = nowSelectPlayer.transform.position;
        }
        selectPlayerEffect.gameObject.SetActive(nowSelectPlayer!=null);
       
        
    }

    /// <summary>
    /// 是否需要销毁卡牌
    /// </summary>
    /// <returns></returns>
    public bool IsDestoryCard()
    {
        if (nowSelectPlayer!=null)
        {
            return true;
        }
        float dis = temporaryCardStartPos.y - temporaryCard.transform.position.y;
        float absDis = dis > 0 ? dis : -dis;
      return absDis > 2.6f;
    }
    
    /// <summary>
    /// 选中卡牌
    /// </summary>
    public void SelectCard()
    {
        // 从鼠标位置创建一条射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Card");
        // 检测射线是否与物体相交
        if (Physics.Raycast(ray, out hit,1000, layerMask))
        {
            if (hit.collider.gameObject!=null)
            {
                nowTaskItem = hit.collider.gameObject.GetComponent<CardItem>();
                nowTaskItem.gameObject.SetActive(false);
            }
        }
    }
    
    /// <summary>
    /// 选中对象
    /// </summary>
    public void SelectEnemy()
    {
        if (nowTaskItem==null)
        {
            nowSelectPlayer = null;
            return;
        }
        

        ECardAttackType etype = nowTaskItem.attackType;
        switch (etype)
        {
             case ECardAttackType.Power:
                 break;
             case ECardAttackType.Single:
                 nowSelectPlayer = GetSelectPlayer("Enemy");
                 break;
             case ECardAttackType.Skill:
                 break;
        }
       
    }
    
    /// <summary>
    /// 获取当前选中的对象
    /// </summary>
    /// <param name="layerName"></param>
    /// <returns></returns>
    public Role GetSelectPlayer(string layerName)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask(layerName);
        // 检测射线是否与物体相交
        if (Physics.Raycast(ray, out hit,1000, layerMask))
        {
            if (hit.collider.gameObject!=null)
            {
               
                return  hit.collider.transform.GetComponent<Role>();
                
            }
        }

        return null;
    }
    /// <summary>
    /// 播放选中结束动画
    /// </summary>
    /// <param name="data"></param>
    public  void PlayEndAnim(CardItem data)
    {
        if (data==null)
        {
            return;
        }
       GameObject temp=Instantiate(CardEffect.gameObject,this.transform);
       CardEffect effect= temp.GetComponent<CardEffect>();
       effect.RefreshShowInfo(data.data);
       effect.StartPlay(temporaryCard.transform.position);
       if (nowSelectPlayer!=null)
       {
           nowSelectPlayer.PlayBeAttackAnim();
       }
    }
    
    
    
     
 }
