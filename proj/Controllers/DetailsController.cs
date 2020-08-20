using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using proj.Models;
using proj.Data;
using AutoMapper;
using proj.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Text;

namespace proj.Controllers
{
    [Authorize]
    [Route("api/Details")]
    [ApiController]
    public class DetailsController : ControllerBase
    {
        private  readonly IProjRepo _repository;
        private readonly IMapper _mapper;

        public DetailsController(IProjRepo repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<DetailReadDto>> GetAllDetails()
        {
            var DetailItems=_repository.GetAllDetails();
            return Ok(_mapper.Map<IEnumerable<DetailReadDto>>(DetailItems));
        }


        [HttpGet("{id}",Name="GetDetailById")]
        public ActionResult <DetailReadDto> GetDetailById(int id)
        {
            var DetailItem=  _repository.GetDetailById(id);
            if(DetailItem!=null)
            {
                return Ok(_mapper.Map<DetailReadDto>(DetailItem));
            }
            return NotFound();
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]DetailAuthenticateDto detailAuthenticateDto)
        {
            var user =_repository.Authenticate(detailAuthenticateDto.uname,detailAuthenticateDto.password);

            if(user==null)
            {
                return BadRequest(new {message = "Username or password is incorrect"});
            }

            var tokenHandler =new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("This is my secret key");
            var tokenDescriptor= new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires =DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new
            {
                Id=user.Id,
                uname=user.uname,
                TokenContext=tokenString
            });
        }
        [AllowAnonymous]

        [HttpPost("createDetail")]
        public ActionResult <DetailReadDto> CreateDetail(DetailCreateDto detailCreateDto)
        {
            var detailModel =_mapper.Map<Detail>(detailCreateDto);
            _repository.CreateDetail(detailModel);
            _repository.SaveChanges();

            var detailReadDto=_mapper.Map<DetailReadDto>(detailModel);
            
            return CreatedAtRoute(nameof(GetDetailById), new {Id = detailReadDto.Id},detailReadDto);
            //return Ok(detailReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateDetail(int id, DetailUpdateDto detailUpdateDto)
        {
            var detailModelFromRepo = _repository.GetDetailById(id);
            if(detailModelFromRepo==null)
            {
                return NotFound();
            }
            _mapper.Map(detailUpdateDto, detailModelFromRepo);
            _repository.UpdateDetail(detailModelFromRepo);
            _repository.SaveChanges();
            return NoContent();
        }
        [HttpPatch("{id}")]
        public ActionResult PartialDetailUpdate(int id, JsonPatchDocument<DetailUpdateDto> patchDoc)
        {
            var detailModelFromRepo = _repository.GetDetailById(id);
            if(detailModelFromRepo==null)
            {
                return NotFound();
            }
            var detailToPatch = _mapper.Map<DetailUpdateDto>(detailModelFromRepo);
            patchDoc.ApplyTo(detailToPatch, ModelState);
            if(!TryValidateModel(detailToPatch))
            {
                return ValidationProblem(ModelState);
            }
             _mapper.Map(detailToPatch, detailModelFromRepo);
            _repository.UpdateDetail(detailModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteDetail(int id)
        {
            var detailModelFromRepo = _repository.GetDetailById(id);
            if(detailModelFromRepo==null)
            {
                return NotFound();
            }
            _repository.DeleteDetail(detailModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}