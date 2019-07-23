using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverButton : MonoBehaviour
{
    public void PointerEnter()
    {
        //Debug.Log("enter1");
        transform.localScale += new Vector3(0.1F, 0.1f, 0.1f); //adjust these values as you see fit
    }

    public void PointerExit()
    {
        //Debug.Log("left1");
        transform.localScale = new Vector3(1, 1, 1);  // assuming you want it to return to its original size when your mouse leaves it.
    }


}