using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardItem : CardBase
{
    public Vector3 root;
    public float rot;
    public float size;
    public float animSpeed=10;
    public float yPos=0;
    public bool isSelect;
    public bool isTake;
    public bool isWaitAttack;
    
    public  void RefreshData(Vector3 root,float rot, float size,float yPos)
    {
        this.root = root;
        this.rot = rot;
        this.size = size;
        this.yPos = -yPos*0.02f;
    }
    public void SetPos()
    {
        float radius = isSelect ? size + 0.2f : size;
        float selectZ = isSelect ? this.yPos - 0.1f : this.yPos;
        float x = root.x + Mathf.Cos(rot)*radius;
        float y = isSelect?-2.75f:root.y + Mathf.Sin(rot)*radius;
        transform.position =new  Vector3(transform.position.x,transform.position.y,root.z+selectZ);
        transform.position = Vector3.Lerp(transform.position, new Vector3(x,  y,root.z+selectZ), Time.deltaTime*animSpeed); 
        float rotZ=isSelect?0:Tools.GetAngleInDegrees( root,transform.position);
        Vector3 localEulerAngles = transform.localEulerAngles;
        Quaternion rotationQuaternion = Quaternion.Euler(new  Vector3(0,0,rotZ));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationQuaternion, Time.deltaTime*animSpeed*30);
    }
    
    public void SetPos1()
    {

        float x = root.x + Mathf.Cos(rot)*size;
        float y = root.y + Mathf.Sin(rot)*size;
        transform.position = Vector3.Lerp(transform.position, new Vector3(x,  y,root.z), Time.deltaTime*animSpeed); 
        float rotZ=Tools.GetAngleInDegrees( root,transform.position);
        Vector3 localEulerAngles = transform.localEulerAngles;
        Quaternion rotationQuaternion = Quaternion.Euler(new  Vector3(0,0,rotZ));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationQuaternion, Time.deltaTime*animSpeed*30);
    }

    // Update is called once per frame
    void Update()
    {
        SetPos();
    }

    private void OnDrawGizmos()
    {
        if (root==null)
        {
            return;
        }
        // Gizmos.DrawLine(transform.position, root.position);
        // Gizmos.color=Color.green;
        // Gizmos.DrawLine(root.position-Vector3.up*size, root.position+Vector3.up*size);
        // Gizmos.DrawLine(root.position-Vector3.left*size, root.position + Vector3.left*size);
        //
        // Gizmos.color=Color.red;
        // Gizmos.DrawLine(transform.position-Vector3.up*2, transform.position+Vector3.up*2);
        // Gizmos.DrawLine(transform.position-Vector3.left*2, transform.position + Vector3.left*2);
        // float rotY= GetAngleInDegrees(transform.position,root.position);
        // UnityEditor.Handles.Label(root.position+Vector3.right-Vector3.up, rotY.ToString());
    }
   
  
}


