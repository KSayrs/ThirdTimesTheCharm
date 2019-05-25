using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SlideButton : MonoBehaviour
{
    [SerializeField] Canvas Canvas = null;
    public Button ContinueButton;
    RectTransform t;
    bool justActivated = true;

    [SerializeField] List<Button> ExteriorButtons = new List<Button>();
    [SerializeField] List<Button> RoomButtons = new List<Button>();
    [SerializeField] List<Button> ExtrasButtons = new List<Button>();

    enum Position
    {
        Exterior,
        Room,
        Extras
    }

    private Position position;
    private List<Button> HiddenButtons = new List<Button>();

    private void Start()
    {
        Canvas.enabled = false;
        t = GetComponent<RectTransform>();
        position = Position.Room;

        DoMatches();
        SetRoomButtonVisibility(false, false, false);
        ContinueButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Room") && justActivated)
        {
            justActivated = false;
            Canvas.enabled = true;
            DoMatches();
            SetActiveCurrentSceneButtons(true);
            ContinueButton.gameObject.SetActive(true);
            SetButtonsBasedOnPosition();
        }

        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("KatScene") && !justActivated)
        {
            justActivated = true;
            Canvas.enabled = false;
        }
    }

    public void SlideLeft()
    {
        t.localPosition = new Vector3(t.localPosition.x + 1920f, t.localPosition.y, t.localPosition.z);
        position--;
        SetButtonsBasedOnPosition();
    }

    public void SlideRight()
    {
        t.localPosition = new Vector3(t.localPosition.x - 1920f, t.localPosition.y, t.localPosition.z);
        position++;
        SetButtonsBasedOnPosition();
    }

    public void ContinueShopping()
    {
        Canvas.enabled = false;
        SetActiveCurrentSceneButtons(false);
        ContinueButton.gameObject.SetActive(false);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("KatScene"));
    }

    void DoMatches()
    {
        var categoryAssigners = FindObjectsOfType<CategoryAssigner>();
        foreach (var assigner in categoryAssigners)
        {
            assigner.SetMatches();
        }
    }

    void SetButtonsBasedOnPosition()
    {
        if (position == Position.Exterior)
        {
            SetRoomButtonVisibility(false, false, true);
        }
        if (position == Position.Room)
        {
            SetRoomButtonVisibility(true, false, false);
        }
        if (position == Position.Extras)
        {
            SetRoomButtonVisibility(false, true, false);
        }
    }

    void SetRoomButtonVisibility(bool room, bool extras, bool exterior)
    {
        foreach (var button in RoomButtons)
        {
            button.gameObject.SetActive(room);
        }
        foreach (var button in ExtrasButtons)
        {
            button.gameObject.SetActive(extras);
        }
        foreach (var button in ExteriorButtons)
        {
            button.gameObject.SetActive(exterior);
        }
    }

    void SetActiveCurrentSceneButtons(bool shouldbeactive)
    {
        var buttons = FindObjectsOfType<Button>();
        foreach (var button in buttons)
        {
            if (ExteriorButtons.Contains(button) || RoomButtons.Contains(button) || ExtrasButtons.Contains(button) || button == ContinueButton)
            {
                Debug.Log("Found matching button: " + button.gameObject.name);
                button.gameObject.SetActive(shouldbeactive);
            }
            else  if (shouldbeactive)
            {
                HiddenButtons.Add(button);
                button.gameObject.SetActive(!shouldbeactive);
            }
        }

        if (shouldbeactive == false)
        {
            foreach (var hiddenButton in HiddenButtons)
            {
                hiddenButton.gameObject.SetActive(!shouldbeactive);
            }
        }

   //     EventSystem.current.UpdateModules();
    }
}
