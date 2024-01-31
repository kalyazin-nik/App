using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Net.Mail;
using Database;
using Entities;
using System.Windows.Forms;

namespace Forms.Forms;

internal class CreateNewEmployeeForm : Form
{
    private DataContext _dataContext = null!;
    private Panel _panel = null!;
    private List<Label> _labels = null!;
    private ComboBox _post = null!;
    private TextBox[] _textBoxes = null!;
    private DateTimePicker _birthDay = null!;
    private MaskedTextBox _phoneNumber = null!;
    private TextBox _mailAddress = null!;
    private ComboBox _familyStatus = null!;
    private TextBox _address = null!;
    private TextBox _hobbies = null!;
    private Button _saveButton = null!;
    private Stack<int> _errorsCounter;

    public CreateNewEmployeeForm(DataContext context)
    {
        CustomizeForm();
        MakePanel();
        MakeLables();
        MakePost();
        MakeTextBoxes();
        MakeBirthDay();
        MakePhoneNumber();
        MakeMailAddress();
        MakeFamilyStatus();
        MakeAddress();
        MakeHobbies();
        MakeSaveButton();

        _dataContext = context;
        _errorsCounter = [];
        for (int i = 0; i < 10; i++)
            _errorsCounter.Push(1);
    }

    private void CustomizeForm()
    {
        Text = "Добавление в базу данных";
        Size = new Size(600, 860);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        TopMost = true;
    }

    private void MakePanel()
    {
        _panel = new() { Location = new(0, 0), Size = new(600, 710), BackColor = Color.White };
        _panel.Paint += new PaintEventHandler(DrawDividingLine!);
        Controls.Add(_panel);
    }

    private void MakeLables()
    {
        var labelSize = new Size(180, 30);

        _labels =
        [
            new() { Text = "Занимаемая должность", Location = new Point(30, 30), Size = labelSize },
            new() { Text = "Фамилия", Location = new Point(30, 80), Size = labelSize },
            new() { Text = "Имя", Location = new Point(30, 130), Size = labelSize },
            new() { Text = "Отчество", Location = new Point(30, 180), Size = labelSize },
            new() { Text = "Дата рождения", Location = new Point(30, 230), Size = labelSize },
            new() { Text = "Телефонный номер", Location = new Point(30, 280), Size = labelSize },
            new() { Text = "Электронная почта", Location = new Point(30, 330), Size = labelSize },
            new() { Text = "Семейное положение", Location = new Point(30, 380), Size = labelSize },
            new() { Text = "Город", Location = new Point(30, 430), Size = labelSize },
            new() { Text = "Адрес", Location = new Point(30, 480), Size = labelSize },
            new() { Text = "Увлечения", Location = new Point(30, 580), Size = labelSize },
            new()
            {
                Text = "Пример: ул Ленина, д 52, кв 67",
                Font = new Font("Tahoma", 8, FontStyle.Regular),
                Location = new Point(250, 555),
                Size = new Size(350, 30),
                ForeColor = Color.Gray
            },
            new()
            {
                Text = "Перечисления выполнять через запятую",
                Font = new Font("Tahoma", 8, FontStyle.Regular),
                Location = new Point(250, 655),
                Size = new Size(350, 30),
                ForeColor = Color.Gray
            },
        ];

        for (int i = 0, y = 30; i < 10; i++, y += 50)
            _labels.Add(
                new() { Text = "*", Name = $"error {i}", Location = new Point(235, y), AutoSize = true, ForeColor = Color.Red });

        _panel.Controls.AddRange(_labels.ToArray());
    }

    private void MakePost()
    {
        _post = new()
        {
            Name = "0",
            Location = new Point(250, 30),
            Size = new Size(300, 30),
            DropDownWidth = 300,
            DropDownStyle = ComboBoxStyle.DropDownList,
            TabIndex = 0,
            AutoSize = false
        };

        _post.Items.AddRange(GetPositions());
        _post.Enter += ErrorLabelToTransparent!;
        _post.Leave += ErrorLabelToRed!;

        _panel.Controls.Add(_post);
    }

    private void MakeTextBoxes()
    {
        var textBoxesSize = new Size(300, 30);
        _textBoxes =
        [
            new() { Name = "1", Location = new Point(250, 80), Size = textBoxesSize, TabIndex = 1, AutoSize = false },
            new() { Name = "2", Location = new Point(250, 130), Size = textBoxesSize, TabIndex = 2, AutoSize = false },
            new() { Name = "3", Location = new Point(250, 180), Size = textBoxesSize, TabIndex = 3, AutoSize = false },
            new() { Name = "8", Location = new Point(250, 430), Size = textBoxesSize, TabIndex = 8, AutoSize = false },
        ];

        foreach (var textBox in _textBoxes)
        {
            textBox.KeyPress += new KeyPressEventHandler(CorrectKeyPress!);
            textBox.Enter += ErrorLabelToTransparent!;
            textBox.Leave += ErrorLabelToRed!;
        }

        _panel.Controls.AddRange(_textBoxes);
    }

    private void MakeBirthDay()
    {
        _birthDay = new()
        {
            Name = "4",
            Location = new Point(250, 230),
            Size = new Size(300, 30),
            TabIndex = 4,
            AutoSize = false,
            MinDate = new DateTime(1950, 01, 01),
            CustomFormat = "dd MMMM yyyy",
            Format = DateTimePickerFormat.Custom
        };

        _birthDay.Enter += ErrorLabelToTransparent!;
        _birthDay.Leave += CorrectBirthDay!;

        _panel.Controls.Add(_birthDay);
    }

    private void MakePhoneNumber()
    {
        _phoneNumber = new()
        {
            Name = "5",
            Location = new Point(250, 280),
            Size = new Size(300, 30),
            Mask = "+7(000)000-00-00",
            TabIndex = 5,
            AutoSize = false
        };

        _phoneNumber.Enter += ErrorLabelToTransparent!;
        _phoneNumber.Leave += CorrectPhoneNumber!;

        _panel.Controls.Add(_phoneNumber);
    }

    private void MakeMailAddress()
    {
        _mailAddress = new() { Name = "6", Location = new Point(250, 330), Size = new Size(300, 30), TabIndex = 6, AutoSize = false };

        _mailAddress.Enter += ErrorLabelToTransparent!;
        _mailAddress.Leave += CorrectMailAddress!;

        _panel.Controls.Add(_mailAddress);
    }

    private void MakeFamilyStatus()
    {
        _familyStatus = new()
        {
            Name = "7",
            Location = new Point(250, 380),
            Size = new Size(300, 30),
            DropDownWidth = 300,
            DropDownStyle = ComboBoxStyle.DropDownList,
            TabIndex = 7,
            AutoSize = false
        };

        _familyStatus.Items.AddRange(["Женат", "Замужем", "Не женат", "Не замужем"]);
        _familyStatus.Enter += ErrorLabelToTransparent!;
        _familyStatus.Leave += ErrorLabelToRed!;

        _panel.Controls.Add(_familyStatus);
    }

    private void MakeAddress()
    {
        _address = new()
        {
            Name = "9",
            Location = new Point(250, 480),
            Size = new Size(300, 70),
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            TabIndex = 9,
        };

        _address.Enter += ErrorLabelToTransparent!;
        _address.Leave += ErrorLabelToRed!;

        _panel.Controls.Add(_address);
    }

    private void MakeHobbies()
    {
        _hobbies = new()
        {
            Location = new Point(250, 580),
            Size = new Size(300, 70),
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            TabIndex = 10
        };

        _panel.Controls.Add(_hobbies);
    }

    private void MakeSaveButton()
    {
        _saveButton = new() { Location = new Point(400, 735), Size = new Size(130, 50), Text = "Сохранить" };

        _saveButton.Click += CreateEmployee!;

        Controls.Add(_saveButton);
    }

    private void DrawDividingLine(object sender, PaintEventArgs e)
    {
        e.Graphics.DrawLine(new Pen(Color.LightGray, 2), new Point(220, 20), new Point(220, 670));
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
    }

    private void CorrectKeyPress(object sender, EventArgs args)
    {
        var textBox = sender as TextBox;
        var keyPress = args as KeyPressEventArgs;

        if (textBox is not null && keyPress is not null)
        {
            if (textBox.Text.Length == 0)
            {
                if ((keyPress.KeyChar >= 97 && keyPress.KeyChar <= 122) ||
                    (keyPress.KeyChar >= 1072 && keyPress.KeyChar <= 1103) ||
                    keyPress.KeyChar == 1105 || keyPress.KeyChar == 1025)
                    keyPress.KeyChar -= (char)32;
                else if (!((keyPress.KeyChar >= 65 && keyPress.KeyChar <= 90) ||
                    (keyPress.KeyChar >= 1040 && keyPress.KeyChar <= 1071)))
                    keyPress.KeyChar = '\0';
            }
            else
            {
                if (!(keyPress.KeyChar == 8 || keyPress.KeyChar == 15 ||
                    keyPress.KeyChar == 1105 || keyPress.KeyChar == 1025 ||
                    (keyPress.KeyChar >= 65 && keyPress.KeyChar <= 90) ||
                    (keyPress.KeyChar >= 97 && keyPress.KeyChar <= 122) ||
                    (keyPress.KeyChar >= 1040 && keyPress.KeyChar <= 1071) ||
                    (keyPress.KeyChar >= 1072 && keyPress.KeyChar <= 1103)))
                    keyPress.KeyChar = '\0';
            }
        }
    }

    private async void CorrectBirthDay(object sender, EventArgs args)
    {
        if (sender is DateTimePicker dateTimePicker && dateTimePicker.Value is DateTime birthDay)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDay.Year;
            if (birthDay.Date > today.AddYears(-age)) age--;
            if (age < 18) ErrorLabelToRed(sender, args);
            await Task.CompletedTask;
        }
    }

    private async void CorrectPhoneNumber(object sender, EventArgs args)
    {
        if (sender is MaskedTextBox phoneNumber)
            if (phoneNumber.Text.Length < 16)
            {
                phoneNumber.Text = "";
                ErrorLabelToRed(sender, args);
            }

        await Task.CompletedTask;
    }

    private async void CorrectMailAddress(object sender, EventArgs args)
    {
        if (sender is TextBox mail)
            try
            {
                _ = new MailAddress(mail.Text);
            }
            catch (Exception)
            {
                mail.Text = "";
                ErrorLabelToRed(sender, args);
            }

        await Task.CompletedTask;
    }

    private async void ErrorLabelToRed(object sender, EventArgs args)
    {
        if (sender is Control control)
            if (await SwitchErrorColor(control, Color.Red, control.Name))
                _errorsCounter.Push(1);
    }

    private async void ErrorLabelToTransparent(object sender, EventArgs args)
    {
        if (sender is Control control)
            if (await SwitchErrorColor(control, Color.Transparent, control.Name))
                _errorsCounter.Pop();
    }

    private async Task<bool> SwitchErrorColor(Control control, Color color, string name)
    {
        if (control.Text == "" || (control is MaskedTextBox && control.Text.Length < 16))
        {
            _panel.Controls[$"error {name}"]!.ForeColor = color;
            await Task.CompletedTask;
            return true;
        }

        await Task.CompletedTask;
        return false;
    }

    private void CreateEmployee(object sender, EventArgs args)
    {
        if (_errorsCounter.Count == 0)
        {
            var employee = new Employee(
                _post.Text,
                _textBoxes[0].Text,
                _textBoxes[1].Text,
                _textBoxes[2].Text,
                _birthDay.Value.Date,
                _phoneNumber.Text,
                _mailAddress.Text,
                _familyStatus.Text,
                _textBoxes[3].Text,
                _address.Text,
                _hobbies.Text is "" ? null :
                    _hobbies.Text.Split(',').Select(s => s.Trim().ToLower()).Where(s => s.Length > 0).ToList(),
                DateTime.Now);

            _dataContext.Employees.Add(employee);
            _dataContext.SaveChanges();

            MessageBox.Show("Сотрудник добавлен в базу данных", "Сохранение выполнено", MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            Close();
        }
        else MessageBox.Show("Остались пустые поля обязательные к заполнению", "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
    }

    private static string[] GetPositions()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        var config = builder.Build();
        var postitons = config.GetSection("Post").Value?.Split(',').Select(s => s.Trim()).Where(s => s.Length > 0).ToArray()!;
        Array.Sort(postitons);

        return postitons;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        _dataContext = null!;
    }
}
