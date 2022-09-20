using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Assets.Serialization
{
    /// <summary>
    /// Serializes a data structure into a specified text stream
    /// Uses a simplified version of JSON.
    /// </summary>
    public class Serializer
    {
        /// <summary>
        /// Serialize an object to a text stream
        /// </summary>
        /// <param name="o">Object to write</param>
        /// <param name="w">Stream to write it to</param>
        public static void Serialize(object o, TextWriter w) => new Serializer(w).WriteObject(o);

        /// <summary>
        /// Serialize an object to a string
        /// </summary>
        /// <param name="o">Object to serialize</param>
        /// <returns>The serialized form of the object</returns>
        public static string Serialize(object o)
        {
            var w = new StringWriter();
            Serialize(o, w);
            return w.ToString();
        }

        /// <summary>
        /// Makes a fresh serializer object to write to the specified stream
        /// Use the WriteObject method to actual serialize something.
        /// </summary>
        /// <param name="writer">Stream to which to serialize</param>
        public Serializer(TextWriter writer)
        {
            Writer = writer;
        }

        /// <summary>
        /// The stream (TextWriter) to write to.
        /// </summary>
        public readonly TextWriter Writer;

        /// <summary>
        /// How much to indent after a newline.
        /// </summary>
        private int indentLevel;

        /// <summary>
        /// Write a string to output.
        /// </summary>
        /// <param name="s">String to write</param>
        private void Write(string s) => Writer.Write(s);

        /// <summary>
        /// Write an integer to output.
        /// </summary>
        /// <param name="i">Number to write</param>
        private void Write(int i) => Writer.Write(i.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Write a float to output.
        /// </summary>
        /// <param name="f"></param>
        private void Write(float f) => Writer.Write(f.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Write a character to output.
        /// </summary>
        /// <param name="c"></param>
        private void Write(char c) => Writer.Write(c);

        /// <summary>
        /// Write a Boolean to output.
        /// </summary>
        /// <param name="b"></param>
        private void Write(bool b) => Writer.Write(b);

        /// <summary>
        /// Start a new line in the output and indent to indentLevel
        /// </summary>
        private void NewLine()
        {
            Writer.WriteLine();
            for (var i = 0; i < 4 * indentLevel; i++)
                Write(' ');
        }

        /// <summary>
        /// Write a field of an object in format "fieldname: value"
        /// </summary>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="fieldValue">Value of the field</param>
        /// <param name="firstOne">If this is the first field printed inside of the object.  This is just so it knows whether to print a comma before the field</param>
        private void WriteField(string fieldName, object fieldValue, bool firstOne)
        {
            if (!firstOne)
            {
                Write(",");
                NewLine();
            }
            Write(fieldName);
            Write(": ");
            WriteObject(fieldValue);
        }

        /// <summary>
        /// Write a { } or [ ] expression whose contents is produced by the specified generator procedure.  See WriteList for an example.
        /// </summary>
        /// <param name="start">Open bracket to use</param>
        /// <param name="generator">Procedure to print the contents inside the brackets, e.g. () => { stuff to print }</param>
        /// <param name="end">Close bracket to use</param>
        private void WriteBracketedExpression(string start, Action generator, string end)
        {
            Write(start);
            indentLevel++;
            NewLine();
            generator();
            indentLevel--;
            NewLine();
            Write(end);
        }

        /// <summary>
        /// Write a list or array in the format: [ elt, elt, elt, ... ]
        /// </summary>
        /// <param name="list"></param>
        private void WriteList(ICollection list)
        {
            if (list.Count == 0)
                Write("[ ]");
            else
                WriteBracketedExpression(
                    "[ ",
                    () =>
                    {
                        var firstItem = true;
                        foreach (var item in list)
                        {
                            if (firstItem)
                                firstItem = false;
                            else
                                Write(", ");
                            WriteObject(item);
                        }
                    },
                    " ]");
        }

                /// <summary>
        /// Table of objects that have already been serialized.
        /// If idTable.ContainsKey(object), then that object has been
        /// serialized and its id number is idTable[object].
        /// </summary>
        private readonly Dictionary<object, int> idTable = new Dictionary<object, int>();

        /// <summary>
        /// id number to give to the next object that we serialize
        /// </summary>
        private int idCounter;

        /// <summary>
        /// Return the ID number attached to this object for this serialization.
        /// If the object hasn't already been assigned an ID, assign one and return (id, true).
        /// Otherwise, use the previously assigned id and return (id, false).
        /// </summary>
        /// <param name="o">Object to be output</param>
        /// <returns>ID assigned to the object and whether that id was just assigned or had already been assigned before this call.</returns>
        private (int id, bool isNew) GetId(object o)
        {
            if (idTable.TryGetValue(o, out var id))
                return (id, false);

            id = idCounter++;
            idTable[o] = id;
            return (id, true);
        }

        /// <summary>
        /// Print out the serialization data for the specified object.
        /// </summary>
        /// <param name="o">Object to serialize</param>
        private void WriteObject(object o)
        {
            switch (o)
            {
                case null:
                    throw new NotImplementedException("Fill me in");
                    break;

                case int i:
                    throw new NotImplementedException("Fill me in");
                    break;

                case float f:
                    throw new NotImplementedException("Fill me in");
                    break;

                // Not: don't worry about handling strings that contain quote marks
                case string s:
                    throw new NotImplementedException("Fill me in");
                    break;

                case bool b:
                    throw new NotImplementedException("Fill me in");
                    break;

                case IList list:
                    throw new NotImplementedException("Fill me in");
                    break;

                default:
                    if (o.GetType().IsValueType)
                        throw new Exception($"Trying to write an unsupported value type: {o.GetType().Name}");

                    WriteComplexObject(o);
                    break;
            }
        }

        /// <summary>
        /// Serialize a complex object (i.e. a class object)
        /// If this object has already been output, just output #id, where is is it's id from GetID.
        /// If it hasn't then output #id { type: "typename", field: value ... }
        /// </summary>
        /// <param name="o">Object to serialize</param>
        private void WriteComplexObject(object o)
        {
            throw new NotImplementedException("Fill me in");
        }
    }
}
