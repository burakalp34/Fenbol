using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPostCode : MonoBehaviour
{
    public GameController GC;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D cls)
    {
        if (cls.gameObject.name == "Ball" && !GC.GameOver) {
            GC.MissedShotAudio.Play();
        }
    }
}