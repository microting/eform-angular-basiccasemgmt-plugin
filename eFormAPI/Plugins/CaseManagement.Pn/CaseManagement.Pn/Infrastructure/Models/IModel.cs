using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CaseManagement.Pn.Infrastructure.Data;

namespace CaseManagement.Pn.Infrastructure.Models
{
    interface IModel
    {
        Task Create(CaseManagementPnDbContext _dbContext);

        Task Update(CaseManagementPnDbContext _dbContext);

        Task Delete(CaseManagementPnDbContext _dbContext);

    }
}
