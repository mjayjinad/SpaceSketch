using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.UIElements;

public class VideoControl : MonoBehaviour
{
    // The hand that swipes over the hologram
    public GameObject rightHand;

    // The object that displays the video
    public Transform videoDisplay;

    // The video player component
    public VideoPlayer tvVideoPlayer;

    // Array of VideoClips to be played
    public VideoClip[] videoClips;

    // Index to keep track of the current video playing
    private int currentVideoIndex = 0;

    // Duration of the swipe animation in seconds
    public float swipeDuration = 0.5f;

    // Offset for the swipe-out effect, determining how far the video moves
    public Vector3 offScreenOffset = new Vector3(130f, 0f, 0f);

    private void OnTriggerEnter(Collider other)
    {
        // Trigger the video swipe when the specified hand enters the collider
        if (other.gameObject == rightHand)
        {
            StartCoroutine(SwipeAndPlayNextVideo());
        }
    }

    private IEnumerator SwipeAndPlayNextVideo()
    {
        // Perform the swipe-out animation
        yield return StartCoroutine(SwipeOut());

        // Update the video clip to the next in the array and reset its position
        currentVideoIndex = (currentVideoIndex + 1) % videoClips.Length;
        tvVideoPlayer.clip = videoClips[currentVideoIndex];
        tvVideoPlayer.Play();
        tvVideoPlayer.transform.localPosition = Vector3.zero;

        // Perform the swipe-in animation to bring in the new video
        yield return StartCoroutine(SwipeIn());
    }

    private IEnumerator SwipeOut()
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

    private IEnumerator SwipeIn()
    {
        // Animate the swipe-in effect over the specified duration
        float elapsedTime = 0;
        Vector3 startPosition = tvVideoPlayer.transform.localPosition - offScreenOffset;
        Vector3 endPosition = Vector3.zero;

        while (elapsedTime < swipeDuration)
        {
            tvVideoPlayer.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / swipeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tvVideoPlayer.transform.localPosition = endPosition;
    }
}