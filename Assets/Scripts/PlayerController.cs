using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    public float speed;
    private int score;
    public Text scoreText;
    public Text winText;
    public Text gameOverText;
    private bool isOnGround = true;
    private bool jumpQueued = false;
    public float jumpHeight = 5.0f;
    private Vector3 startPostion;
    private int pointsToWin;    
    private GameObject[] capsules;
    private GameObject[] cubes;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        score = 0;
        SetCountText();
        InitializeText();
        startPostion = rb.position;
        GetSceneObjects();
        pointsToWin = capsules.Length * 2 + cubes.Length;
        Debug.Log(pointsToWin);

    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
     

        Vector3 movement = new Vector3(moveHorizontal, 0.0f , moveVertical);
        rb.AddForce(movement * speed);

        if (jumpQueued)
        {
            rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
            isOnGround = false;
            jumpQueued = false;
        }     
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && isOnGround) 
            jumpQueued = true;
        if (rb.position.y < -5)
            gameOverText.text = "Game Over";   
        if(rb.position.y < -20)
        {
            Reset();
            gameOverText.text = "";
        }
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            score++;
            SetCountText();
        }
        if (other.gameObject.CompareTag("Pick Up Capsule"))
        {
            other.gameObject.SetActive(false);
            score+= 2;
            SetCountText();
        }
        if (other.gameObject.CompareTag("FinishLine") )
        {
            if (score >= pointsToWin)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
                winText.text = "You Win";
            }
            else
            {
                Freeze();
                winText.text = "You need " + (pointsToWin - score) + " more points.";
            }
        }
        
    }


    private void OnTriggerExit(Collider other) { 
    if (other.gameObject.CompareTag("FinishLine"))                 
         winText.text = "";            
    }

  
        

     void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Is on ground");
            isOnGround = true;
            Debug.Log(isOnGround);
        }
    }

     void OnCollisionExit(Collision other)
    {
      
    }
    void InitializeText()
    {
        winText.text = "";
        gameOverText.text = "";

    }
    void SetCountText()
    {
        scoreText.text = "Score: " + score.ToString();        
    }
    private void Reset()
    {
        ResetPosition();
        ReactivateObjects();
        score = 0;
        InitializeText();
    }

    void ResetPosition()
    {
        rb.position = startPostion;
        Freeze();
    }

    void Freeze()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void GetSceneObjects()
    {
      //  Debug.Log("Get scene objects");
        capsules = GameObject.FindGameObjectsWithTag("Pick Up Capsule");
        //Debug.Log("Capsules " + capsules.Length);
        cubes = GameObject.FindGameObjectsWithTag("Pick Up");
        //Debug.Log("Cubes " + cubes.Length);
    }

    void ReactivateObjects()
    {
        foreach (var capsule in capsules)
        {
         //   Debug.Log(capsule.name );
            capsule.SetActive(true);
        }
        foreach (var cube in cubes)
            cube.SetActive(true);
    }

  
}
