using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] KeyCode[] MoveKeys;
    [SerializeField] bool[] Moves;
    [SerializeField] Vector2[] Directions;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] GameObject Ball, OtherPlayer, GCGO, Area;
    [SerializeField] GameController GC;
    [SerializeField] float ballDist, playerDist, speedMultiplier, ballSpeed, PlayerKick;
    [SerializeField] SpriteRenderer areaSR;
    [SerializeField] Color areaColor;
    [SerializeField] AudioSource Kick;
    // Start is called before the first frame update
    void Start()
    {
        areaColor = areaSR.color;
        GC = GCGO.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        //areaSR.color = areaColor;
        Area.transform.localScale = new Vector3(GC.PushRadius * 2, GC.PushRadius * 2, GC.PushRadius * 2);
        ballDist = Mathf.Sqrt(((this.transform.position.x - Ball.transform.position.x) * (this.transform.position.x - Ball.transform.position.x)) + ((this.transform.position.y - Ball.transform.position.y) * (this.transform.position.y - Ball.transform.position.y)));
        playerDist = Mathf.Sqrt(((this.transform.position.x - OtherPlayer.transform.position.x) * (this.transform.position.x - OtherPlayer.transform.position.x)) + ((this.transform.position.y - OtherPlayer.transform.position.y) * (this.transform.position.y - OtherPlayer.transform.position.y)));
        for (int i = 0; i < 5; i++) {
            if (Input.GetKeyDown(MoveKeys[i]))
            {
                Moves[i] = true;
                if (i == 4) {
                    areaSR.color = Color.white;
                    speedMultiplier = 125;
                }
            }
            if (Input.GetKeyUp(MoveKeys[i]))
            {
                Moves[i] = false;
                if (i == 4)
                {
                    areaSR.color = areaColor;
                    speedMultiplier = 250;
                }
            }
        }
    }
    void FixedUpdate() {
        for (int i = 0; i < 5; i++)
        {
            if (Moves[i]) {
                rb2d.AddForce(Directions[i] * Time.fixedDeltaTime * speedMultiplier * GC.GameSpeed);
            }
        }
        if (Moves[4] && ballDist < Area.transform.localScale.x / 2) {
            Vector3 ballDirection = Ball.transform.position - this.transform.position;
            Rigidbody2D rb2dBall = Ball.GetComponent<Rigidbody2D>();
            rb2dBall.AddForce(ballDirection * Time.fixedDeltaTime * ballSpeed * GC.GameSpeed);
            Kick.Play();
        }
        if (Moves[4] && playerDist < Area.transform.localScale.x && GC.Kickable)
        {
            Vector3 ballDirection = OtherPlayer.transform.position - this.transform.position;
            Rigidbody2D rb2dBall = OtherPlayer.GetComponent<Rigidbody2D>();
            rb2dBall.AddForce(ballDirection * Time.fixedDeltaTime * PlayerKick * GC.GameSpeed);
        }
    }
}