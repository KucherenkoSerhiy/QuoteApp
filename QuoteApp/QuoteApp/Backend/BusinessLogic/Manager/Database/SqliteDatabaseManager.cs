using System.Data.SQLite;
using QuoteApp.Globals;

namespace QuoteApp.Backend.BusinessLogic.Manager.Database
{
    public class SqliteDatabaseManager
    {
        #region Singleton

        private SqliteDatabaseManager _instance;

        public SqliteDatabaseManager Instance => _instance ?? (_instance = new SqliteDatabaseManager());

        #endregion

        private SqliteDatabaseManager()
        {
            SQLiteConnection.CreateFile(QuoteAppConstants.DatabaseFilePath);

            SQLiteConnection m_dbConnection =
                new SQLiteConnection("Data Source=" + QuoteAppConstants.DatabaseFilePath + ";Version=3;");
            m_dbConnection.Open();

            string sql = "create table highscores (name varchar(20), score int)";

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "insert into highscores (name, score) values ('Me', 9001)";

            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            m_dbConnection.Close();
        }
    }
}