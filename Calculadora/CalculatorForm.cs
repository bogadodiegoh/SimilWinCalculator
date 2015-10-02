using System;
using System.Linq;
using System.Windows.Forms;

namespace Calculator
{
    public partial class CalculatorForm : Form
    {
        #region Properties

        private long numberOne { get; set; }
        private long numberTwo { get; set; }
        private decimal numberOneComma { get; set; }
        private decimal numberTwoComma { get; set; }
        private bool shouldClean { get; set; }
        private string op { get; set; }

        #endregion

        #region Public Methods

        public CalculatorForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        #region Numbers

        private void numberClick(object sender, EventArgs e)
        {
            if (this.txtOperacionesTexto.Text == "0")
                this.txtOperacionesTexto.Text = string.Empty;
            this.UpdateResult(((Button)sender).Text);
        }

        #endregion

        #region Operators

        private void operadoresClick(object sender, EventArgs e)
        {
            updateResultsWithOperators(((Button)sender).Text);
        }

        #endregion

        private void btnMC_Click(object sender, EventArgs e)
        {

        }

        private void btnMR_Click(object sender, EventArgs e)
        {

        }

        private void btnMS_Click(object sender, EventArgs e)
        {

        }

        private void btnMPlus_Click(object sender, EventArgs e)
        {

        }

        private void btnMMinus_Click(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            applyBack();
        }

        private void btnCE_Click(object sender, EventArgs e)
        {
            initializeResultCE();
        }

        private void C_Click(object sender, EventArgs e)
        {
            initializeResult();
        }

        private void btnPlusMinus_Click(object sender, EventArgs e)
        {
            if (this.txtResult.Text.IndexOf("-") == 0)
                this.txtResult.Text = this.txtResult.Text.Substring(1);
            else
            {
                if (this.txtResult.Text != "0")
                    this.txtResult.Text = "-" + txtResult.Text;
            }
        }

        private void btnSquared_Click(object sender, EventArgs e)
        {
            applySquared();
        }

        private void btnPorc_Click(object sender, EventArgs e)
        {
            if (this.txtResult.Text == "0" || this.txtOperacionesTexto.Text == string.Empty)        
            {
                this.txtResult.Text = "0";
                this.txtOperacionesTexto.Text = "0";            
            }            
            else
            {
                if (hasOperators())
                {
                    if (this.numberOne != 0)
	                {
                        var value = Convert.ToInt64(this.txtResult.Text);
                        var result =  (this.numberOne * value) / 100;
                        this.txtOperacionesTexto.Text += result;
                        this.txtResult.Text = result.ToString();
                    }                    
                }
            }
        }

        private void btnFrac_Click(object sender, EventArgs e)
        {

        }

        private void btnEqu_Click(object sender, EventArgs e)
        {
            var value = ((Button)sender).Text;
            if (hasOperators())
                Operate(value);
            if (this.txtOperacionesTexto.Text == "0" && this.txtResult.Text == "0")
                this.txtOperacionesTexto.Text = string.Empty;
        }

        private void btnComma_Click(object sender, EventArgs e)
        {
            this.UpdateResult(((Button)sender).Text);
        }

        private void CalculadoraForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            string value = e.KeyChar.ToString();

            switch (value)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    this.UpdateResult(value);
                    break;
                case "+":
                case "-":
                case "*":
                case "/":
                    updateResultsWithOperators(value);
                    break;
                case "=":
                    if (hasOperators())
                        Operate(value);
                    break;
                case "\b":
                    applyBack();
                    break;
            }
        }

        #endregion

        #region Private Methods

        private void initializeResultCE()
        {
            txtResult.Text = "0";
        }

        private void initializeResult()
        {
            txtResult.Text = "0";
            txtOperacionesTexto.Text = string.Empty;
        }

        private void Operate(string value)
        {
            if (this.txtResult.Text.Contains(",") || this.txtOperacionesTexto.Text.Contains(","))
            {
                decimal result = 0;

                if (endsWithOperator())
                {
                    if (this.numberTwoComma != 0)
                        result = getResult(this.numberOneComma, this.numberTwoComma,this.op);
                    else
                    {
                        if (value == "=")
                            result = getResult(this.numberOneComma, Convert.ToDecimal(this.txtResult.Text),this.op);
                    }
                }

                this.numberOneComma = 0;
                this.numberTwoComma = 0;
                this.txtResult.Text = result.ToString();
            }
            else
            {
                long result = 0;

                if (endsWithOperator())
                {
                    if (this.numberTwo != 0)
                    {
                        this.numberTwo = Convert.ToInt64(this.txtResult.Text);
                        result = getResult(this.numberOne, this.numberTwo,this.op);
                    }
                    else
                    {
                        if (value == "=")
                            result = getResult(this.numberOne, Convert.ToInt64(this.txtResult.Text),this.op);
                    }
                }
                else if (hasOperators())
                {
                    if (value == "=")
                        result = getResult(this.numberOne, Convert.ToInt64(this.txtResult.Text),this.op);
                }
                                                    
                this.numberOne = 0;
                this.numberTwo = 0;
                this.txtResult.Text = result.ToString();
                this.op = string.Empty;
            }

            this.txtOperacionesTexto.Text = string.Empty;
            this.shouldClean = true;
        }

        private long getResult(long numberOne, long numberTwo,string ope)
        {
            long result = 0;

            if (ope == "+")
                result = numberOne + numberTwo;

            if (ope == "-")
                result = numberOne - numberTwo;

            if (ope == "/")
            {
                if (numberTwo != 0)
                    result = numberOne / numberTwo;
                else
                    this.txtResult.Text = "Error";
            }

            if (ope == "*")
                result = numberOne * numberTwo;

            return result;
        }

        private decimal getResult(decimal numberOne, decimal numberTwo,string ope)
        {
            decimal result = 0;

            if (txtOperacionesTexto.Text.EndsWith("+"))
                result = numberOne + numberTwo;

            if (txtOperacionesTexto.Text.EndsWith("-"))
                result = numberOne - numberTwo;

            if (txtOperacionesTexto.Text.EndsWith("/"))
            {
                if (numberTwo != 0)
                    result = numberOne / numberTwo;
                else
                    this.txtResult.Text = "Error";
            }

            if (txtOperacionesTexto.Text.EndsWith("*"))
                result = numberOne * numberTwo;

            return result;
        }

        private bool hasOperators()
        {
            return textContains("+") || textContains("-") || textContains("*") || textContains("/");
        }

        private bool textContains(string op)
        {
            return txtOperacionesTexto.Text.Contains(op);
        }

        private bool endsWithOperator()
        {
            return textoEndsWith("+") || textoEndsWith("-") || textoEndsWith("*") || textoEndsWith("/");
        }

        private bool textoEndsWith(string op)
        {
            return txtOperacionesTexto.Text.EndsWith(op);
        }

        private void UpdateResult(string value)
        {
            if (shouldClean)
            {
                this.txtResult.Text = string.Empty;
                shouldClean = false;
            }

            if (this.txtResult.Text.Contains(",") || this.txtOperacionesTexto.Text.Contains(","))
            {
                if (endsWithOperator())
                {
                    if (value == ",")
                    {
                        if (!this.txtResult.Text.Contains(","))
                            this.txtResult.Text += value;
                    }
                    else
                    {
                        if (this.numberOneComma != 0)
                        {
                            if (this.numberTwoComma == 0)
                            {
                                this.txtResult.Text = string.Empty;
                                this.txtResult.Text = value;
                                this.numberTwoComma = Convert.ToDecimal(value);
                            }
                            else
                            {
                                this.txtResult.Text += value;
                                this.numberTwoComma = Convert.ToDecimal(this.txtResult.Text);
                            }
                        }
                    }
                }
                else
                {
                    if (this.txtResult.Text == "0" && value != ",")
                        this.txtResult.Text = value;
                    else
                    {
                        if (value == ",")
                        {
                            if (!this.txtResult.Text.Contains(","))
                                this.txtResult.Text += value;
                        }
                        else
                            this.txtResult.Text += value;
                    }
                }
            }
            else
            {
                if (endsWithOperator())
                {
                    if (this.numberOne != 0)
                    {
                        if (this.numberTwo == 0)
                        {
                            this.txtResult.Text = string.Empty;
                            this.txtResult.Text = value;
                            this.numberTwo = Convert.ToInt16(value);
                        }
                        else
                        {
                            this.txtResult.Text += value;
                            if (!this.txtResult.Text.Contains(','))                                                                                        
                                this.numberTwo = Convert.ToInt64(this.txtResult.Text);                                                     
                        }
                    }

                }
                else
                {
                    if (this.txtResult.Text == "0" && value != ",")
                        this.txtResult.Text = value;
                    else
                    {
                        if (value == ",")
                        {
                            if (!this.txtResult.Text.Contains(","))
                                this.txtResult.Text += value;
                        }
                        else
                            this.txtResult.Text += value;
                    }
                }
            }
        }

        private void updateResultsWithOperators(string value)
        {
            if (!endsWithOperator())
            {
                //if (this.txtResult.Text == "0")
                //{
                //    this.txtResult.Text += value;
                //    this.txtOperacionesTexto.Text = this.txtResult.Text + value;
                //}
                if (!this.txtOperacionesTexto.Text.EndsWith(value))
                {
                    if (this.txtResult.Text.Contains(","))
                        this.numberOneComma = Convert.ToDecimal(this.txtResult.Text);
                    else
                        this.numberOne = Convert.ToInt64(this.txtResult.Text);
                    this.txtOperacionesTexto.Text = this.txtResult.Text + value;
                    this.op = value;
                }
            }
            else
            {
                this.txtOperacionesTexto.Text += this.txtResult.Text + value;
                if (this.txtResult.Text.Contains(","))
                {
                    this.numberTwoComma = Convert.ToDecimal(this.txtResult.Text);
                    this.numberOneComma = getResult(this.numberOne, this.numberTwo,this.op);
                    this.numberTwoComma = 0;
                    this.txtResult.Text = this.numberOneComma.ToString();
                }
                else
                {                    
                    this.numberTwo = Convert.ToInt64(this.txtResult.Text);
                    this.numberOne = getResult(this.numberOne, this.numberTwo,this.op);
                    this.numberTwo = 0;
                    this.txtResult.Text = this.numberOne.ToString();
                }
                this.op = value;
            }
        }

        private void applyBack()
        {
            if (txtResult.Text != "0")
            {
                if (txtResult.Text.Length == 1)
                    initializeResult();
                else
                    txtResult.Text = txtResult.Text.Substring(0, txtResult.Text.Length - 1);
            }
        }

        private void applySquared()
        {
            double value = Convert.ToDouble(this.txtResult.Text);
            double result = Math.Sqrt(value);
            this.txtResult.Text = result.ToString();
            string sqrtText = string.Format("sqrt({0})", value);

            if (this.txtOperacionesTexto.Text != string.Empty)
                this.txtOperacionesTexto.Text += sqrtText;
            else
                this.txtOperacionesTexto.Text = sqrtText;
        }

        #endregion      

    }
}
