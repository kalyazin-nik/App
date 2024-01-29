using Forms.Forms;

namespace Forms;

public static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new AddNewEmployeeForm());
    }
}