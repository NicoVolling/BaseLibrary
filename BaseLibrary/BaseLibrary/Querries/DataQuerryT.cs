using BaseLibrary.Data.Objects;
using BaseLibrary.Data.Connectivity.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BaseLibrary.Data.Querries
{
    /// <summary>
    /// Die Generische Klasse <see cref="DataQuerry{T}"/> dient der Datenbankabfrage (Microsoft SQL-Server) in Kombination mit <see cref="DataObject"/>.
    /// </summary>
    /// <remarks>
    /// Wenn eine Abfrage erfolgt, wird das <see cref="DataObjectsReceived"/>-Ereignis ausgelöst und die erhaltenen Daten werden in Form einer Auflistung von <see cref="T_DO"/> übergeben.
    /// </remarks>
    /// <typeparam name="T_DO">Typ des <see cref="DataObject"/></typeparam>
    public class DataQuerry<T_DO> where T_DO : DataObject, new()
    {
        /// <summary>
        /// Der Eventhandler, der die Auflistung von <see cref="T_DO"/> enthält.
        /// </summary>
        /// <param name="Sender">Das auslösende Objekt.</param>
        /// <param name="DataObjects">Die Auslistung von <see cref="T_DO"/>, die die <see cref="T_DO"/> enthält, die bei der Umwandlung von dem <see cref="DataSet"/> aus der SQL-Abfrage zurückgegeben wurden.</param>
        public delegate void DataObjectsReceivedEventhandler(DataQuerry<T_DO> Sender, List<T_DO> DataObjects);

        /// <summary>
        /// Der Eventhandler, der das <see cref="DataSet"/> enthält.
        /// </summary>
        /// <param name="Sender">Das auslösende Objekt.</param>
        /// <param name="DataSet">Das <see cref="DataSet"/>, das die Daten enthält, die bei der SQL-Abfrage zurückgegeben wurden.</param>
        public delegate void DataSetReceivedEventhandler(DataQuerry<T_DO> Sender, DataSet DataSet);

        /// <summary>
        /// Der Eventhanlder, der den Fehler enthält, der beim Ausführen der SQL-Abfrage aufgetreten ist.
        /// </summary>
        /// <param name="Sender">Das auslösende Objekt.</param>
        /// <param name="Message">Der <see cref="string"/>, der den Fehler enthält, der beim Ausführen der SQL-Abfrage aufgetreten ist.</param>
        public delegate void ErrorReceivedEventhandler(DataQuerry<T_DO> Sender, string Message);

        /// <summary>
        /// Das event, das ausgelöst wird, wenn die SQL-Abfrage erfolgreich war und die Daten aus dem erhaltenen <see cref="DataSet"/> erfolgreich in eine Auflistung von <see cref="T_DO"/> umgewandelt wurden.
        /// </summary>
        public event DataObjectsReceivedEventhandler DataObjectsReceived;

        /// <summary>
        /// Das event, das ausgelöst wird, wenn die SQL-Abfrage erfolgreich war.
        /// </summary>
        public event DataSetReceivedEventhandler DataSetReceived;

        /// <summary>
        /// Das event, das ausgelöst wird, wenn bei der SQL-Abfrage ein Fehler aufgetreten ist.
        /// </summary>
        public event ErrorReceivedEventhandler ErrorReceived;

        /// <summary>
        /// Mit Aufruf dieser Methode wird das <paramref name="SQL_Statement"/> zusammen mit den <paramref name="Parameters"/> als SQL-Abfrage an den <see cref="SQL_Server"/> gesendet.
        /// </summary>
        /// <param name="SQL_Statement">Der <see cref="string"/>, der die gesamte Abfrage enthält.</param>
        /// <param name="Parameters">Die Parameter, die mithilfe der <see cref="string.Format(string, object[])"/>-Methode in das <paramref name="SQL_Statement"/> eingefügt werden.</param>
        /// <remarks>
        /// Löst folgende Events aus:
        /// <list type="bullet">
        /// <item>
        /// <see cref="DataObjectsReceived"/>
        /// </item>
        /// <item>
        /// <see cref="DataSetReceived"/>
        /// </item>
        /// <item>
        /// <see cref="ErrorReceived"/>
        /// </item>
        /// </list>
        /// </remarks>
        public void RunQuerry(string SQL_Statement, params string[] Parameters)
        {
            SQL_Server SQL = new SQL_Server();
            SQL.Connect(new Action<ConnectionState>((ConnectionState) =>
            {
                if (ConnectionState != ConnectionState.Open)
                {
                    OnErrorReceived("Verbindung zum Server fehlgeschlagen");
                    return;
                }
                string Statement = Parameters.Length > 0 ? string.Format(SQL_Statement, Parameters) : SQL_Statement;
                Tracing.Trace.Write(Tracing.Trace.TraceType_Debug, this.GetType().Name, this.GetType().GetMethod("RunQuerry").Name, Statement);
                SQL.ExecuteSQL(Statement, new Action<SQL_Result>((SQL_Result) =>
                {
                    if (!SQL_Result.Success)
                    {
                        Tracing.Trace.Write(Tracing.Trace.TraceType_Error, this.GetType().Name, this.GetType().GetMethod("RunQuerry").Name, Statement);
                        OnErrorReceived("Unbekannter SQL-Fehler aufgetreten" + Statement);
                        return;
                    }

                    OnDataSetReceived(SQL_Result.Data);

                    List<T_DO> dO = new List<T_DO>();
                }));
            }));
        }

        private void OnDataObjectsReceived(List<T_DO> dO)
        {
            DataObjectsReceivedEventhandler handler = DataObjectsReceived;
            handler?.Invoke(this, dO);
        }

        private void OnDataSetReceived(DataSet ds)
        {
            List<T_DO> dataObjects = new List<T_DO>();
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    T_DO dO = new T_DO();
                    dO.FromDataRow(dr);
                    dataObjects.Add(dO);
                }
            }
            OnDataObjectsReceived(dataObjects);
            DataSetReceivedEventhandler handler = DataSetReceived;
            handler?.Invoke(this, ds);
        }

        private void OnErrorReceived(string Message)
        {
            ErrorReceivedEventhandler handler = ErrorReceived;
            handler?.Invoke(this, Message);
        }
    }
}