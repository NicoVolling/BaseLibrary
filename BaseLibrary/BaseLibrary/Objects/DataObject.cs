using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace BaseLibrary.Data.Objects
{
    /// <summary>
    /// Das <see cref="DataObject"/> kann Daten von einem <see cref="DataSet"/> in die entsprechend genannte Eigenschaft umwandeln und umgekehrt.
    /// <para/>Der Erbe dieser Klasse muss Eigenschaften aufweisen, deren Namen mit den Namen der entsprechenden Spalten im <see cref="DataSet"/> übereinstimmen.
    /// Außerdem dürfen die Eigenschaften nicht schreibgeschützt sein.
    /// </summary>
    public class DataObject
    {
        /// <summary>
        /// Kontruktor
        /// </summary>
        /// <remarks>
        /// Hier wird <see cref="DataObject.SetPropertyFriendlyNames"/> aufgerufen
        /// </remarks>
        public DataObject()
        {
            SetPropertyFriendlyNames();
        }

        /// <summary>
        /// Die ID zur eindeutigen auseinanderhaltung von verschiedenen Datensätzen.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gibt das <see cref="Dictionary{TKey, TValue}"/> zurück, dass die Eigenschaften und Bezeichnungen enthält.
        /// </summary>
        /// <remarks>
        /// <para/>Um Eigenschaften hinzuzufügen, die <see cref="DataObject.SetPropertyFriendlyNames()"/>-Methode überschreiben.
        /// </remarks>
        public Dictionary<string, string> PropertyFriendlyNames { get; protected set; } = new Dictionary<string, string>();

        /// <summary>
        /// Mithilfe dieser Methode werden die Werte einer <see cref="DataRow"/> in die entsprechenden Eigenschaften dieses Objektes geschrieben.
        /// </summary>
        /// <param name="DataRow">Die <see cref="DataRow"/>, aus der die Werte gelesen werden</param>
        public void FromDataRow(DataRow DataRow)
        {
            Type t = this.GetType();
            foreach (PropertyInfo property in t.GetProperties())
            {
                if (DataRow.Table.Columns.Contains(property.Name))
                {
                    if (property.CanWrite)
                    {
                        property.SetValue(this, DataRow[property.Name]);
                    }
                }
            }
        }

        /// <summary>
        /// Mithilfe dieser Methode lassen sich die Werte eine nach <paramref name="PropertyName"/> benannten Eigenschaft auslesen.
        /// </summary>
        /// <typeparam name="T">Der Typ des Rückgabewertes</typeparam>
        /// <param name="PropertyName">Der Name der zu lesenden Eigenschaft</param>
        /// <returns>Gibt den Wert vom Typ <typeparamref name="T"/> oder default(<typeparamref name="T"/>), wenn die Eigenschaft nicht gefunden oder nicht ausgelesen werden kann zurück</returns>
        public T GetValue<T>(string PropertyName)
        {
            Type t = this.GetType();
            foreach (PropertyInfo property in t.GetProperties())
            {
                if (property.Name == PropertyName && property.PropertyType == typeof(T))
                {
                    return (T)property.GetValue(this);
                }
                if (property.Name == PropertyName && typeof(T) == typeof(Object))
                {
                    return (T)((object)property.GetValue(this).ToString());
                }
            }
            return default(T);
        }

        /// <summary>
        /// Mithilfe dieser Methode wird der <paramref name="String"/> in einen <see cref="string"/> umgewandelt, der die Werte dieses Objekte enthält, sofern diese in dem <paramref name="String"/> gefordert werden.
        /// </summary>
        /// <param name="String">Eingabezeichenkette. Folgende Syntax verwenden um einen Wert einzufügen: <c>"{this.PropertyName}"</c>, wobei <c>"PropertyName"</c> der Name der Eigenschaft ist</param>
        /// <returns>Gibt den <see cref="string"/> zurück, der alle angeforderten Werte dieses Objektes an den angegebenen Stellen enthält</returns>
        public string InsertDataIntoString(string String)
        {
            string res = String;
            Type t = this.GetType();
            foreach (PropertyInfo property in t.GetProperties())
            {
                res = res.Replace("{" + "this." + property.Name + "}", GetValue<string>(property.Name));
            }
            return res;
        }

        /// <summary>
        /// Mithilfe dieser Methode werden alle Eigenschaften dieses Objektes, die gelesen werden können in eine <see cref="DataRow"/> geschrieben.
        /// </summary>
        /// <returns>Gibt die <see cref="DataRow"/> zurück, die die Eigenschaften dieses Objektes enthält</returns>
        public DataRow ToDataRow()
        {
            DataSet ds = ToEmptyDataSet();
            Type t = this.GetType();
            DataRow dr = ds.Tables[t.Name].Rows.Add();
            foreach (PropertyInfo property in t.GetProperties())
            {
                if (property.CanRead)
                {
                    dr[property.Name] = property.GetValue(this);
                }
            }
            return dr;
        }

        /// <summary>
        /// Mithilfe dieser Methode werden alle Eigenschaften dieses Objektes, die gelesen werden können in ein <see cref="DataSet"/> geschrieben.
        /// </summary>
        /// <returns>Gibt das <see cref="DataSet"/> zurück, die die Eigenschaften dieses Objektes enthält</returns>
        public DataSet ToDataSet()
        {
            DataSet ds = ToEmptyDataSet();
            Type t = this.GetType();
            DataRow dr = ds.Tables[t.Name].Rows.Add();
            foreach (PropertyInfo property in t.GetProperties())
            {
                dr[property.Name] = property.GetValue(this);
            }
            return ds;
        }

        /// <summary>
        /// Mithilfe dieser Methode wird ein <see cref="DataSet"/> generiert, das die Kopfdaten, jedoch nicht die Werte dieses Objektes enthält und zurückgegeben.
        /// </summary>
        /// <returns>Gibt das <see cref="DataSet"/> zurück, die die Tabelle und Spalten enthält, die die Struktur dieses Objektes wiederspiegeln</returns>
        public DataSet ToEmptyDataSet()
        {
            DataSet ds = new DataSet();
            Type t = this.GetType();
            ds.Tables.Add(t.Name);
            foreach (PropertyInfo property in t.GetProperties())
            {
                ds.Tables[t.Name].Columns.Add(property.Name);
            }
            return ds;
        }

        /// <summary>
        /// Fügt eine Eigenschaft dem <see cref="Dictionary{TKey, TValue}"/>, <see cref="DataObject.PropertyFriendlyNames"/>, hinzu.
        /// </summary>
        /// <remarks>
        /// Zur Verwendung in <see cref="DataObject.SetPropertyFriendlyNames"/>
        /// </remarks>
        /// <param name="Name">Der Name der Eigenschaft</param>
        /// <param name="FriendlyName">Die Bezeichnung für die Eigenschaft</param>
        protected void AddProperty(string Name, string FriendlyName)
        {
            PropertyFriendlyNames.Add(Name, FriendlyName);
        }

        /// <summary>
        /// Diese Methode wird im Konstruktor <see cref="DataObject()"/> aufgerufen.
        /// </summary>
        /// <remarks>
        /// Diese Methode überschreiben, um mithilfe von <see cref="DataObject.AddProperty(string, string)"/> Eigenschaften dem <see cref="Dictionary{TKey, TValue}"/> <see cref="DataObject.PropertyFriendlyNames"/> hinzuzufügen
        /// </remarks>
        protected virtual void SetPropertyFriendlyNames()
        {
        }
    }
}