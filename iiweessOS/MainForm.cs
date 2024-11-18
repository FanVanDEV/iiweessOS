using iiweessOS.Controllers;
using iiweessOS.Models;
using iiweessOS.Utils;
using System;
using System.Drawing;
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

        private string _prompt;
        private string _currentInput = "";

        private readonly FileSystemModel _fs = null;
        private readonly Config _config = null;
        private readonly ShellController _shellController = null;

        public MainForm()
        {       
            try
            {
                _config = ConfigParser.LoadConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "config.yml"));

                _fs = new FileSystemModel(_config.User == "root" ? "/root" : $"/home/{_config.User}");
                _fs.LoadFromTar(_config.Filesystem);
                _fs.ChangeDirectory("~");

                _shellController = new ShellController(new CommandFactory(_fs));
            }
            catch (FileNotFoundException e)
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

            UpdatePrompt();
            DisplayPrompt();
        }

        private void UpdatePrompt()
        {
            string currentDirectory = "/" + _fs.GetCurrentDirectory();
            string path = currentDirectory; 

            if (_config.User == "root" && currentDirectory.StartsWith("/root")
                )
            {
                path = "~" + currentDirectory
                    .Substring(5);
            } else if (_config.User != "root" && currentDirectory.StartsWith("/home/" + _config.User))
            {
                int dirLength = $"/home/{_config.User}".Length;

                path = "~" + currentDirectory
                    .Substring(dirLength);
            }

            path = path.Substring(0, path.Length - 1);

            string symbol = _config.User == "root" ? "#" : "$";

            _prompt = $"{_config.User}@emulator:{path}{symbol} ";
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

        private void InsertText(string text, int position)
        {
            _currentInput = _currentInput.Insert(position - (terminalTextBox.Text.Length - _currentInput.Length), text);

            terminalTextBox.Text = terminalTextBox.Text.Insert(position, text);

            terminalTextBox.SelectionStart = position + 1;
            terminalTextBox.SelectionLength = 0;
        }

        private void AppendText(string text)
        {
            terminalTextBox.AppendText(text);
            terminalTextBox.SelectionStart = terminalTextBox.Text.Length;
            terminalTextBox.ScrollToCaret();
        }

        private void SetText(string text)
        {
            terminalTextBox.Text = text;
            terminalTextBox.SelectionStart = terminalTextBox.Text.Length;
            terminalTextBox.ScrollToCaret();
        }

        private void RemoveSymbol(int cursorPosition)
        {
            _currentInput = _currentInput.Remove(cursorPosition - (terminalTextBox.Text.Length - _currentInput.Length) - 1, 1);
            terminalTextBox.Text = terminalTextBox.Text.Remove(cursorPosition - 1, 1);

            terminalTextBox.SelectionStart = cursorPosition - 1;
            terminalTextBox.ScrollToCaret();
        }

        private void terminalTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar))
            {
                int cursorPosition = terminalTextBox.SelectionStart;

                if (cursorPosition < terminalTextBox.Text.Length - _currentInput.Length)
                {
                    e.Handled = true;
                    return;
                }

                InsertText(e.KeyChar.ToString(), cursorPosition);

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

            if (keyData == Keys.Left)
            {
                int cursorPosition = terminalTextBox.SelectionStart;
                
                if (cursorPosition <= terminalTextBox.Text.Length - _currentInput.Length)
                {
                    terminalTextBox.SelectionStart = cursorPosition + 1;
                    terminalTextBox.ScrollToCaret();
                    return false;
                }
            }

            if (_currentInput.Length > 0)
            {
                if (keyData == Keys.Back)
                {
                    int cursorPosition = terminalTextBox.SelectionStart;

                    RemoveSymbol(cursorPosition);

                    return true;
                }

                if (keyData == Keys.Delete)
                {
                    int cursorPosition = terminalTextBox.SelectionStart;

                    if (cursorPosition - (terminalTextBox.Text.Length - _currentInput.Length) + 1 > _currentInput.Length) return false;

                    RemoveSymbol(cursorPosition + 1);

                    return true;
                }
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ExecuteCurrentInput()
        {
            AppendText("\n");

            if (_currentInput.Length != 0)
            {
                string result = _shellController.ExecuteCommand(_currentInput);
                switch (result)
                {
                    case "exit":
                        Application.Exit();
                        return;
                    case "clear":
                        SetText("");
                        break;
                    default:
                        if (result.Length > 0)
                            AppendText(result + "\n");
                        break;
                }
            }

            _currentInput = "";
            UpdatePrompt();
            DisplayPrompt();
        }

    }
}
