using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CardData", menuName = "Data/CardData")]
public class CardData : ScriptableObject
{
    [Tooltip("卡牌名称")]
    public string cardName;
    [Tooltip("类型名称")]
    public string typeName;
    [Tooltip("效果描述")]
    public string des;
    [Tooltip("费用")]
    public string red_orbCount;
    [Tooltip("卡片类型")]
    public ECardAttackType eType;
    [Tooltip("底图")]
    public Sprite bg;
    [Tooltip("类型")]
    public Sprite power;
    [Tooltip("类型")]
    public Sprite red_orb;
    [Tooltip("稀有度")]
    public Sprite banner;
    [Tooltip("卡片主图")]
    public Sprite form;
   
}
