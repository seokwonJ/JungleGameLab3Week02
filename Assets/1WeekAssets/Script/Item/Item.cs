using UnityEngine;

public class Item : MonoBehaviour
{ 
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,60f * Time.deltaTime);
    }
}
