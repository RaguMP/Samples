using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TransactionSampleWebAPI.Entity;

namespace TransactionSampleWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SQLServerController : ControllerBase
    {
        private readonly StudentDbContext context;

        public SQLServerController(StudentDbContext context)
        {
            this.context = context;
        }

        [HttpPost("{brandId}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public async Task<ActionResult> AddProjects(int brandId)
        {
            int retryCount = 0;

            var projectDetails = new TrProjectDetails()
            {
                BrandId = brandId,
                IsActive = true
            };
            context.TrProjectDetails.Add(projectDetails);

            while (retryCount < 3)
            {
                try
                {
                    var sequenceIdDetails = await context.TrProjectSequenceMapper.FirstOrDefaultAsync(i => i.IsActive && i.BrandId == brandId)
                                            .ConfigureAwait(false);

                    projectDetails.BrandProjectId = sequenceIdDetails.SequenceId;
                    sequenceIdDetails.SequenceId++;

                    if (retryCount == 0)
                    {
                        var sequenceId = new SqlParameter("sequenceId", sequenceIdDetails.SequenceId);
                        var dbBrandId = new SqlParameter("brandId", brandId);
                        context.Database.ExecuteSqlRaw("UPDATE TR_ProjectSequenceMapper SET SequenceId = @sequenceId WHERE BrandId = @brandId", dbBrandId, sequenceId);
                    }

                    await context.SaveChangesAsync().ConfigureAwait(false);

                    var projectIdMapperDetails = new TrProjectMapper()
                    {
                        ProjectId = projectDetails.Id,
                        IsActive = true
                    };

                    await context.TrProjectMapper.AddAsync(projectIdMapperDetails).ConfigureAwait(false);
                    await context.SaveChangesAsync().ConfigureAwait(false);


                    retryCount = 3;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    retryCount++;
                    EntityEntry exceptionEntry = ex.Entries.Single();
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    exceptionEntry.OriginalValues.SetValues(databaseEntry);
                }
            }

            return new JsonResult(new { result = projectDetails.BrandProjectId });
        }

        [HttpPost("transaction/{brandId}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public async Task<ActionResult> AddProjectsUsingTransactions(int brandId)
        {
            int retryCount = 0;
            int projectId = 0;

            using (var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false))
            {
                var projectDetails = new TrProjectDetails()
                {
                    BrandId = brandId,
                    IsActive = true
                };
                await context.TrProjectDetails.AddAsync(projectDetails).ConfigureAwait(false);

                while (retryCount < 3)
                {
                    try
                    {
                        var sequenceIdDetails = await context.TrProjectSequenceMapper.FirstOrDefaultAsync(i => i.IsActive && i.BrandId == brandId)
                                                .ConfigureAwait(false);

                        projectDetails.BrandProjectId = sequenceIdDetails.SequenceId;
                        sequenceIdDetails.SequenceId++;

                        if (retryCount == 0)
                        {
                            var sequenceId = new SqlParameter("sequenceId", sequenceIdDetails.SequenceId);
                            var dbBrandId = new SqlParameter("brandId", brandId);
                            context.Database.ExecuteSqlRaw("UPDATE TR_ProjectSequenceMapper SET SequenceId = @sequenceId WHERE BrandId = @brandId", dbBrandId, sequenceId);
                        }

                        await context.SaveChangesAsync().ConfigureAwait(false);

                        var projectIdMapperDetails = new TrProjectMapper()
                        {
                            ProjectId = projectDetails.Id,
                            IsActive = true
                        };

                        await context.TrProjectMapper.AddAsync(projectIdMapperDetails).ConfigureAwait(false);
                        await context.SaveChangesAsync().ConfigureAwait(false);


                        await transaction.CommitAsync().ConfigureAwait(false);

                        projectId = projectDetails.BrandProjectId;
                        retryCount = 3;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        retryCount++;
                        EntityEntry exceptionEntry = ex.Entries.Single();
                        var databaseEntry = exceptionEntry.GetDatabaseValues();
                        exceptionEntry.OriginalValues.SetValues(databaseEntry);
                    }
                }
            }

            return new JsonResult(new { result = projectId });
        }

        [HttpPost("transactionrollback/{brandId}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public async Task<ActionResult> AddProjectsUsingTransactionsWithRollback(int brandId)
        {
            int retryCount = 0;
            int projectId = 0;

            while (retryCount < 3)
            {
                using (var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false))
                {
                    var projectDetails = new TrProjectDetails()
                    {
                        BrandId = brandId,
                        IsActive = true
                    };
                    await context.TrProjectDetails.AddAsync(projectDetails).ConfigureAwait(false);

                    try
                    {
                        var sequenceIdDetails = await context.TrProjectSequenceMapper.FirstOrDefaultAsync(i => i.IsActive && i.BrandId == brandId)
                                                .ConfigureAwait(false);

                        projectDetails.BrandProjectId = sequenceIdDetails.SequenceId;
                        sequenceIdDetails.SequenceId++;

                        if (retryCount == 0)
                        {
                            var sequenceId = new SqlParameter("sequenceId", sequenceIdDetails.SequenceId);
                            var dbBrandId = new SqlParameter("brandId", brandId);
                            context.Database.ExecuteSqlRaw("UPDATE TR_ProjectSequenceMapper SET SequenceId = @sequenceId WHERE BrandId = @brandId", dbBrandId, sequenceId);
                        }

                        await context.SaveChangesAsync().ConfigureAwait(false);

                        var projectIdMapperDetails = new TrProjectMapper()
                        {
                            ProjectId = projectDetails.Id,
                            IsActive = true
                        };

                        await context.TrProjectMapper.AddAsync(projectIdMapperDetails).ConfigureAwait(false);
                        await context.SaveChangesAsync().ConfigureAwait(false);


                        await transaction.CommitAsync().ConfigureAwait(false);

                        projectId = projectDetails.BrandProjectId;
                        retryCount = 3;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        retryCount++;
                        await transaction.RollbackAsync().ConfigureAwait(false);
                        EntityEntry exceptionEntry = ex.Entries.Single();
                        var databaseEntry = exceptionEntry.GetDatabaseValues();
                        exceptionEntry.OriginalValues.SetValues(databaseEntry);
                    }
                }
            }

            return new JsonResult(new { result = projectId });
        }
    }
}