using iiweessOS.Models;
using iiweessOS.Utils;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace iiweessOS
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private readonly string _prompt;
        private string _currentInput = "";

        public MainForm()
        {
            Config config = null;
            FileSystemModel fs = new FileSystemModel();

            try
            {
                config = ConfigParser.LoadConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "config.yml"));
                fs.LoadFromTar(config.Filesystem);
            } catch (FileNotFoundException e)
            {
                MessageBox.Show(e.Message, "Error while finding file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            StyleForm();
            InitializeComponent();
            InitPanel();

            this.Resize += MainForm_Resize;
            this.SizeChanged += MainForm_SizeChanged;
            terminalTextBox.KeyPress += terminalTextBox_KeyPress;

            _prompt = $"{config.User}@emulator:~$ ";

            DisplayPrompt();
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            UpdateMaximizeButton();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            UpdateMaximizeButton();
        }
        private void UpdateMaximizeButton()
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                resizeButton.Text = "🗗";
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                resizeButton.Text = "🗖";
            }
        }

        private void InitPanel()
        {
            this.MouseDown += new MouseEventHandler(Form_MouseDown);
            headPanel.MouseDown += new MouseEventHandler(Form_MouseDown);
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void StyleForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void collapseButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void resizeButton_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
            UpdateMaximizeButton();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void DisplayPrompt()
        {
            AppendText(_prompt);
        }

        private void AppendText(string text)
        {
            terminalTextBox.AppendText(text);
            terminalTextBox.SelectionStart = terminalTextBox.Text.Length;
            terminalTextBox.ScrollToCaret();
        }

        private void terminalTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar))
            {
                _currentInput += e.KeyChar;
                AppendText(e.KeyChar.ToString());
                e.Handled = true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                ExecuteCurrentInput();
                return true;
            }

            if (keyData == Keys.Back && _currentInput.Length > 0)
            {
                _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
                terminalTextBox.Text = terminalTextBox.Text.Substring(0, terminalTextBox.Text.Length - 1);

                terminalTextBox.SelectionStart = terminalTextBox.Text.Length;
                terminalTextBox.ScrollToCaret();

                return true;
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        private void ExecuteCurrentInput()
        {
            AppendText("\n");
            var result = _currentInput;
            if (result == "exit")
            {
                Application.Exit();
                return;
            }
            if (result != "") 
                AppendText(result + "\n");  
            _currentInput = "";
            DisplayPrompt();
        }
    
    }
}
