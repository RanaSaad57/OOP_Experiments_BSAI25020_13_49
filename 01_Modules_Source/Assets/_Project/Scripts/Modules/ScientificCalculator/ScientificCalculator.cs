using System;

namespace OOPLab.Modules.ScientificCalculator
{
    public class ScientificCalculator
    {
        public double Add(double left, double right)
        {
            return left + right;
        }

        public double Subtract(double left, double right)
        {
            return left - right;
        }

        public double Multiply(double left, double right)
        {
            return left * right;
        }

        public double Divide(double left, double right)
        {
            if (right == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero.");
            }

            return left / right;
        }

        public double Power(double value, double power)
        {
            return Math.Pow(value, power);
        }

        public double Square(double value)
        {
            return value * value;
        }

        public double SquareRoot(double value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Cannot calculate square root of a negative number.");
            }

            return Math.Sqrt(value);
        }

        public double Sin(double degrees)
        {
            return Math.Sin(DegreesToRadians(degrees));
        }

        public double Cos(double degrees)
        {
            return Math.Cos(DegreesToRadians(degrees));
        }

        public double Tan(double degrees)
        {
            return Math.Tan(DegreesToRadians(degrees));
        }

        public double Log10(double value)
        {
            if (value <= 0)
            {
                throw new ArgumentException("Log value must be greater than zero.");
            }

            return Math.Log10(value);
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
