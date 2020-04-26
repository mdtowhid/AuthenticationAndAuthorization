using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthAsp.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;

        public SQLEmployeeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Employee Add(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            var res = context.Employees.FirstOrDefault(x => x.Id == id);
            if(res != null)
            {
                context.Employees.Remove(res);
                context.SaveChanges();
            }
            return res;
        }

        public Employee GetEmployee(int id) => context.Employees.FirstOrDefault(x => x.Id == id);

        public IEnumerable<Employee> GetEmployees() => context.Employees.ToList();

        public Employee Update(Employee employee)
        {
            var emp = context.Employees.Attach(employee);
            emp.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employee;
        }
    }
}
