using System.Windows.Forms;

namespace course_work
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            _Form1 = this;
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Файлы txt (*.txt)|*.txt";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader rdr = new StreamReader(fileDialog.FileName);
                string line = rdr.ReadToEnd();
                rdr.Close();
                richTextBox1.Text = line;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Analyzator s = new Analyzator(richTextBox1.Text);
            try
            {
                s.Parse();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                MessageBox.Show($"Error! {ex.Message}");
            }
            foreach (Lex a in s.lexes)
            {
                listBox1.Items.Add($"{a.Lexema}    :    {a.Type}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            textBox1.Clear();
            Analyzator s = new Analyzator(richTextBox1.Text);
            s.Parse();
            List<Token> tokens = new List<Token>();
            foreach (Lex a in s.lexes)
            {
                string currentLexem = a.Lexema;
                if (a.Type == "разделитель")
                {
                    if (Token.IsSpecialSymbol(Convert.ToChar(a.Lexema)))
                    {
                        Token token = new Token(Token.SS[Convert.ToChar(a.Lexema)]);
                        token.DCR = currentLexem;
                        tokens.Add(token);
                    }
                }
                else if (a.Type == "идентификатор")
                {
                    if (Token.IsSpecialWord(a.Lexema))
                    {
                        Token token = new Token(Token.SW[a.Lexema]);
                        token.DCR = currentLexem;
                        tokens.Add(token);
                    }
                    else
                    {
                        Token token = new Token(TokenType.ID);
                        token.Value = currentLexem;
                        token.DCR = currentLexem;
                        tokens.Add(token);
                    }
                }
                else
                {
                    Token token = new Token(TokenType.LIT);
                    token.Value = currentLexem;
                    token.DCR = currentLexem;
                    tokens.Add(token);
                }
            }
            foreach (Token a in tokens)
            {
                listBox1.Items.Add($"{a.Type}    :    {a.Value}");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            textBox1.Clear();
            Analyzator s = new Analyzator(richTextBox1.Text);
            s.Parse();
            List<Token> tokens = new List<Token>();
            foreach (Lex a in s.lexes)
            {
                string currentLexem = a.Lexema;
                if (a.Type == "разделитель")
                {
                    if (Token.IsSpecialSymbol(Convert.ToChar(a.Lexema)))
                    {
                        Token token = new Token(Token.SS[Convert.ToChar(a.Lexema)]);
                        token.DCR = currentLexem;
                        tokens.Add(token);
                    }
                }
                else if (a.Type == "идентификатор")
                {
                    if (Token.IsSpecialWord(a.Lexema))
                    {
                        Token token = new Token(Token.SW[a.Lexema]);
                        token.DCR = currentLexem;
                        tokens.Add(token);
                    }
                    else
                    {
                        Token token = new Token(TokenType.ID);
                        token.Value = currentLexem;
                        token.DCR = currentLexem;
                        tokens.Add(token);
                    }
                }
                else
                {
                    Token token = new Token(TokenType.LIT);
                    token.Value = currentLexem;
                    token.DCR = currentLexem;
                    tokens.Add(token);
                }
            }
            foreach (Token a in tokens)
            {
                listBox1.Items.Add($"{a.Type}    :    {a.Value}");
            }

            LR lR = new LR(tokens);
            lR.Start();

            //Deycstra deycstra = new Deycstra();
            //deycstra.Start();
        }

        public static Form1 _Form1;

        public void update(string message)
        {
            textBox1.Text += message + Environment.NewLine;
        }

        public void update1()
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}