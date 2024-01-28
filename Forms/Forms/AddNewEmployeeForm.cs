namespace Forms.Forms;

internal class AddNewEmployeeForm : Form
{
    public AddNewEmployeeForm()
    {
        Size = new Size(1600, 800);
        var label = new Label
        {
            Location = new Point(0, 0),
            Size = new Size(200, 30),
            Text = "Enter a number"
        };
        var box = new TextBox
        {
            Location = new Point(0, label.Bottom),
            Size = label.Size
        };
        var button = new Button
        {
            Location = new Point(0, box.Bottom),
            Size = label.Size,
            Text = "Increment!"
        };
        Controls.Add(label);
        Controls.Add(box);
        Controls.Add(button);
        button.Click += (sender, args) => box.Text = (int.Parse(box.Text) + 1).ToString();

    }
}
