 
using System.Windows; 

namespace RegExpCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Calculator calcEquatObj;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            equation.Text = "";
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            string equat = equation.Text;
			
			if(equat.Length > 0)
				equation.Text = equat.Remove(equat.Length - 1, 1);
        }

        private void Num7_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += "7";
        }

        private void Num8_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += "8";
        }

        private void Num9_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += "9";
        }

        private void Num4_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += "4";
        }

        private void Num5_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += "5";
        }

        private void Num6_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += "6";
        }

        private void Num1_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += "1";
        }

        private void Num2_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += "2";
        }

        private void Num3_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += "3";
        }

        private void Num0_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += "0";
        }

        private void Dot_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += ".";
        }

        private void LeftBracket_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += "(";
        }

        private void RightBracket_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += ")";
        }

        private void Degree_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += "^";
        }

        private void Division_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += "/";
        }

        private void Multiply_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += "*";
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += "-";
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            equation.Text += "+";
        }
         
        private void EqualSign_Click(object sender, RoutedEventArgs e)
        {
            calcEquatObj = equation.Text;

            if (IsCurrectInput())
            {
                equation.Text = "Result: " + calcEquatObj.ToString();
            } 
        }

        private bool IsCurrectInput()
        {
            if (!calcEquatObj.CheckBracketsNotation())
            {
                MessageBox.Show("Count of opening and closing brackets must be the same.", "",MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            } 
            if (!calcEquatObj.CheckOperationsNotation())
            {
                MessageBox.Show("The operations between members in equation must be unambiguous.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!calcEquatObj.CheckOperationInBracketsNotation())
            {
                MessageBox.Show("Operations can't exist after openBracket or before closeBracket.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!calcEquatObj.CheckIncorrectCharacters())
            {
                MessageBox.Show("The equation exist incorrect characters.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!calcEquatObj.CheckCorrectFloatNumbers())
            {
                MessageBox.Show("The float numbers must have one dot and at least one num in mantissa.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
