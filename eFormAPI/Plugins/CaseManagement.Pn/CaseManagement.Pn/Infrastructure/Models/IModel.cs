using System;
using System.Collections.Generic;
using System.Text;
using CaseManagement.Pn.Infrastructure.Data;

namespace CaseManagement.Pn.Infrastructure.Models
{
    interface IModel
    {
        void Save(CaseManagementPnDbAnySql _dbContext);

        void Update(CaseManagementPnDbAnySql _dbContext);

        void Delete(CaseManagementPnDbAnySql _dbContext);

    }
}
