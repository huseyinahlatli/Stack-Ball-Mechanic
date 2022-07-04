using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallControl : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float horizontalLimit;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private TMP_Text ballCountText;
    [SerializeField] private List<GameObject> ballList = new List<GameObject>();
    private float movement;
    private int gateNumber;
    private int targetCount;

    void Update()
    {
        HorizontalBallMovement();    
        ForwardBallMovement();
        UpdateBallCountText(); 
    }

    private void HorizontalBallMovement()
    {
        float newMovement;
        if(Input.GetMouseButton(0))
            movement = Input.GetAxisRaw("Mouse X");
        else
            movement = 0;
            
        newMovement = transform.position.x + movement * horizontalSpeed * Time.deltaTime;
        newMovement = Mathf.Clamp(newMovement, -horizontalLimit, horizontalLimit);
        transform.position = new Vector3(newMovement, transform.position.y, transform.position.z);
    }

    private void ForwardBallMovement()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
    }

    private void UpdateBallCountText()
    {
        ballCountText.text = ballList.Count.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("StackBall"))
        {
            other.gameObject.transform.SetParent(transform);
            other.gameObject.GetComponent<SphereCollider>().enabled = false;
            other.gameObject.transform.localPosition = new Vector3(0f, 0f, ballList[ballList.Count - 1].transform.localPosition.z - 1f);
            ballList.Add(other.gameObject);
        }

        if(other.gameObject.CompareTag("Gate"))
        {
            gateNumber = other.gameObject.GetComponent<GateController>().GetGateNumber();
            targetCount = ballList.Count + gateNumber;
            if(gateNumber > 0)
                IncreaseBallCount();
            else if(gateNumber < 0)
                DecreaseBallCount();
        }
    }

    private void IncreaseBallCount()
    {
        for(int i=0; i<gateNumber; i++)
        {
            GameObject newBall = Instantiate(ballPrefab);
            newBall.transform.SetParent(transform);
            newBall.GetComponent<SphereCollider>().enabled = false;
            newBall.transform.localPosition = new Vector3(0f, 0f, ballList[ballList.Count - 1].transform.localPosition.z - 1f);
            ballList.Add(newBall);
            newBall.SetActive(true);
        }
    }

    private void DecreaseBallCount()
    {
        if(targetCount > 0)
        {
            for(int x=ballList.Count-1; x>=targetCount; x--)
            {
                Destroy(ballList[x]);
                ballList.RemoveAt(x);
            }
        }
        else
        {   
            for(int y=ballList.Count-1; y>0; y--)
            {
                Destroy(ballList[y]);
                ballList.RemoveAt(y);
            }
        }
    }
}
