using System; 
using System.Text.RegularExpressions;

namespace RegExpCalc
{
    public class Calculator
    {
        #region RegExp patterns for check input

        readonly string _patUnambiguousOp = @"(([-+*/\^][+*/\^])|([-][-]))";
        readonly string _patOpInBrackets = @"(([(][+*/\^])|([-+*/\^][)]))";
        readonly string _patOpIncorrectSymb = @"[^\d\-+/*\^\(\)\.]";
        readonly string _patBadFloatNum = @"((\d+\.\d+\.)|(\d+\.[\-+/*\^\(\)\.]))";

        #endregion

        #region RegExp patterns for solving equation

        const string patFloatNumber = @"(-?\d+(?:\.\d+)?)(?:[eE][+\-]\d+)?";

        readonly string _patOpDegree = @"(" + patFloatNumber + @"[\^]" + patFloatNumber + ")";
        readonly string _patOpMul =    @"(" + patFloatNumber + @"[*]" + patFloatNumber + ")";
        readonly string _patOpDiv =    @"(" + patFloatNumber + @"[/]" + patFloatNumber + ")";
        readonly string _patOpMinus =  @"(" + patFloatNumber + @"[-]" + patFloatNumber + ")";
        readonly string _patOpPlus =   @"(" + patFloatNumber + @"[+]" + patFloatNumber + ")";

        readonly string _patExistEquatInBrackets = @"\b*\(\S+\)\b*";
        readonly string _patNextBrackets = @"(?<=\()([^()]+)(?=\))";

        readonly string _patMinusBeforeDegMulDiv = @"([^+*/\(\-\^])(-\d+(?:\.\d+)?[*/\^]-?\b*\(?)";
        string _patMinusBeforeBrackets = @"(-)(\()";

        #endregion

        string _equation = "";

        public Calculator()
        {
            _equation = "0";
        }

        public Calculator(string equation)
        {
            _equation = equation;
        }

        private string SolveEquation(string equation)
        {
            if (!ExistBrackets(equation))
                return ResultOfSimpleEquat(equation);

            equation = ConvertMinusToPlusMinus(equation);

            return SolveEquation(SolveNextNestedBrackets(equation));
        }

        private bool ExistBrackets(string input) =>
            new Regex(_patExistEquatInBrackets).IsMatch(input);

        private string ResultOfSimpleEquat(string equat)
        {
            string partEquat = GetPartEquation(equat);

            if (partEquat != "")
            {
                equat = equat.Replace(partEquat, GetResOperation(partEquat));
                return ResultOfSimpleEquat(equat);
            }

            return equat;
        }

        private string ConvertMinusToPlusMinus(string input)
        =>
            new Regex(_patMinusBeforeDegMulDiv).IsMatch(input)
                ? Regex.Replace(input, _patMinusBeforeDegMulDiv, @"$1+$2")
                : input;

        private string SolveNextNestedBrackets(string equat)
        {
            string equatInNestedBrackets = GetNextNestedEquation(equat);

            string res = ResultOfSimpleEquat(equatInNestedBrackets);

            equat = equat.Replace("(" + equatInNestedBrackets + ")", res);

            return equat;
        }

        private string GetPartEquation(string equat)
        {
            string pieceEquat = "";

            if (pieceEquat == "")
                pieceEquat = new Regex(_patOpDegree).Match(equat).Groups[1].Value;

            if (equat.Contains("*") && equat.Contains("/"))
            {
                if (equat.IndexOf("*") < equat.IndexOf("/")) 
                    pieceEquat = new Regex(_patOpMul).Match(equat).Groups[1].Value;
                else 
                    pieceEquat = new Regex(_patOpDiv).Match(equat).Groups[1].Value;
            }
            else
            {
                if (pieceEquat == "")
                    pieceEquat = new Regex(_patOpMul).Match(equat).Groups[1].Value;

                if (pieceEquat == "")
                    pieceEquat = new Regex(_patOpDiv).Match(equat).Groups[1].Value;
            }
             
            if (pieceEquat == "")
                pieceEquat = new Regex(_patOpMinus).Match(equat).Groups[1].Value;

            if (pieceEquat == "")
                pieceEquat = new Regex(_patOpPlus).Match(equat).Groups[1].Value;

            return pieceEquat;
        }

        private string GetResOperation(string str)
        {
            string op = new Regex(@"\d+([+*/\^\-])").Match(str).Groups[1].Value;

            string[] tempNums = Regex.Split(str, @"(?<=\d)[\" + op + "]");

            double res = 0;
            switch (op)
            {
                case "+":
                    res = double.Parse(tempNums[0]) + double.Parse(tempNums[1]);
                    break;
                case "-":
                    res = double.Parse(tempNums[0]) - double.Parse(tempNums[1]);
                    break;
                case "*":
                    res = double.Parse(tempNums[0]) * double.Parse(tempNums[1]);
                    break;
                case "/":
                    res = double.Parse(tempNums[0]) / double.Parse(tempNums[1]);
                    break;
                case "^":
                    res = Math.Pow(double.Parse(tempNums[0]), double.Parse(tempNums[1]));
                    break;
                default:
                    Console.WriteLine("No match op: " + op + " " + str);//del
                    break;
            }
            return res.ToString();
        }

        private string GetNextNestedEquation(string equat) =>
            new Regex(_patNextBrackets).Match(equat).Value;

        public bool CheckBracketsNotation() =>
            Regex.Matches(_equation, @"\(").Count == Regex.Matches(_equation, @"\)").Count;

        public bool CheckOperationsNotation() =>
            new Regex(_patUnambiguousOp).IsMatch(_equation) == false;

        public bool CheckOperationInBracketsNotation() =>
            new Regex(_patOpInBrackets).IsMatch(_equation) == false;

        public bool CheckIncorrectCharacters() =>
            new Regex(_patOpIncorrectSymb).IsMatch(_equation) == false;

        public bool CheckCorrectFloatNumbers() =>
            new Regex(_patBadFloatNum).IsMatch(_equation) == false;

        private static string RemoveSpaceSymb(string input) => Regex.Replace(input, @"\s", "");

        private void ParseToConvenientForm()
        {
            _equation = Regex.Replace(_equation, @"\s", "");

            if (new Regex(_patMinusBeforeBrackets).IsMatch(_equation))
                _equation = Regex.Replace(_equation, _patMinusBeforeBrackets, @"-1*$2");
        }

        public static implicit operator Calculator(string inputEquat) => new Calculator(RemoveSpaceSymb(inputEquat));

        public override string ToString()
        {
            ParseToConvenientForm();

            return SolveEquation(_equation);
        }  
    }
}
