using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Net.Mail;
using Database;
using Entities;

namespace Forms.Forms;

internal class CreateNewEmployeeForm : Form
{
    private DataContext _dataContext = null!;
    private Label[] _labels = null!;
    private ComboBox _post = null!;
    private TextBox[] _textBoxes = null!;
    private DateTimePicker _birthDay = null!;
    private MaskedTextBox _phoneNumber = null!;
    private TextBox _mailAddress = null!;
    private ComboBox _familyStatus = null!;
    private TextBox _address = null!;
    private TextBox _hobbies = null!;
    private Button _saveButton = null!;

    public CreateNewEmployeeForm()
    {
        _dataContext = DbContextFactory.CreateDataContext();
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await CustomizeForm();
        await MakeLablesAsync();
        await MakePostAsync();
        await MakeTextBoxesAsync();
        await MakeBirthDayAsync();
        await MakePhoneNumberAsync();
        await MakeMailAddressAsync();
        await MakeFamilyStatusAsync();
        await MakeAddressAsync();
        await MakeHobbiesAsync();
        await MakeSaveButtonAsync();
        await _dataContext.Employees.LoadAsync();
    }

    private async Task CustomizeForm()
    {
        Text = "Добавление в базу данных";
        Size = new Size(600, 820);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        TopMost = true;

        await Task.CompletedTask;
    }

    private async Task MakeLablesAsync()
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
            new() { Location = new Point(220, 20), Size = new Size(1, 660), AutoSize = false, BorderStyle = BorderStyle.FixedSingle },
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
            }
        ];

        Controls.AddRange(_labels);
        await Task.CompletedTask;
    }

    private async Task MakePostAsync()
    {
        _post = new()
        {
            Location = new Point(250, 30),
            Size = new Size(300, 30),
            DropDownWidth = 300,
            DropDownStyle = ComboBoxStyle.DropDownList,
            TabIndex = 0,
            AutoSize = false
        };

        _post.Items.AddRange(GetPositions());
        _post.Enter += RemoveErrorLabel!;

        Controls.Add(_post);
        await Task.CompletedTask;
    }

    private async Task MakeTextBoxesAsync()
    {
        var textBoxesSize = new Size(300, 30);

        _textBoxes =
        [
            new() { Location = new Point(250, 80), Size = textBoxesSize, TabIndex = 1, AutoSize = false },
            new() { Location = new Point(250, 130), Size = textBoxesSize, TabIndex = 2, AutoSize = false },
            new() { Location = new Point(250, 180), Size = textBoxesSize, TabIndex = 3, AutoSize = false },
            new() { Location = new Point(250, 430), Size = textBoxesSize, TabIndex = 8, AutoSize = false },
        ];

        foreach (var textBox in _textBoxes)
        {
            textBox.KeyPress += new KeyPressEventHandler(CorrectKeyPress!);
            textBox.Enter += RemoveErrorLabel!;
        }

        Controls.AddRange(_textBoxes);
        await Task.CompletedTask;
    }

    private async Task MakeBirthDayAsync()
    {
        _birthDay = new()
        {
            Location = new Point(250, 230),
            Size = new Size(300, 30),
            TabIndex = 4,
            AutoSize = false,
            MinDate = new DateTime(1950, 01, 01),
            CustomFormat = "dd MMMM yyyy",
            Format = DateTimePickerFormat.Custom
        };

        _birthDay.Enter += RemoveErrorLabel!;

        Controls.Add(_birthDay);
        await Task.CompletedTask;
    }

    private async Task MakePhoneNumberAsync()
    {
        _phoneNumber = new()
        {
            Location = new Point(250, 280),
            Size = new Size(300, 30),
            Mask = "+7(000)000-00-00",
            TabIndex = 5,
            AutoSize = false
        };

        _phoneNumber.Enter += RemoveErrorLabel!;

        Controls.Add(_phoneNumber);
        await Task.CompletedTask;
    }

    private async Task MakeMailAddressAsync()
    {
        _mailAddress = new() { Location = new Point(250, 330), Size = new Size(300, 30), TabIndex = 6, AutoSize = false };

        _mailAddress.Enter += RemoveErrorLabel!;

        Controls.Add(_mailAddress);
        await Task.CompletedTask;
    }

    private async Task MakeFamilyStatusAsync()
    {
        _familyStatus = new()
        {
            Location = new Point(250, 380),
            Size = new Size(300, 30),
            DropDownWidth = 300,
            DropDownStyle = ComboBoxStyle.DropDownList,
            TabIndex = 7,
            AutoSize = false
        };

        _familyStatus.Items.AddRange(["Женат", "Замужем", "Не женат", "Не замужем"]);
        _familyStatus.Enter += RemoveErrorLabel!;

        Controls.Add(_familyStatus);
        await Task.CompletedTask;
    }

    private async Task MakeAddressAsync()
    {
        _address = new()
        {
            Location = new Point(250, 480),
            Size = new Size(300, 70),
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            TabIndex = 9,
        };

        _address.Enter += RemoveErrorLabel!;

        Controls.Add(_address);
        await Task.CompletedTask;
    }

    private async Task MakeHobbiesAsync()
    {
        _hobbies = new()
        {
            Location = new Point(250, 580),
            Size = new Size(300, 70),
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            TabIndex = 10
        };

        Controls.Add(_hobbies);
        await Task.CompletedTask;
    }

    private async Task MakeSaveButtonAsync()
    {
        _saveButton = new() { Location = new Point(420, 690), Size = new Size(130, 50), Text = "Сохранить" };

        Controls.Add(_saveButton);

        _saveButton.Click += CreateEmployee!;
        await Task.CompletedTask;
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
                else keyPress.KeyChar = '\0';
            }
            else
            {
                if (!(keyPress.KeyChar == 8 || keyPress.KeyChar == 1105 || keyPress.KeyChar == 1025 ||
                    (keyPress.KeyChar >= 65 && keyPress.KeyChar <= 90) ||
                    (keyPress.KeyChar >= 97 && keyPress.KeyChar <= 122) ||
                    (keyPress.KeyChar >= 1040 && keyPress.KeyChar <= 1071) ||
                    (keyPress.KeyChar >= 1072 && keyPress.KeyChar <= 1103)))
                    keyPress.KeyChar = '\0';
            }
        }
    }

    private void RemoveErrorLabel(object sender, EventArgs args)
    {
        if (sender is Control control)
        {
            var point = new Point(control.Location.X - 15, control.Location.Y);

            for (int i = 0; i < Controls.Count; i++)
                if (Controls[i].Location == point)
                {
                    Controls.RemoveAt(i);
                    break;
                }
        }
    }

    private void CreateEmployee(object sender, EventArgs args)
    {
        if (!TryMakeErrorLabelRequiredField())
        {
            var employee = new Employee(_textBoxes[0].Text, _textBoxes[1].Text, _textBoxes[2].Text, _textBoxes[3].Text,
                _birthDay.Value, _phoneNumber.Text, _mailAddress.Text, _familyStatus.Text, _textBoxes[4].Text,
                _address.Text, _hobbies.Text is "" ? null :
                    _hobbies.Text.Split(',').Select(s => s.Trim().ToLower()).Where(s => s.Length > 0).ToList(),
                DateTime.Now);

            _dataContext.Employees.Add(employee);
            _dataContext.SaveChanges();

            MessageBox.Show("Сотрудник добавлен в базу данных", "Сохранение выполнено", MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            Close();
        }
    }

    private bool TryMakeErrorLabelRequiredField()
    {
        var flag = false;

        if (_post.SelectedIndex == -1)
            flag = TryAddErrorLabel(_familyStatus.Location.X - 15, _familyStatus.Location.Y);

        foreach (var textBox in _textBoxes)
            if (textBox.Text is "")
                flag = TryAddErrorLabel(textBox.Location.X - 15, textBox.Location.Y);

        if (_birthDay.Value.Year > DateTime.Today.Year - 18)
            flag = TryAddErrorLabel(_birthDay.Location.X - 15, _birthDay.Location.Y);

        if (_phoneNumber.Text.Length < 16)
            flag = TryAddErrorLabel(_phoneNumber.Location.X - 15, _phoneNumber.Location.Y);

        try { _ = new MailAddress(_mailAddress.Text); }
        catch (Exception) { flag = TryAddErrorLabel(_mailAddress.Location.X - 15, _mailAddress.Location.Y); }

        if (_familyStatus.SelectedIndex == -1)
            flag = TryAddErrorLabel(_familyStatus.Location.X - 15, _familyStatus.Location.Y);

        if (_address.Text == "")
            flag = TryAddErrorLabel(_address.Location.X - 15, _address.Location.Y);

        return flag;
    }

    private bool TryAddErrorLabel(int x, int y)
    {
        Controls.Add(new Label() { Text = "*", Location = new Point(x, y), ForeColor = Color.Red });
        return true;
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
        _dataContext?.Dispose();
        _dataContext = null!;
    }
}
