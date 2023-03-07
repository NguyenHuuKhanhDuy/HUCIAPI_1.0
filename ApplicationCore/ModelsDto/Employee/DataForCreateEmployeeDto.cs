using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ModelsDto.Employee
{
    public class DataForCreateEmployeeDto
    {
        public List<SalaryType> SalaryTypes { get; set;} = new List<SalaryType>();

        public List<Rule> Rules { get; set;} = new List<Rule>();

    }
}
