using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    //can't user getter setter here or wouldn't be able to modify in unity inspector
    [SerializeField] GameObject health;

    public void SetHP(float hpNormalized)
    {
        //set health to scale ratio 1f, decimal 0.0 to 1.0 for size of health image to give the illusion of health being drained
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }

    public IEnumerator SetHPSmooth(float newHP)
    {
        float currHP = health.transform.localScale.x;
        float changeAmt = currHP - newHP;

        while(currHP - newHP > Mathf.Epsilon)
        {
            currHP -= changeAmt * Time.deltaTime;
            health.transform.localScale = new Vector3(currHP, 1f);
            yield return null;
        }
        health.transform.localScale = new Vector3(newHP, 1f);
    }
}
