using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BaseLibrary.Data.Connectivity.SQL
{
    /// <summary>
    /// Eine Klasse zur Bereitstellung von Funktionen zur Kommunikation mit einem Microsoft SQL-Server
    /// </summary>
    public class SQL_Server
    {
        /// <summary>
        /// Kontruktur
        /// </summary>
        public SQL_Server()
        {
            SQL = new SqlConnection();
        }

        /// <summary>
        /// Konstruktor mit übergabe einer bereits bestehenden Instanz von <see cref="SQL_Server"/>
        /// </summary>
        /// <param name="SQL_Server">Bereitsbestehende Instanz von <see cref="SQL_Server"/>, die weiter gegeben werden soll.</param>
        public SQL_Server(SQL_Server SQL_Server)
        {
            SQL = SQL_Server.SQL;
        }

        /// <summary>
        /// Statische Verbindungszeichenfolge, die genutzt wird, um sich mit dem Microsoft SQL-Server zu verbinden.
        /// </summary>
        /// <remarks>
        /// Beispiel für eine Verbindungszeichenfolge:
        /// <example>
        /// "<c>Data source=127.0.0.1,1433\MSSQLSERVER; Network Library=DBMSSOCN; Initial Catalog=Database01; User ID=User01; Password=Password123;</c>"
        /// </example>
        /// </remarks>
        public static string ConnectionString { get; set; } = @"";

        /// <summary>
        /// Der Verbindungsstatus der <see cref="SqlConnection"/>
        /// </summary>
        public ConnectionState ConnectionState
        {
            get
            {
                return SQL.State;
            }
        }

        /// <summary>
        /// Die <see cref="SqlConnection"/> wird zur Verbindung mit dem Microsoft SQL-Server genutzt.
        /// </summary>
        public SqlConnection SQL
        {
            get; protected set;
        }

        /// <summary>
        /// Versucht ein Verbindung mit dem Microsoft SQL-Server herzustellen und gibt den Status der Verbindung zurück.
        /// </summary>
        /// <returns>Gibt den Verbindungsstatus <see cref="ConnectionState"/> zurück.</returns>
        public ConnectionState Connect()
        {
            try
            {
                SQL.ConnectionString = ConnectionString;
                SQL.Open();
            }
            catch { }
            return ConnectionState;
        }

        /// <summary>
        /// Die Asynchrone Methode versuch eine Verbindung mit dem Microsoft SQL-SErver herzustellen und ruft die übergebene Methode <paramref name="OnConnected"/> auf, sobald es gelungen oder fehlgeschlagen ist.
        /// </summary>
        /// <param name="OnConnected">Die Methode, die beim erfolgreichen Aufbau oder bei Auftreten eines Fehlers aufgerufen wird.</param>
        public async void Connect(Action<ConnectionState> OnConnected)
        {
            SQL.ConnectionString = ConnectionString;
            await Task.Run(() => { Connect(); });
            OnConnected.Invoke(ConnectionState);
        }

        /// <summary>
        /// Führt das übergebene <paramref name="SQL_Statement"/> auf dem Microsoft SQL-Server aus und gibt das Ergebnis als <see cref="SQL_Result"/> zurück.
        /// </summary>
        /// <param name="SQL_Statement">Die Zeichenfolge, die die SQL-Abfrage enthält.</param>
        /// <returns>Gibt das Ergebnis der SQL-Abfrage zurück.</returns>
        public SQL_Result ExecuteSQL(string SQL_Statement)
        {
            SQL_Result result = new SQL_Result(false, null);
            if (ConnectionState != ConnectionState.Open && Connect() != ConnectionState.Open)
            {
                return result;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL_Statement;
            cmd.Connection = SQL;
            DataSet ds = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd;
            try
            {
                sda.Fill(ds);
                result.Data = ds;
                result.Success = true;
            }
            catch
            {
                result.Success = false;
            }
            return result;
        }

        /// <summary>
        /// Asnynchroner Aufruf von <see cref="ExecuteSQL(string)"/>. Sobald eine Antwort von dem Microsoft SQL-Server vorliegt wird <paramref name="CallBack"/> aufgerufen.
        /// </summary>
        /// <param name="SQL_Statement">Die Zeichenfolge, die die SQL-Abfrage enthält.</param>
        /// <param name="CallBack">Der <see cref="Delegate"/>, der aufgerufen wird, wenn eine Antwort von dem Server vorliegt.</param>
        public async void ExecuteSQL(string SQL_Statement, Action<SQL_Result> CallBack)
        {
            SQL_Result results = new SQL_Result(false, null);
            await Task.Run(() => { results = ExecuteSQL(SQL_Statement); });
            CallBack.Invoke(results);
        }

        /// <summary>
        /// Führt eine SQL-Abfrage aus und gibt zurück, ob sie erfolgreich war. Siehe <seealso cref="ExecuteSQL(string)"/>
        /// </summary>
        /// <param name="SQL_Statement">Die Zeichenfolge, die die SQL-Abfrage enthält.</param>
        /// <returns>Gibt zurück, ob ein Fehler bei der Abfrage aufgetreten ist:
        /// <para>
        /// <list type="table">
        /// <item>
        /// -2 = Verbindung konnte nicht hergestellt werden.
        /// </item>
        /// <item>
        /// -1 = SQL-Abfrage erzeugt einen Fehler.
        /// </item>
        /// <item>
        /// 1 = Kein Fehler aufgetreten.
        /// </item>
        /// </list>
        /// </para>
        /// </returns>
        public int ExecuteSQL_NoDataSet(string SQL_Statement)
        {
            if (ConnectionState != ConnectionState.Open)
            {
                return -2;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL_Statement;
            cmd.Connection = SQL;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }
            return 1;
        }

        /// <summary>
        /// Führt eine SQL-Abfrage asynchron aus und führt <paramref name="CallBack"/> aus sobald eine Antwort von dem Microsoft SQL-Server von dem Server vorliegt.
        /// </summary>
        /// <param name="SQL_Statement">Die Zeichenfolge, die die SQL-Abfrage enthält.</param>
        /// <param name="CallBack">Der <see cref="Delegate"/>, der aufgerufen wird, wenn eine Antwort von dem Server vorliegt. Siehe: <seealso cref="ExecuteSQL_NoDataSet(string)"/></param>
        public async void ExecuteSQL_NoDataSet(string SQL_Statement, Action<int> CallBack)
        {
            int i = -3;
            await Task.Run(() => { i = ExecuteSQL_NoDataSet(SQL_Statement); });
            CallBack.Invoke(i);
        }
    }
}