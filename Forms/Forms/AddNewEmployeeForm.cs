using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Database;
using Entities;

namespace Forms.Forms;

internal class AddNewEmployeeForm : Form
{
    private DataContext _dataContext = null!;
    private Label[] _labels = null!;
    private TextBox[] _textBoxes = null!;
    private DateTimePicker _birthDayBox = null!;
    private MaskedTextBox _phoneNumberBox = null!;
    private ComboBox _familyStatusBox = null!;
    private TextBox _textBoxAddress = null!;
    private TextBox _textBoxHobbies = null!;
    private Button _saveButton = null!;

    public AddNewEmployeeForm()
    {
        _dataContext = DbContextFactory.CreateDataContext();
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await CustomizeForm();
        await MakeLablesAsync();
        await MakeTextBoxesAsync();
        await MakeBirthDayBoxAsync();
        await MakePhoneNumberBoxAsync();
        await MakeFamilyStatusBoxAsync();
        await MakeTextBoxAddressAsync();
        await MakeTextBoxHobbiesAsync();
        await MakeSaveButtonAsync();
        await _dataContext.Employees.LoadAsync();
    }

    private async Task CustomizeForm()
    {
        Size = new Size(600, 820);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        //TopMost = true;

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
            new() { Text = "Увлечения", Location = new Point(30, 580), Size = labelSize }
        ];
        Controls.AddRange(_labels);
        Controls.Add(new Label()
        {
            Location = new Point(220, 20),
            Size = new Size(1, 660),
            AutoSize = false,
            BorderStyle = BorderStyle.FixedSingle
        });

        await Task.CompletedTask;
    }

    private async Task MakeTextBoxesAsync()
    {
        var textBoxesSize = new Size(300, 30);

        _textBoxes =
        [
            new() { Location = new Point(250, 30), Size = textBoxesSize, TabIndex = 0, AutoSize = false },
            new() { Location = new Point(250, 80), Size = textBoxesSize, TabIndex = 1, AutoSize = false },
            new() { Location = new Point(250, 130), Size = textBoxesSize, TabIndex = 2, AutoSize = false },
            new() { Location = new Point(250, 180), Size = textBoxesSize, TabIndex = 3, AutoSize = false },
            new() { Location = new Point(250, 330), Size = textBoxesSize, TabIndex = 6, AutoSize = false },
            new() { Location = new Point(250, 430), Size = textBoxesSize, TabIndex = 8, AutoSize = false },
        ];

        foreach (var textBox in _textBoxes)
        {
            textBox.KeyPress += new KeyPressEventHandler(CorrectKeyPress!);
            textBox.Enter += (sender, args) =>
            {
                if (sender is TextBox textBox)
                    RemoveErrorLabel(textBox.Location.X - 15, textBox.Location.Y);
            };
        }

        Controls.AddRange(_textBoxes);
        await Task.CompletedTask;
    }

    private async Task MakeBirthDayBoxAsync()
    {
        _birthDayBox = new DateTimePicker()
        {
            Location = new Point(250, 230),
            Size = new Size(300, 30),
            TabIndex = 4,
            AutoSize = false,
            MinDate = new DateTime(1950, 01, 01),
            //MaxDate = DateTime.Today,
            CustomFormat = "dd MMMM yyyy",
            Format = DateTimePickerFormat.Custom
        };
        _birthDayBox.Enter += (sender, args) =>
        {
            if (sender is DateTimePicker birthDay)
                RemoveErrorLabel(birthDay.Location.X - 15, birthDay.Location.Y);
        };

        Controls.Add(_birthDayBox);
        await Task.CompletedTask;
    }

    private async Task MakePhoneNumberBoxAsync()
    {
        _phoneNumberBox = new MaskedTextBox()
        {
            Location = new Point(250, 280),
            Size = new Size(300, 30),
            Mask = "+7(000)000-00-00",
            TabIndex = 5,
            AutoSize = false
        };
        _phoneNumberBox.Enter += (sender, args) =>
        {
            if (sender is MaskedTextBox phoneNumber)
                RemoveErrorLabel(phoneNumber.Location.X - 15, phoneNumber.Location.Y);
        };

        Controls.Add(_phoneNumberBox);
        await Task.CompletedTask;
    }

    private async Task MakeFamilyStatusBoxAsync()
    {
        _familyStatusBox = new ComboBox()
        {
            Location = new Point(250, 380),
            Size = new Size(300, 30),
            DropDownWidth = 300,
            DropDownStyle = ComboBoxStyle.DropDownList,
            TabIndex = 7,
            AutoSize = false
        };
        _familyStatusBox.Items.AddRange(["Женат", "Замужем", "Не женат", "Не замужем"]);
        _familyStatusBox.Enter += (sender, args) =>
        {
            if (sender is ComboBox familyStatus)
                RemoveErrorLabel(familyStatus.Location.X - 15, familyStatus.Location.Y);
        };

        Controls.Add(_familyStatusBox);
        await Task.CompletedTask;
    }

    private async Task MakeTextBoxAddressAsync()
    {
        _textBoxAddress = new()
        {
            Location = new Point(250, 480),
            Size = new Size(300, 70),
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            TabIndex = 9,
        };
        _textBoxAddress.Enter += (sender, args) =>
        {
            if (sender is TextBox textBox)
                RemoveErrorLabel(textBox.Location.X - 15, textBox.Location.Y);
        };

        Controls.Add(_textBoxAddress);
        Controls.Add(new Label()
        {
            Text = "Пример: ул Ленина, д 52, кв 67",
            Font = new Font("Tahoma", 8, FontStyle.Regular),
            Location = new Point(250, 555),
            Size = new Size(350, 30),
            ForeColor = Color.Gray
        });

        await Task.CompletedTask;
    }

    private async Task MakeTextBoxHobbiesAsync()
    {
        _textBoxHobbies = new()
        {
            Location = new Point(250, 580),
            Size = new Size(300, 70),
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            TabIndex = 10,
        };

        Controls.Add(_textBoxHobbies);
        Controls.Add(new Label() 
        {
            Text = "Перечисления выполнять через запятую",
            Font = new Font("Tahoma", 8, FontStyle.Regular),
            Location = new Point(250, 655), 
            Size = new Size(350, 30),
            ForeColor = Color.Gray 
        });
        await Task.CompletedTask;
    }

    private async Task MakeSaveButtonAsync()
    {
        _saveButton = new Button
        {
            Location = new Point(420, 690),
            Size = new Size(130, 50),
            Text = "Сохранить",
        };
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
            if (keyPress.KeyChar == ' ') keyPress.KeyChar = '\0';
            else if (textBox.Text.Length == 0)
            {
                if ((keyPress.KeyChar >= 1072 && keyPress.KeyChar <= 1103) || (keyPress.KeyChar >= 97 && keyPress.KeyChar <= 122))
                    keyPress.KeyChar -= (char)32;
            }
        }
    }

    private bool TryMakeLabelRequiredField()
    {
        var flag = false;

        foreach (var textBox in _textBoxes)
        {
            if (textBox.Text is "")
                flag = TryMakeErrorLabel(textBox.Location.X - 15, textBox.Location.Y);
        }

        if (_birthDayBox.Value.Year > DateTime.Today.Year - 18)
            flag = TryMakeErrorLabel(_birthDayBox.Location.X - 15, _birthDayBox.Location.Y);

        if (_phoneNumberBox.Text.Length < 16)
            flag = TryMakeErrorLabel(_phoneNumberBox.Location.X - 15, _phoneNumberBox.Location.Y);

        if (_familyStatusBox.SelectedIndex == -1)
            flag = TryMakeErrorLabel(_familyStatusBox.Location.X - 15, _familyStatusBox.Location.Y);

        if(_textBoxAddress.Text == "")
            flag = TryMakeErrorLabel(_textBoxAddress.Location.X - 15, _textBoxAddress.Location.Y);

        return flag;
    }

    private bool TryMakeErrorLabel(int x, int y)
    {
        Controls.Add(new Label()
        {
            Text = "*",
            Location = new Point(x, y),
            ForeColor = Color.Red
        });

        return true;
    }

    private void RemoveErrorLabel(int x, int y)
    {
        for (int i = 0; i < Controls.Count; i++)
        {
            var point = new Point(x, y);
            if (Controls[i].Location == point)
            {
                Controls.RemoveAt(i);
                break;
            }
        }
    }

    private void CreateEmployee(object sender, EventArgs args)
    {
        if (!TryMakeLabelRequiredField())
        {
            var employee = new Employee(
                _textBoxes[0].Text,
                _textBoxes[1].Text,
                _textBoxes[2].Text,
                _textBoxes[3].Text,
                _birthDayBox.Value,
                _phoneNumberBox.Text,
                _textBoxes[4].Text,
                _familyStatusBox.Text,
                _textBoxes[5].Text,
                _textBoxAddress.Text,
                _textBoxHobbies.Text is "" ? null :
                    _textBoxHobbies.Text.Split(',').Select(s => s.Trim()).Where(s => s.Length > 0).ToList(),
                DateTime.Now);
            _dataContext.Employees.Add(employee);
            _dataContext.SaveChanges();
            MessageBox.Show(
                "Сотрудник добавлен в базу данных",
                "Сохранение выполнено",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);

            Close();
        }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        _dataContext?.Dispose();
        _dataContext = null!;
    }
}
