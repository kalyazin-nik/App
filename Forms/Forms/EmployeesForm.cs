using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text;
using Database;
using Entities;

namespace Forms.Forms;

internal class EmployeesForm : Form
{
    private DataContext _dataContext;
    private BindingList<Employee> _bindingList = null!;
    private DataGridView _dataGrid = null!;
    private Button[] _buttons = null!;
    private bool _isFullTable;

    public EmployeesForm()
    {
        _dataContext = DbContextFactory.CreateDataContext();
        _dataContext.Employees.Load();
        _bindingList = _dataContext.Employees.Local.ToBindingList();
        CustomizeForm();
        MakeDataGrid();
        MakeButtons();
        _isFullTable = true;
    }

    private void CustomizeForm()
    {
        Text = "Список сотрудников";
        Size = new Size(1500, 900);
        MinimumSize = new(900, 600);
        DoubleBuffered = true;
    }

    private void MakeDataGrid()
    {
        _dataGrid = new();
        Controls.Add(_dataGrid);

        _dataGrid.Location = new Point(0, 0);
        _dataGrid.Size = new Size(1500, 750);
        _dataGrid.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

        _dataGrid.EnableHeadersVisualStyles = false;
        _dataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray;
        _dataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        _dataGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.LightGray;
        _dataGrid.ColumnHeadersDefaultCellStyle.Font = new Font(_dataGrid.Font, FontStyle.Bold);
        _dataGrid.BackgroundColor = Color.White;
        _dataGrid.GridColor = Color.LightGray;
        _dataGrid.BorderStyle = BorderStyle.None;
        _dataGrid.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
        _dataGrid.RowsDefaultCellStyle.SelectionBackColor = Color.LightGray;

        _dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
        _dataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        _dataGrid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        _dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _dataGrid.RowHeadersVisible = false;
        _dataGrid.MultiSelect = false;
        _dataGrid.AutoGenerateColumns = false;
        _dataGrid.AllowUserToAddRows = false;

        _dataGrid.Columns.AddRange(
        [
            new DataGridViewTextBoxColumn() { Name = "id", DataPropertyName = "Id", ReadOnly = true, Width = 30 },
            new DataGridViewTextBoxColumn() { Name = "Должность", DataPropertyName = "Post", ReadOnly = true, Width = 200 },
            new DataGridViewTextBoxColumn() { Name = "Фамилия", DataPropertyName = "Surname", ReadOnly = true, Width = 200 },
            new DataGridViewTextBoxColumn() { Name = "Имя", DataPropertyName = "Name", ReadOnly = true, Width = 200 },
            new DataGridViewTextBoxColumn() { Name = "Отчество", DataPropertyName = "Patronymic", ReadOnly = true, Width = 200 },
            new DataGridViewTextBoxColumn() { Name = "Дата рождения", DataPropertyName = "DateOfBirth", ReadOnly = true, Width = 130 },
            new DataGridViewTextBoxColumn() { Name = "Возраст", ReadOnly = true, Width = 90 },
            new DataGridViewTextBoxColumn() { Name = "Номер телефона", DataPropertyName = "PhoneNumber", ReadOnly = true, Width = 150 },
            new DataGridViewTextBoxColumn() { Name = "Электронная почта", DataPropertyName = "Mail", ReadOnly = true, Width = 300 },
            new DataGridViewTextBoxColumn() { Name = "Семейное положение", DataPropertyName = "FamilyStatus", ReadOnly = true, Width = 110 },
            new DataGridViewTextBoxColumn() { Name = "Город", DataPropertyName = "City", ReadOnly = true, Width = 200 },
            new DataGridViewTextBoxColumn() { Name = "Адрес", DataPropertyName = "Address", ReadOnly = true, Width = 400 },
            new DataGridViewTextBoxColumn() { Name = "Увлечения", ReadOnly = true, Width = 400, ValueType = typeof(string) },
            new DataGridViewTextBoxColumn() { Name = "Дата добавления", DataPropertyName = "CreatedAt", ReadOnly = true, Width = 160 }
        ]);

        _dataGrid.DataSource = _bindingList;

        _dataGrid.RowsAdded += (sender, args) => { UpdateRowOtherCalculate(args.RowIndex, args.RowCount); };
        _dataGrid.RowValidated += (sender, args) => { UpdateRowOtherCalculate(args.RowIndex, 1); };
        _dataGrid.CellDoubleClick += (sender, args) =>
        { if (args.RowIndex > -1) new CreateUpdateEmployeeForm(_dataContext, _bindingList[args.RowIndex]).ShowDialog(); };
    }

    private void MakeButtons()
    {
        _buttons =
        [
            new() { Location = new(50, 775), Size = new(130, 50), Text = "Меньше", Anchor = AnchorStyles.Left | AnchorStyles.Bottom },
            new() { Location = new(200, 775), Size = new(130, 50), Text = "Больше", Anchor = AnchorStyles.Left | AnchorStyles.Bottom },
            new() { Location = new(1000, 775), Size = new(130, 50), Text = "Удалить", Anchor = AnchorStyles.Right | AnchorStyles.Bottom },
            new() { Location = new(1150, 775), Size = new(130, 50), Text = "Изменить", Anchor = AnchorStyles.Right | AnchorStyles.Bottom },
            new() { Location = new(1300, 775), Size = new(130, 50), Text = "Добавить", Anchor = AnchorStyles.Right | AnchorStyles.Bottom }
        ];

        _buttons[0].Click += (sender, args) => { if (_isFullTable) ChangeVisibleColumns(_isFullTable = false); };
        _buttons[1].Click += (sender, args) => { if (!_isFullTable) ChangeVisibleColumns(_isFullTable = true); };
        _buttons[2].Click += DeleteEmployee!;
        _buttons[3].Click += (sender, args) =>
        { new CreateUpdateEmployeeForm(_dataContext, _bindingList[_dataGrid.CurrentCell.RowIndex]).ShowDialog(); };

        _buttons[4].Click += (sender, args) => { new CreateUpdateEmployeeForm(_dataContext).ShowDialog(); };

        Controls.AddRange(_buttons);
    }

    private void DeleteEmployee(object sender, EventArgs e)
    {
        if (MessageBox.Show($"Вы собираетесь совершить удаление сотрудника из базы данных." +
            $"\nЭта операция не имеет обратного действия.\nПродолжить?", "Удаление", MessageBoxButtons.YesNo,
            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly)
            == DialogResult.Yes)
        {
            _dataContext.Employees.Remove(_bindingList[_dataGrid.CurrentCell.RowIndex]);
            _dataContext.SaveChanges();
        }
    }

    private void ChangeVisibleColumns(bool flag)
    {
        _dataGrid.Columns["Отчество"].Visible = flag;
        _dataGrid.Columns["Дата рождения"].Visible = flag;
        _dataGrid.Columns["Семейное положение"].Visible = flag;
        _dataGrid.Columns["Увлечения"].Visible = flag;
        _dataGrid.Columns["Дата добавления"].Visible = flag;
    }

    private void UpdateRowOtherCalculate(int rowIndex, int rowCount)
    {
        for (int i = rowIndex; i < rowCount + rowIndex; i++)
        {
            if (_bindingList[i].Hobbies is not null && _bindingList[i].Hobbies!.Count > 0)
            {
                var result = new StringBuilder();

                foreach (var hobbie in _bindingList[i].Hobbies!)
                    result.Append($"{hobbie}, ");

                _dataGrid["Увлечения", i].Value = result.Remove(result.Length - 2, 2).ToString();
            }
            var dateOfBirth = _bindingList[i].DateOfBirth;
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;

            _dataGrid["Возраст", i].Value = age;
        }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        _dataContext.Dispose();
        _dataGrid.Dispose();
        _dataContext = null!;
        _bindingList = null!;
        _dataGrid = null!;
    }
}
