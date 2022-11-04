using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;

namespace App1
{
    [Activity(Label = "SimpleCalc", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private TextView calculatorText;
        private TextView calculatorTextHex;
        private TextView calculatorTextBin;

        bool operationFlag = false;

        private string[] passNumbers = new string[2];
        private string @operator;

        private const string MathOpAdd = "+";
        private const string MathOpSub = "-";
        private const string MathOpMult = "x";
        private const string MathOpDiv = "/";
        private const string MathOpExponen2 = "a^2";
        private const string MathOpExponen3 = "a^3";
        private const string MathOpDeflect = "1/x";
        private const string MathOpRoot2 = "sqr^2";
        private const string MathOpRoot3 = "sqe^3";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            calculatorText = FindViewById<TextView>(Resource.Id.clcTextViewDec);
            calculatorTextHex = FindViewById<TextView>(Resource.Id.clcTextViewHex);
            calculatorTextBin = FindViewById<TextView>(Resource.Id.clcTextViewBin);
        }

        private bool checkIfBtnDecOrPoint(string btnText)
        {
            if ("0123456789.".Contains(btnText))
            {
                return true;
            }
            return false;
        }

        private void clearAllValues()
        {
            if (operationFlag == true)
            {
                passNumbers[0] = null;
                passNumbers[1] = null;
                @operator = null;
                CalcTextUpdate();
                operationFlag = false;
            }
        }

        private bool checkIfNumberIsEmpty(string numberToCheck)
        {
            if (passNumbers[0] == null || passNumbers[0] == "")
            {
                return true;
            }

            return false;
        }

        private bool checkIfBasicOperator(string btnText)
        {
            if("/x+-".Contains(btnText))
            {
                return true;
            }
            return false;
        }

        private bool checkIfEqualSignClicked(string btnText)
        {
            if ("=" == btnText)
            {
                return true;
            }
            return false;
        }

        private bool checkIfSpecialOperator(string btnText)
        {
            string[] table = new string[] { "a^2", "a^3", "1/x", "sqr^2", "sqr^3" };

            for(uint loop = 0; loop < table.Length; loop++)
            {
                if (table[loop] == btnText)
                {
                    return true;
                }
            }

            return false;
        }

        private bool checkIfMultipleZeroClicked(string btnText)
        {
            if("00" == btnText || "000" == btnText)
            {
                return true;
            }
            return false;
        }

        private bool checkIfBackSignClicked(string btnText)
        {
            if("bac" == btnText)
            {
                return true;
            }
            return false;
        }

        private void decBtnClickedMethod(string btnText)
        {
            clearAllValues();

            AddDigitOrDecimalPoint(btnText);
        }

        private void basicOperatorBtnClickedMethod(string btnText)
        {
            AddOperator(btnText);
            operationFlag = false;
        }

        private void equalOoperatorBtnClickedMethod()
        {
            CalculateData();
            operationFlag = true;
        }

        private void specialOperatorBtnClickedMethod(string btnText, string passNumber)
        {
            if (checkIfNumberIsEmpty(passNumbers[0]))
            {
                return;
            }

            @operator = btnText;
            CalculateData();
            operationFlag = true;
        }

        private void multipleZerosBtnClickedMethod(string btnText)
        {
            clearAllValues();

            AddDigitOrDecimalPoint(btnText);
        }

        private void clearLastDigitBtnClickedMethod()
        {
            int index = @operator == null ? 0 : 1;

            if (passNumbers[index].Length > 0)
            {
                passNumbers[index] = passNumbers[index].Remove(passNumbers[index].Length - 1);
                CalcTextUpdate();
            }
        }

        [Java.Interop.Export("ButtonClick")]
        public void ButtonClick(View v)
        {
            Button button = (Button)v;

            if(checkIfBtnDecOrPoint(button.Text))
            {
                decBtnClickedMethod(button.Text);
            }
            else if(checkIfBasicOperator(button.Text))
            {
                basicOperatorBtnClickedMethod(button.Text);
            }
            else if(checkIfEqualSignClicked(button.Text))
            {
                equalOoperatorBtnClickedMethod();
            }
            else if(checkIfSpecialOperator(button.Text))
            {
                specialOperatorBtnClickedMethod(button.Text, passNumbers[0]);
            }
            else if(checkIfMultipleZeroClicked(button.Text))
            {
                multipleZerosBtnClickedMethod(button.Text);
            }
            else if(checkIfBackSignClicked(button.Text))
            {
                clearLastDigitBtnClickedMethod();
            }
            else
            {
                clearScreen();
            }
        }

        private void clearScreen()
        {
            passNumbers[0] = passNumbers[1] = null;
            @operator = null;
            CalcTextUpdate();
        }

        private void setDataIfNoResult(double? result, string newOperator)
        {
            if (result != null)
            {
                passNumbers[0] = result.ToString();
                @operator = newOperator;
                passNumbers[1] = null;
                CalcTextUpdate();
            }
        }

        private void CalculateData(string newOperator = null)
        {
            double? result = null;
            double? first = passNumbers[0] == null ? null : (double?)double.Parse(passNumbers[0]);
            double? second = passNumbers[1] == null ? null : (double?)double.Parse(passNumbers[1]);

            switch (@operator)
            {
                case MathOpAdd:
                    result = first + second;
                    break;
                case MathOpDiv:
                    result = first / second;
                    break;
                case MathOpMult:
                    result = first * second;
                    break;
                case MathOpSub:
                    result = first - second;
                    break;
                case MathOpExponen2:
                    result = first * first;
                    break;
                case MathOpExponen3:
                    result = first * first * first;
                    break;
                case MathOpDeflect:
                    result = 1 / first;
                    break;
                case MathOpRoot2:
                    result = Math.Sqrt((double)first);
                    break;
                case MathOpRoot3:
                    result = Math.Pow((double)first, (1.0 / 3.0));
                    break;
            }

            setDataIfNoResult(result, newOperator);
        }

        private void AddOperator(string value)
        {
            if(passNumbers[1] != null)
            {
                CalculateData(value);
            }
              
            @operator = value;
            CalcTextUpdate();
        }

        private void AddDigitOrDecimalPoint(string value)
        {
            int index = @operator == null ? 0 : 1;

            if(value == "." && passNumbers[index].Contains("."))
            {
                return;
            }

            passNumbers[index] += value;

            CalcTextUpdate();
        }

        private void CalcTextUpdate()
        {
            int number1Tmp = 0;
            int number2Tmp = 0;
            string number1TmpSt = "";
            string number2TmpSt = "";

            if (int.TryParse(passNumbers[0], out number1Tmp))
            {
                number1TmpSt = number1Tmp.ToString("X");
            }

            if (int.TryParse(passNumbers[1], out number2Tmp))
            {
                number2TmpSt = number2Tmp.ToString("X");
            }

            calculatorText.Text = $"{passNumbers[0]} {@operator} {passNumbers[1]}";
            calculatorTextHex.Text = "Hex:" + $"{number1TmpSt} {@operator} {number2TmpSt}";
            calculatorTextBin.Text = "Bin:" + Convert.ToString(number1Tmp, 2) + $"{@operator}" + Convert.ToString(number2Tmp, 2);
        }
    }
}

