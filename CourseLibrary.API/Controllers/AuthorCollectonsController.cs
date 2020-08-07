﻿using AutoMapper;
using CourseLibrary.API.Helper;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authorcollections")]
    public class AuthorCollectonsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorCollectonsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("({ids})",Name ="GetAuthorCollection")]
        public IActionResult GetAuthorCollection([FromRoute] [ModelBinder(BinderType =typeof(ArrayModelBinding))]IEnumerable<Guid> ids)
        {
            if(ids == null)
            {
                return BadRequest();
            }
            var authorEntities = _courseLibraryRepository.GetAuthors(ids);
            if(ids.Count() != authorEntities.Count())
            {
                return NotFound();
            }
            var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            return Ok(authorsToReturn);
        }

        public ActionResult<IEnumerable<AuthorDto>> CreateAuthorCollection(IEnumerable<AuthorForCreationDto> authorCollection)
        {
            var authorEntities = _mapper.Map<IEnumerable<Entities.Author>>(authorCollection);
            foreach(var author in authorEntities)
            {
                _courseLibraryRepository.AddAuthor(author);
            }
            _courseLibraryRepository.Save();
            var authorCollectionToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            var idsAsString = string.Join(",", authorCollectionToReturn.Select(a => a.Id));
            return CreatedAtRoute("GetAuthorCollection",new { ids = idsAsString },authorCollectionToReturn);
        }
    }
}
