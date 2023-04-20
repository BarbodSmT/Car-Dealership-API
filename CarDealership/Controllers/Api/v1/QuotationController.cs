using AutoMapper;
using CarDealership.Models.DTO;
using Common.Enums;
using Common.Exceptions;
using Data.Contracts;
using Data.Repositories;
using Entities;
using Entities.UserManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFramework.Api;
using WebFramwork.Api;

namespace CarDealership.Controllers.Api.V1;
[ApiVersion("1")]
public class QuotationController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IRepository<Quotation> _repository;
    public QuotationController(IMapper mapper, IRepository<Quotation> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    [HttpGet]
    [Authorize(Roles = Permission.Manager)]
    public virtual async Task<ApiResult<List<Quotation>>> GetAll(CancellationToken cancellationToken)
    {
        var quotations = await _repository.Table.ToListAsync(cancellationToken);
        if (!quotations.Any())
            return NotFound();
        return Ok(quotations);
    }

        [HttpGet("{id:int}")]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ApiResult<Quotation>> Get(int id, CancellationToken cancellationToken)
        {
            var quotation = await _repository.Table.SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);
            if (quotation == null)
                return NotFound();
            return quotation;
        }

        [HttpPost]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ApiResult<Quotation>> Create(QuotationDTO quotationDto, CancellationToken cancellationToken)
        {
            var quotation = _mapper.Map<Quotation>(quotationDto);
            quotation.CreatedByUserId = userId;
            await _repository.AddAsync(quotation, cancellationToken);
            return Ok(quotation);
        }
        [HttpPut]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ApiResult> Update(int id, QuotationDTO quotation, CancellationToken cancellationToken)
        {
            var updateQuotation = await _repository.GetByIdAsync(cancellationToken, id);
            if (updateQuotation == null)
                return NotFound();
            updateQuotation.ModifiedByUserId = userId;
            updateQuotation = _mapper.Map<Quotation>(quotation);
            await _repository.UpdateAsync(updateQuotation, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ApiResult> Quotation(int id, CancellationToken cancellationToken)
        {
            var contract = await _repository.GetByIdAsync(cancellationToken, id);
            if (contract == null)
                return NotFound();
            await _repository.DeleteAsync(contract, cancellationToken);

            return Ok();
        }
}