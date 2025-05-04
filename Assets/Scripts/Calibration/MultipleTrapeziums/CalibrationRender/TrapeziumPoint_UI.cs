using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapeziumPoint_UI : MonoBehaviour
{
    // reference to trapezium point model data
    private TrapeziumPoint trapeziumPointModel;

    // reference to Trapeziums_UI (shared/overall trapezium UI data)
    private Trapeziums_UI trapeziumsUI;

    // store id
    private TrapeziumPointId id;

    // store connected lines (if any)
    private List<TrapeziumLine_UI> trapeziumLines;

    public void SetTrapeziumPointModel(TrapeziumPoint model)
    {
        trapeziumPointModel = model;
    }

    public TrapeziumPoint GetTrapeziumPointModel()
    {
        return trapeziumPointModel;
    }

    public void SetTrapeziumsUI(Trapeziums_UI ui)
    {
        trapeziumsUI = ui;
    }

    public void SetId(TrapeziumPointId id)
    {
        this.id = id;
    }

    public void AddTrapeziumLine(TrapeziumLine_UI line)
    {
        trapeziumLines.Add(line);
    }

    // Start is called before the first frame update
    void Awake()
    {
        trapeziumLines = new List<TrapeziumLine_UI>();
    }

    public void DragPoint()
    {
        // get mouse position
        Vector2 mousePos = Input.mousePosition;
        // clamp within space
        float xPos = Mathf.Clamp(mousePos.x, 0, trapeziumsUI.RenderWidth);
        float yPos = Mathf.Clamp(mousePos.y, trapeziumsUI.RenderHeight, 0);
        // update position to mouse position
        transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
        // clamp updated position
        transform.localPosition = trapeziumsUI.ClampTrapeziumPoint(id);

        // convert position to webcam resolution
        int webcamX = (int)((trapeziumsUI.RenderWidth - transform.localPosition.x) / trapeziumsUI.RenderWidthScale);
        int webcamY = (int)(transform.localPosition.y / trapeziumsUI.RenderHeightScale);

        // update position of trapezium point model data
        trapeziumPointModel.setPos(webcamX, webcamY);

        // update connected trapeziums and lines for drawing the trapeziums
        UpdateTrapeziums();
    }

    public void UpdateTrapeziums()
    {
        // update connected trapeziums
        trapeziumPointModel.UpdateConnectedTrapeziums();

        // update connected lines
        foreach (TrapeziumLine_UI line in trapeziumLines)
            line.UpdateLine();
    }

    public void EndDragEvent()
    {
        // Automatically save updated position to PlayerPrefs, as the Trapezium Point Manager gets updated and not get reverted
        // even if Save button was not pressed
        TrapeziumPoint.SaveToPlayerPrefs(trapeziumPointModel, id);
    }
}
