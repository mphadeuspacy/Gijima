using System.Collections.Generic;
using Gijima.Hulamin.Core.Entities;
using System;
using System.Threading.Tasks;
using Gijima.Hulamin.Core.Validation.Abstracts;
using System.Data;
using System.Data.SqlClient;
using Gijima.Hulamin.Core.Exceptions;
using Microsoft.ApplicationBlocks.Data;

namespace Gijima.Hulamin.Data.Persistence
{
    public class StandardRepository : IRepository
    {
        private SpecificationHandler SpecificationHandler { get; }
        private string _connectionString;

        public StandardRepository(ISetUpSpecificationHandler specificationHandlerSetUp, string connectionString)
        {
            SpecificationHandler = specificationHandlerSetUp.SetUpChain();
            _connectionString = string.IsNullOrWhiteSpace(connectionString) == false ? connectionString : throw new ArgumentException();
        }

        public async Task CreateAsync(IEntity entity)
        {
            ValidateEntity(entity);

            try
            {
                if(entity is Product productEntity )
                   SqlHelper.ExecuteNonQuery(_connectionString, CommandType.StoredProcedure, "CreateEntityProduct",
                                         new SqlParameter("@Name", productEntity.Name),
                                         new SqlParameter("@SupplierId", productEntity.SupplierId),
                                         new SqlParameter("@CategoryId", productEntity.CategoryId),
                                         new SqlParameter("@QuantityPerUnit", productEntity.QuantityPerUnit),
                                         new SqlParameter("@UnitPrice", productEntity.UnitPrice),
                                         new SqlParameter("@UnitsInStock", productEntity.UnitsInStock),
                                         new SqlParameter("@UnitsOnOrder", productEntity.UnitsOnOrder),
                                         new SqlParameter("@ReorderLevel", productEntity.ReorderLevel),
                                         new SqlParameter("@Discontinued", productEntity.Discontinued));
            }
            catch (Exception sqlException)
            {
                throw new BusinessException(sqlException.Message);
            }           
        }

        public async Task<List<IEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(IEntity entity)
        {
            throw new NotImplementedException();
        }

        private void ValidateEntity(IEntity entity)
        {
            SpecificationHandler.HandleSpecificationRequest(entity);
        }
    }
}
