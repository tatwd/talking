using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Talking.Api.Data;
using Talking.Api.Repository;

namespace Talking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentRepository _commentRepository;

        public CommentsController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var data = _commentRepository.GetComments();
                return Ok(new { code = 0, data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 5, message = ex.Message });
            }
        }

        [HttpGet("{postUrl}")]
        public IActionResult Get(string postUrl)
        {
            try
            {
                var data = _commentRepository.GetComments(postUrl);
                return Ok(new { code = 0, data });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { code = 5, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Comment comment)
        {
            try
            {
                _commentRepository.InsertComment(comment);
                return Ok(comment.ID);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
