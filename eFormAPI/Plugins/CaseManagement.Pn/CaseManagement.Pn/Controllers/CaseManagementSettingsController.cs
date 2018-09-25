using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using CaseManagement.Pn.Infrastructure.Data;
using CaseManagement.Pn.Infrastructure.Data.Entities;
using CaseManagement.Pn.Infrastructure.Helpers;
using CaseManagement.Pn.Infrastructure.Models;
using eFormApi.BasePn.Consts;
using eFormApi.BasePn.Infrastructure;
using eFormApi.BasePn.Infrastructure.Models.API;
using NLog;

namespace CaseManagement.Pn.Controllers
{
    [Authorize]
    public class CaseManagementSettingsController : ApiController
    {
        private readonly Logger _logger;
        private readonly CaseManagementPnDbContext _dbContext;
        private readonly EFormCoreHelper _coreHelper = new EFormCoreHelper();

        public CaseManagementSettingsController()
        {
            _dbContext = CaseManagementPnDbContext.Create();
            _logger = LogManager.GetCurrentClassLogger();
        }
        
        [HttpGet]
        [Authorize(Roles = EformRoles.Admin)]
        [Route("api/case-management-pn/settings")]
        public OperationDataResult<CaseManagementPnSettingsModel> GetSettings()
        {
            try
            {
                var result = new CaseManagementPnSettingsModel();
                var customerSettings = _dbContext.CaseManagementSettings.FirstOrDefault();
                if (customerSettings?.SelectedTemplateId != null)
                {
                    result.SelectedTemplateId = (int) customerSettings.SelectedTemplateId;
                    result.SelectedTemplateName =customerSettings.SelectedTemplateName;
                }
                else
                {
                    result.SelectedTemplateId = null;
                }
                return new OperationDataResult<CaseManagementPnSettingsModel>(true, result);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                _logger.Error(e);
                return new OperationDataResult<CaseManagementPnSettingsModel>(false,
                    CustomersPnLocaleHelper.GetString("ErrorObtainingCustomersInfo"));
            }
        }


        [HttpPost]
        [Authorize(Roles = EformRoles.Admin)]
        [Route("api/case-management-pn/settings")]
        public OperationResult UpdateSettings(CaseManagementPnSettingsModel caseManagementSettingsModel)
        {
            try
            {
                var caseManagementSettings = _dbContext.CaseManagementSettings.FirstOrDefault();
                if (caseManagementSettings == null)
                {
                    caseManagementSettings = new CaseManagementSetting()
                    {
                        SelectedTemplateId = caseManagementSettingsModel.SelectedTemplateId
                    };
                    _dbContext.CaseManagementSettings.Add(caseManagementSettings);
                }
                else
                {
                    caseManagementSettings.SelectedTemplateId = caseManagementSettingsModel.SelectedTemplateId;
                }

                if (caseManagementSettingsModel.SelectedTemplateId != null)
                {
                    var core = _coreHelper.GetCore();
                    var template = core.TemplateRead((int) caseManagementSettingsModel.SelectedTemplateId);
                    caseManagementSettings.SelectedTemplateName = template.Label;
                }
                _dbContext.SaveChanges();
                return new OperationResult(true,
                    CustomersPnLocaleHelper.GetString("CustomerUpdatedSuccessfully"));
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                _logger.Error(e);
                return new OperationResult(false,
                    CustomersPnLocaleHelper.GetString("ErrorWhileUpdatingCustomerInfo"));
            }
        }

        //[HttpPost]
        //[Route("api/customers-pn/get-all")]
        //public OperationDataResult<CustomersModel> GetCustomers(CustomersRequestModel pnRequestModel)
        //{
        //    try
        //    {
        //        var customersPnModel = new CustomersModel();
        //        var customersQuery = _dbContext.Customers.AsQueryable();
        //        if (!string.IsNullOrEmpty(pnRequestModel.SortColumnName))
        //        {
        //            if (pnRequestModel.IsSortDsc)
        //            {
        //                customersQuery = customersQuery
        //                    .OrderByDescending(pnRequestModel.SortColumnName);
        //            }
        //            else
        //            {
        //                customersQuery = customersQuery
        //                    .OrderBy(pnRequestModel.SortColumnName);
        //            }
        //        }
        //        else
        //        {
        //            customersQuery = _dbContext.Customers
        //                .OrderBy(x => x.Id);
        //        }

        //        customersQuery = customersQuery
        //            .Skip(pnRequestModel.Offset)
        //            .Take(pnRequestModel.PageSize);

        //        var customers = customersQuery.ToList();
        //        customersPnModel.Total = _dbContext.Customers.Count();
        //        var fields = _dbContext.CustomerFields
        //            .Include("Field")
        //            .Select(x => new FieldPnUpdateModel()
        //            {
        //                FieldStatus = x.FieldStatus,
        //                Id = x.FieldId,
        //                Name = x.Field.Name,
        //            }).ToList();

        //        foreach (var customer in customers)
        //        {
        //            var customerModel = new CustomerModel()
        //            {
        //                Id = customer.Id,
        //            };
        //            foreach (var field in fields)
        //            {
        //                if (field.FieldStatus == FieldPnStatus.Enabled)
        //                {
        //                    var fieldModel = new FieldPnModel
        //                    {
        //                        Id = field.Id,
        //                        Name = field.Name,
        //                    };
        //                    var val = customer.GetPropValue(field.Name);
        //                    if (val != null)
        //                    {
        //                        fieldModel.Value = val.ToString();
        //                    }

        //                    customerModel.Fields.Add(fieldModel);
        //                }
        //            }

        //            if (customerModel.Fields.Any())
        //            {
        //                // Mode Id field to top
        //                var index = customerModel.Fields.FindIndex(x => x.Name == "Id");
        //                var item = customerModel.Fields[index];
        //                customerModel.Fields[index] = customerModel.Fields[0];
        //                customerModel.Fields[0] = item;
        //            }

        //            customersPnModel.Customers.Add(customerModel);
        //        }

        //        return new OperationDataResult<CustomersModel>(true, customersPnModel);
        //    }
        //    catch (Exception e)
        //    {
        //        Trace.TraceError(e.Message);
        //        _logger.Error(e);
        //        return new OperationDataResult<CustomersModel>(false,
        //            CustomersPnLocaleHelper.GetString("ErrorObtainingCustomersInfo"));
        //    }
        //}

        //[HttpGet]
        //[Route("api/customers-pn/{id}")]
        //public OperationDataResult<CustomerFullModel> GetSingleCustomer(int id)
        //{
        //    try
        //    {
        //        var customer = _dbContext.Customers.Select(x => new CustomerFullModel()
        //            {
        //                Id = x.Id,
        //                Description = x.Description,
        //                Phone = x.Phone,
        //                CityName = x.CityName,
        //                CustomerNo = x.CustomerNo,
        //                ZipCode = x.ZipCode,
        //                Email = x.Email,
        //                ContactPerson = x.ContactPerson,
        //                CreatedBy = x.CreatedBy,
        //                CompanyAddress = x.CompanyAddress,
        //                CompanyName = x.CompanyName,
        //            })
        //            .FirstOrDefault(x => x.Id == id);

        //        if (customer == null)
        //        {
        //            return new OperationDataResult<CustomerFullModel>(false,
        //                CustomersPnLocaleHelper.GetString("CustomerNotFound"));
        //        }

        //        return new OperationDataResult<CustomerFullModel>(true, customer);
        //    }
        //    catch (Exception e)
        //    {
        //        Trace.TraceError(e.Message);
        //        _logger.Error(e);
        //        return new OperationDataResult<CustomerFullModel>(false,
        //            CustomersPnLocaleHelper.GetString("ErrorObtainingCustomersInfo"));
        //    }
        //}

        //[HttpPost]
        //[Route("api/customers-pn")]
        //public OperationResult CreateCustomer(CustomerFullModel customerPnCreateModel)
        //{
        //    try
        //    {
        //        var customer = new Customer()
        //        {
        //            CityName = customerPnCreateModel.CityName,
        //            CompanyAddress = customerPnCreateModel.CompanyAddress,
        //            CompanyName = customerPnCreateModel.CompanyName,
        //            ContactPerson = customerPnCreateModel.ContactPerson,
        //            CreatedBy = customerPnCreateModel.CreatedBy,
        //            CustomerNo = customerPnCreateModel.CustomerNo,
        //            CreatedDate = DateTime.UtcNow,
        //            Description = customerPnCreateModel.Description,
        //            Email = customerPnCreateModel.Email,
        //            Phone = customerPnCreateModel.Phone,
        //            ZipCode = customerPnCreateModel.ZipCode
        //        };
        //        _dbContext.Customers.Add(customer);
        //        _dbContext.SaveChanges();
        //        // create item
        //        var customerSettings = _dbContext.CustomerSettings.FirstOrDefault();
        //        if (customerSettings?.RelatedEntityGroupId != null)
        //        {
        //            var core = _coreHelper.GetCore();
        //            var entityGroup = core.EntityGroupRead(customerSettings.RelatedEntityGroupId.ToString());
        //            if (entityGroup == null)
        //            {
        //                return new OperationResult(false, "Entity group not found");
        //            }

        //            var nextItemUid = entityGroup.EntityGroupItemLst.Count;
        //            Trace.TraceError("1 - "+nextItemUid);
        //            Trace.TraceError("customerid - "+customer.Id);
        //            Trace.TraceError("1 - "+nextItemUid);

        //            var item = core.EntitySearchItemCreate(entityGroup.Id, $"Customer {customer.Id}", "",
        //                nextItemUid.ToString());
        //            customer.RelatedEntityId = nextItemUid;
        //            customer.RelatedEntityId = nextItemUid;
        //            _dbContext.Entry(customerSettings).State = EntityState.Modified;
        //            _dbContext.SaveChanges();
        //        }

        //        return new OperationResult(true,
        //            CustomersPnLocaleHelper.GetString("CustomerCreated"));
        //    }
        //    catch (Exception e)
        //    {
        //        Trace.TraceError(e.Message);
        //        _logger.Error(e);
        //        return new OperationResult(false,
        //            CustomersPnLocaleHelper.GetString("ErrorWhileCreatingCustomer"));
        //    }
        //}

        //[HttpPut]
        //[Route("api/customers-pn")]
        //public OperationResult UpdateCustomer(CustomerFullModel customerUpdateModel)
        //{
        //    try
        //    {
        //        var customer = _dbContext.Customers.FirstOrDefault(x => x.Id == customerUpdateModel.Id);
        //        if (customer == null)
        //        {
        //            return new OperationResult(false,
        //                CustomersPnLocaleHelper.GetString("CustomerNotFound"));
        //        }

        //        customer.Description = customerUpdateModel.Description;
        //        customer.CityName = customerUpdateModel.CityName;
        //        customer.CompanyAddress = customerUpdateModel.CompanyAddress;
        //        customer.ContactPerson = customerUpdateModel.ContactPerson;
        //        customer.CreatedBy = customerUpdateModel.CreatedBy;
        //        customer.CustomerNo = customerUpdateModel.CustomerNo;
        //        customer.CompanyName = customerUpdateModel.CompanyName;
        //        customer.Email = customerUpdateModel.Email;
        //        customer.Phone = customerUpdateModel.Phone;
        //        customer.ZipCode = customerUpdateModel.ZipCode;
        //        _dbContext.SaveChanges();
        //        return new OperationDataResult<CustomersModel>(true,
        //            CustomersPnLocaleHelper.GetString("CustomerUpdatedSuccessfully"));
        //    }
        //    catch (Exception e)
        //    {
        //        Trace.TraceError(e.Message);
        //        _logger.Error(e);
        //        return new OperationDataResult<CustomersModel>(false,
        //            CustomersPnLocaleHelper.GetString("ErrorWhileUpdatingCustomerInfo"));
        //    }
        //}


        //[HttpDelete]
        //[Route("api/customers-pn/{id}")]
        //public OperationResult DeleteCustomer(int id)
        //{
        //    try
        //    {
        //        var customer = _dbContext.Customers.FirstOrDefault(x => x.Id == id);
        //        if (customer == null)
        //        {
        //            return new OperationResult(false,
        //                CustomersPnLocaleHelper.GetString("CustomerNotFound"));
        //        }
        //        var core = _coreHelper.GetCore();
        //        if (customer.RelatedEntityId != null)
        //        {
        //            core.EntityItemDelete((int) customer.RelatedEntityId);
        //        }
        //        _dbContext.Customers.Remove(customer);
        //        _dbContext.SaveChanges();
        //        return new OperationResult(true,
        //            CustomersPnLocaleHelper.GetString("CustomerDeletedSuccessfully"));
        //    }
        //    catch (Exception e)
        //    {
        //        Trace.TraceError(e.Message);
        //        _logger.Error(e);
        //        return new OperationDataResult<CustomerFullModel>(false,
        //            CustomersPnLocaleHelper.GetString("ErrorWhileDeletingCustomer"));
        //    }
        //}

        //[HttpPost]
        //[Authorize(Roles = EformRoles.Admin)]
        //[Route("api/customers-pn/settings")]
        //public OperationResult UpdateSettings(CustomerSettingsModel customerUpdateModel)
        //{
        //    try
        //    {
        //        var customerSettings = _dbContext.CustomerSettings.FirstOrDefault();
        //        if (customerSettings == null)
        //        {
        //            var customerSettingsNew = new CustomerSettings()
        //            {
        //                RelatedEntityGroupId = customerUpdateModel.RelatedEntityId
        //            };
        //            _dbContext.CustomerSettings.Add(customerSettingsNew);
        //        }
        //        else
        //        {
        //            customerSettings.RelatedEntityGroupId = customerUpdateModel.RelatedEntityId;
        //            _dbContext.Entry(customerSettings).State = EntityState.Modified;
        //        }

        //        _dbContext.SaveChanges();
              
        //        var core = _coreHelper.GetCore();
        //        Trace.TraceError("1");
        //        var entityGroup = core.EntityGroupRead(customerUpdateModel.RelatedEntityId.ToString());
        //        if (entityGroup == null)
        //        {
        //            return new OperationResult(false, "Entity group not found");
        //        }

        //        var nextItemUid = entityGroup.EntityGroupItemLst.Count;
               
        //        var customers = _dbContext.Customers.ToList();
        //        foreach (var customer in customers)
        //        {
        //            if (customer.RelatedEntityId != null && customer.RelatedEntityId.Value > 0)
        //            {
        //                core.EntityItemDelete(customer.RelatedEntityId.Value);
        //            }
        //            var item = core.EntitySearchItemCreate(entityGroup.Id, $"Customer {customer.Id}", "",
        //                nextItemUid.ToString());
        //            Trace.TraceError("4");
        //            var forUpdate = _dbContext.Customers.FirstOrDefault(x => x.Id == customer.Id);
        //            if (forUpdate != null)
        //            {
        //                forUpdate.RelatedEntityId = nextItemUid;
        //                _dbContext.Entry(forUpdate).State = EntityState.Modified;
        //                _dbContext.SaveChanges();

        //                Payment payment = new Payment();
        //                payment = db.Payments.Find(orderId);
        //                payment.Status = true;
        //                db.Entry(payment).State = EntityState.Modified;
        //                db.SaveChanges();
        //            }

                    
        //            nextItemUid++;
        //        }
                
               
        //        return new OperationDataResult<CustomersModel>(true,
        //            CustomersPnLocaleHelper.GetString("CustomerUpdatedSuccessfully"));
        //    }
        //    catch (Exception e)
        //    {
        //        Trace.TraceError(e.Message);
        //        _logger.Error(e);
        //        return new OperationDataResult<CustomersModel>(false,
        //            CustomersPnLocaleHelper.GetString("ErrorWhileUpdatingCustomerInfo"));
        //    }
        //}
    }
}