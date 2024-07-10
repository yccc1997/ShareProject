using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools 
{
    /// <summary>
    /// 获取两个向量之间的弧度值0-2π
    /// </summary>
    /// <param name="positionA">点A坐标</param>
    /// <param name="positionB">点B坐标</param>
    /// <returns></returns>
    public static float GetAngleInDegrees(Vector3 positionA,Vector3 positionB)
    {
        // 计算从A指向B的向量
        Vector3 direction = positionB - positionA;
        // 将向量标准化
        Vector3 normalizedDirection = direction.normalized;
        // 计算夹角的弧度值
        float dotProduct = Vector3.Dot(normalizedDirection, Vector3.up);
        float angleInRadians = Mathf.Acos(dotProduct);

        //判断夹角的方向：通过计算一个参考向量与两个物体之间的叉乘，可以确定夹角是顺时针还是逆时针方向。这将帮助我们将夹角的范围扩展到0到360度。
        Vector3 cross = Vector3.Cross(normalizedDirection, Vector3.up);
        if (cross.z>0)
        {
            angleInRadians = 2*Mathf.PI - angleInRadians;
        }
        // 将弧度值转换为角度值
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
        return angleInDegrees;
    }
    
    /// <summary>
    /// 获取塞贝尔曲线控制点列表
    /// </summary>
    /// <param name="Count">列表点数量（越大越精准）</param>
    /// <param name="startPoint">起始点</param>
    /// <param name="controlPoint1">起点控制点</param>
    /// <param name="controlPoint2">终点控制点</param>
    /// <param name="endPoint">终点</param>
    /// <returns></returns>
    public static  Vector3[] GetBezierCurve(int Count, Vector3 startPoint, Vector3 controlPoint1, Vector3 controlPoint2, Vector3 endPoint)
    {
        Vector3[] posS=new  Vector3[Count];
        for (int i = 0; i < Count; i++)
        {
            float t = i / (float) (Count - 1); // 参数 t 在 0 到 1 之间
            Vector3 position = CalculateBezierPoint(t, startPoint, controlPoint1, controlPoint2, endPoint);
            posS[i] = position;
        }

        return posS;
    }

    /// <summary>
    /// 获取塞贝尔曲线中间点
    /// </summary>
    /// <param name="t">参数 t 在 0 到 1 之间</param>
    /// <param name="startPoint">起始点</param>
    /// <param name="controlPoint1">起点控制点</param>
    /// <param name="controlPoint2">终点控制点</param>
    /// <param name="endPoint">终点</param>
    /// <returns></returns>
    public static Vector3 CalculateBezierPoint(float t, Vector3 startPoint, Vector3 controlPoint1, Vector3 controlPoint2, Vector3 endPoint)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 point = uuu * startPoint; // (1-t)^3 * P0
        point += 3 * uu * t * controlPoint1; // 3 * (1-t)^2 * t * P1
        point += 3 * u * tt * controlPoint2; // 3 * (1-t) * t^2 * P2
        point += ttt * endPoint; // t^3 * P3

        return point;
    }
}
