using Employees.Data;
using Microsoft.EntityFrameworkCore;

namespace Employees.Pages
{
    public partial class Index
    {
        public bool ShowCreate { get; set; }

        public bool ShowEdit { get; set; }

        public int EditingId { get; set; }

        //Get context
        private EmployeeDataContext? _context;

        private Employee? EmployeeToUpdate { get; set; }

        //Insert on all employees from database into this list
        public List<Employee>? OurEmployees { get; set; }

        //Create the new employee variable
        public Employee? NewEmployee { get; set; }



        //blazor components - when this loads this is called below
        protected override async Task OnInitializedAsync()
        {
            ShowCreate = false;
            await ShowEmployees();
        }

        //------------Create-------------------------

        //this display the form and attaches the object instance for creation of employee
        public void ShowCreateForm()
        {
            ShowCreate = true;
            NewEmployee = new Employee();
        }


        //All the code for back-end logic to create a record
        public async Task CreateNewEmployee()
        {
            using var context = await EmployeeDataContextFactory.CreateDbContextAsync();

            if (NewEmployee is not null)
            {
                context.Employees.Add(NewEmployee);
                context?.SaveChangesAsync();
            }

            ShowCreate = false;
            await ShowEmployees();
            NewEmployee = new();
        }

        //--------READ-------------

        //Read from the database
        public async Task ShowEmployees()
        {
            //spinning up the database
            using var _context = await EmployeeDataContextFactory.CreateDbContextAsync();

            if (_context is not null)
            {

                OurEmployees = await _context.Employees.ToListAsync();
            }

            //Not to dispose the context as this loses the instance connection
            //if(_context is not null)
            //{
            //    await _context.DisposeAsync();
            //}
        }


        //--------UPDATE --------

        //Show the edit form to update the record details for particular employee
        public async Task ShowEditForm(Employee OurEmployee)
        {
            //spin up the db context
            using var _context = await EmployeeDataContextFactory.CreateDbContextAsync();
            if (_context is not null)
            {
                //get the latest employee details from the database. Should only bring up one person as ID
                EmployeeToUpdate = _context.Employees.FirstOrDefault(x => x.Id == OurEmployee.Id);

            }
            ShowEdit = true;
            EditingId = OurEmployee.Id;
        }




        public async Task UpdateEmployees()
        {
            try
            {
                _context = await EmployeeDataContextFactory.CreateDbContextAsync();
                if (_context is not null)
                {

                    if (_context is not null)
                    {
                        if (EmployeeToUpdate is not null)
                        {
                            _context.Employees.Update(EmployeeToUpdate);
                        }

                        await _context.SaveChangesAsync();
                    }
                }
                ShowEdit = false;
                await ShowEmployees();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error updating employee: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }


        //--------------DELETE----------------------------
        public async Task DeleteEmployee(Employee OurEmployee)
        {
            using var context = await EmployeeDataContextFactory.CreateDbContextAsync();
            if (context is not null)
            {
                if (OurEmployee is not null)
                {
                    context.Employees.Remove(OurEmployee);
                }
                await context.SaveChangesAsync();
                await ShowEmployees();
            }
        
        }
    }
}
