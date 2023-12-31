using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net.Http.Headers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Products;
using TalabatAppApis.Dtos;
using TalabatAppApis.Errors;
using TalabatAppApis.Helpers;

namespace TalabatAppApis.Controllers
{

    public class ProductController : BaseAPIController
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork , IMapper mapper )
        {
           
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        [CashedAttribute(600)]
        [HttpGet("GetProductsWithFilter")]
        public async Task<ActionResult< Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProdcutSpecParams SpecParams)
        {
            var spec = new ProductWithTypeAndBrandSpecifications(SpecParams);
            var products = await _unitOfWork.Repository<Product>().GetAllAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            var countSpec = new ProductWithFiltersForCountSpecification(SpecParams);
            var Count = await _unitOfWork.Repository<Product>().GetCountAsync(countSpec);

            return Ok(new Pagination<ProductToReturnDto>(SpecParams.PageSize, SpecParams.PageIndex,Count,data));
        }


        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [CashedAttribute(600)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetById(int id)
        {
            var spec = new ProductWithTypeAndBrandSpecifications(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(spec);

            if (product == null)
            {
                return NotFound(new ApiResponse(404)); 
            }

            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }





        [CashedAttribute(600)]
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
            var spec = new ProductBrandSpecifications();
            var productBrand = await _unitOfWork.Repository<ProductBrand>().GetAllAsync(spec);
            return Ok(productBrand);
        }






        [CashedAttribute(600)]
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllTypes()
        {
            var spec = new ProductTypeSpecifications();
            var productType = await _unitOfWork.Repository<ProductType>().GetAllAsync(spec);
            return Ok(productType);
        }


    }
}
