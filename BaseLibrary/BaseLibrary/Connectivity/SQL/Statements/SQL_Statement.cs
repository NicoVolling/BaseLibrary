using BaseLibrary.Data.Objects;
using BaseLibrary.Data.Querries;
using System;
using System.Collections.Generic;
using System.Data;

namespace BaseLibrary.Data.Connectivity.SQL.Statements
{
    /// <summary>
    /// Diese abtrakte und generische Klasse implementiert die Schnittstelle <see cref="ISQL_Statement"/> und stellt die Ereignisse bereit, die von der <see cref="Data.Querries.DataQuerry{T}"/> ausgelöst werden.
    /// </summary>
    /// <typeparam name="T_DO">Der Typ des <see cref="Data.Objects.DataObject"/></typeparam>
    public abstract class SQL_Statement<T_DO> : ISQL_Statement where T_DO : DataObject, new()
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public SQL_Statement()
        {
            DQ = new DataQuerry<T_DO>();
            DQ.ErrorReceived += DQ_ErrorReceived;
            DQ.DataSetReceived += DQ_DataSetReceived;
            DQ.DataObjectsReceived += DQ_DataObjectsReceived1;
        }

        /// <summary>
        /// Der Eventhandler, der die Auflistung von <see cref="T_DO"/> enthält.
        /// </summary>
        /// <param name="Sender">Das auslösende Objekt.</param>
        /// <param name="DataObjects">Die Auslistung von <see cref="T_DO"/>, die die <see cref="T_DO"/> enthält, die bei der Umwandlung von dem <see cref="DataSet"/> aus der SQL-Abfrage zurückgegeben wurden.</param>
        public delegate void DataObjectsReceivedEventhandler(SQL_Statement<T_DO> Sender, List<T_DO> DataObjects);

        /// <summary>
        /// Der Eventhandler, der das <see cref="DataSet"/> enthält.
        /// </summary>
        /// <param name="Sender">Das auslösende Objekt.</param>
        /// <param name="DataSet">Das <see cref="DataSet"/>, das die Daten enthält, die bei der SQL-Abfrage zurückgegeben wurden.</param>
        public delegate void DataSetReceivedEventhandler(SQL_Statement<T_DO> Sender, DataSet DataSet);

        /// <summary>
        /// Der Eventhanlder, der den Fehler enthält, der beim Ausführen der SQL-Abfrage aufgetreten ist.
        /// </summary>
        /// <param name="Sender">Das auslösende Objekt.</param>
        /// <param name="Message">Der <see cref="string"/>, der den Fehler enthält, der beim Ausführen der SQL-Abfrage aufgetreten ist.</param>
        public delegate void ErrorReceivedEventhandler(SQL_Statement<T_DO> Sender, string Message);

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
        /// Die <see cref="Data.Querries.DataQuerry{T_DO}"/>, die mit dem Microsoft SQL-Server kommuniziert.
        /// </summary>
        public DataQuerry<T_DO> DQ { get; }

        /// <summary>
        /// Das SQL-Statement, das zur Löschung des Datensatzes benötigt wird.
        /// </summary>
        protected abstract string Statement_DELETE { get; }

        /// <summary>
        /// Das SQL-Statement, das zum hinzufügen eines Datensatzes benötigt wird.
        /// </summary>
        protected abstract string Statement_INSERT { get; }

        /// <summary>
        /// Das SQL-Statement, das zur Abfrage eines Datensatzes benötigt wird.
        /// </summary>
        protected abstract string Statement_SELECT { get; }

        /// <summary>
        /// Das SQL-Statement, das zur Abfrage aller Datensätze benötigt wird.
        /// </summary>
        protected abstract string Statement_SELECTALL { get; }

        /// <summary>
        /// Das SQL-Statement, das zur Veränderung des Datensatzes benötigt wird.
        /// </summary>
        protected abstract string Statement_UPDATE { get; }

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
        public virtual void RunQuerry(string SQL_Statement, params string[] Parameters)
        {
            DQ.RunQuerry(SQL_Statement, Parameters);
        }

        /// <summary>
        /// Löscht das übergebene <see cref="T_DO"/>.
        /// </summary>
        /// <param name="DataObject">Das <see cref="T_DO"/>, das von der Aktion betroffen ist.</param>
        public virtual void RunQuerry_DELETE(Object DataObject)
        {
            RunQuerry(((T_DO)DataObject).InsertDataIntoString(Statement_DELETE));
        }

        /// <summary>
        /// Löscht das übergebene <see cref="T_DO"/>.
        /// </summary>
        /// <param name="ID">Die ID des betroffenen Datensatzes.</param>
        public void RunQuerry_DELETE(string ID)
        {
            RunQuerry(new T_DO() { ID = ID }.InsertDataIntoString(Statement_DELETE));
        }

        /// <summary>
        /// Fügt das übergebene <see cref="T_DO"/> hinzu.
        /// </summary>
        /// <param name="DataObject">Das <see cref="T_DO"/>, das von der Aktion betroffen ist.</param>
        public virtual void RunQuerry_INSERT(Object DataObject)
        {
            RunQuerry(((T_DO)DataObject).InsertDataIntoString(Statement_INSERT));
        }

        /// <summary>
        /// Gibt das übergebene <see cref="T_DO"/> zurück.
        /// </summary>
        /// <param name="DataObject">Das <see cref="T_DO"/>, das von der Aktion betroffen ist.</param>
        public virtual void RunQuerry_SELECT(Object DataObject)
        {
            RunQuerry(((T_DO)DataObject).InsertDataIntoString(Statement_SELECT));
        }

        /// <summary>
        /// Gibt alle übergebenen <see cref="T_DO"/> zurück.
        /// </summary>
        public virtual void RunQuerry_SELECTALL()
        {
            RunQuerry(Statement_SELECTALL);
        }

        /// <summary>
        /// Verändert das übergebene <see cref="T_DO"/>.
        /// </summary>
        /// <param name="DataObject">Das <see cref="T_DO"/>, das von der Aktion betroffen ist.</param>
        public virtual void RunQuerry_UPDATE(Object DataObject)
        {
            RunQuerry(((T_DO)DataObject).InsertDataIntoString(Statement_UPDATE));
        }

        private void DQ_DataObjectsReceived1(DataQuerry<T_DO> Sender, List<T_DO> DataObjects)
        {
            OnDataObjectsReceived(DataObjects);
        }

        private void DQ_DataSetReceived(DataQuerry<T_DO> Sender, DataSet DataSet)
        {
            OnDataSetReceived(DataSet);
        }

        private void DQ_ErrorReceived(DataQuerry<T_DO> Sender, string Message)
        {
            OnErrorReceived(Message);
        }

        private void OnDataObjectsReceived(List<T_DO> DataObjects)
        {
            DataObjectsReceivedEventhandler handler = DataObjectsReceived;
            handler?.Invoke(this, DataObjects);
        }

        private void OnDataSetReceived(DataSet DataSet)
        {
            DataSetReceivedEventhandler handler = DataSetReceived;
            handler?.Invoke(this, DataSet);
        }

        private void OnErrorReceived(string Message)
        {
            ErrorReceivedEventhandler handler = ErrorReceived;
            handler?.Invoke(this, Message);
        }
    }
}