using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Method for identifying a trapezium point
 */
public struct TrapeziumPointId
{
    public int i;
    public int j;

    public TrapeziumPointId(int i, int j)
    {
        this.i = i;
        this.j = j;
    }

    public override bool Equals(object obj)
    {
        TrapeziumPointId other = (TrapeziumPointId)obj;
        return i == other.i && j == other.j;
    }

    public override int GetHashCode()
    {
        return i * 100 + j;  // hashcode = i0j
    }

    public override string ToString()
    {
        return string.Format("{0},{1}", i, j);
    }
}

public class TrapeziumPoint
{
    public int x { get; private set; }
    public int y { get; private set; }

    private List<Trapezium> connectedTrapeziums;

    public TrapeziumPoint()
    {
        connectedTrapeziums = new List<Trapezium>();
    }

    public Vector3 Pos
    {
        get
        {
            return new Vector3(x, y, 0);
        }
    }

    public void setPos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void AddConnectedTrapezium(Trapezium trapezium)
    {
        connectedTrapeziums.Add(trapezium);
    }

    public void UpdateConnectedTrapeziums()
    {
        // TO-DO
        foreach (Trapezium trapezium in connectedTrapeziums)
        {
            trapezium.UpdateTrapeziumNormals();
            trapezium.UpdateMinMaxWidthHeight();
        }
    }

    public override string ToString()
    {
        return string.Format("{0},{1}", x, y);
    }

    public static TrapeziumPoint Parse(string str)
    {
        string[] coord = str.Split(',');
        TrapeziumPoint point = new TrapeziumPoint();
        //Debug.Log(coord[0]);
        point.x = int.Parse(coord[0]);
        point.y = int.Parse(coord[1]);

        return point;
    }

    public static void SaveToPlayerPrefs(TrapeziumPoint point, TrapeziumPointId id)
    {
        // get setting mode
        char settingMode = (AppManager.instance.SettingMode == CalibrationSettings.CalibrationSetting.SETTING_A) ? 'A' : 'B';
        // save to trapeziums data store
        TrapeziumsDataStore.SetPointData(AppManager.instance.SettingMode, id.i, id.j, point.ToString());
    }

}
