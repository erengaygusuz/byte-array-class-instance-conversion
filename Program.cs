/*
    Author: Eren Gaygusuz
*/

// Library definitions
using System;
using System.Reflection;

namespace GithubExamples
{
    // Lets think about you have a class named Example. 
    // Inside of it, there are some variables which have different types like int, uint, ulong, float etc.
    // If you want to convert this class instance to byte array, you can use the following method. 
    // Also you can convert a byte array to a class instance with giving the order of types of class variables.

    class Program
    {
        static void Main(string[] args)
        {
            // Creating an instance of Example Class and assigning some values into its variables.
            Example example1 = new Example();

            example1.variable1 = true;
            example1.variable2 = -10001;
            example1.variable3 = 10111;
            example1.variable4 = 'E';
            example1.variable5 = -2000000;
            example1.variable6 = 2000500;
            example1.variable7 = 0.2f;
            example1.variable8 = 0.8;
            example1.variable9 = -35000000;
            example1.variable10 = 30000000;

            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine("Example1 instance variables: " + "\n");

            // Printing values of example1 instance variables
            Console.Write(example1.variable1 + " ");
            Console.Write(example1.variable2 + " ");
            Console.Write(example1.variable3 + " ");
            Console.Write(example1.variable4 + " ");
            Console.Write(example1.variable5 + " ");
            Console.Write(example1.variable6 + " ");
            Console.Write(example1.variable7 + " ");
            Console.Write(example1.variable8 + " ");
            Console.Write(example1.variable9 + " ");
            Console.Write(example1.variable10 + "\n");

            Console.WriteLine("------------------------------------------------------------------------------");

            // Conversion class instance to byte array and assigning this byte array to temporary array
            byte[] tempArr = Conversion.ObjectToByteArray(example1);

            // Creating a new instance to see the deconversion of temporary byte array to class instance
            Example example2 = new Example();

            // Creating a string array variable for the conversion type order and 
            // Assigning variables type conversion order, I use the BitConverter methods names
            // Below is the whole list of conversion methods of BitConverter which will be use

            // ToBoolean for bool
            // ToChar fof char
            // ToDouble for double
            // ToInt16 for short
            // ToInt32 for int
            // ToInt64 for long
            // ToSingle for float
            // ToUInt16 for ushort
            // ToUInt32 for uint
            // ToUInt64 for ulong

            // The order should be the same types of instance of class variables
            string[] conversionTypes = new string[] { "ToBoolean", "ToInt16", "ToUInt16", "ToChar", "ToInt32", "ToUInt32", "ToSingle", "ToDouble", "ToInt64", "ToUInt64"};

            // Assigning the return value of ByteArrayToObject function 
            // I gave two paramaters, first one is temporary array, second one is conversion types array
            example2 = Conversion.ByteArrayToObject<Example>(tempArr, conversionTypes);

            Console.WriteLine("Example2 instance variables: " + "\n");

            // Printing values of example2 instance variables
            Console.Write(example2.variable1 + " ");
            Console.Write(example2.variable2 + " ");
            Console.Write(example2.variable3 + " ");
            Console.Write(example2.variable4 + " ");
            Console.Write(example2.variable5 + " ");
            Console.Write(example2.variable6 + " ");
            Console.Write(example2.variable7 + " ");
            Console.Write(example2.variable8 + " ");
            Console.Write(example2.variable9 + " ");
            Console.Write(example2.variable10 + "\n");

            Console.WriteLine("------------------------------------------------------------------------------");

            Console.ReadKey();
        }
    }

    // This is our class with different types of variables
    public class Example
    {
        public bool variable1;
        public short variable2;
        public ushort variable3;
        public char variable4;
        public int variable5;
        public uint variable6;
        public float variable7;
        public double variable8;
        public long variable9;
        public ulong variable10;
    }

    // This class will be use for conversion and we do not have to create instance of it.
    // So it can be static.
    public static class Conversion
    {
        // This the class instance conversion to byte array function
        // I used generics so you can give a parameter which is any type of class object
        public static byte[] ObjectToByteArray<T>(T obj)
        {
            int order = 0;
            dynamic value;

            byte[] array = new byte[GetAllParametersByteSize<T>(obj)];

            for (int i = 0; i < GetParameters<T>(obj).Length; i++)
            {
                value = GetParameters<T>(obj)[i].GetValue(obj);

                for (int j = 0; j < BitConverter.GetBytes(value).Length; j++)
                {
                    array[order] = BitConverter.GetBytes(value)[j];
                    order++;
                }
            }

            return array;
        }

        // This is the byte array to class instance conversion function
        // I used generics so you can give byte array to convert any type of class instance
        // You should also give the conversion type array like a parameter
        public static T ByteArrayToObject<T>(byte[] arr, string[] conversionTypes)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));

            for (int i = 0; i < GetParameters<T>(obj).Length; i++)
            {
                var value = TakePartOfArray(arr, GetAllParametersByteSize<T>(obj, i), GetAllParametersByteSize<T>(obj, i + 1));

                var method = typeof(BitConverter).GetMethod(conversionTypes[i]);

                GetParameters<T>(obj)[i].SetValue(obj, method.Invoke(null, new object[] { value, 0 }));
            }

            return obj;
        }

        // This function is for the taking a part of array
        private static byte[] TakePartOfArray(byte[] arr, int first, int last)
        {
            byte[] tempArr = new byte[last - first];

            Array.Copy(arr, first, tempArr, 0, last - first);

            return tempArr;
        }

        // This function is for the getting all variables byte size of class instance
        private static int GetAllParametersByteSize<T>(T obj)
        {
            Type fieldsType = typeof(T);
            int topByteSize = 0;
            dynamic value;

            FieldInfo[] fields = fieldsType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < fields.Length; i++)
            {
                value = fields[i].GetValue(obj);

                topByteSize += BitConverter.GetBytes(value).Length;
            }

            return topByteSize;
        }

        // This function is for the getting byte size of variables of specific part class instance 
        private static int GetAllParametersByteSize<T>(T obj, int length)
        {
            Type fieldsType = typeof(T);
            int topByteSize = 0;
            dynamic value;

            FieldInfo[] fields = fieldsType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < length; i++)
            {
                value = fields[i].GetValue(obj);

                topByteSize += BitConverter.GetBytes(value).Length;
            }

            return topByteSize;
        }

        // This function is reaching all variables of class instance
        private static FieldInfo[] GetParameters<T>(T obj)
        {
            Type fieldsType = typeof(T);

            FieldInfo[] fields = fieldsType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            return fields;
        }
    }
}
