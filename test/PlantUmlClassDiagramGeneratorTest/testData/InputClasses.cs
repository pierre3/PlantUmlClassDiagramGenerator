using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PlantUmlClassDiagramGeneratorTest
{
    public class ClassA
    {
        private readonly int intField = 100;
        private static string strField;
        protected double X = 0, Y = 1, Z = 2;
        private IList<int> list = new List<int>();

        protected int PropA { get; private set; }
        protected internal string PropB { get; protected set; }
        internal double PropC { get; } = 3.141592;
        public string PropD => "expression-bodied property";

        public ClassA() { }
        static ClassA() { strField = "static field"; }

        protected virtual void VirtualMethod() { }
        public override string ToString()
        {
            return intField.ToString();
        }

        public static string StaticMethod() { return strField; }
        public void ExpressonBodiedMethod(int x) => x * x;
    }

    internal abstract class ClassB
    {
        public ClassA publicA;
        private int field_1;
        public abstract int PropA { get; protected set; }

        protected virtual string VirtualMethod() { return "virtual"; }
        public abstract string AbstractMethod(int arg1, double arg2);
    }

    internal sealed class ClassC : ClassB, INotifyPropertyChanged
    {
        private static readonly string readonlyField = "ReadOnly";
        public override int PropA { get; protected set; } = 100;

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        public override string AbstractMethod(int arg1, double arg2)
        {
            return readonlyField;
        }

        protected override string VirtualMethod()
        {
            return base.VirtualMethod();
        }
    }

    public struct Vector
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector(Vector source)
            : this(source.X, source.Y, source.Z)
        { }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(
                a.X + b.X,
                a.Y + b.Y,
                a.Z + b.Z);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(
                a.X - b.X,
                a.Y - b.Y,
                a.Z - b.Z);
        }
    }

    [Flags]
    enum EnumA
    {
        AA = 0x0001,
        BB = 0x0002,
        CC = 0x0004,
        DD = 0x0008,
        EE = 0x0010
    }

    class NestedClass
    {
        public int A { get; }
        public InnerClass B { get; }
        public class InnerClass
        {
            public string X { get; } = "xx";
            public void MethodX() { }

            public struct InnerStruct
            {
                public int A { get; }
                public InnerStruct(int a)
                {
                    A = 0;
                }
            }
        }
    }
}
