﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewPlayerController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector3 _initialPosition;
    private Vector3 leftinfo = new Vector3(140, 0, 0);
    private Vector3 Rightinfo = new Vector3(-140, 0, 0);
    private Vector3 _initialRotation;
    private float _distanceMoved;
    private bool _swipeLeft;
    float duration = 0.5f; // Adjust the duration as needed for the desired smoothness.
    private float elapsedTime = 0f;
    private StateCard currenState = StateCard.None;
    private float time = 0;
    
    public string TitleCard;
    public string ParagraphText;
    public TextMeshProUGUI titleCard;
    public TextMeshProUGUI paragraphText;
    void Start()
    {
        titleCard.text = TitleCard;
        paragraphText.text = ParagraphText;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        currenState = StateCard.None;
            transform.localPosition =
                new Vector2(transform.localPosition.x + eventData.delta.x, transform.localPosition.y);
            if (transform.localPosition.x - _initialPosition.x > 0)
            {
                transform.localEulerAngles = new Vector3(0, 0,
                    Mathf.LerpAngle(0, -10, (_initialPosition.x + transform.localPosition.x) / (Screen.width / 2)));
            }
            else
            {
                transform.localEulerAngles = new Vector3(0, 0,
                    Mathf.LerpAngle(0, 10, (_initialPosition.x - transform.localPosition.x) / (Screen.width / 2)));
            }
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _initialPosition = transform.localPosition;
        _initialRotation = transform.localEulerAngles;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _distanceMoved = Mathf.Abs(transform.localPosition.x - _initialPosition.x);
        if (_distanceMoved < 0.4 * Screen.width)
        {
            elapsedTime = 0;
            currenState = StateCard.ReturnCard;
        }
        else
        {
            if (transform.localPosition.x > _initialPosition.x)
            {
                _swipeLeft = false;
            }
            else
            {
                _swipeLeft = true;
            }

            currenState = StateCard.CardMove;
        }
    }

    private IEnumerator MovedCard()
    {
        float time = 0;
        while (time < 0.1f)
        {
            time += Time.deltaTime;

            if (_swipeLeft)
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x - Screen.width, time), transform.localPosition.y, 0);
                GetComponent<Image>().color = Color.Lerp(Color.white, Color.red, Mathf.SmoothStep(1, 0, 4 * time));
            }
            else
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x + Screen.width, time), transform.localPosition.y, 0);
                GetComponent<Image>().color = Color.Lerp(Color.white, Color.green, Mathf.SmoothStep(1, 0, 4 * time));
            }

            //  GetComponent<Image>().color = new Color(1, 1, 1, Mathf.SmoothStep(1, 0, 4 * time));
            yield return null;
        }

        Destroy(gameObject);
    }

    private void MoveCard()
    {
     
        if (time < 0.2f)
        {
            time += Time.deltaTime;

            if (_swipeLeft)
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x - Screen.width, time), transform.localPosition.y, 0);
                GetComponent<Image>().color = Color.Lerp(Color.white, Color.red, Mathf.SmoothStep(1, 0, 4 * time));
            }
            else
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x + Screen.width, time), transform.localPosition.y, 0);
                GetComponent<Image>().color = Color.Lerp(Color.white, Color.green, Mathf.SmoothStep(1, 0, 4 * time));
            }

            //  GetComponent<Image>().color = new Color(1, 1, 1, Mathf.SmoothStep(1, 0, 4 * time));

        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void ReturnCardToPosition()
    {
        Quaternion initialRotation = Quaternion.Euler(new Vector3(0, 0, 0)); // Set the initial rotation to zero degrees around the z-axis

        if (elapsedTime < duration)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, new Vector3(0, 0, 0), elapsedTime / duration);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, initialRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
        }
        else
        {
            transform.localPosition = new Vector3(0, 0, 0);
            transform.localRotation = initialRotation;
            currenState = StateCard.None;
        }
    }


    private IEnumerator ReturnToInitialPosition()
    {
        float duration = 0.2f; // Adjust the duration as needed for the desired smoothness.
        float elapsedTime = 0f;

        Quaternion initialRotation = Quaternion.Euler(_initialRotation);

        while (elapsedTime < duration)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, _initialPosition, elapsedTime / duration);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, initialRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the card ends up exactly at the initial position and rotation.
        transform.localPosition = new Vector3(0,0,0);
        transform.localRotation = initialRotation;
    }

    private void Update()
    {
        if (transform.localPosition.x > Rightinfo.x && transform.localPosition.x > leftinfo.x)
        {
            Debug.LogWarning("Show info Right");
            GetComponent<Image>().color = Color.Lerp(GetComponent<Image>().color, Color.gray, Time.deltaTime);
        }
        else if (transform.localPosition.x < leftinfo.x && transform.localPosition.x < Rightinfo.x)
        {
            Debug.LogWarning("Show info Left");
            GetComponent<Image>().color = Color.Lerp(GetComponent<Image>().color, Color.gray, Time.deltaTime);
        }
        else
        {
            GetComponent<Image>().color = Color.Lerp(GetComponent<Image>().color, Color.white, Time.deltaTime);
        }
        Debug.LogWarning(currenState.ToString());
        switch (currenState)
        {
            case StateCard.None:
                
                break;
            case StateCard.CardMove:
                MoveCard();
                break;
            case StateCard.ReturnCard:
                ReturnCardToPosition();
                break;
        }
    }
    private enum StateCard
    {
        None,
        CardMove,
        ReturnCard
    }
}