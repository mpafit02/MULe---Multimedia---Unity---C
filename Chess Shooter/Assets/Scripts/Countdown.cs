using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(AudioSource))]
public class Countdown : MonoBehaviour
{
    public Texture[] textures;
    int count = 0;
    public AudioSource audioData;


    private float _currentScale = InitScale;
    private const float TargetScale = 4f;
    private const float InitScale = 1f;
    private const int FramesCount = 25;
    private const float AnimationTimeSeconds = 0.25f;
    private float _deltaTime = AnimationTimeSeconds / FramesCount;
    private float _dx = (TargetScale - InitScale) / FramesCount;
    private bool _upScale = true;


    private IEnumerator Breath()
    {
        while (count < 4)
        {
            while (_upScale)
            {
                _currentScale += _dx;
                if (_currentScale > TargetScale)
                {
                    _upScale = false;
                    _currentScale = TargetScale;
                }
                transform.localScale = Vector3.one * _currentScale;
                yield return new WaitForSeconds(_deltaTime);
            }

            while (!_upScale)
            {
                _currentScale -= _dx;
                if (_currentScale < InitScale)
                {
                    _upScale = true;
                    _currentScale = InitScale;
                }
                transform.localScale = Vector3.one * _currentScale;
                yield return new WaitForSeconds(_deltaTime);
            }

            count++;
            if (count < 4)
            {
                GetComponent<RawImage>().texture = textures[count];
            }

        }

        if (count >= 4)
        {
            GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>().enableSpawner();
            gameObject.active = false;
        }
    }



    public void activate()
    {
        count = 0;
        gameObject.active = true;
        _upScale = true;
        audioData = GetComponent<AudioSource>();
        audioData.Play(0);
        GetComponent<RawImage>().texture = textures[count];
        StartCoroutine(Breath());
    }



}
