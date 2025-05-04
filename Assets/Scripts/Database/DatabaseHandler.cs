using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseHandler : MonoBehaviour {

    public static DatabaseHandler instance = null;

    [SerializeField]
    private string databaseName = "userdatabase";
    private SqliteDatabase sqlDB = null;

    public SqliteDatabase GetSqlDB()
    {
        return sqlDB;
    }

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);  // destroy this component as one already exists

        sqlDB = new SqliteDatabase(databaseName + ".db");
    }

}
