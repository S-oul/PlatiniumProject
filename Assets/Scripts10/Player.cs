using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject LeftArm;
    float LeftArmValue = 0;
    float punchAcc = .01f;
    float punchCD = .8f;
    bool canLeftPunch = true;

    public float speed = 5;
    public float Force = 5;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);

        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0,0);

        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }




        if (Input.GetMouseButtonUp(0) && LeftArmValue >= .8f)
        {
            LeftArmValue = 0;
            LeftArm.transform.localPosition = new Vector3(LeftArm.transform.localPosition.x, 2);
            StartCoroutine(CdPunch());
        }

        if (Input.GetMouseButton(0) && canLeftPunch)
        {
            LeftArmValue += punchAcc;
            LeftArm.transform.localPosition = new Vector3(LeftArm.transform.localPosition.x, Mathf.Lerp(1, 0, LeftArmValue));
        }
        else if (canLeftPunch)
        {
            LeftArmValue -= punchAcc * 2;
            LeftArm.transform.localPosition = new Vector3(LeftArm.transform.localPosition.x, Mathf.Lerp(0, 1, LeftArmValue));
        }

        LeftArmValue = Mathf.Clamp01(LeftArmValue);
    }

    IEnumerator CdPunch()
    {
        canLeftPunch = false;
        float aa = 0;
        while (LeftArm.transform.localPosition.y != 0)
        {
            aa += .1f;
            LeftArm.transform.localPosition = new Vector3(LeftArm.transform.localPosition.x, Mathf.Lerp(1.5f, 0, aa));
            //print(LeftArm.transform.localPosition.z);   

            yield return null;
        }
        canLeftPunch = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        print("HELLO");
        Vector2 Dir = col.transform.position - transform.position;
        Debug.DrawRay(transform.position, Dir, Color.red, 9999);
        col.GetComponent<Rigidbody2D>().AddForce(Dir * Force, ForceMode2D.Impulse);
    }
}
