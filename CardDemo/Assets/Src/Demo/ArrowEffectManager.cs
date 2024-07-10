using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEffectManager : MonoBehaviour
{
    public GameObject reticleBlock;
    public GameObject reticleArrow;
    public int MaxCount = 18;
    public Vector3 startPoint; // 起始点
    private Vector3 controlPoint1; // 控制点1
    private Vector3 controlPoint2; // 控制点2
    private Vector3 endPoint; // 结束点
    private List<GameObject> ArrowItemList;
    private List<SpriteRenderer> RendererItemList;
    private Animator Arrow_anim;
    private bool isSelect;

    public bool IsSelect
    {
        get => isSelect;
        set
        {
            if (isSelect != value)
            {
                isSelect = value;
                if (value==true)
                {
                    PlayAnim();
                }
            }
        }
    }

    private void Awake()
    {
        InitData();
    }

    private void InitData()
    {
        ArrowItemList = new List<GameObject>();
        RendererItemList = new List<SpriteRenderer>();
        for (int i = 0; i < MaxCount; i++)
        {
            GameObject Arrow = (i == MaxCount - 1) ? reticleArrow : reticleBlock;
            GameObject temp = Instantiate(Arrow, this.transform);
            if (i == MaxCount - 1)
            {
                Arrow_anim = temp.GetComponent<Animator>();
            }

            ArrowItemList.Add(temp);
            SpriteRenderer re = temp.transform.GetChild(0).GetComponent<SpriteRenderer>();
            if (re != null)
            {
                RendererItemList.Add(re);
            }
        }
    }

    public void Update()
    {
        Move();
        DrawBezierCurve();
    }

    private void DrawBezierCurve()
    {
        for (int i = 0; i < ArrowItemList.Count; i++)
        {
            float t = i / (float) (ArrowItemList.Count - 1); // 参数 t 在 0 到 1 之间

            Vector3 position =Tools.CalculateBezierPoint(t, startPoint, controlPoint1, controlPoint2, endPoint);
            ArrowItemList[i].gameObject.SetActive(i != ArrowItemList.Count - 2);
            ArrowItemList[i].transform.position = position;
            ArrowItemList[i].transform.localScale = Vector3.one * (t / 2f) + Vector3.one * 0.3f;
            if (i > 0)
            {
                float SignedAngle = Vector2.SignedAngle(Vector2.up,
                    ArrowItemList[i].transform.position - ArrowItemList[i - 1].transform.position);
                Vector3 euler = new Vector3(0, 0, SignedAngle);
                ArrowItemList[i].transform.rotation = Quaternion.Euler(euler);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawLine(startPoint, controlPoint1);
        // Gizmos.DrawSphere(startPoint, 0.1f);
        // Gizmos.DrawSphere(controlPoint1, 0.1f);
        // Gizmos.DrawLine(endPoint, controlPoint2);
        // Gizmos.DrawSphere(endPoint, 0.1f);
        // Gizmos.DrawSphere(controlPoint2, 0.1f);
    }

    public void Move()
    {
        Vector3 mousePosition = Input.mousePosition; // 获取鼠标位置
        mousePosition.z = Camera.main.nearClipPlane; // 设置z坐标为摄像机近裁剪平面的位置
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 2f;
        endPoint = worldPosition;

        controlPoint1 = (Vector2) startPoint + (worldPosition - startPoint) * new Vector2(-0.3f, 0.8f);
        controlPoint2 = (Vector2) startPoint + (worldPosition - startPoint) * new Vector2(0.1f, 1.4f);
        controlPoint1.z = startPoint.z;
        controlPoint2.z = startPoint.z;
    }

    /// <summary>
    /// 设置起始位置
    /// </summary>
    public void SetStartPos(Vector3 pos)
    {
        startPoint = pos;
    }

    /// <summary>
    /// 设置颜色
    /// </summary>
    public void SetColor(Color color)
    {
        if (RendererItemList == null)
        {
            return;
        }

        for (int i = 0; i < RendererItemList.Count; i++)
        {
            RendererItemList[i].color = color;
        }
    }

    /// <summary>
    /// 设置颜色
    /// </summary>
    /// <param name="isSelect"></param>
    public void SetColor(bool isSelect)
    {
        IsSelect = isSelect;
        if (isSelect)
        {
            SetColor(Color.red);
        }
        else
        {
            SetColor(Color.white);
        }
    }

    private void PlayAnim()
    {
        if (Arrow_anim == null)
        {
            return;
        }
        Arrow_anim.SetTrigger("select");
    }
}