
using Employees.Data;

namespace Employees.Pages
{
    public partial class Index
    {
        public bool ShowCreate { get; set; }

        //Get context
        private EmployeeDataContext? _context;

        //Create the new employee variable
        public Employee NewEmployee { get; set; }



        //blazor components - when this loads this is called below
        protected override async Task OnInitializedAsync()
        {
            ShowCreate = false;
        }

        //this display the form and attaches the object instance for creation of employee
        public void ShowCreateForm()
        {
            ShowCreate = true;
            NewEmployee = new Employee();
        }

        //All the code for back-end logic to create a record
        public async Task CreateNewEmployee()
        {
            _context ??= await EmployeeDataContextFactory.CreateDbContextAsync();

            if(NewEmployee is not null)
            {
                context?.Employees.Add(NewEmployee);
                context?.SaveChangesAsync();
            }

            ShowCreate = false;
        }

    }
}
