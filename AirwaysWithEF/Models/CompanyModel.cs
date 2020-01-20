using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace AirwaysWithEF.Models
{
    public class CompanyModel:Company
    {
        public CompanyModel(Company company)
        {
            ID_comp = company.ID_comp;
            name = company.name;
        }

        public CompanyModel() { }
    }
}
