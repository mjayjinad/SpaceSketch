using UnityEngine;
using TMPro;
using System;

public class TimeDisplay : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dayText;

    private void Update()
    {
        // Get the current time
        DateTime currentTime = DateTime.Now;

        // Format the time as a string with hours, minutes, and AM/PM
        string formattedTime = currentTime.ToString("hh:mm tt");

        // Format the day of the week as a string
        string formattedDay = currentTime.ToString("ddd").ToUpper(); // To get TUE for Tuesday

        // Update the TextMeshPro UI elements with the formatted time and day
        timeText.text = formattedTime;
        dayText.text = formattedDay;
    }
}