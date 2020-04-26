using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthAsp.Models
{
    public class MockEmployeeRepository 
    {
        private List<Employee> _employees;

        public MockEmployeeRepository()
        {
            _employees = AllEmployees();
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employees.Max(x => x.Id) + 1;
            _employees.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            var res = _employees.FirstOrDefault(x => x.Id == id);
            _employees.Remove(res);
            return res;
        }

        public Employee GetEmployee(int id) =>
            _employees.FirstOrDefault(x => x.Id == id);

        public IEnumerable<Employee> GetEmployees() => _employees;

        public Employee Update(Employee employee)
        {
            var res = _employees.FirstOrDefault(x => x.Id == employee.Id);

            res.Name = employee.Name;
            res.Email = employee.Email;
            res.Department = employee.Department;

            return res;
        }

        private List<Employee> AllEmployees()
        {
            return new List<Employee>
            {
                new Employee { Id = 1, Name="Jane", Email="jane@gmail.com", Department = Dept.Audit},
                new Employee { Id = 2, Name="Doe", Email="doe@gmail.com", Department=Dept.EEE},
                new Employee { Id = 3, Name="C", Email="a@gmail.com", Department=Dept.Finance},
                new Employee { Id = 4, Name="B", Email="ab@gmail.com", Department=Dept.HR},
                new Employee { Id = 5, Name="D", Email="axd@gmail.com", Department=Dept.CS},
            };
        }
    }
}
