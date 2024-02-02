using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Net.Mail;
using System.Text;
using Database;
using Entities;

namespace Forms.Forms;

internal class CreateUpdateEmployeeForm : Form
{
    private const int BOX_COUNT = 10;
    private const int MIN_AGE_EMPLOYEE = 18;
    private const int PHONE_NUMBER_LENGTH = 16;
    private const int STRING_MAX_LENGTH = 50;

    private DataContext _dataContext = null!;
    private Employee? _employee;
    private readonly bool _isCreate;
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
    private Stack<int> _errorsCounter = null!;

    public CreateUpdateEmployeeForm(DataContext context)
    {
        _dataContext = context;
        _isCreate = true;

        _errorsCounter = [];
        for (int i = 0; i < BOX_COUNT; i++)
            _errorsCounter.Push(1);
    }

    public CreateUpdateEmployeeForm(DataContext context, Employee employee)
    {
        _dataContext = context;
        _employee = employee;
        _errorsCounter = [];
        _isCreate = false;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
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
        if (!_isCreate) FillForm();
    }

    private void CustomizeForm()
    {
        Text = _isCreate ? "Добавление сотрудника в базу данных" : "Изменение информации о сотруднике";
        Size = new (600, 860);
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
            new() { Text = "Занимаемая должность", Location = new(30, 30), Size = labelSize },
            new() { Text = "Фамилия", Location = new(30, 80), Size = labelSize },
            new() { Text = "Имя", Location = new(30, 130), Size = labelSize },
            new() { Text = "Отчество", Location = new(30, 180), Size = labelSize },
            new() { Text = "Дата рождения", Location = new(30, 230), Size = labelSize },
            new() { Text = "Телефонный номер", Location = new(30, 280), Size = labelSize },
            new() { Text = "Электронная почта", Location = new(30, 330), Size = labelSize },
            new() { Text = "Семейное положение", Location = new(30, 380), Size = labelSize },
            new() { Text = "Город", Location = new(30, 430), Size = labelSize },
            new() { Text = "Адрес", Location = new(30, 480), Size = labelSize },
            new() { Text = "Увлечения", Location = new(30, 580), Size = labelSize },
            new()
            {
                Text = "Пример: ул Ленина, д 52, кв 67",
                Font = new("Tahoma", 8, FontStyle.Regular),
                Location = new(250, 555),
                Size = new(350, 30),
                ForeColor = Color.Gray
            },
            new()
            {
                Text = "Перечисления выполнять через запятую",
                Font = new("Tahoma", 8, FontStyle.Regular),
                Location = new(250, 655),
                Size = new(350, 30),
                ForeColor = Color.Gray
            },
        ];

        for (int i = 0, y = 30; i < BOX_COUNT; i++, y += 50)
            _labels.Add(new() 
            { 
                Text = "*", 
                Name = $"error {i}", 
                Location = new(235, y), 
                AutoSize = true, 
                ForeColor = _isCreate ? Color.Red : Color.Transparent 
            });

        _panel.Controls.AddRange(_labels.ToArray());
    }

    private void MakePost()
    {
        _post = new()
        {
            Name = "0",
            Location = new(250, 30),
            Size = new(300, 30),
            DropDownWidth = 300,
            DropDownStyle = ComboBoxStyle.DropDownList,
            TabIndex = 0,
            AutoSize = false
        };

        _post.Items.AddRange(GetPositions());
        _post.Enter += ErrorLabelToTransparent!;
        _post.Leave += CorrectComboBox!;

        _panel.Controls.Add(_post);
    }

    private void MakeTextBoxes()
    {
        var textBoxesSize = new Size(300, 30);
        _textBoxes =
        [
            new() { Name = "1", Location = new(250, 80), Size = textBoxesSize, TabIndex = 1, AutoSize = false },
            new() { Name = "2", Location = new(250, 130), Size = textBoxesSize, TabIndex = 2, AutoSize = false },
            new() { Name = "3", Location = new(250, 180), Size = textBoxesSize, TabIndex = 3, AutoSize = false },
            new() { Name = "8", Location = new(250, 430), Size = textBoxesSize, TabIndex = 8, AutoSize = false },
        ];

        foreach (var textBox in _textBoxes)
        {
            textBox.KeyPress += new KeyPressEventHandler(CorrectKeyPress!);
            textBox.Enter += ErrorLabelToTransparent!;
            textBox.Leave += CorrectTextBox!;
        }

        _textBoxes[3].KeyPress -= new KeyPressEventHandler(CorrectKeyPress!);
        _textBoxes[3].KeyPress += (sender, args) =>
        {
            if (sender is TextBox textBox && args is KeyPressEventArgs keyPress)
            {
                if (textBox.Text.Length == 0 ||
                textBox.Text.Length == STRING_MAX_LENGTH ||
                !(keyPress.KeyChar == 32 || keyPress.KeyChar == 45))
                    CorrectKeyPress(sender, args);
            }
        };
        _panel.Controls.AddRange(_textBoxes);
    }

    private void MakeBirthDay()
    {
        _birthDay = new()
        {
            Name = "4",
            Location = new(250, 230),
            Size = new(300, 30),
            TabIndex = 4,
            AutoSize = false,
            MinDate = new(1950, 01, 01),
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
            Location = new(250, 280),
            Size = new(300, 30),
            Mask = "+7(000)000-00-00",
            TabIndex = 5,
            AutoSize = false
        };

        _phoneNumber.MouseClick += (sender, args) => { _phoneNumber.SelectionStart = 3; };
        _phoneNumber.Enter += ErrorLabelToTransparent!;
        _phoneNumber.Leave += CorrectPhoneNumber!;

        _panel.Controls.Add(_phoneNumber);
    }

    private void MakeMailAddress()
    {
        _mailAddress = new() { Name = "6", Location = new(250, 330), Size = new(300, 30), TabIndex = 6, AutoSize = false };

        _mailAddress.Enter += ErrorLabelToTransparent!;
        _mailAddress.Leave += CorrectMailAddress!;

        _panel.Controls.Add(_mailAddress);
    }

    private void MakeFamilyStatus()
    {
        _familyStatus = new()
        {
            Name = "7",
            Location = new(250, 380),
            Size = new(300, 30),
            DropDownWidth = 300,
            DropDownStyle = ComboBoxStyle.DropDownList,
            TabIndex = 7,
            AutoSize = false
        };

        _familyStatus.Items.AddRange(["Женат", "Замужем", "Не женат", "Не замужем"]);
        _familyStatus.Enter += ErrorLabelToTransparent!;
        _familyStatus.Leave += CorrectComboBox!;

        _panel.Controls.Add(_familyStatus);
    }

    private void MakeAddress()
    {
        _address = new()
        {
            Name = "9",
            Location = new(250, 480),
            Size = new(300, 70),
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            TabIndex = 9,
        };

        _address.Enter += ErrorLabelToTransparent!;
        _address.Leave += CorrectTextBox!;

        _panel.Controls.Add(_address);
    }

    private void MakeHobbies()
    {
        _hobbies = new()
        {
            Location = new(250, 580),
            Size = new(300, 70),
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            TabIndex = 10
        };

        _panel.Controls.Add(_hobbies);
    }

    private void MakeSaveButton()
    {
        _saveButton = new() { Location = new(400, 735), Size = new(130, 50), Text = "Сохранить" };

        _saveButton.Click += SaveEmployee!;

        Controls.Add(_saveButton);
    }

    private void FillForm()
    {
        if (_employee is not null)
        {
            _post.Text = _employee.Post;
            _textBoxes[0].Text = _employee.Surname;
            _textBoxes[1].Text = _employee.Name;
            _textBoxes[2].Text = _employee.Patronymic;
            _birthDay.Value = _employee.DateOfBirth;
            _phoneNumber.Text = _employee.PhoneNumber;
            _mailAddress.Text = _employee.Mail;
            _familyStatus.Text = _employee.FamilyStatus;
            _textBoxes[3].Text = _employee.City;
            _address.Text = _employee.Address;

            if (_employee.Hobbies is not null)
            {
                var result = new StringBuilder();

                foreach (var hobbie in _employee.Hobbies)
                    result.Append($"{hobbie}, ");

                _hobbies.Text = result.Remove(result.Length - 2, 2).ToString();
            }
            else _hobbies.Text = "";
        }
    }

    private void DrawDividingLine(object sender, PaintEventArgs args)
    {
        args.Graphics.DrawLine(new(Color.LightGray, 2), new(220, 20), new(220, 670));
        args.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        args.Graphics.Save();
    }

    private void CorrectKeyPress(object sender, EventArgs args)
    {
        if (sender is TextBox textBox && args is KeyPressEventArgs keyPress)
        {
            if (textBox.Text.Length == 0)
            {
                if ((keyPress.KeyChar >= 97 && keyPress.KeyChar <= 122) ||
                    (keyPress.KeyChar >= 1072 && keyPress.KeyChar <= 1103) ||
                    keyPress.KeyChar == 1105 || keyPress.KeyChar == 1025)
                {
                    keyPress.KeyChar -= (char)32;
                }
                else if (!((keyPress.KeyChar >= 65 && keyPress.KeyChar <= 90) ||
                    (keyPress.KeyChar >= 1040 && keyPress.KeyChar <= 1071)))
                {
                    keyPress.KeyChar = '\0';
                }

            }
            else
            {
                if (textBox.Text.Length == STRING_MAX_LENGTH)
                {
                    keyPress.KeyChar = (char)8;
                }
                else if (!(keyPress.KeyChar == 8 || keyPress.KeyChar == 15 ||
                    keyPress.KeyChar == 1105 || keyPress.KeyChar == 1025 ||
                    (keyPress.KeyChar >= 65 && keyPress.KeyChar <= 90) ||
                    (keyPress.KeyChar >= 97 && keyPress.KeyChar <= 122) ||
                    (keyPress.KeyChar >= 1040 && keyPress.KeyChar <= 1071) ||
                    (keyPress.KeyChar >= 1072 && keyPress.KeyChar <= 1103)))
                {
                    keyPress.KeyChar = '\0';
                }
            }
        }
    }

    private async void CorrectComboBox(object sender, EventArgs args)
    {
        if (sender is ComboBox comboBox && comboBox.SelectedIndex == -1)
            ErrorLabelToRed(sender, args);

        await Task.CompletedTask;
    }

    private async void CorrectTextBox(object sender, EventArgs args)
    {
        if (sender is TextBox textBox && textBox.Text.Length == 0)
            ErrorLabelToRed(sender, args);

        await Task.CompletedTask;
    }

    private async void CorrectBirthDay(object sender, EventArgs args)
    {
        if (sender is DateTimePicker dateTimePicker && dateTimePicker.Value is DateTime birthDay)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDay.Year;
            if (birthDay.Date > today.AddYears(-age)) age--;
            if (age < MIN_AGE_EMPLOYEE) ErrorLabelToRed(sender, args);
            await Task.CompletedTask;
        }
    }

    private async void CorrectPhoneNumber(object sender, EventArgs args)
    {
        if (sender is MaskedTextBox phoneNumber)
            if (phoneNumber.Text.Length < PHONE_NUMBER_LENGTH)
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
        {
            await SwitchErrorColor(Color.Red, control.Name);
            _errorsCounter.Push(1);
        }
    }

    private async void ErrorLabelToTransparent(object sender, EventArgs args)
    {
        if (sender is Control control && _panel.Controls[$"error {control.Name}"]!.ForeColor == Color.Red)
        {
            await SwitchErrorColor(Color.Transparent, control.Name);
            _errorsCounter.Pop();
        }
    }

    private async Task SwitchErrorColor(Color color, string name)
    {
        _panel.Controls[$"error {name}"]!.ForeColor = color;
        await Task.CompletedTask;
    }

    private void SaveEmployee(object sender, EventArgs args)
    {
        if (_errorsCounter.Count == 0)
        {
            _employee ??= new Employee();

            _employee.Post = _post.Text;
            _employee.Surname = _textBoxes[0].Text;
            _employee.Name = _textBoxes[1].Text;
            _employee.Patronymic = _textBoxes[2].Text;
            _employee.DateOfBirth = _birthDay.Value.Date;
            _employee.PhoneNumber = _phoneNumber.Text;
            _employee.Mail = _mailAddress.Text;
            _employee.FamilyStatus = _familyStatus.Text;
            _employee.City = _textBoxes[3].Text;
            _employee.Address = _address.Text;
            _employee.Hobbies = _hobbies.Text is "" ? null :
                    _hobbies.Text
                        .Split(',')
                        .Select(s => s.Trim().ToLower())
                        .Where(s => s.Length > 0 && s.Length <= STRING_MAX_LENGTH)
                        .ToList();

            if (_isCreate) _dataContext.Employees.Add(_employee);
            else _dataContext.Employees.Update(_employee);
            _dataContext.SaveChanges();

            MessageBox.Show(_isCreate ? "Сотрудник добавлен в базу данных" : "Сотрудник обновлен в базе данных",
                "Сохранение выполнено", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

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
