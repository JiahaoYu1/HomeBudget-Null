using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Threading;
using System.Data.SqlClient;

// ===================================================================
// Very important notes:
// ... To keep everything working smoothly, you should always
//     dispose of EVERY SQLiteCommand even if you recycle a 
//     SQLiteCommand variable later on.
//     EXAMPLE:
//            Database.newDatabase(GetSolutionDir() + "\\" + filename);
//            var cmd = new SQLiteCommand(Database.dbConnection);
//            cmd.CommandText = "INSERT INTO categoryTypes(Description) VALUES('Whatever')";
//            cmd.ExecuteNonQuery();
//            cmd.Dispose();
//
// ... also dispose of reader objects
//
// ... by default, SQLite does not impose Foreign Key Restraints
//     so to add these constraints, connect to SQLite something like this:
//            string cs = $"Data Source=abc.sqlite; Foreign Keys=1";
//            var con = new SQLiteConnection(cs);
//
// ===================================================================


namespace Budget
{
    public class Database
    {

        public static SQLiteConnection dbConnection { get { return _connection; } }
        private static SQLiteConnection _connection;

        // ===================================================================
        // create and open a new database
        // ===================================================================
        public static void newDatabase(string filename)
        {
            Category.CategoryType[] categoryTypes = (Category.CategoryType[])Enum.GetValues(typeof(Category.CategoryType));

            // If there was a database open before, close it and release the lock
            CloseDatabaseAndReleaseFile();

            SQLiteConnection.CreateFile(filename);

            _connection = new SQLiteConnection($"Data Source={filename};Version=3;Foreign Keys=1");
            _connection.Open();

            using (var cmd = new SQLiteCommand(_connection))
            {

                // drop any existing tables
                cmd.CommandText = "DROP TABLE IF EXISTS expenses";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DROP TABLE IF EXISTS categories";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DROP TABLE IF EXISTS categoryTypes";
                cmd.ExecuteNonQuery();

                // create new tables
                cmd.CommandText = "CREATE TAB" +
                    "LE categoryTypes (Id INTEGER PRIMARY KEY, Description TEXT NOT NULL)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE categories (Id INTEGER PRIMARY KEY, Description TEXT NOT NULL, TypeId INTEGER NOT NULL, FOREIGN KEY (TypeId) REFERENCES categoryTypes(Id))";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE expenses (Id INTEGER PRIMARY KEY, CategoryId INTEGER NOT NULL, Date TEXT NOT NULL, Description TEXT NOT NULL, Amount DOUBLE NOT NULL, FOREIGN KEY(CategoryId) REFERENCES categories(Id))";
                cmd.ExecuteNonQuery();


                cmd.CommandText = "DELETE FROM categoryTypes";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM categories";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM expenses";
                cmd.ExecuteNonQuery();

                //// insert default category types
                foreach (Category.CategoryType categoryType in categoryTypes)
                {
                    cmd.CommandText = $"INSERT INTO categoryTypes (Description) VALUES ('{categoryType.ToString()}')";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ===================================================================
        // open an existing database
        // ===================================================================
        public static void existingDatabase(string filename)
        {

            //CloseDatabaseAndReleaseFile();

            _connection = new SQLiteConnection($"Data Source={filename};Foreign Keys=1;");
            _connection.Open();
        }

       // ===================================================================
       // close existing database, wait for garbage collector to
       // release the lock before continuing
       // ===================================================================
        static public void CloseDatabaseAndReleaseFile()
        {
            if (Database.dbConnection != null)
            {
                // close the database connection
                Database.dbConnection.Close();
                

                // wait for the garbage collector to remove the
                // lock from the database file
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }

}
