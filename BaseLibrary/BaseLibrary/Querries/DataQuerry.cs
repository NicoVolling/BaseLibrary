using BaseLibrary.Data.Objects;
using BaseLibrary.Data.Connectivity.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BaseLibrary.Data.Querries
{
    /// <summary>
    /// Die Klasse <see cref="DataQuerry"/> dient der Datenbankabfrage (Microsoft SQL-Server) in Kombination mit <see cref="DataObject"/>.
    /// </summary>
    /// <remarks>
    /// Wenn eine Abfrage erfolgt, wird das <see cref="DataObjectsReceived"/>-Ereignis ausgelöst und die erhaltenen Daten werden in Form einer Auflistung von <see cref="DataObject"/> übergeben.
    /// </remarks>
    /// <inheritdoc/>
    public class DataQuerry : DataQuerry<DataObject>
    {
        /// <summary>
        /// Der Eventhandler, der die Auflistung von <see cref="DataObject"/> enthält.
        /// </summary>
        /// <param name="Sender">Das auslösende Objekt.</param>
        /// <param name="DataObjects">Die Auslistung von <see cref="DataObject"/>, die die <see cref="DataObject"/> enthält, die bei der Umwandlung von dem <see cref="DataSet"/> aus der SQL-Abfrage zurückgegeben wurden.</param>
        public new delegate void DataObjectsReceivedEventhandler(DataQuerry Sender, List<DataObject> DataObjects);

        /// <summary>
        /// Das <see cref="event"/>, das ausgelöst wird, wenn die SQL-Abfrage erfolgreich war und die Daten aus dem erhaltenen <see cref="DataSet"/> erfolgreich in eine Auflistung von <see cref="DataObject"/> umgewandelt wurden.
        /// </summary>
        public new event DataObjectsReceivedEventhandler DataObjectsReceived;

        /// <summary>
        /// Das <see cref="event"/>, das ausgelöst wird, wenn die SQL-Abfrage erfolgreich war.
        /// </summary>
        public new event DataSetReceivedEventhandler DataSetReceived;

        /// <summary>
        /// Das <see cref="event"/>, das ausgelöst wird, wenn bei der SQL-Abfrage ein Fehler aufgetreten ist.
        /// </summary>
        public new event ErrorReceivedEventhandler ErrorReceived;

        /// <summary>
        /// Mit Aufruf dieser generischen Methode wird das <paramref name="SQL_Statement"/> zusammen mit den <paramref name="Parameters"/> als SQL-Abfrage an den <see cref="SQL_Server"/> gesendet.
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
        /// <typeparam name="T">Typ des DataObjects, das zurückgegeben werden soll</typeparam>
        public void RunQuerry<T>(string SQL_Statement, params string[] Parameters) where T : DataObject, new()
        {
            SQL_Server SQL = new SQL_Server();
            SQL.Connect(new Action<ConnectionState>((ConnectionState) =>
            {
                if (ConnectionState != ConnectionState.Open)
                {
                    OnErrorReceived<T>("Verbindung zum Server fehlgeschlagen");
                    return;
                }
                Tracing.Trace.Write(Tracing.Trace.TraceType_Debug, this.GetType().Name, this.GetType().GetMethod("RunQuerry").Name, SQL_Statement);
                SQL.ExecuteSQL(string.Format(SQL_Statement, Parameters), new Action<SQL_Result>((SQL_Result) =>
                {
                    if (!SQL_Result.Success)
                    {
                        Tracing.Trace.Write(Tracing.Trace.TraceType_Error, this.GetType().Name, this.GetType().GetMethod("RunQuerry").Name, SQL_Statement);
                        OnErrorReceived<T>("Unbekannter SQL-Fehler aufgetreten" + SQL_Statement);
                        return;
                    }

                    OnDataSetReceived<T>(SQL_Result.Data);

                    List<DataObject> dO = new List<DataObject>();
                }));
            }));
        }

        private void OnDataObjectsReceived(List<DataObject> dO)
        {
            DataObjectsReceivedEventhandler handler = DataObjectsReceived;
            handler?.Invoke(this, dO);
        }

        private void OnDataSetReceived<T>(DataSet ds) where T : DataObject, new()
        {
            List<DataObject> dataObjects = new List<DataObject>();
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    T dO = new T();
                    dO.FromDataRow(dr);
                    dataObjects.Add(dO);
                }
            }
            OnDataObjectsReceived(dataObjects);
            DataSetReceivedEventhandler handler = DataSetReceived;
            handler?.Invoke(this, ds);
        }

        private void OnErrorReceived<T>(string Message) where T : DataObject, new()
        {
            ErrorReceivedEventhandler handler = ErrorReceived;
            handler?.Invoke(this, Message);
        }
    }
}