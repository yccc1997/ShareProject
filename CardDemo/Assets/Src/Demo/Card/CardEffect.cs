using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect : CardBase
{
    /// <summary>
    /// 中间位置
    /// </summary>
    public Vector3 centerPos;

    /// <summary>
    /// 结束位置
    /// </summary>
    public Vector3 endPos;

    public Vector3 controlPoint1;
    public Vector3 controlPoint2;
    public GameObject LineObj;

    public int stage = 0;

    public float speed = 20;

    private float waitMaxTime = 0.3f;

    private float nowWaitTime = 0;

    private Vector3[] pathS;

    private int nowIndex = 0;

    // Start is called before the first frame update
    void Awake()
    {
        pathS = Tools.GetBezierCurve(20, centerPos, controlPoint1, controlPoint2, endPos);
        LineObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Play();
    }

    public void StartPlay(Vector3 pos)
    {
        transform.position = pos;
        stage = 0;
    }

    /// <summary>
    /// 居中阶段播放
    /// </summary>
    public void Play()
    {
        switch (stage)
        {
            case 0:
                resetData();
                stage = 1;
                break;
            case 1:
                transform.position = Vector3.Lerp(transform.position, centerPos, Time.deltaTime * speed);
                if (Vector3.Distance(transform.position, centerPos) <= 0.2f)
                {
                    nowWaitTime += Time.deltaTime;
                    if (nowWaitTime >= waitMaxTime)
                    {
                        stage = 2;
                        LineObj.SetActive(true);
                    }
                }

                break;
            case 2:
                if (nowIndex >= pathS.Length)
                {
                    stage = 3;
                    Invoke("DestroyThis", 1f); 
                    break;
                }

                transform.position = Vector3.Lerp(transform.position, pathS[nowIndex], Time.deltaTime * speed*3.5f);
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 0.01f, Time.deltaTime * 10);
                if (Vector3.Distance(transform.position, pathS[nowIndex]) <= 0.1f)
                {
                    nowIndex++;
                }

                break;
        }
    }

    private void resetData()
    {
        nowWaitTime = 0;
        nowIndex = 0;
    }
    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
    
    private void OnDrawGizmos()
    {
        // pathS = Tools.GetBezierCurve(20, centerPos, controlPoint1, controlPoint2, endPos);
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawLine(centerPos, controlPoint1);
        // Gizmos.DrawSphere(centerPos, 0.1f);
        // Gizmos.DrawSphere(controlPoint1, 0.1f);
        // Gizmos.DrawLine(endPos, controlPoint2);
        // Gizmos.DrawSphere(endPos, 0.1f);
        // Gizmos.DrawSphere(controlPoint2, 0.1f);
        //
        // Gizmos.color = Color.green;
        // for (int i = 0; i < pathS.Length; i++)
        // {
        //     Gizmos.DrawSphere(pathS[i], 0.1f);
        // }
    }
}