using System;
using System.ComponentModel.DataAnnotations;
using Microting.eFormApi.BasePn.Database.Base;

namespace CaseManagement.Pn.Infrastructure.Data.Entities
{
    public class Vehicle : BaseEntity
    {
        [StringLength(100)]
        public string ContractNumber { get; set; }

        [StringLength(250)]
        public string CustomerName { get; set; }

        [StringLength(100)]
        public string Brand { get; set; }

        [StringLength(100)]
        public string ModelName { get; set; }
        public DateTime RegistrationDate { get; set; }

        [StringLength(17)]
        public string VinNumber { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
    }
}