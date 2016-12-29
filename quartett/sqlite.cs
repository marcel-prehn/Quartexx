using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace quartett
{
    public static class sqlite
    {
        public static SQLiteConnection startConnection()
        {
            try
            {
                SQLiteConnection sqlConnection = new SQLiteConnection();
                sqlConnection.ConnectionString = "data source=" + Application.StartupPath + @"\quartett.db";
                sqlConnection.Open();
                return sqlConnection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }
        }

        public static void killConnection(SQLiteConnection SqlConnection)
        {
            try
            {
                SqlConnection.Close();
                SqlConnection.Dispose();
            }
            catch (SQLiteException sqlex)
            {
                MessageBox.Show(sqlex.InnerException.ToString());
            }
        }

        public static DataTable getCard(int Id)
        {
            
            SQLiteConnection connection = startConnection();
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT * FROM porn WHERE id = " + Id + ";";
            DataTable sqlData = new DataTable();
            SQLiteDataReader reader = command.ExecuteReader();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command.CommandText, connection);
            adapter.Fill(sqlData);
            killConnection(connection);
            return sqlData;
        }

        public static string getName(int Id)
        {

            SQLiteConnection connection = startConnection();
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT name FROM porn WHERE id = " + Id + ";";
            DataTable sqlData = new DataTable();
            SQLiteDataReader reader = command.ExecuteReader();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command.CommandText, connection);
            adapter.Fill(sqlData);
            killConnection(connection);
            return sqlData.Rows[0]["name"].ToString();
        }

        public static int randomNumber(int Min, int Max)
        {
            Random random = new Random();
            return random.Next(Min, Max);
        }
    }
}
