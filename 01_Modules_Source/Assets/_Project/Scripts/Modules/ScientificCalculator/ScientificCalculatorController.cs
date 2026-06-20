using System;
using TMPro;
using UnityEngine;

namespace OOPLab.Modules.ScientificCalculator
{
    public class ScientificCalculatorController : MonoBehaviour
    {
        [Header("Input Fields")]
        [SerializeField] private TMP_InputField firstNumberInput;
        [SerializeField] private TMP_InputField secondNumberInput;

        [Header("Output")]
        [SerializeField] private TMP_Text resultText;

        private readonly ScientificCalculator calculator = new ScientificCalculator();

        private void Start()
        {
            ShowResult("Result: 0");
        }

        public void Add()
        {
            RunBinaryOperation(calculator.Add);
        }

        public void Subtract()
        {
            RunBinaryOperation(calculator.Subtract);
        }

        public void Multiply()
        {
            RunBinaryOperation(calculator.Multiply);
        }

        public void Divide()
        {
            RunBinaryOperation(calculator.Divide);
        }

        public void Power()
        {
            RunBinaryOperation(calculator.Power);
        }

        public void Square()
        {
            RunSingleOperation(calculator.Square);
        }

        public void SquareRoot()
        {
            RunSingleOperation(calculator.SquareRoot);
        }

        public void Sin()
        {
            RunSingleOperation(calculator.Sin);
        }

        public void Cos()
        {
            RunSingleOperation(calculator.Cos);
        }

        public void Tan()
        {
            RunSingleOperation(calculator.Tan);
        }

        public void Log10()
        {
            RunSingleOperation(calculator.Log10);
        }

        public void Clear()
        {
            firstNumberInput.text = string.Empty;
            secondNumberInput.text = string.Empty;
            ShowResult("Result: 0");
        }

        private void RunBinaryOperation(Func<double, double, double> operation)
        {
            if (!TryGetFirstNumber(out double first) || !TryGetSecondNumber(out double second))
            {
                return;
            }

            TryShowOperationResult(() => operation(first, second));
        }

        private void RunSingleOperation(Func<double, double> operation)
        {
            if (!TryGetFirstNumber(out double first))
            {
                return;
            }

            TryShowOperationResult(() => operation(first));
        }

        private bool TryGetFirstNumber(out double value)
        {
            return TryReadNumber(firstNumberInput, "Enter first number.", out value);
        }

        private bool TryGetSecondNumber(out double value)
        {
            return TryReadNumber(secondNumberInput, "Enter second number.", out value);
        }

        private bool TryReadNumber(TMP_InputField inputField, string errorMessage, out double value)
        {
            if (inputField == null || !double.TryParse(inputField.text, out value))
            {
                ShowResult(errorMessage);
                value = 0;
                return false;
            }

            return true;
        }

        private void TryShowOperationResult(Func<double> operation)
        {
            try
            {
                double result = operation();
                string formattedResult = $"{result:0.####}";
                ShowResult($"Result: {formattedResult}");

                if (firstNumberInput != null)
                {
                    firstNumberInput.text = formattedResult;
                }
            }
            catch (Exception exception)
            {
                ShowResult(exception.Message);
            }
        }

        private void ShowResult(string message)
        {
            if (resultText != null)
            {
                resultText.text = message;
            }
        }
    }
}
