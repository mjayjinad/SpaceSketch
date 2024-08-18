using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialControls : MonoBehaviour
{
    // The object that displays the video
    public Transform videoDisplay;

    // The object that displays the video
    public Button nextBtn, PreviousBtn;

    // The video player component
    public VideoPlayer tvVideoPlayer;
    
    // The video player name text
    public TMP_Text videoTxt;

    // Duration of the swipe animation in seconds
    public float swipeDuration = 0.5f;

    // Offset for the swipe-out effect, determining how far the video moves
    public Vector3 offScreenOffset = new Vector3(130f, 0f, 0f);

    // Array of VideoClips to be played
    public VideoClip[] videoClips;

    // Array of video clip name
    public string[] videoStrings;

    // Initial position of the video player
    private Vector3 initalPosition;

    // Index to keep track of the current video playing
    private int currentVideoIndex = 0;

    private void Start()
    {
        nextBtn.onClick.AddListener(() => PlayNextVideo());
        PreviousBtn.onClick.AddListener(() => PlayPreviousVideo());

        initalPosition = tvVideoPlayer.transform.localPosition;
    }

    public void PlayNextVideo()
    {
        if(currentVideoIndex < videoClips.Length-1)
        {
            currentVideoIndex++;
            StartCoroutine(SwipeAndPlayNextVideo());
        }
    }
    
    public void PlayPreviousVideo()
    {
        if(currentVideoIndex > 0)
        {
            currentVideoIndex--;
            StartCoroutine(SwipeAndPlayPreviousVideo());
        }
    }

    private IEnumerator SwipeAndPlayNextVideo()
    {
        // Perform the swipe-out animation
        yield return StartCoroutine(NextVideoSwipeOut());

        // Update the video clip to the next in the array and reset its position
        tvVideoPlayer.clip = videoClips[currentVideoIndex];
        tvVideoPlayer.Play();
        //tvVideoPlayer.transform.localPosition = Vector3.zero;
        tvVideoPlayer.transform.localPosition = initalPosition;

        //Display the video text
        videoTxt.text = videoStrings[currentVideoIndex];

        // Perform the swipe-in animation to bring in the new video
        yield return StartCoroutine(NextVideoSwipeIn());
    }
    
    private IEnumerator SwipeAndPlayPreviousVideo()
    {
        // Perform the swipe-out animation
        yield return StartCoroutine(PreviousVideoSwipeOut());

        // Update the video clip to the next in the array and reset its position
        tvVideoPlayer.clip = videoClips[currentVideoIndex];
        tvVideoPlayer.Play();
        //tvVideoPlayer.transform.localPosition = Vector3.zero;
        tvVideoPlayer.transform.localPosition = initalPosition;

        //Display the video text
        videoTxt.text = videoStrings[currentVideoIndex];

        // Perform the swipe-in animation to bring in the new video
        yield return StartCoroutine(PreviousVideoSwipeIn());
    }

    private IEnumerator NextVideoSwipeOut()
    {
        // Animate the swipe-out effect over the specified duration
        float elapsedTime = 0;
        Vector3 startPosition = videoDisplay.localPosition;
        Vector3 endPosition = startPosition + offScreenOffset;

        while (elapsedTime < swipeDuration)
        {
            tvVideoPlayer.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / swipeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tvVideoPlayer.transform.localPosition = endPosition;
    }

    private IEnumerator NextVideoSwipeIn()
    {
        // Animate the swipe-in effect over the specified duration
        float elapsedTime = 0;
        Vector3 startPosition = tvVideoPlayer.transform.localPosition - offScreenOffset;
        //Vector3 endPosition = Vector3.zero;
        Vector3 endPosition = initalPosition;

        while (elapsedTime < swipeDuration)
        {
            tvVideoPlayer.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / swipeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tvVideoPlayer.transform.localPosition = endPosition;
    }

    private IEnumerator PreviousVideoSwipeOut()
    {
        // Animate the swipe-out effect over the specified duration
        float elapsedTime = 0;
        Vector3 startPosition = videoDisplay.localPosition;
        Vector3 endPosition = startPosition - offScreenOffset;

        while (elapsedTime < swipeDuration)
        {
            tvVideoPlayer.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / swipeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tvVideoPlayer.transform.localPosition = endPosition;
    }

    private IEnumerator PreviousVideoSwipeIn()
    {
        // Animate the swipe-in effect over the specified duration
        float elapsedTime = 0;
        Vector3 startPosition = tvVideoPlayer.transform.localPosition + offScreenOffset;
        //Vector3 endPosition = Vector3.zero;
        Vector3 endPosition = initalPosition;

        while (elapsedTime < swipeDuration)
        {
            tvVideoPlayer.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / swipeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tvVideoPlayer.transform.localPosition = endPosition;
    }
}
