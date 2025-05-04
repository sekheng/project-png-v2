using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace User
{
    public enum Gender
    {
        Male,
        Female,
        //Gender_Other
    }

    public enum Diagnosis
    {
        ASD,
        GDD,
        Others,
    }

    public static class Utility
    {
        public static string dateFormat = "yyyy/MM/dd HH:mm:ss [tt]";

        /// <summary>
        ///  Function to calculate the age based off DOB compared to local DateTime (from computer)
        /// </summary>
        /// <returns> Calculated age value </returns>
        public static int CalculateAge(int dobDate, int dobMonth, int dobYear)
        {
            int age = 0;
            System.DateTime currTime = System.DateTime.Now;
            age = currTime.Year - dobYear;  // the age to-be this year

            /// check whether user would have reached the age to-be for this year
            /// if did not reach the age to-be, subtract 1 from age
            if (currTime.Month < dobMonth)  // has not reached birthday month
            {
                age -= 1;
            }
            else if (currTime.Month == dobMonth)    // is currently birthday month
            {
                // check date
                if (currTime.Day < dobDate) // is before birthday
                    age -= 1;
            }

            return age;
        }

        public static string DoubleQuote(string text)
        {
            return "\"" + text + "\"";
        }

        public static DataTable SearchForUser(string u_name, string u_class)
        {
            // SELECT * FROM users_table WHERE name = "exampleName" AND class = "exampleClass";
            string query = "SELECT * FROM users_table WHERE name = " + DoubleQuote(u_name) + " AND class = " + DoubleQuote(u_class) + ";";
            return DatabaseHandler.instance.GetSqlDB().ExecuteQuery(query);
        }

        /// <summary>
        ///  Query to the database to check if user exists by checking against name and class
        /// </summary>
        /// <returns> True if user exists </returns>
        public static bool DoesUserExist(string u_name, string u_class)
        {
            DataTable searchUsersTable = SearchForUser(u_name, u_class);

            if (searchUsersTable.Rows.Count > 0)    // user already exists
                return true;

            return false;
        }

        /// <summary>
        ///  Query to the database to insert a new user
        /// </summary>
        public static void InsertNewUserQuery(string u_name, string u_class,
            int dob_date, int dob_month, int dob_year,
            Gender gender, Diagnosis diagnosis, string remarks)
        {
            // calculate age
            int age = CalculateAge(dob_date, dob_month, dob_year);

            // convert enums into text
            string u_gender = gender.ToString();
            string u_diagnosis = diagnosis.ToString();
            
            // INSERT INTO users_table(name, class, dob_date, dob_month, dob_year, age, gender, diagnosis, remarks)
            // VALUES("exampleName", "exampleClass", 01, 01, 2000, 18, "Male", "Others", "nil");
            string insertQuery = "INSERT INTO users_table(name, class, dob_date, dob_month, dob_year, age, gender, diagnosis, remarks) " +
                                      "VALUES(" + DoubleQuote(u_name) + ", " + DoubleQuote(u_class) + ", " +    // "VALUES("exampleName", "exampleClass", "
                                      dob_date.ToString() + ", " + dob_month.ToString() + ", " + dob_year.ToString() + ", " + age.ToString() + ", " +   // "01, 01, 2000, 18, "
                                      DoubleQuote(u_gender) + ", " + DoubleQuote(u_diagnosis) + ", " + DoubleQuote(remarks) + ");";  // ""Male", "Others", "nil");"

            Debug.Log(insertQuery);

            DatabaseHandler.instance.GetSqlDB().ExecuteNonQuery(insertQuery);
        }

        /// <summary>
        ///  Query to the database to get task information
        /// </summary>
        public static DataTable GetTaskInformation()
        {
            // SELECT * FROM task_information;
            string query = "SELECT * FROM task_information;";
            DataTable taskInfoTable = DatabaseHandler.instance.GetSqlDB().ExecuteQuery(query);

            return taskInfoTable;
        }

        /// <summary>
        ///  Query to the database to update the user's age
        /// </summary>
        public static void UpdateAge(string u_name, string u_class, int newAge)
        {
            // UPDATE users_table SET age = newAge WHERE name = "exampleName" AND class = "exampleClass";
            string query = "UPDATE users_table SET age = " + newAge.ToString() +    // "UPDATE users_table SET age = 20"
                " WHERE name = " + DoubleQuote(u_name) + " AND class = " + DoubleQuote(u_class) + ";";  // " WHERE name = "exampleName" AND class = "exampleClass";"
            DatabaseHandler.instance.GetSqlDB().ExecuteNonQuery(query);
        }

        /// <summary>
        ///  Query to the database to update the user's age
        /// </summary>
        public static void UpdateClass(string u_name, string u_class, string newClass)
        {
            // UPDATE users_table SET class = "newExampleClass" WHERE name = "exampleName" AND class = "exampleClass";
            string query = "UPDATE users_table SET class = " + DoubleQuote(newClass) +    // "UPDATE users_table SET class = "newExampleClass""
                " WHERE name = " + DoubleQuote(u_name) + " AND class = " + DoubleQuote(u_class) + ";";  // " WHERE name = "exampleName" AND class = "exampleClass";"
            DatabaseHandler.instance.GetSqlDB().ExecuteNonQuery(query);
        }

        /*/// <summary>
        ///  Query to the database to insert a new session record
        /// </summary>
        public static void InsertNewSessionEntry(string taskName, string u_name, string u_class,
            string datetime, string objective, string sustainedAttention, int numCorrect,
            float respTime, int guidanceLvl, int numBumps, bool isGuided, string remarks)
        {
            string taskRef = taskName.Substring(0, 1);

            /// Get total number of questions
            // SELECT * FROM task_information WHERE task_reference = "A";
            string selectQuery = "SELECT * FROM task_information WHERE task_reference = " + DoubleQuote(taskRef) + ";";
            Debug.Log(selectQuery);
            DataTable taskInfoTable = DatabaseHandler.instance.GetSqlDB().ExecuteQuery(selectQuery);

            /// Error check for number of entries
            if (taskInfoTable.Rows.Count == 0)
            {    // no entries
                Debug.Log("Task information not found");
                return;
            }

            /// Get number of questions
            int numQuestions = (int)taskInfoTable.Rows[0]["num_questions"];

            /// Calculate average response time
            float averageResponseTime = respTime / numQuestions;

            /// Calculate accuracy
            int accuracy = (int)(100 * (float)numCorrect / numQuestions);

            /// Convert isGuided to text
            string guided;
            if (isGuided)
                guided = "Yes";
            else
                guided = "No";

            /// Execute insert query
            // INSERT INTO session_table(name, class, datetime, task_attempted, objective, sustained_attention_mins,
            // num_correct, accuracy, average_response_time_secs, guidance_level, num_bumps, was_guided)
            // VALUES("exampleName", "exampleClass", "21/04/2018 10:03:00 AM", "X) TaskName", "Objective", "02:11",
            // 1, 50, 5, 1, 10, "Yes", "Remarks");
            string insertQuery = "INSERT INTO session_table(name, class, datetime, task_attempted, objective, sustained_attention_mins, num_correct, accuracy, average_response_time_secs, guidance_level, num_bumps, was_guided, remarks) " +
                "VALUES(" + DoubleQuote(u_name) + ", " + DoubleQuote(u_class) + ", " +  // "VALUES("exampleName", "exampleClass", "
                DoubleQuote(datetime) + ", " + DoubleQuote(taskName) + ", " + DoubleQuote(objective) + ", " +   // ""21/04/2018 10:03:00 AM", "X) TaskName", "Objective", "
                DoubleQuote(sustainedAttention) + ", " + numCorrect.ToString() + ", " + accuracy.ToString() + ", " +    // ""02:11", 1, 50, "
                averageResponseTime.ToString() + ", " + guidanceLvl.ToString() + ", " + numBumps.ToString() + ", " +    // "5, 1, 10, "
                DoubleQuote(guided) + ", " + DoubleQuote(remarks) + ");";    // ""Yes", "Remarks");"

            Debug.Log(insertQuery);
            DatabaseHandler.instance.GetSqlDB().ExecuteNonQuery(insertQuery);
        }*/

        /// <summary>
        ///  Query to the database to insert a new session record
        /// </summary>
        public static void InsertNewSessionEntry(string effectName, string u_name, string u_class,
            string datetime, //string objective, string sustainedAttention, int numCorrect,
                             //float respTime, int guidanceLvl, int numBumps, bool isGuided, string remarks)
                             string results)
            //int numCorrect, int numQuestions)
        {
            /// Error check for number of entries
            //if (taskInfoTable.Rows.Count == 0)
            //{    // no entries
            //    Debug.Log("Task information not found");
            //    return;
            //}

            /// Calculate accuracy
            //int accuracy = (int)(100 * (float)numCorrect / numQuestions);

            /// Execute insert query
            // INSERT INTO session_table(name, class, datetime, task_attempted, objective, sustained_attention_mins,
            // num_correct, accuracy, average_response_time_secs, guidance_level, num_bumps, was_guided)
            // VALUES("exampleName", "exampleClass", "21/04/2018 10:03:00 AM", "X) TaskName", "Objective", "02:11",
            // 1, 50, 5, 1, 10, "Yes", "Remarks");
            string insertQuery = "INSERT INTO session_table(name, class, datetime, effect, results) " +
                "VALUES(" + DoubleQuote(u_name) + ", " + DoubleQuote(u_class) + ", " +  // "VALUES("exampleName", "exampleClass", "
                DoubleQuote(datetime) + ", " + DoubleQuote(results) + ");";
                //DoubleQuote(effectName) + ", " + numCorrect.ToString() + ", " + accuracy.ToString() + ");";

            Debug.Log(insertQuery);
            DatabaseHandler.instance.GetSqlDB().ExecuteNonQuery(insertQuery);
        }

        /// <summary>
        ///  Query to the database to get calibration settings table
        /// </summary>
        public static DataTable GetCalibrationSettingsTable()
        {
            // SELECT * FROM developer_settings;
            string query = "SELECT * FROM calibration_table;";
            DataTable calibrationTable = DatabaseHandler.instance.GetSqlDB().ExecuteQuery(query);

            return calibrationTable;
        }

        ///// <summary>
        /////  Query to the database to update settings
        ///// </summary>
        //public static void UpdateSettings(string movementSpeed, string rotationSpeed)
        //{
        //    // UPDATE developer_settings SET movement_speed = 10, rotation_speed = 10;
        //    string query = "UPDATE developer_settings SET movement_speed = " + movementSpeed +  // "UPDATE developer_settings SET movement_speed = 10"
        //        ", rotation_speed = " + rotationSpeed + ";";    // ", rotation_speed = 10;"
        //    DatabaseHandler.instance.GetSqlDB().ExecuteNonQuery(query);
        //}

        /// <summary>
        ///  Query to the database to update settings
        /// </summary>
        public static void UpdateCalibrationSettings(CalibrationSettings.CalibrationSetting settingMode, string colorThreshold, string minBlobSize, string maxBlobSize,
            string redBlobLifetime, string crossLifetime, string skipValue, string showCrosses,
            string horizontalFlip, string verticalFlip)
        {
            // UPDATE developer_settings SET color_threshold = 10, min_blob_size = 100, max_blob_size = 1000, red_blob_lifetime = 15, cross_lifetime = 15,
            // skip_value = 15, show_crosses = 1, horizontal_flip = 0, vertical_flip = 0
            // WHERE setting = "settingA"; // WHERE setting = "settingB";
            string setting = (settingMode == CalibrationSettings.CalibrationSetting.SETTING_A) ? "settingA" : "settingB";

            string query = "UPDATE calibration_table SET color_threshold = " + colorThreshold + ", min_blob_size = " + minBlobSize + ", max_blob_size = " + maxBlobSize +
                ", red_blob_lifetime = " + redBlobLifetime + ", cross_lifetime = " + crossLifetime + ", skip_value = " + skipValue + ", show_crosses = " + showCrosses +
                ", horizontal_flip = " + horizontalFlip + ", vertical_flip = " + verticalFlip
                + " WHERE setting = \"" + setting + "\";";
            DatabaseHandler.instance.GetSqlDB().ExecuteNonQuery(query);
        }

    }

}
