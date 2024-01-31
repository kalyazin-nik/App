using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text;
using Database;
using Entities;

namespace Forms.Forms;

internal class EmployeesForm : Form
{
    private DataContext _dataContext;
    private BindingList<Employee> _employees = null!;
    private DataGridView _dataGrid = null!;
    private Button[] _buttons = null!;

    public EmployeesForm()
    {
        _dataContext = DbContextFactory.CreateDataContext();
        _dataContext.Employees.Load();
        CustomizeForm();
        MakeDataGrid();
        MakeButtons();
    }

    private void CustomizeForm()
    {
        Text = "Список сотрудников";
        Size = new Size(1500, 900);
        MinimumSize = new(900, 600);
    }

    private void MakeDataGrid()
    {
        _dataGrid = new();
        Controls.Add(_dataGrid);

        _dataGrid.Location = new Point(-1, 0);
        _dataGrid.Size = new Size(1500, 750);
        _dataGrid.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

        _dataGrid.EnableHeadersVisualStyles = false;
        _dataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray;
        _dataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        _dataGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Gray;
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
            new DataGridViewTextBoxColumn() { Name = "Увлечения", ReadOnly = true, Width = 400, ValueType = typeof(string) }
        ]);

        _dataGrid.DataSource = _employees = _dataContext.Employees.Local.ToBindingList();
        _dataGrid.RowsAdded += UpdateRowOtherCalculate!;
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

        _buttons[4].Click += (sender, args) => { new CreateNewEmployeeForm(_dataContext).Show(Owner); };
        Controls.AddRange(_buttons);
    }

    private void UpdateRowOtherCalculate(object sender, DataGridViewRowsAddedEventArgs args)
    {
        for (int i = args.RowIndex; i < args.RowCount + args.RowIndex; i++)
        {
            if (_employees[i].Hobbies is not null && _employees[i].Hobbies!.Count > 0)
            {
                var result = new StringBuilder();

                foreach (var hobbie in _employees[i].Hobbies!)
                    result.Append($"{hobbie}, ");

                _dataGrid["Увлечения", i].Value = result.Remove(result.Length - 2, 2).ToString();
            }
            var dateOfBirth = (DateTime)_dataGrid["Дата рождения", i].Value;
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;

            _dataGrid["Возраст", i].Value = age;
        }
    }
}
