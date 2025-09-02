
using Employees.Data;
using Microsoft.EntityFrameworkCore;

namespace Employees.Pages
{
    public partial class Index
    {
        public bool ShowCreate { get; set; }

        //Get context
        private EmployeeDataContext? _context;


        //Insert on all employees from database into this list
        public List<Employee>? OurEmployees { get; set; }

        //Create the new employee variable
        public Employee NewEmployee { get; set; }



        //blazor components - when this loads this is called below
        protected override async Task OnInitializedAsync()
        {
            ShowCreate = false;
            await ShowEmployees();
        }

        //Read from the database
        public async Task ShowEmployees()
        {
            //spinning up the database
            _context ??= await EmployeeDataContextFactory.CreateDbContextAsync();

            if(_context is not null)
            {

                OurEmployees = await _context.Employees.ToListAsync();
            }

            if(_context is not null)
            {
                await _context.DisposeAsync();
            }
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
                _context?.Employees.Add(NewEmployee);
                _context?.SaveChangesAsync();
            }

            ShowCreate = false;
            NewEmployee = new();
        }

    }
}
