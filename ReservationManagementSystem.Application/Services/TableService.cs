using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReservationManagementSystem.Application.Repositories.Interface;
using ReservationManagementSystem.Application.Services.Interface;
using ReservationManagementSystem.Core.Entities;
using ReservationManagementSystem.Core.Enums;
using ReservationManagementSystem.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Application.Services
{
    public class TableService : ITableService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminRepository _adminRepository;
        private readonly IConfiguration _configuration;
        private readonly ITableRepository _tableRepository;
        private readonly ILogger<TableService> _logger;
        private readonly string SqlConn;

        public TableService(IHttpContextAccessor httpContextAccessor, IAdminRepository adminRepository, IConfiguration configuration, ITableRepository tableRepository, ILogger<TableService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _adminRepository = adminRepository;
            _configuration = configuration;
            _tableRepository = tableRepository;
            _logger = logger;
            SqlConn = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<ResponseViewModel> CreateRestaurantTable(TableViewModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.TableName))
                    return new ResponseViewModel { message = "All fields are required", status = false, data = "No data available" };

                string adminUsername = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Name)?.Value;
                if (adminUsername is null) 
                    return new ResponseViewModel { message = "Contact your admin to resolve this", status = false, data = "No data available" };
                
                int adminRole = Convert.ToInt32(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Role)?.Value);
                if(adminRole is not (int)Roles.PrimaryAdmin)
                    return new ResponseViewModel { message = "Permission denied", status = false, data = "No data available" };

                var adminInfo = await _adminRepository.GetAdminByUsername(adminUsername, SqlConn);
                if(adminInfo.RestuarantId is null)
                    return new ResponseViewModel { message = "Admin must be profiled to a restuarant", status = false, data = "No data available" };

                Table table = new Table
                {
                    TableName = model.TableName,
                    TableQuantity = model.TableQuantity,
                    IsActive = true,
                    RestuarantId = (int)adminInfo.RestuarantId
                };

                var createNewTable = await _tableRepository.AddRestuarantTable(table);
                if(createNewTable.status is false)
                    return new ResponseViewModel { message = "Table was not created", status = false, data = "No data available" };

                return new ResponseViewModel { message = "Table created", status = true, data = "No data available" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new ResponseViewModel { message = "Table was not created, please try again.", status = false, data = "No data available" };
            }
        }
    }
}
