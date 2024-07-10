using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour
{
    private Animator anim;
    public GameObject beAttackEffect;
    void Awake()
    {
        anim = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PlayBeAttackAnim()
    {
        anim.SetTrigger("beAttck");
       GameObject temp= Instantiate(beAttackEffect);
       Vector3 POS = transform.position;
       POS.z = 10;
       temp.transform.position = POS;
    }
}
