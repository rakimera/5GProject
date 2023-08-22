using Application.Interfaces;
using Application.Models.CompanyLicense;
using AutoMapper;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyLicensesController : Controller
    {
        private readonly IServiceWrapper _service;
        private readonly IMapper _mapper;

        public CompanyLicensesController(IMapper mapper, IServiceWrapper service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var baseResponse = _service.CompanyLicenseService.GetAll();
            if (baseResponse.Success)
                return Ok(baseResponse);
            return NotFound(baseResponse);
        }

        [HttpGet("{oid}")]
        public async Task<IActionResult> Get(string oid)
        {
            var baseResponse = await _service.CompanyLicenseService.GetByOid(oid);
            if (baseResponse.Success)
                return Ok(baseResponse);
            return NotFound(baseResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateCompanyLicenseDto model)
        {
            CompanyLicenseDto licenseDto = _mapper.Map<CompanyLicenseDto>(model);
            var baseResponse = await _service.CompanyLicenseService.CreateAsync(licenseDto, User.Identity.Name);

            if (baseResponse.Success)
                return Ok(baseResponse);
            return BadRequest(baseResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateCompanyLicenseDto model)
        {
            var baseResponse = await _service.CompanyLicenseService.UpdateLicense(model);
            if (baseResponse.Success)
                return Ok(baseResponse);
            return BadRequest(baseResponse);
        }

        [HttpDelete("{oid}")]
        public async Task<IActionResult> Delete(string oid)
        {
            var baseResponse = await _service.CompanyLicenseService.Delete(oid);
            if (baseResponse.Success)
                return Ok(baseResponse);
            return NotFound(baseResponse);
        }

        [HttpGet("index")]
        public async Task<IActionResult> Get([FromQuery] DataSourceLoadOptionsBase loadOptions)
        {
            var loadResult = await _service.CompanyLicenseService.GetLoadResult(loadOptions);
            return Ok(loadResult);
        }
    }
}