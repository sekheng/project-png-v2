using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Class to handle the logged in user
public class UserHandler : MonoBehaviour {

    public static UserHandler instance = null;

    private string user_name = "";
    private string user_class = "";

    private User.Gender user_gender = User.Gender.Male; // default

    // Settings
    //private DeveloperSettings developerSettings;
    private CalibrationSettings calibrationSettings;    // calibration settings table in database
    //private CalibrationSettings defaultCalibrationSettings; // default calibration settings

    [SerializeField]
    [Tooltip("Column in settings table in database")]
    private string[] settingsTableColumns;

    // Use this for initialization
    void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);  // destroy this component as one already exists


        // get reference to settings table from database
        //developerSettings = new DeveloperSettings(User.Utility.GetSettingsTable(), settingsTableColumns);
        UpdateCalibrationSettings();
    }

    /// <summary>
    ///  Updates the CalibrationSettings struct object to store data from the database
    /// </summary>
    public void UpdateCalibrationSettings()
    {
        calibrationSettings = new CalibrationSettings(User.Utility.GetCalibrationSettingsTable(), settingsTableColumns);
    }

    public string GetUserName()
    {
        return user_name;
    }

    public string GetUserClass()
    {
        return user_class;
    }

    public User.Gender GetUserGender()
    {
        return user_gender;
    }

    //public DeveloperSettings GetSettings()
    //{
    //    return developerSettings;
    //}
    
    public CalibrationSettings GetCalibrationSettings()
    {
        return calibrationSettings;
    }

    public void SetUser(string newName, string newClass)
    {
        user_name = newName;
        user_class = newClass;

        //SetUserGender();
    }
    
    private void SetUserGender()
    {
        DataTable userTable = User.Utility.SearchForUser(user_name, user_class);

        // error check, but it should not fail
        if (userTable.Rows.Count == 0)    // user does not exist
        {
            Debug.Log("User does not exist.");
            return;
        }

        User.Gender gender = (User.Gender)System.Enum.Parse(typeof(User.Gender), userTable.Rows[0]["gender"].ToString());

        user_gender = gender;

    }

    public void CheckUserAge()
    {
        DataTable userTable = User.Utility.SearchForUser(user_name, user_class);

        // error check, but it should not fail
        if (userTable.Rows.Count == 0)    // user does not exist
        {
            Debug.Log("User does not exist.");
            return;
        }

        int recordedAge = (int)userTable.Rows[0]["age"];

        int dob_date = (int)userTable.Rows[0]["dob_date"];
        int dob_month = (int)userTable.Rows[0]["dob_month"];
        int dob_year = (int)userTable.Rows[0]["dob_year"];
        int calculatedAge = User.Utility.CalculateAge(dob_date, dob_month, dob_year);

        if (recordedAge != calculatedAge)
        {
            // UPDATE data in database
            User.Utility.UpdateAge(user_name, user_class, calculatedAge);
            Debug.Log("User's age updated to " + calculatedAge);
        }
    }

}